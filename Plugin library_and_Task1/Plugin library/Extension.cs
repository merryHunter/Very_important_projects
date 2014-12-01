using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MyExtension
{
    ///Allow clone objects.
    public static class ObjectExtension
    {

        public static object Clone(this object item)
        {
            /// If primitive, then it's copying by value.
            if (item.GetType().IsPrimitive)
            {
                object res = item;
                return res;
            }
            /// Else we cannot afford just "copy" object,so we need deep cloning.
            FieldInfo[] fieldInfo = item.GetType().GetFields();
            object obj = Activator.CreateInstance(item.GetType());
            foreach (var f in fieldInfo)
            {
                if (f.FieldType.Namespace != item.GetType().Namespace)
                    f.SetValue(obj, f.GetValue(item));
                else
                {
                    object o = f.GetValue(item);
                    f.SetValue(obj, o.Clone());
                }
            }
            return obj;
        }
    }
}
