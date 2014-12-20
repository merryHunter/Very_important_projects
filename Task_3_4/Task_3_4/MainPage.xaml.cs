using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Task_3_4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
#region Class members

        private static SyncMutex mutex = new SyncMutex();

        /// To counting threads tried capture lock and enter the critical section.
        private static volatile Queue<int> threads = new Queue<int>();

        private static volatile int threadCount = 0;

        /// Buffer(logger) for all actions connected with mutex and threads.
        private static volatile string output = "";

        /// For printing a little bit more info about mutex's actions.
        Stopwatch stopWatch = new Stopwatch();

#endregion

        public MainPage()
        {
            this.InitializeComponent();
            stopWatch.Start();
            output += '\n';
        }

#region Class methods
        
        public async Task LongTimeOperation()
        {
            threads.Enqueue(++threadCount);
            output += threads.Last().ToString() + "-th try Lock at " + String.Format("{0:00}:{1:00}\n",
                                        stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds);
            await mutex.Lock();
                CriticalSection(false); 
            mutex.Release();

            output += threads.Dequeue().ToString() + "-th release at  " + String.Format("{0:00}:{1:00}\n",
                                         stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds);
        }

        public async Task LongTimeOperationUsing()
        {
           threads.Enqueue(++threadCount);
           output += threads.Last().ToString() + "-th try Lock at " + String.Format("{0:00}:{1:00}\n",
                                        stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds);

           using (await mutex.LockSection())
           {
               CriticalSection(false);
           }

           output += threads.Dequeue().ToString() + "-th release at  " + String.Format("{0:00}:{1:00}\n",
                                        stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds);
        }

        private async void OnMutexClicked(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => LongTimeOperation());
            textBox.Text += output;
            output = "";
        }

        private async void onMutexUsingClicked(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => LongTimeOperationUsing());
            textBox.Text += output;
            output = "";
        }

        /// <summary>
        /// Critical code section, mutex must allow only one thread entered.
        /// </summary>
        /// <param name="isSynchronouslyCall">
        /// True if it called by pressing "Critical code start".
        /// It means that code will be executing in the UI thread.
        /// </param>
        private void CriticalSection(bool isSynchronouslyCall)
        {
            if (isSynchronouslyCall)
                output += " critical section starts...\n";
            else output += threads.Peek().ToString() + "critical section starts...\n";

            //Long operation.
            Task.Delay(3000).Wait();

            TimeSpan ts = stopWatch.Elapsed;
            if (isSynchronouslyCall)
                output += " ends at\t " + String.Format("{0:00}:{1:00}\n", ts.Seconds, ts.Milliseconds);
            else output += threads.Peek().ToString() + " ends at\t " + String.Format("{0:00}:{1:00}\n",
                                                               ts.Seconds, ts.Milliseconds );
        }

        private void onParalelBoxClicked(object sender, RoutedEventArgs e)
        {
            paralelBox.Text += "Clicked\n";
        }

        private void onClearBtnClicked(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
            paralelBox.Text = "";
            output = "";
            threadCount = 0;
            threads.Clear();
            stopWatch.Restart();
        }

        private void OnCriticalCodeClicked(object sender, RoutedEventArgs e)
        {
            CriticalSection(true);
            textBox.Text += output;
            output = "";
        }

       
#endregion

    }
}
