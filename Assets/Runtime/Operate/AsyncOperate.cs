/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AsyncOperate.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/25/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections;
using System.Threading;

namespace MGS.Operate
{
    public abstract class AsyncOperate<T> : IAsyncOperate<T>
    {
        public event Action<float> OnProgressed;
        public event Action<T, Exception> OnCompleted;

        public T Result { protected set; get; }

        public bool IsDone { protected set; get; }

        public float Progress { protected set; get; }

        public Exception Error { protected set; get; }

        protected Thread thread;

        public virtual void ExecuteAsync()
        {
            if (thread == null)
            {
                Reset();
                thread = new Thread(() =>
                {
                    try
                    {
                        Result = OnExecute();
                    }
                    catch (Exception ex)
                    {
                        Error = ex;
                        AbortAsync();
                    }
                    finally
                    {
                        OnFinally();
                    }
                })
                { IsBackground = true };
                thread.Start();
            }
        }

        public virtual IEnumerator WaitDone()
        {
            var progress = 0f;
            while (!IsDone)
            {
                if (progress != Progress)
                {
                    progress = Progress;
                    InvokeOnProgressed(Progress);
                }
                yield return null;
            }
            InvokeOnCompleted(Result, Error);
        }

        public virtual void AbortAsync()
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
                thread = null;
            }
            IsDone = true;
        }

        protected abstract T OnExecute();

        protected virtual void Reset()
        {
            IsDone = false;
            Progress = 0;
            Result = default;
            Error = null;
        }

        protected virtual void OnFinally()
        {
            thread = null;
            IsDone = true;
        }

        protected void InvokeOnProgressed(float progress)
        {
            OnProgressed?.Invoke(progress);
        }

        protected void InvokeOnCompleted(T result, Exception error)
        {
            OnCompleted?.Invoke(result, error);
        }
    }
}