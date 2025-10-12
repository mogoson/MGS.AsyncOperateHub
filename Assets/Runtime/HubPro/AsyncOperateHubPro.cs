/*************************************************************************
 *  Copyright © 2025 Mogoson All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AsyncOperateHubPro.cs
 *  Description  :  Default.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  09/26/2025
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MGS.Operate
{
    public class AsyncOperateHubPro : AsyncOperateHub, IAsyncOperateHubPro
    {
        public bool IsActive { get { return cruiser != null; } }

        public int Interval
        {
            set
            {
                if (interval != value)
                {
                    interval = value;
                    instruction = new WaitForSeconds(interval / 1000f);
                }
            }
            get { return interval; }
        }

        public int Concurrency { set; get; }

        public int Waitings { get { return waitings.Count; } }

        protected int interval;
        protected Coroutine cruiser;
        protected YieldInstruction instruction;
        protected IList<IAsyncOperate> waitings = new List<IAsyncOperate>();
        protected ICollection<IAsyncOperate> temp = new List<IAsyncOperate>();

        public AsyncOperateHubPro(int interval = 250, int concurrency = 3)
        {
            this.interval = interval;
            Concurrency = concurrency;
            instruction = new WaitForSeconds(interval / 1000f);
        }

        public void Activate()
        {
            if (cruiser == null)
            {
                cruiser = StartCoroutine(StartCruiser());
            }
        }

        public void Deactivate()
        {
            if (cruiser != null)
            {
                StopCoroutine(cruiser);
                cruiser = null;
            }
        }

        public override IAsyncOperate<T> Enqueue<T>(IAsyncOperate<T> operate)
        {
            if (!waitings.Contains(operate))
            {
                waitings.Add(operate);
            }
            return operate;
        }

        public override void Dequeue(IAsyncOperate operate)
        {
            base.Dequeue(operate);
            waitings.Remove(operate);
        }

        public void Clear(bool workings, bool waitings)
        {
            if (workings)
            {
                base.Clear();
            }
            if (waitings)
            {
                this.waitings.Clear();
            }
        }

        public override void Clear()
        {
            base.Clear();
            waitings.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            cruiser = null;
            instruction = null;
            waitings = null;
            temp = null;
        }

        protected IEnumerator StartCruiser()
        {
            while (true)
            {
                CruiserTick();
                yield return instruction;
            }
        }

        protected virtual void CruiserTick()
        {
            temp.Clear();
            foreach (var operate in workings.Keys)
            {
                if (operate.IsDone)
                {
                    temp.Add(operate);
                }
            }
            foreach (var operate in temp)
            {
                workings.Remove(operate);
            }
            while (waitings.Count > 0 && workings.Count < Concurrency)
            {
                var operate = waitings[0];
                waitings.RemoveAt(0);

                operate.ExecuteAsync();
                var waitDone = StartCoroutine(operate.WaitDone());
                workings.Add(operate, waitDone);
            }
        }
    }
}