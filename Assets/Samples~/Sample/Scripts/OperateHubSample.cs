/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  OperateHubSample.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/25/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Threading;
using UnityEngine;

namespace MGS.Operate.Sample
{
    public class OperateHubSample : MonoBehaviour
    {
        class OperateA : AsyncOperate<string>
        {
            protected override string OnExecute()
            {
                Thread.Sleep(1000);
                Progress = 0.5f;

                Thread.Sleep(2000);
                Progress = 1.0f;

                return string.Empty;
            }
        }

        class OperateB : AsyncOperate<byte[]>
        {
            protected override byte[] OnExecute()
            {
                Thread.Sleep(1500);
                Progress = 0.5f;

                Thread.Sleep(1000);
                Progress = 1.0f;

                return new byte[] { };
            }
        }

        private void Start()
        {
            var operateHub = new AsyncOperateHub();

            var operateA = operateHub.Enqueue(new OperateA());
            operateA.OnProgressed += progress => Debug.Log($"operateA progress:{progress}");
            operateA.OnCompleted += (result, error) => Debug.Log($"operateA result:{result}");

            var operateB = operateHub.Enqueue(new OperateB());
            operateB.OnProgressed += progress => Debug.Log($"operateB progress:{progress}");
            operateB.OnCompleted += (result, error) => Debug.Log($"operateB result:{result}");
        }
    }
}