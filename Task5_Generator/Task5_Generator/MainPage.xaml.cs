// Created by Ivan Chernuha 29.11.2014.
// chernuhaiv@gmail.com
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Task5_Generator;
using System.Threading.Tasks;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Task5_Generator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private RandomGenerator generator;

        public MainPage()
        {
            this.InitializeComponent();
            generator = new RandomGenerator();
        }
        /// <summary>
        /// Start generate random sequence and write it ot output text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            int N = 1;
            try { 
                N = Int32.Parse(inputBox.Text);
            }
            catch (Exception ex)
            { 
                MessageDialog msg = new MessageDialog("Invalid N parameter!"); 
                msg.ShowAsync();
                return;
            }
            foreach (int i in generator.NextInt(N))
            {
                 outputBox.Text += i.ToString() + "\n";
            }
        }
    }
}
