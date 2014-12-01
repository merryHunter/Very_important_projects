using System.Collections.Generic;
using System.Reflection;
using System;
namespace Org.Plugin
{
#region Common Plugin interface
    public interface IPlugin<T>
    {
        T Modify(T param);
    }
#endregion

#region Basic plugins classes
    class AbsoluteIntValuePlugin<T> : IPlugin<int>
    {
        public int Modify(int param)
        {
            if (param >= 0)
                return param;
            else return -param;
        }

    }
    
    class ConvertTimeToUtcPlugin<T> : IPlugin<System.DateTime>
    {
        public System.DateTime Modify(System.DateTime localTime)
        {
            return localTime.ToUniversalTime();
        }
    }
#endregion

#region Collection and Pluginable
    class CollectionPlugin<T> : IPlugin<T>
    {
        private List<IPlugin<T>> plugins;

        public CollectionPlugin()
        {
            this.plugins = new List<IPlugin<T>>();
        }

        public CollectionPlugin(List<IPlugin<T>> plugins)
        {
            this.plugins = new List<IPlugin<T>>(plugins);
        }

        public List<IPlugin<T>> Plugins
        {
            get
            {
                return plugins;
            }
        }

        /// <summary>
        /// ??? ///?????
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public T Modify(T param)
        {
            for (int i = 0; i < plugins.Count; ++i)
                param = plugins[i].Modify(param);
            return param;
        }

        public void AddPlugin(IPlugin<T> plugin) { plugins.Add(plugin); }

        public void RemovePlugin(int index)
        {
            try
            {
                plugins.RemoveAt(index);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                System.Console.WriteLine("Invalid index! ");
            } 
        }

    }

    class StringSpacePlugin<T> : IPlugin<string>
    {
        /// <summary>
        /// Method inserts tab spaces between words 
        /// or other separate constructions contained in the string.
        /// It increase string's readability.
        /// </summary>
        /// <param name="param"></param>
        /// <returns> modified param string </returns>
        public string Modify(string param)
        {
            for (int i = 0; i < param.Length; ++i)
            {
                if (param[i] == ' ')
                    param = param.Insert(i++, "\t");
            }
            return param;
        }
    }

    class CharacterSpacePlugin<T> : IPlugin<string>{
        /// <summary>
        /// Method inserts one space between every character
        /// contained in the string.
        /// It increase string's readability.
        /// </summary>
        /// <param name="param"></param>
        /// <returns> modified param string </returns>
        public string Modify(string param)
        {
            string rezult = "";
            for (int i = 0; i < param.Length; ++i)
            {            
                    rezult += param[i] + " ";
            }
            return rezult;
        }
    }

    /// <summary>
    /// Plugin and pluginable at the same time.
    /// Make string more readable.
    /// </summary>
    class ReadableStringPlugin<T> : IPlugin<string>
    {
        public string Modify(string param)
        {
            return new StringSpacePlugin<string>()
                .Modify(param)
                .ToUpper();
        }
    }
#endregion

#region Handlers
class BasePluginHandler<T>
    {
        protected IPlugin<T> myPlugin;
        protected T myData;
        private CollectionPlugin<string> plugCollection;
        private int dataStr;

        public BasePluginHandler(IPlugin<T> plugin, T data)
        {
            myData = data;
            myPlugin = plugin;
        }

#region Properties
        public IPlugin<T> Plugin
        {
            get
            {
                return myPlugin;
            }
            set
            {
                myPlugin = value;
            }
        }
        public T Data
        {
            get
            {
                return myData;
            }
            set
            {
                myData = value;
            }
        }
#endregion
        virtual public void ModifyData()
        {
            myPlugin.Modify(myData);
        }

        virtual public void PrintModifiedData()
        {
            System.Console.WriteLine(myPlugin.Modify(myData));
        }
    }
#endregion
}
