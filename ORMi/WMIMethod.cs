﻿#if NET40
using ORMi.net40;
#endif

using ORMi.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ORMi
{
    public static class WMIMethod
    {
        /// <summary>
        /// Executes an WMI instance method with no parameter.
        /// </summary>
        /// <param name="obj">Instance which will be instanciated to call the method.</param>
        /// <returns></returns>
        public static dynamic ExecuteMethod(object obj)
        {
            WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

            var mth = new StackTrace().GetFrame(1).GetMethod();
            string methodName = mth.Name;

            ManagementClass genericClass = new ManagementClass(TypeHelper.GetNamespace(obj), TypeHelper.GetClassName(obj), null);

            ManagementObject instance = TypeHelper.GetManagementObject(genericClass, obj);

            ManagementBaseObject result = instance.InvokeMethod(methodName, null, null);

            return TypeHelper.LoadDynamicObject(result);
        }

        /// <summary>
        /// Executes an instance method with parameters. 
        /// </summary>
        /// <param name="obj">Instance which will be instanciated to call the method.</param>
        /// <param name="parameters">Anonymous object with properties matching the parameter names of the method.</param>
        /// <returns></returns>
        public static dynamic ExecuteMethod(object obj, dynamic parameters)
        {
            WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

            var frame = new StackTrace().GetFrames().Skip(1).First(x => x.GetMethod().DeclaringType.Namespace != "System.Dynamic");

            string methodName = frame.GetMethod().Name;

            ManagementClass genericClass = new ManagementClass(TypeHelper.GetNamespace(obj), TypeHelper.GetClassName(obj), null);

            ManagementObject instance = TypeHelper.GetManagementObject(genericClass, obj);

            ManagementBaseObject inParams = genericClass.GetMethodParameters(methodName);

            foreach (PropertyInfo p in parameters.GetType().GetProperties())
            {
                inParams[p.Name] = p.GetValue((object)parameters);
            }

            ManagementBaseObject result = instance.InvokeMethod(methodName, inParams, null);

            return TypeHelper.LoadDynamicObject(result);
        }

        /// <summary>
        /// Executes a static method without parameters.
        /// </summary>
        /// <returns></returns>
        public static dynamic ExecuteStaticMethod()
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            string methodName = mth.Name;

            Type t = mth.ReflectedType;

            ManagementClass cls = new ManagementClass(TypeHelper.GetNamespace(t), TypeHelper.GetClassName(t), null);

            ManagementBaseObject result = cls.InvokeMethod(methodName, null, null);

            return TypeHelper.LoadDynamicObject(result);
        }

        /// <summary>
        /// Executes a static method with parameters.
        /// </summary>
        /// <param name="parameters">Anonymous object with properties matching the WMI method parameters</param>
        /// <returns></returns>
        public static dynamic ExecuteStaticMethod(dynamic parameters)
        {
            var frame = new StackTrace().GetFrames().Skip(2).First(x => x.GetMethod().DeclaringType.Namespace != "System.Dynamic");

            string methodName = frame.GetMethod().Name;

            Type t = frame.GetMethod().ReflectedType;

            ManagementClass cls = new ManagementClass(TypeHelper.GetNamespace(t), TypeHelper.GetClassName(t), null);

            ManagementBaseObject inParams = cls.GetMethodParameters(methodName);

            foreach (PropertyInfo p in parameters.GetType().GetProperties())
            {
                inParams[p.Name] = p.GetValue((object)parameters);
            }

            ManagementBaseObject result = cls.InvokeMethod(methodName, inParams, null);

            return TypeHelper.LoadDynamicObject(result);
        }
    }
}
