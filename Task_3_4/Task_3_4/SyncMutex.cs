using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace Task_3_4
{
    public class SyncMutex : IDisposable
    {
        private volatile Queue<Task> tasks = new Queue<Task>();

        private readonly object lockable = new object();

        public async Task Lock()
        {
            /// Wait while previous tasks become completed.
            while (tasks.Count > 0)
               await Task.Delay(500);

            lock (lockable)
            {
                tasks.Enqueue(new Task(() => { }));
            }
        }

        public void Release(){
            lock (lockable)
            {
               if (tasks.Count > 0 && tasks.Peek().Status != TaskStatus.RanToCompletion)
                    Task.Factory.StartNew(() => tasks.Dequeue()).Wait();
            }
        }
        
        public async Task<IDisposable> LockSection()
        {
            while (tasks.Count > 0)
                await Task.Delay(500);

            lock (lockable)
            {
                tasks.Enqueue(new Task(() => { }));
                return this;
            }
        }

        public void Dispose()
        {
            Release();
        }
        
    }
}
