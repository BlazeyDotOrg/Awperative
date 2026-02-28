using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AwperativeKernel.Attributes;


namespace AwperativeKernel;

/// <summary>
/// Base class for all Awperative Entities. Responsible for Managing hierarchy between Components and Scenes, has Extensive Component Manipulation Available.
/// Also transfers Time and Carries most of the responsibilities akin to the Component.
/// </summary>
/// <remarks> Please don't inherit this. I don't know why you would</remarks>
/// <author> Avery Norris </author>
public abstract partial class ComponentDocker : IEnumerable, IEnumerable<Component>, IEquatable<Component>, IEquatable<ComponentDocker>
{
    
    public bool Equals(Component __other) {
        if (this is Component component) {
            return component == __other;
        } else return false;
    }
    
    public bool Equals(ComponentDocker __other) {
        return this == __other;
    }
    

    //blocks external inheritance
    internal ComponentDocker() {}
    

    IEnumerator IEnumerable.GetEnumerator() {
        return  GetEnumerator();
    }
    
    public IEnumerator<Component> GetEnumerator() {
        Console.WriteLine("enumerator called" + _Components.Count);
        return new ComponentDockEnum([.._Components]);
    }
    
    
    
    /// <summary>
    /// List of all Components belonging to the Docker, Please Use Add, Get, Move and Destroy to modify it.
    /// </summary>
    public ImmutableArray<Component> Components => [.._Components];
    
    
    
    /// <summary>
    /// Amount of all Components in the Docker
    /// </summary>
    public int Count => _Components.Count;
    


    /// <summary>
    /// Core of Docker, contains all of our precious Components. Sorts them by their priorities with highest going first.
    /// If they are equal it defaults to hash codes to ensure consistent Behavior
    /// </summary>
    internal List<Component> _Components = new();
    internal Dictionary<Type, HashSet<Component>> _ComponentDictionary = new();
    internal Dictionary<string, HashSet<Component>> _taggedComponents = new();
    
    
    
    /// <summary>
    /// How Priority is sorted.
    /// </summary>
    private readonly static Comparer<Component> _componentSorter = Comparer<Component>.Create((a, b) => {
        int result = b.Priority.CompareTo(a.Priority);
        return (result != 0) ? result : a.GetHashCode().CompareTo(b.GetHashCode());
    });





    /// <summary>
    /// Resorts member of Component list to match the Priority.
    /// </summary>
    /// <param name="__component"> Component to modify</param>
    /// <param name="__priority"> New priority for Component</param>
    internal void UpdatePriority(Component __component, int __priority) {
        //add ownership enforcement/method
        _Components.Sort(_componentSorter);
        __component._priority = __priority;
    }

    
    //internal void TryEvent(Component __component, Awperative.TimeEvent __timeEvent) => __component.TryEvent(__timeEvent);


