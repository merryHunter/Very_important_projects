using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Task2;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Task2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Class membres

        MyTask myTask = new MyTask();

        Action taskAction =  () =>
            {
                MessageDialog msgDialog = new MessageDialog("Your message", "Your title"); ;
                msgDialog.ShowAsync();
            };
        //For initializing
        Func<string> funcParam = ()=>{ return ""; } ;

        #endregion

        public MainPage()
        {
            this.InitializeComponent();

        }

        #region Class methods

        #region Click handlers

        private void OnActionClickd(object sender, RoutedEventArgs e)
        {
            Task actionParam = new Task(taskAction);
            Action<Task> action = DisplayMessage;
            Task t = myTask.Continue(action, actionParam);
            Task.Factory.StartNew(() => t);
            MessageDialog msg = new MessageDialog("Action", "Thread ID: " +
                Environment.CurrentManagedThreadId.ToString()); 
            msg.ShowAsync();
        }
        
        private void OnFuncClicked(object sender, RoutedEventArgs e)
        {
            Func<Task<string>> funcTask = DisplayMessageTask;
            MyTaskGeneric<string> myGeneric = new MyTaskGeneric<string>(funcParam);
            string res = myGeneric.Continue(funcTask).Result;
            DisplayMessage(res);
        }

        private void OnFuncTaskTClicked(object sender, RoutedEventArgs e)
        {
            Task<string> taskParam = new Task<string>(funcParam);
            Func<Task, string> funcTask = DisplayMessageTaskResult;
            MyTaskGeneric<string> myGeneric = new MyTaskGeneric<string>(funcParam);
            string res =  myGeneric.Continue(funcTask).Result;
            DisplayMessage(res);
        }

        private void OnFuncTaskTaskTClicked(object sender, RoutedEventArgs e)
        {
            Action act = () =>
            {
                MessageDialog msgD = new MessageDialog("Display message TaskTask so on.",
                    "Thread ID: " + Environment.CurrentManagedThreadId.ToString());
                msgD.ShowAsync();
            };
            Task taskParam = new Task(act);
            Func<Task, Task<string>> funcTask = DisplayMessageTaskTaskT;
            MyTaskGeneric<string> myGeneric = new MyTaskGeneric<string>(funcParam);
            Task<string> res = myGeneric.Continue(funcTask);
            DisplayMessage(res.Result);
        }
        #endregion

        #region Displaying messages methods

        private void DisplayMessage(Task task)
        {
            MessageDialog msg = new MessageDialog(task.ToString(), "Thread ID: " +
                 Environment.CurrentManagedThreadId.ToString());
            msg.ShowAsync();
        }

        private Task<string> DisplayMessageTask()
        {
            return Task<string>.Factory.StartNew(() =>
            {
                MessageDialog msg = new MessageDialog("Display message Task", "Thread ID: " +
                    Environment.CurrentManagedThreadId.ToString());
                msg.ShowAsync();
                return "Excellent Func.";
            });
        }

        private void DisplayMessage(string message)
        {
            MessageDialog msg = new MessageDialog(message, "Thread ID: " +
                Environment.CurrentManagedThreadId.ToString());
            msg.ShowAsync();
        }

        private string DisplayMessageTaskResult(Task arg)
        {
            MessageDialog msg = new MessageDialog("Task from func.", "Thread ID: " +
                Environment.CurrentManagedThreadId.ToString());
            msg.ShowAsync();
            return "DisplayMessageTaskResult";
        }

        private Task<string> DisplayMessageTaskTaskT(Task arg)
        {
            MessageDialog msg = new MessageDialog("Task from func.", "Thread ID: " +
                Environment.CurrentManagedThreadId.ToString());
            msg.ShowAsync();
            return Task<string>.Factory.StartNew(() =>
            {
                MessageDialog msgD = new MessageDialog("Display message Func< Task, Task<string>>",
                    "Thread ID: " + Environment.CurrentManagedThreadId.ToString());
                msgD.ShowAsync();
                return "Excellent Func<Task, Task<string>>.";
            });
        } 
#endregion

        #endregion
    }
}
