using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AwperativeKernel;


namespace Awperative.Kernel.Overhead.Reflection;


internal static class EventManager
{
    /// <summary> Holds an associated action for each component and a time event. Is built with CompileType() during Start().</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static Dictionary<Type, Action<Component>[]> _TypeAssociatedTimeEvents = [];
    
    
    /// <summary> All types of time based events in Awperative.</summary>
    internal static readonly ImmutableArray<string> Events = ["Load", "Unload", "Update", "Draw", "Create", "Remove"];
    
    
    /// <summary> Compiles a single type, and stores its events in the dictionary.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void CompileType(Type __type) {
        if (!__type.IsSubclassOf(typeof(Component))) return;


        List<Action<Component>> timeEvents = [];

        foreach (MethodInfo eventMethod in Events.Select(__type.GetMethod)) {
            if (eventMethod == null) { timeEvents.Add(null); continue; }

            ParameterExpression ComponentInstanceParameter = Expression.Parameter(typeof(Component), "__component");
            UnaryExpression Casting = Expression.Convert(ComponentInstanceParameter, __type);
            MethodCallExpression Call = Expression.Call(Casting, eventMethod);
            Expression<Action<Component>> Lambda = Expression.Lambda<Action<Component>>(Call, ComponentInstanceParameter);
            timeEvents.Add(Lambda.Compile());
        }

        _TypeAssociatedTimeEvents.Add(__type, timeEvents.ToArray());
    }
}