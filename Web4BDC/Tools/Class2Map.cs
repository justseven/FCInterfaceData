using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Web4BDC.Tools
{
    public class Class2Map
    {
        /// <summary>
        ///
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IDictionary<String, Object> ToMap(Object o)
        {
            IDictionary<String, Object> map = new Dictionary<string, object>();
            Type t = o.GetType(); 
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            } 
            return map;
        }
        public static IDictionary<string, string> ToMap2(Object o)
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
            Type t = o.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    object value=mi.Invoke(o, new Object[] { });
                    if(value!=null)
                    {
                        map.Add(p.Name, value.ToString());
                    }
                    else
                    map.Add(p.Name, "");
                }
            }
            return map;
        }
    }
}