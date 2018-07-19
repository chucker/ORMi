using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.net40
{
    public static class MemberInfoExtensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute
            => (T)Attribute.GetCustomAttribute(element, typeof(T));

        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit) where T : Attribute
            => (T)Attribute.GetCustomAttribute(element, typeof(T), inherit);
    }
}
