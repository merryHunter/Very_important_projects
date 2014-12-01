// Created by Ivan Chernuha 24.11.2014.
// chernuhaiv@gmail.com

using System;
using System.Collections.Generic;
using Org.Plugin;
using System.Threading;
using MyExtension;
namespace Plugins

{
    class Program
    {
        static void Main(string[] args)
        {
            InteractiveMenu();
        }
        private static void InteractiveMenu()
        {
            OptionInfo();
            var pressedKey = 0;
            while(pressedKey != 7){
                Console.WriteLine("Press a number");
                pressedKey = Convert.ToInt32(Console.ReadLine());
                switch (pressedKey)
                {
                    case 1: 
                        Console.WriteLine("Press an integer.");
                        var dataInt = Convert.ToInt32(Console.ReadLine());
                        var plugInt = new AbsoluteIntValuePlugin<int>();
                        var intHandler = new BasePluginHandler<int>(plugInt, dataInt);
                        
                        Console.WriteLine("Absolute value:");
                        intHandler.PrintModifiedData();
                        Console.WriteLine("Properties: \n{0}\n{1}", 
                            intHandler.Data, intHandler.Plugin);
                        break;
                    case 2:
                        var plugDate = new ConvertTimeToUtcPlugin<DateTime>();
                        var dataDate = DateTime.Now;
                        var dateHandler = new BasePluginHandler<DateTime>(plugDate,dataDate);
                        Console.WriteLine("Current loval time is \n{0}", dateHandler.Data);
                        Console.WriteLine("Current UTC is");
                        dateHandler.PrintModifiedData();
                        Console.WriteLine("Properties: \n{0}\n{1}",
                                 dateHandler.Data, dateHandler.Plugin);
                        break;
                    case 3:
                        Console.WriteLine("Press your string. It must contain some spaces.");
                        var dataStr = Convert.ToString(Console.ReadLine());
                        var plugins = new List<IPlugin<string>>();
                        plugins.Add(new StringSpacePlugin<string>());
                        plugins.Add(new CharacterSpacePlugin<string>());
                        var plugCollection = new CollectionPlugin<string>(plugins);
                        var collectionHandler = new BasePluginHandler<string>(plugCollection, dataStr);
                        collectionHandler.PrintModifiedData();
                        Console.WriteLine("Properties: \n{0}\n{1}", 
                            collectionHandler.Data, collectionHandler.Plugin);
                        break;
                    case 4:
                        Console.WriteLine("Press your string. It must contain some spaces.");
                        var dataPluginableStr = Convert.ToString(Console.ReadLine());
                        var plug = new ReadableStringPlugin<string>();
                        var readableHandler = new BasePluginHandler<string>(plug, dataPluginableStr);
                        readableHandler.PrintModifiedData();
                        Console.WriteLine("Properties: \n{0}\n{1}",
                                    readableHandler.Data, readableHandler.Plugin);
                        break;
                    case 5: 
                        Console.Clear();
                        OptionInfo();
                        break;
                    case 6:
                        Console.WriteLine("Clone object.");
                        Mutex mymutex = new Mutex();
                        var v = ObjectExtension.Clone(mymutex);
                        Console.WriteLine(mymutex.ToString()+'\n');
                        Console.WriteLine(v.ToString()+'\n');
                        break;
                    case 7:
                        break;
                    default: Console.WriteLine("Invalid number. Try again!");
                            break;
                }
            }
        }

        private static void OptionInfo()
        {
            Console.WriteLine("Hello! This programm demonstrate my plugin's oppotunities.");
            Console.WriteLine("Please, press a number of statement:");
            Console.WriteLine("1. Get absolute value of integer.");
            Console.WriteLine("2. Get local time in UTC represantation.");
            Console.WriteLine("3. Modify your string by several plugins using \n" +
                "plugin's collection.");
            Console.WriteLine("4. Modify data by pluginable plugin.");
            Console.WriteLine("5. Clear screen");
            Console.WriteLine("6. Exit");
        }
    }
}
