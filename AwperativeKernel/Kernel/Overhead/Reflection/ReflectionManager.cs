using System;
using System.Collections.Generic;
using System.Reflection;
using AwperativeKernel;


namespace Awperative.Kernel.Overhead.Reflection;


/// <summary>
/// Manages all Awperative reflection based activities, right now limited to registering events.
/// </summary>
/// <author> Avery Norris </author>
internal static class ReflectionManager
{
    /// <summary> Resolves all the modules from the calling assembly and module manager</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveModules(Assembly[] __assemblies) {
        foreach (Assembly assembly in __assemblies) ResolveAssembly(assembly);
    }
    
    /// <summary> Resolves all the types in an assembly.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveAssembly(Assembly __assembly) {
        foreach (Type type in __assembly.GetTypes()) {
            EventManager.CompileType(type);
        }
    }
}