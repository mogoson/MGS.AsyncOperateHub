/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IAsyncOperateHubPro.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/26/2025
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.Operate
{
    public interface IAsyncOperateHubPro : IAsyncOperateHub, ICruiser
    {
        int Concurrency { set; get; }

        int Waitings { get; }

        void Clear(bool workings, bool waitings);
    }
}