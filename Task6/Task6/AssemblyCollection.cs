//Created by Ivan Chernuha 27.11.14
//chernuhaiv@gmail.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.Security.Policy;
using System.IO;
namespace MyAssembly.Collection
{
    class AssemblyCollection
    {
		#region Class members	
	
        /// Current executing assembly in AppDomain.
        private static volatile List<Assembly> executingAssemblies = new List<Assembly>();

        private static Dictionary<Type, ConstructorInfo>
            assemblyDictionary = new Dictionary<Type,ConstructorInfo>();

        private static AppDomain currentDomain = AppDomain.CurrentDomain;

        /// Updating period for scanning is 20 seconds.
        private static int period = 20000;
		
        /// While programme running, background scanning continue.
        private bool isProgrammeRunning = true;

		#endregion
		
		#region Class methods
		
        public void AddLoadedAssemblies()
        {
            Assembly[] currentAssembly = currentDomain.GetAssemblies();
            foreach (var assembly in currentAssembly)
            {
                /// Search every assembly in executingAssemblies
                /// if some assembly have not been added
                /// (because it become changed or it's new), then add.
                if (!IsLoadedAssembly(assembly))
                {
                    executingAssemblies.Add(assembly);
                    AddToDictionary(assembly);
                }
            }
            
        }

        private bool IsLoadedAssembly(Assembly assembly) 
        {
            foreach (var a in executingAssemblies)
                if (a == assembly)
                    return true;
                
            return false;
        }

        private void AddToDictionary(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (var t in types)
            {
                try
                {
                    /// Ignore if it has no default ctor.
                    if( t.GetConstructor(Type.EmptyTypes) == null)
                        continue;
                    assemblyDictionary.Add(t, t.GetConstructor(Type.EmptyTypes));
                }
                catch (Exception ex) { }
            }
        }

        public Object Create(Type t)
        {
                ConstructorInfo ctor;
                assemblyDictionary.TryGetValue(t, out ctor);
                Type paramType = t.GetType();

                /// According to logic, we need to restore only 
                /// objects with default ctor, but all of them parameterless.
                Object[] parameters = new Object[0];
                return ctor.Invoke(parameters);
        }
       /// Scan periodically if any assemblies has been loaded.
        public async Task BackgroundAssemblyScanning()
        {
            /// Scan all time.
            while (isProgrammeRunning)
            {
                 AddLoadedAssemblies();
                 await Task.Delay(period);
            }
        }
        /// <summary>
        /// Concatanate assemblies names in one string.
        /// </summary>
        /// <returns>Presentable form of all executing assemblies</returns>
        public string GetExecutingAssemblyNames()
        {
            string result = "";
            int counter = 0;
            foreach (var a in executingAssemblies)
                result += (++counter).ToString() + "\n" + a.GetName().ToString() + "\n";

            return result;
        }
        
		#endregion
    }
}
