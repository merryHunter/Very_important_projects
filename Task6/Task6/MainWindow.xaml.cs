using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using MyAssembly.Collection;
namespace Task6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Assembly loadedByUser;
        Type[] types = {};
        AssemblyCollection assemblies = new AssemblyCollection();
        public MainWindow()
        {
            InitializeComponent();
            init();
          
        }
        /// <summary>
        /// Start background assembly scanning in another thread.
        /// </summary>
        private async void init()
        {
            await Task.Factory.StartNew(() => assemblies.BackgroundAssemblyScanning());
        }
        /// <summary>
        /// Load assembly from application current directory(/bin/debug).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Assembly_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(
                                Assembly.GetExecutingAssembly().Location);

                foreach (string dll in Directory.GetFiles(path, "*.dll"))
                    loadedByUser = Assembly.LoadFile(dll);
                types = loadedByUser.GetTypes();
            }
            catch (Exception exc)
            {
                MessageBoxResult warning = MessageBox.Show("Could not load dll \n" +
                                "Try change access properties of dll at Debug directory");   
            }
        }
        /// <summary>
        /// Print to the textBox all executing assemblies. 
        /// So you can see changes, when some other assemblies loaded.
        /// Also it shows any created objects from stored default ctor in dictionary. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Demonstrate_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text +=  assemblies.GetExecutingAssemblyNames();
            textBox.Text += '\n';
            List<Object> instances = new List<Object>();
            foreach (var t in types)
            {
                try
                {
                    instances.Add(assemblies.Create(t));
                }
                catch (Exception exc) 
                {
                    continue; 
                }
            }
            foreach(var t in instances)
                textBox.Text += '\n' + t.ToString();
        }


    }
}
