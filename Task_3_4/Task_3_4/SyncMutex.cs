using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Task_3_4
{
    class SyncMutex
    {
        ///Atomic mutex primitive.
        private static volatile bool locked = false;

        private static volatile Queue<Task> tasks = new Queue<Task>();

        private static volatile Queue<CancellationTokenSource> cts = 
                            new Queue<CancellationTokenSource>();

        public async Task Lock()
        {
            while (locked)
                await Task.Delay(1000);
            /// Capture.
            locked = true;
            CancellationTokenSource ct = new CancellationTokenSource();
            cts.Enqueue(ct);
            var t = new Task(() =>
            {
                while (true)
                    Task.Delay(200);
            }, ct.Token);
            tasks.Enqueue(t);
            await Task.Delay(5000);
            await Task.Factory.StartNew(() => t);

        }

        public void Release()
        {
            cts.Dequeue().Cancel();
            tasks.Dequeue();
            locked = false;
        }
    }
}
