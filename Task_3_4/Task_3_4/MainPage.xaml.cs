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

        /// This array will be filled by random integers and then sorted.
        /// Sorting takes some time so we got 
        /// time-long operation in critical section.
        private int[] arr;

        /// Considering buble-sort complexity, it will be enough.
        private static int N = 20000;
        
        private static volatile Queue<int> threads = new Queue<int>();

        private static volatile int threadCount = 0;

        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            arr = new int[N];
            Random r = new Random();
            for (var i = 0; i < N; ++i)
                arr[i] = r.Next();

        }

        #region Class methods

        private async void OnButtonClicked(object sender, RoutedEventArgs e)
        {
             await SortRandomArrayForFiveSeconds();
        
        }

        private async Task SortRandomArrayForFiveSeconds()
        {
            threads.Enqueue(++threadCount);
            textBox.Text += threads.Last().ToString() + "-th try Lock...\n";

            await mutex.Lock();
                textBox.Text += threads.Peek().ToString() + "-th entered in ";
                CriticalSection();
            mutex.Release();

            textBox.Text += threads.Dequeue().ToString() + "-th released...\n";
        }
          
        private void CriticalSection()
        {
            textBox.Text += "critical section start...";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            /// Bubble-sort.
            int temp = 0;
            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1; sort++)
                {
                    if (arr[sort] > arr[sort + 1])
                    {
                        temp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = temp;
                    }
                }
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            textBox.Text += "...end ";
            // Format and display the TimeSpan value. 
            textBox.Text += String.Format("{0:00}:{1:00}:{2:00}.{3:00}\n",
              ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds );
        }

        private void onParalelBoxClicked(object sender, RoutedEventArgs e)
        {
            paralelBox.Text += "Clicked\n";
        }
        
        /// This method run synchronously!
        private void OnCloneBoxCLicked(object sender, RoutedEventArgs e)
        {
            CriticalSection();
        }
        #endregion
    }
}
