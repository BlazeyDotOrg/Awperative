using System;
using System.Collections.Generic;
using System.Reflection;
using AwperativeKernel;


namespace Awperative.Kernel.Overhead.Reflection;


internal static class ReflectionManager
{

    /// <summary> List of all modules in Awperative</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static List<IModule> _modules = [];
    
    /// <summary> Resolves all the modules from the calling assembly and module manager</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveModules(Assembly[] __assemblies) {
        foreach (Assembly assembly in __assemblies) ResolveAssembly(assembly);
        
        DependencyManager.ResolveFulfillment();
    }
    
    /// <summary> Resolves all the types in an assembly.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveAssembly(Assembly __assembly) {
        foreach (Type type in __assembly.GetTypes()) {
            DependencyManager.ResolveDependency(type);
            EventManager.CompileType(type);
        }
    }
}