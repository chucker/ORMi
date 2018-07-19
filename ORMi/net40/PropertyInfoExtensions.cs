using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ORMi.net40
{
    public static class PropertyInfoExtensions
    {
        public static object GetValue(this PropertyInfo propertyInfo, object obj)
            => propertyInfo.GetValue(obj, null);
    }
}
