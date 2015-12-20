﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerBrowser.Helpers
{
    public class AsyncManualResetEvent
    {
        private volatile TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        public Task WaitAsync() { return tcs.Task; }

        public async Task<bool> WaitAsync(int timeout)
        {
            var task = tcs.Task;
            return await Task.WhenAny(task, Task.Delay(timeout)) == task;
        }

        public void Set() { tcs.TrySetResult(true); }

        public void Reset()
        {
            while (true)
            {
                var current = tcs;
                if (!current.Task.IsCompleted ||
                    Interlocked.CompareExchange(ref tcs, new TaskCompletionSource<bool>(), current) == current)
                    return;
            }
        }
    }
}
