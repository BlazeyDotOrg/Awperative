using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace AwperativeKernel;


public abstract partial class ComponentDocker
{
       
    
    /// <summary> Tells you whether the docker contains a component with the given tag </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains([DebugAttributes.NotNull] string __tag) => DebugAttributes.NotNull.VerifyOrThrow(__tag) && _componentTagDictionary.ContainsKey(__tag);


    /// <summary> Tells you whether the docker contains a component with the given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains<__Type>() where __Type : Component => _componentTypeDictionary.ContainsKey(typeof(__Type));
    
    /// <summary> Tells you whether the docker contains a component with a given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains<__Type>([DebugAttributes.NotNull] string __tag) where __Type : Component => DebugAttributes.NotNull.VerifyOrThrow(__tag) && GetAll<__Type>(__tag).Any();
    
    /// <summary> Tells you whether the docker contains a component with all the given tags </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags) => GetAll(__tags).Any();
    
    /// <summary> Tells you whether the docker contains a component with all the given tags and the type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains<__Type>([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags) where __Type : Component => GetAll<__Type>(__tags).Any();
    
    /// <summary> Tells you whether the docker contains the given component.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Contains([DebugAttributes.ComponentNotNull] Component __component) => DebugAttributes.NotNull.VerifyOrThrow(__component) && _componentTypeDictionary.TryGetValue(__component.GetType(), out var components) && components.Contains(__component);

    /// <summary> Tells you whether the docker contains the all the given components.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool ContainsAll([DebugAttributes.EnumerableNotNull] IEnumerable<Component> __components) => __components.All(x => _components.Contains(x));
    


    /// <summary> Gets all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)] 
    public IEnumerable<Component> GetAll() => _components;

    /// <summary> Finds the first component with a given Type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public __Type Get<__Type>() where __Type : Component => _componentTypeDictionary.TryGetValue(typeof(__Type), out HashSet<Component> Components) ? (__Type)Components.FirstOrDefault() : null;

    /// <summary> Tries to find the first component with a given tag, and returns false if there is none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGet<__Type>(out __Type __component) where __Type : Component { __component = Get<__Type>(); return __component != null; }



    /// <summary> Gets all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public IEnumerable<__Type> GetAll<__Type>() where __Type : Component => _componentTypeDictionary.TryGetValue(typeof(__Type), out HashSet<Component> components) ? components.OfType<__Type>().ToList() : [];

    /// <summary> Tries to get all components of a given type and returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGetAll<__Type>(out IEnumerable<__Type> __components) where __Type : Component { __components = GetAll<__Type>(); return __components.Any(); }
    
    
    
    /// <summary> Finds all components that have all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public IEnumerable<Component> GetAll([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags) {
        if (!DebugAttributes.EnumerableNotNull.VerifyOrThrow(__tags)) return [];
        
        HashSet<Component> components;
        if (_componentTagDictionary.TryGetValue(__tags.First(), out var firstComponents)) components = firstComponents; else return [];
        
        foreach(var tag in __tags)
            if (_componentTagDictionary.TryGetValue(tag, out var taggedComponents))
                components.RemoveWhere(x => !taggedComponents.Contains(x));

        return components;
    }
    
    /// <summary> Tries to find all components that have all the given tags, returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGetAll([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags, out IEnumerable<Component> __components) 
    { __components = GetAll(__tags); return __components.Any(); }
    
    
    
    
    
    /// <summary> Finds all Components that have the given type, and all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public IEnumerable<__Type> GetAll<__Type>([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags) where __Type : Component {

        if (!__tags.Any())
            return [];
        
        HashSet<__Type> components = [];

        if (_componentTagDictionary.TryGetValue(__tags.First(), out var firstComponents)) 
            foreach (var component in firstComponents) if (component is __Type typedComponent) components.Add(typedComponent);
        
        foreach(string tag in __tags)
            if (_componentTagDictionary.TryGetValue(tag, out var taggedComponents)) 
                components.RemoveWhere(x => !taggedComponents.Contains(x as Component));

        return components.ToList();
    }
    
    /// <summary> Tries to find all the components that have the given tags and type, returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGetAll<__Type>([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags, out IEnumerable<__Type> __components) where __Type : Component 
    { __components = GetAll<__Type>(__tags); return __components.Any(); }
    
    
    
    
    
    /// <summary> Gets all the components with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public IEnumerable<Component> GetAll(string __tag) => GetAll([__tag]);
    
    /// <summary> Tries to get all the components with the given tag, returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGetAll([DebugAttributes.NotNull] string __tag, out IEnumerable<Component> __components) { __components = GetAll(__tag); return __components.Any(); }
    
    
    
    /// <summary> Gets all the components that have a certain type, and a certain tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public IEnumerable<__Type> GetAll<__Type>([DebugAttributes.NotNull] string __tag) where __Type : Component => GetAll<__Type>([__tag]);
    
    /// <summary> Tries to get all the components with a certain tag, and a type. Returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGetAll<__Type>([DebugAttributes.NotNull] string __tag, out IEnumerable<__Type> __components) where __Type : Component { __components = GetAll<__Type>(__tag); return __components.Any(); }

    
    
    /// <summary> Gets the first component with all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public Component Get([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags) => GetAll(__tags).FirstOrDefault();
    
    /// <summary> Tries to get the first component with all the given tags. Returns false if there are none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGet([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags, out Component __component) { __component = Get(__tags); return __component != null; }
    
    

    /// <summary> Finds the first component with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public Component Get([DebugAttributes.NotNull] string __tag) => GetAll([__tag]).FirstOrDefault();
    
    /// <summary> Tries to find the first component with the given tag, returns false if there is none</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool TryGet([DebugAttributes.NotNull] string __tag, out Component __component) { __component = Get(__tag); return __component != null; }


    
    /// <summary> Gets the first component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public __Type Get<__Type>([DebugAttributes.NotNull] string __tag) where __Type : Component => GetAll<__Type>(__tag).FirstOrDefault();

    /// <summary> Tries to get the first component with the given type and tag, returns false if there is none.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public bool TryGet<__Type>([DebugAttributes.NotNull] string __tag, out __Type __component) where __Type : Component { __component = Get<__Type>(__tag); return __component != null; }
    
    
    
}