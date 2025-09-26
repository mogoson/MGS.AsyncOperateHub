/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  OperateHubProSample.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/26/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Threading;
using UnityEngine;

namespace MGS.Operate.Sample
{
    public class OperateHubProSample : MonoBehaviour
    {
        class Operate : AsyncOperate<string>
        {
            string name;
            int time;

            public Operate(string name, int time)
            {
                this.name = name;
                this.time = time;
            }

            protected override string OnExecute()
            {
                Debug.Log($"Operate{name} start");
                Thread.Sleep(time);
                return name;
            }
        }

        private void Start()
        {
            var operateHub = new AsyncOperateHubPro();
            for (int i = 0; i < 10; i++)
            {
                operateHub.Enqueue(new Operate($"Operate {i}", Random.Range(1000, 5000)));
            }
            operateHub.Activate();
        }
    }
}