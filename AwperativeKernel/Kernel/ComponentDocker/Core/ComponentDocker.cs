using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AwperativeKernel;

/// <summary>
/// Base class for all Awperative objects. Responsible for Managing hierarchy between Components and Scenes, has Extensive Component Manipulation Available.
/// Also transfers Time and Carries most of the responsibilities akin to the Component.
/// </summary>
/// <remarks> Please don't inherit this. I don't know why you would</remarks>
/// <author> Avery Norris </author>
public abstract partial class ComponentDocker : IEnumerable, IEnumerable<Component>, IEquatable<Component>, IEquatable<ComponentDocker>, IEquatable<Scene>
{
    
    //Blocks external inheritance
    internal ComponentDocker() {}
    

    
    /// <summary> Core of the Docker, holds all of the Components, sorted by update priority.</summary>
    [UnsafeInternal] internal readonly List<Component> _components = [];
    /// <summary> Holds a list of Components at each of their types. This optimizes Get&lt;Type&gt; to O(1) </summary>
    [UnsafeInternal] internal readonly Dictionary<Type, HashSet<Component>> _componentTypeDictionary = new();
    /// <summary> Stores a Component in a list at each of their tags. This optimizes Get(string tag) to O(1)</summary>
    [UnsafeInternal] internal readonly Dictionary<string, HashSet<Component>> _componentTagDictionary = new();
    
    
    
    //Indexers to allow juicy speedy: for loops
    public Component this[[ValueFitsRange] int __index] {
        get => !ValueFitsRange.VerifyOrThrow(__index, 0, _components.Count) ? null : _components[__index];
        set { if (!ValueFitsRange.VerifyOrThrow(__index, 0, _components.Count)) return; _components[__index] = value; }
    }

    //Allows uint indexing, which makes me mad because nobody ever does those. To convert it over we just mask the largest bit, and cast to integer. That way
    //no sign flipping buisness happens.
    public Component this[uint __index] => this[(int) (__index & 0x7FFFFFFF)];
    
    
    //Enumerators for convenient foreach loops
    IEnumerator IEnumerable.GetEnumerator() { return  GetEnumerator(); } 
    public IEnumerator<Component> GetEnumerator() { return new ComponentDockEnum([.._components]); }
    
    
    /// <summary>Amount of Components attached to the Docker</summary>
    public int Count => _components.Count;
    
    
    
    /// <summary>Compares the Docker to another Scene.</summary>
    public bool Equals(Scene __other) {
        if (this is Scene scene)
            return scene == __other;
        return false;
    }
    
    /// <summary>Compares the Docker to another Component.</summary>
    public bool Equals(Component __other) {
        if (this is Component component)
            return component == __other;
        return false;
    }
    
    /// <summary>Compares the Docker to another Docker.</summary>
    public bool Equals(ComponentDocker __other) {
        return this == __other;
    }





    /// <summary>
    /// Resorts member of Component list to match the Priority.
    /// </summary>
    /// <param name="__component"> Component to modify</param>
    /// <param name="__priority"> New priority for Component</param>
    [UnsafeInternal]
    internal void UpdatePriority(Component __component, int __priority) {
        _components.Sort(Awperative._prioritySorter);
    }
    
    /// <summary>
    /// Sends an event to all Children and tells them to continue it.
    /// </summary>
    /// <param name="__timeEvent"> Type of event to send</param>
    [UnsafeInternal]
    internal void ChainEvent(int __timeEvent) {
        for (int i = 0; i < _components.Count; i++) {
            _components[i].TryEvent(__timeEvent);
            _components[i].ChainEvent(__timeEvent);
        }
    }
    
    /// <summary>
    /// Add a Component into the lists and dictionaries.
    /// </summary>
    /// <param name="__component"></param>
    [UnsafeInternal]
    private void AddComponentToLists(Component __component) {
        var Type = __component.GetType();
        _components.Add(__component); 
        if (!_componentTypeDictionary.TryAdd(Type, [__component])) _componentTypeDictionary[Type].Add(__component);

        foreach (var tag in __component._tags) AddTagToComponent(tag, __component);
    }

    /// <summary>
    /// Removes a Component from the lists and dictionaries.
    /// </summary>
    /// <param name="__component"></param>
    [UnsafeInternal]
    private void RemoveComponentFromLists(Component __component) {
        var Type = __component.GetType();
        _components.Remove(__component); 
        
        if(_componentTypeDictionary.TryGetValue(Type, out var value)) value.Remove(__component);
        
        foreach (var tag in __component._tags) RemoveTagFromComponent(tag, __component);
    }

    /// <summary>
    /// Hashes a Component in the tag dictionary
    /// </summary>
    /// <param name="__tag">Tag to add</param>
    /// <param name="__component">Component to add it to</param>
    [UnsafeInternal]
    internal void AddTagToComponent(string __tag, Component __component) {
        if (!_componentTagDictionary.TryAdd(__tag, [__component])) 
            _componentTagDictionary[__tag].Add(__component);
    }

    /// <summary>
    /// Unhashes a Component from the tag dictionary
    /// </summary>
    /// <param name="__tag">Tag to remove</param>
    /// <param name="__component">Component to remove it from</param>
    [UnsafeInternal]
    internal void RemoveTagFromComponent(string __tag, Component __component) {
        if(!_componentTagDictionary.ContainsKey(__tag)) _componentTagDictionary[__tag].Remove(__component);
    }
    
    
    
    /// <summary>
    /// All children belonging to the Component.
    /// </summary>
    [CalculatedProperty, CalculatedPropertyExpense("Very Low")]
    public IReadOnlyList<Component> Children => _components;
    
    
    
    /// <summary>
    /// All children and their children until the bottom of the scene. Uses Breadth First Search.
    /// </summary>
    [CalculatedProperty, CalculatedPropertyExpense("Medium O(Children)")]
    public IReadOnlyList<Component> AllChildren => GetAllChildren();
    public IReadOnlyList<Component> GetAllChildren() {
        List<Component> targets = [.._components];
        for (int i = 0; i < targets.Count; i++) targets.InsertRange(i + 1, targets[i]._components);
        return [..targets];
    }
}