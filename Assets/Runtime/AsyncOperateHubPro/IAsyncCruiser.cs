/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  IAsyncCruiser.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/16/2025
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.Operate
{
    public interface IAsyncCruiser
    {
        bool IsActive { get; }

        int Interval { set; get; }

        void Activate();

        void Deactivate();
    }
}