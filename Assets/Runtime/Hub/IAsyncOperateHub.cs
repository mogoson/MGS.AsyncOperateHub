/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IAsyncOperateHub.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/25/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System;

namespace MGS.Operate
{
    public interface IAsyncOperateHub : IDisposable
    {
        int Workings { get; }

        IAsyncOperate<T> Enqueue<T>(IAsyncOperate<T> operate);

        void Dequeue(IAsyncOperate operate);

        void Clear();
    }
}