/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AsyncOperateHub.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/25/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace MGS.Operate
{
    public class AsyncOperateHub : IAsyncOperateHub
    {
        protected class OperateHubBehaviour : MonoBehaviour { }
        protected OperateHubBehaviour behaviour;

        public int Workings { get { return workings.Count; } }
        protected IDictionary<IAsyncOperate, Coroutine> workings = new Dictionary<IAsyncOperate, Coroutine>();

        public AsyncOperateHub()
        {
            behaviour = new GameObject(nameof(OperateHubBehaviour)).AddComponent<OperateHubBehaviour>();
            Object.DontDestroyOnLoad(behaviour.gameObject);
        }

        public virtual IAsyncOperate<T> Enqueue<T>(IAsyncOperate<T> operate)
        {
            if (!workings.ContainsKey(operate))
            {
                operate.OnCompleted += (result, error) => workings.Remove(operate);
                operate.ExecuteAsync();
                var waitDone = behaviour.StartCoroutine(operate.WaitDone());
                workings.Add(operate, waitDone);
            }
            return operate;
        }

        public virtual void Dequeue(IAsyncOperate operate)
        {
            if (workings.ContainsKey(operate))
            {
                operate.AbortAsync();
                behaviour.StopCoroutine(workings[operate]);
                workings.Remove(operate);
            }
        }

        public virtual void Clear()
        {
            foreach (var operate in workings)
            {
                operate.Key.AbortAsync();
                behaviour.StopCoroutine(operate.Value);
            }
            workings.Clear();
        }

        public virtual void Dispose()
        {
            Clear();
            workings = null;

            Object.Destroy(behaviour.gameObject);
            behaviour = null;
        }
    }
}