    internal void ChainEvent(int __timeEvent) {
        for (int i = 0; i < _Components.Count; i++) {
            _Components[i].TryEvent(__timeEvent);
            _Components[i].ChainEvent(__timeEvent);
        }
    }







    
    








    
    
    
    


    




    
    /// <summary>
    /// Add component to its proper place in the dictionary and resort values to match priorities.
    /// </summary>
    /// <param name="__component"> Component to hash</param>
    /// <param name="__tag"> Value to try and hash</param>
    internal void HashTaggedComponent(Component __component, string __tag) {
        

        if (!__component._tags.Add(__tag)) {
            Debug.LogError("Component already has tag!", ["Component", "Type", "Tag", "Docker"],
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), __tag, GetHashCode().ToString()]); return;
        }

        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components)) {
            components.Add(__component);
            
        } else { _taggedComponents.Add(__tag, new SortedSet<Component>(_componentSorter)); _taggedComponents[__tag].Add(__component); }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="__component"></param>
    /// <param name="__tag"></param>
    internal void UnhashTaggedComponent(Component __component, string __tag) {

        if (!__component._tags.Remove(__tag)) {
            Debug.LogError("Component already doesn't have that tag!", ["Component", "Type", "Tag", "Docker"],
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), __tag, GetHashCode().ToString()]); return;
        }
        
        
        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components)) {
            components.Remove(__component);
            
            if(components.Count == 0)
                _taggedComponents.Remove(__tag);
        }
    }



    

    /// <summary>
    /// Finds the first instance of a component with a given tag
    /// </summary>
    /// <param name="__tag"> Tag to search for</param>
    /// <returns></returns>
    internal Component Get(string __tag) {
        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components))
            return ((Component[])[..components])[0];

        return null;
    }

    
    
    /// <summary>
    /// Finds the first instance of a component with a given tag
    /// </summary>
    /// <param name="__tag"> Tag to search for</param>
    /// <param name="__component">Component that has been found</param>
    /// <returns></returns>
    internal bool TryGet(string __tag, out Component __component) { __component = Get(__tag); return __component != null; }

    

    /// <summary>
    /// Finds all Components with a given tag
    /// </summary>
    /// <param name="__tag"></param>
    /// <returns></returns>
    internal ImmutableArray<Component> GetAll(string __tag) {
        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components))
            return [..components];

        return [];
    }


    
    /// <summary>
    /// Searches for all Components with a given tag
    /// </summary>
    /// <param name="__tag"></param>
    /// <param name="__components"></param>
    /// <returns></returns>
    internal bool TryGetAll(string __tag, out ImmutableArray<Component> __components) { __components = GetAll(__tag); return __components.Length > 0; }



    /// <summary>
    /// Finds the first Component that has all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    internal Component Get(List<string> __tags) { ImmutableArray<Component> returnValue = GetAll(__tags); return returnValue.Length > 0 ? returnValue[0] : null; }



    /// <summary>
    /// Finds the first Component that has all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    internal bool TryGet(List<string> __tags, out Component __component) { __component = Get(__tags); return __component != null; }



    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    internal ImmutableArray<Component> GetAll(List<string> __tags) {

        if (__tags.Count == 0)
            return [];
        
        SortedSet<Component> foundComponents = _taggedComponents[__tags[0]];

        for (int i = 1; i < __tags.Count; i++) {
            foreach (Component component in (Component[])[..foundComponents]) {
                if (!_taggedComponents[__tags[i]].Contains(component)) foundComponents.Remove(component);
            }
        }

        return [..foundComponents];
    }



    /// <summary>
    /// Tries to get all components with the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <param name="__components"></param>
    /// <returns></returns>
    internal bool TryGetAll(List<string> __tags, out ImmutableArray<Component> __components) { __components = GetAll(__tags); return __components.Length > 0; }
    
    
    
    

    /// <summary>
    /// Searches and returns the first Component of a type found on the Docker.
    /// </summary>
    /// <typeparam name="__Type"> The Type of Component to search for</typeparam>
    /// <returns></returns>
    public __Type Get<__Type>() where __Type : Component {
        
        
        
        //Iterates through the loop and returns if a match is found
        foreach (Component component in (Component[])[.._Components])
            if (component is __Type foundComponent) return foundComponent;

        
        
        //Throws error if there is no Component found
        Debug.LogError("Docker does not have target Component", ["Type", "Docker"], 
            [typeof(__Type).ToString(), GetHashCode().ToString()]); return null;
    }


    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="__component"></param>
    /// <typeparam name="__Type"></typeparam>
    /// <returns></returns>
    public bool TryGet<__Type>(out __Type __component) where __Type : Component { __component = Get<__Type>(); return __component != null; }
    
    
    
    
    
    /// <summary>
    /// Searches and returns all Components of a type found on the Docker.
    /// </summary>
    /// <typeparam name="__Type"> The Type of Components to search for</typeparam>
    /// <returns></returns>
    public ImmutableArray<__Type> GetAll<__Type>() where __Type : Component {


        
        List<__Type> foundComponents = [];

        
        //Iterates through the loop and returns if a match is found
        foreach (Component component in (Component[])[.._Components]) {
            if (component is __Type foundComponent) {
                foundComponents.Add(foundComponent);
            }
        }

        
        //Throws error if there is no Component found
        if (foundComponents.Count == 0) {
            Debug.LogError("Docker does not have target Component", ["Type", "Docker"],
                [typeof(__Type).ToString(), GetHashCode().ToString()]);
            return [];
        }

        
        
        return [..foundComponents];
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="__components"></param>
    /// <typeparam name="__Type"></typeparam>
    /// <returns></returns>
    public bool TryGetAll<__Type>(out ImmutableArray<__Type> __components) where __Type : Component { __components = GetAll<__Type>(); return __components.Length > 0; }
    
    
    
    
    
    


    private void AddComponentToLists(Component __component) {
        var Type = __component.GetType();
        _Components.Add(__component); 
        if (!_ComponentDictionary.TryAdd(Type, [__component])) _ComponentDictionary[Type].Add(__component);
        
        for(var i = 0; i < __component.Tags.Length; i++) { HashTaggedComponent(__component.Tags[i], __component); }
    }

    private void RemoveComponentFromLists(Component __component) {
        var Type = __component.GetType();
        _Components.Remove(__component); 
        
        if(!_ComponentDictionary.ContainsKey(Type)) _ComponentDictionary[Type].Remove(__component);
        
        for(var i = 0; i < __component.Tags.Length; i++) { UnhashTaggedComponent(__component.Tags[i], __component); }
    }

    private void HashTaggedComponent(string __tag, Component __component) {
        if (!_taggedComponents.TryAdd(__component.Tags[i], [__component])) 
            _taggedComponents[__component.Tags[i]].Add(__component);
    }

    private void UnhashTaggedComponent(string __tag, Component __component) {
        if(!_taggedComponents.ContainsKey(__tag)) _taggedComponents[__tag].Remove(__component);
    }

}