/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IAsyncOperate.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/25/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections;

namespace MGS.Operate
{
    public interface IAsyncOperate
    {
        bool IsDone { get; }

        float Progress { get; }

        Exception Error { get; }

        void ExecuteAsync();

        IEnumerator WaitDone();

        void AbortAsync();
    }

    public interface IAsyncOperate<T> : IAsyncOperate
    {
        event Action<float> OnProgressed;
        event Action<T, Exception> OnCompleted;

        T Result { get; }
    }
}