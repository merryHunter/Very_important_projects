//Created by Ivan Chernuha 27.11.14
//chernuhaiv@gmail.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Task2
{
    class MyTask 
    {
              
        private Task last;
        
        public MyTask()
        {
            last = new Task(() => { });
            Task.Factory.StartNew(() => last);
        }

        #region Class methods

        public Task Continue(Action action)
        {
            last = Task.Factory.StartNew(() => action, 
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            return last;
        }

        public Task Continue(Action<Task> action, Task antecedent)
        {
            last = Task.Factory.StartNew(() => action(antecedent),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            return last;
        }
        #endregion
    }

    class MyTaskGeneric<T>
    {
        private Task<T> lastGeneric;

        public MyTaskGeneric(Func<T> func)
        {

            lastGeneric = new Task<T>(() => func() );
            Task.Factory.StartNew(() => lastGeneric);
        }

        #region Class methods

        public Task<T> Continue(Func<T> func)
        {
            lastGeneric = Task<T>.Factory.StartNew(() => func(),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            return lastGeneric;
        }

        public Task<T> Continue(Func<Task<T>> func)
        {
             lastGeneric = Task<T>.Factory.StartNew(() => func().Result, 
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            return lastGeneric;
        }

        public Task<T> Continue(Func<Task, T> func)
        {
            lastGeneric = Task<T>.Factory.StartNew(() => func(lastGeneric),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            return lastGeneric;
        }

        public Task<T> Continue(Func<Task, Task<T>> func)
        {
            lastGeneric = Task<T>.Factory.StartNew(() => func(lastGeneric).Result,
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
            return lastGeneric;
        }
        #endregion
    }
}
