

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;


namespace AwperativeKernel;


public abstract partial class ComponentDocker
{
       
    
    /// <summary>
    /// Returns a bool based on if the Docker contains a Component with the given tag or not
    /// </summary>
    /// <param name="__tag"></param>
    /// <returns></returns>
    public bool Contains([NotNull] string __tag) 
        => NotNull.VerifyOrThrow(__tag) && _componentTagDictionary.ContainsKey(__tag);


    /// <summary>
    /// Returns a bool for if the Docker contains a Component of that type or not
    /// </summary>
    /// <typeparam name="__Type">Type of the Component</typeparam>
    public bool Contains<__Type>() where __Type : Component 
        => _componentTypeDictionary.ContainsKey(typeof(__Type));
    
    
    
    /// <summary>
    /// Returns a bool based on if the current __Component is owned by this Docker
    /// </summary>
    /// <param name="__component"></param>
    /// <returns></returns>
    public bool Contains([ComponentNotNull,DockerOwns] Component __component) 
        => NotNull.VerifyOrThrow(__component) && DockerOwns.VerifyOrThrow(this, __component) && _components.Contains(__component);
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool Contains<__Type>(string __tag) {
        return GetAll<__Type>(__tag).Any();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool Contains(IList<string> __tags) { 
        return GetAll(__tags).Any();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool Contains(IEnumerable<string> __tags) {
        return GetAll(__tags).Any();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool Contains<__Type>(IList<string> __tags) {
        return GetAll<__Type>(__tags).Any();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool Contains<__Type>(IEnumerable<string> __tags) {
        return GetAll<__Type>(__tags).Any();
    }
    
    



    /// <summary>
    /// Searches and returns the first Component of a type found on the Docker.
    /// </summary>
    /// <typeparam name="__Type"> The Type of Component to search for</typeparam>
    /// <returns></returns>
    public __Type Get<__Type>() where __Type : Component {
        return _componentTypeDictionary.TryGetValue(typeof(__Type), out var Components) ? (__Type)Components.FirstOrDefault() : null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="__component"></param>
    /// <typeparam name="__Type"></typeparam>
    /// <returns></returns>
    public bool TryGet<__Type>(out __Type __component) where __Type : Component {
        __component = Get<__Type>(); return __component != null;
    }



    /// <summary>
    /// Searches and returns all Components of a type found on the Docker.
    /// </summary>
    /// <typeparam name="__Type"> The Type of Components to search for</typeparam>
    /// <returns></returns>
    public IReadOnlyList<__Type> GetAll<__Type>() where __Type : Component {
        return (IReadOnlyList<__Type>)_componentTypeDictionary.GetValueOrDefault(typeof(__Type)).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="__components"></param>
    /// <typeparam name="__Type"></typeparam>
    /// <returns></returns>
    public bool TryGetAll<__Type>(out IReadOnlyList<__Type> __components) where __Type : Component {
        __components = GetAll<__Type>(); return __components.Any();
    }
    
    
    
    
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public IReadOnlyList<Component> GetAll(IList<string> __tags) {

        if (__tags.Count == 0)
            return [];
        
        HashSet<Component> components;

        if (_componentTagDictionary.TryGetValue(__tags[0], out var firstComponents)) components = firstComponents; else return [];
        
        for (var i = 1; i < __tags.Count; i++)
            if (_componentTagDictionary.TryGetValue(__tags[i], out var taggedComponents)) 
                foreach (var component in components) if (!taggedComponents.Contains(component)) components.Remove(component);else return [];

        return components.ToList();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool TryGetAll(IList<string> __tags, out IReadOnlyList<Component> __components) {
        __components = GetAll(__tags); return __components.Any();
    }
    
    
    
    /// <summary>
    /// Finds all Components that have all the given tags, slower than GetAll() using IList. If you are being high performance, use that.
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public IReadOnlyList<Component> GetAll(IEnumerable<string> __tags) {

        if (!__tags.Any())
            return [];
        
        HashSet<Component> components;

        if (_componentTagDictionary.TryGetValue(__tags.First(), out var firstComponents)) components = firstComponents; else return [];
        
        foreach(var tag in __tags)
            if (_componentTagDictionary.TryGetValue(tag, out var taggedComponents)) 
                foreach (var component in components) if (!taggedComponents.Contains(component)) components.Remove(component);else return [];

        return components.ToList();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool TryGetAll(IEnumerable<string> __tags, out IReadOnlyList<Component> __components) {
        __components = GetAll(__tags); return __components.Any();
    }
    
    
    
    
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public IReadOnlyList<__Type> GetAll<__Type>(IList<string> __tags) {

        if (!__tags.Any())
            return [];
        
        HashSet<__Type> components = [];

        if (_componentTagDictionary.TryGetValue(__tags[0], out var firstComponents)) 
            foreach (var component in firstComponents) if (component is __Type typedComponent) components.Add(typedComponent);
        
        for (var i = 1; i < __tags.Count; i++)
            if (_componentTagDictionary.TryGetValue(__tags[i], out var taggedComponents)) 
                foreach (var component in components) if (!taggedComponents.Contains(component as Component)) components.Remove(component);else return [];

        return components.ToList();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool TryGetAll<__Type>(IList<string> __tags, out IReadOnlyList<__Type> __components) {
        __components = GetAll<__Type>(__tags); return __components.Any();
    }
    
    
    
    
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public IReadOnlyList<__Type> GetAll<__Type>(IEnumerable<string> __tags) {

        if (!__tags.Any())
            return [];
        
        HashSet<__Type> components = [];

        if (_componentTagDictionary.TryGetValue(__tags.First(), out var firstComponents)) 
            foreach (var component in firstComponents) if (component is __Type typedComponent) components.Add(typedComponent);
        
        foreach(string tag in __tags)
            if (_componentTagDictionary.TryGetValue(tag, out var taggedComponents)) 
                foreach (var component in components) if (!taggedComponents.Contains(component as Component)) components.Remove(component);else return [];

        return components.ToList();
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool TryGetAll<__Type>(IEnumerable<string> __tags, out IReadOnlyList<__Type> __components) {
        __components = GetAll<__Type>(__tags); return __components.Any();
    }
    
    
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public IReadOnlyList<Component> GetAll(string __tag) {
        return GetAll([__tag]);
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool TryGetAll(string __tag, out IReadOnlyList<Component> __components) {
        __components = GetAll(__tag); return __components.Any();
    }
    
    
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public IReadOnlyList<__Type> GetAll<__Type>(string __tag) {
        return GetAll<__Type>([__tag]);
    }
    
    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    public bool TryGetAll<__Type>(string __tag, out IReadOnlyList<__Type> __components) {
        __components = GetAll<__Type>(__tag); return __components.Any();
    }



    public Component Get(IList<string> __tags) {
        return GetAll(__tags).FirstOrDefault();
    }

    public bool TryGet(IList<string> __tags, out Component __component) {
        __component = Get(__tags); return __component != null;
    }

    
    
    public Component Get(IEnumerable<string> __tags) {
        return GetAll(__tags).FirstOrDefault();
    }
    
    public bool TryGet(IEnumerable<string> __tags, out Component __component) {
        __component = Get(__tags); return __component != null;
    }



    public __Type Get<__Type>(IList<string> __tags) {
        return GetAll<__Type>(__tags).FirstOrDefault();
    }
    
    public bool TryGet<__Type>(IList<string> __tags, out __Type __component) {
        __component = Get<__Type>(__tags); return __component != null;
    }



    public Component Get(string __tag) {
        return GetAll([__tag]).FirstOrDefault();
    }
    
    public bool TryGet(string __tag, out Component __component) {
        __component = Get(__tag); return __component != null;
    }



    public __Type Get<__Type>(string __tag) {
        return Get<__Type>([__tag]);
    }

    public bool TryGet<__Type>(string __tag, out __Type __component) {
        __component = Get<__Type>([__tag]); return __component != null;
    }
    
    
    
    
    
    
    
    
    
}