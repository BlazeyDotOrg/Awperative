using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AwperativeKernel;


namespace Awperative.Kernel.Overhead.Reflection;


internal static class DependencyManager
{
    
    /// <summary> Resolves any needed Awperative dependencies! Given the Type, it searches recursively for all interfaces. If any of the interfaces
    /// Has the RequiredModule() attribute, or one of the interfaces in required modules. It will assign it to the assosiated field in the Awperative class.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveDependency(Type __type) {
        
        Console.WriteLine(__type.FullName);

        foreach (Type typeInterface in __type.GetInterfaces()) {
            //Console.WriteLine("     " + typeInterface.FullName);
            object[] dependencyInfo = typeInterface.GetCustomAttributes(typeof(DependencyAttribute), true);

            if (dependencyInfo.Length == 0) continue;

            foreach (object attribute in dependencyInfo) {
                if (attribute is not DependencyAttributes.RequiredModule moduleAttribute) continue;
                
                PropertyInfo data = typeof(AwperativeKernel.Awperative).GetProperty(moduleAttribute.Source, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (data != null) data.SetValue(null, Activator.CreateInstance(__type));
            }
        }
    }

    /// <summary> Checks all dependency marked variables in Awperative, and ensures that all are present!</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveFulfillment() {

        foreach (PropertyInfo propertyInfo in typeof(AwperativeKernel.Awperative).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
            object[] attributes = propertyInfo.GetCustomAttributes(typeof(DependencyAttribute), true);
            
            if (attributes.Length == 0) continue;
            
            IModule module = propertyInfo.GetValue(null) as IModule;

            if (module == null) throw new Exception("Failed to find dependency! " + propertyInfo.Name);
            
            ReflectionManager._modules.Add(module);
            module.Start();
        }
    }
}