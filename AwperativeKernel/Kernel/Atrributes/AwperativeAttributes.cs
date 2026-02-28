using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace AwperativeKernel;


/// <summary>
/// Requires that the Docker owns the parameter
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DockerOwns : Attribute
{
    
    
    /// <summary>
    /// Verifies if the Component is actually owned by Docker. Throws an error if not.
    /// </summary>
    /// <param name="__docker">Docker we are checking ownership for</param>
    /// <param name="__component">Component to check for</param>
    public static bool VerifyOrThrow(ComponentDocker __docker, Component __component) {
        if (__docker.Contains(__component)) return true;
        
        Debug.LogError("Docker does not own the Component!", ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
            __component.GetType().Name, 
            __component.Name,
            __component.GetHashCode().ToString("N0"), 
            __docker.GetType().Name, 
            __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
            __docker.GetHashCode().ToString("N0")
        ]);
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the Docker does not own the parameter
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DockerDoesntOwn : Attribute
{
    
    
    /// <summary>
    /// Verifies if the Component is actually not owned by Docker. Throws an error if not.
    /// </summary>
    /// <param name="__docker">Docker we are checking ownership for</param>
    /// <param name="__component">Component to check for</param>
    public static bool VerifyOrThrow(ComponentDocker __docker, Component __component) {
        if (!__docker.Contains(__component)) return true;
        
        Debug.LogError("Docker owns the Component!", ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
            __component.GetType().Name, 
            __component.Name,
            __component.GetHashCode().ToString("N0"), 
            __docker.GetType().Name, 
            __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
            __docker.GetHashCode().ToString("N0")
        ]);
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the component is not attached to any Docker
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class OrphanComponent : Attribute
{


    /// <summary>
    /// Verifies if the Component is an orphan, not fully accurate, because it only checks if the docker is null, this means it is technically possible
    /// to have a component already attached to a docker, and still verify it as false if Docker is null, but this should be impossible in practice.
    /// </summary>
    /// <param name="__component"></param>
    /// <returns></returns>
    public static bool VerifyOrThrow(Component __component) {
        if (__component.ComponentDocker == null) return true;
        
        Debug.LogError("Component is already owned!", ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
            __component.GetType().Name, 
            __component.Name,
            __component.GetHashCode().ToString("N0"), 
            __component.ComponentDocker.GetType().Name, 
            __component.ComponentDocker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
            __component.ComponentDocker.GetHashCode().ToString("N0")
        ]);
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the Component is not null
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class ComponentNotNull : Attribute
{


    /// <summary>
    /// Verifies if the Component is not null! Throws an error otherwise.
    /// </summary>
    /// <param name="__component"></param>
    /// <returns></returns>
    public static bool VerifyOrThrow(Component __component) {
        if (__component != null) return true;

        Debug.LogError("Component is null!");
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the Docker is not null
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DockerNotNull : Attribute
{


    /// <summary>
    /// Verifies if the Docker is not null! Throws an error otherwise.
    /// </summary>
    /// <param name="__componentDocker"></param>
    /// <returns></returns>
    public static bool VerifyOrThrow(ComponentDocker __componentDocker) {
        if (__componentDocker != null) return true;

        Debug.LogError("Docker is null!");
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the Scene is not null
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class SceneNotNull : Attribute
{


    /// <summary>
    /// Verifies if the Scene is not null! Throws an error otherwise.
    /// </summary>
    /// <returns></returns>
    public static bool VerifyOrThrow(Scene __scene) {
        if (__scene != null) return true;

        Debug.LogError("Scene is null!");
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that everything in the collection is not null
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class CollectionNotNull : Attribute
{


    /// <summary>
    /// Verifies if the Scene is not null! Throws an error otherwise.
    /// </summary>
    /// <returns></returns>
    public static bool VerifyOrThrow(Collection<object> __collection) {
        for (var i = 0; i < __collection.Count; i++)
            if (__collection[i] == null)
                Debug.LogError("A Given Collection has null members!", ["Type"], [__collection.GetType().Name]);
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that everything in the collection is not null
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class EnumeratorNotNull : Attribute
{


    /// <summary>
    /// Verifies if the Scene is not null! Throws an error otherwise.
    /// </summary>
    /// <returns></returns>
    public static bool VerifyOrThrow(IEnumerable<object> __enumerator) {
        foreach (object obj in __enumerator)
            if (obj == null)
                Debug.LogError("A Given Enumerator has null members!", ["Type"], [__enumerator.GetType().Name]);
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the object is not null
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class NotNull : Attribute
{


    /// <summary>
    /// Verifies if the Scene is not null! Throws an error otherwise.
    /// </summary>
    /// <returns></returns>
    public static bool VerifyOrThrow(Object __object) {
        if (__object != null) return true;

        Debug.LogError("A Given parameter is null!", ["Type"], 
            [__object.GetType().Name
        ]);
        
        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the Docker is a different docker than the one given
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class DifferentDocker : Attribute
{


    /// <summary>
    /// Verifies if the Dockers are different!
    /// </summary>
    /// <returns></returns>
    public static bool VerifyOrThrow(ComponentDocker __docker, ComponentDocker __other) {
        if (__docker != __other) return true;

        Debug.LogError("The Dockers are the same!", ["DockerType, DockerName, DockerHash"], [
            __docker.GetType().Name, 
            __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
            __docker.GetHashCode().ToString("N0")
        ]);

        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the index fits a given collection
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class ValueFitsRange : Attribute
{


    /// <summary>
    /// Verifies if the value fits a range
    /// </summary>
    /// <param name="__componentDocker"></param>
    /// <returns></returns>
    public static bool VerifyOrThrow(int __index, int __min, int __max) {
        if (__index >= __min && __index <= __max) return true;

        Debug.LogError("Value does not fit range!", ["Index"], [__index.ToString("N0")]);

        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the index fits a given collection
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class CollectionContains : Attribute
{


    /// <summary>
    /// Verifies if the value fits a range
    /// </summary>
    /// <param name="__componentDocker"></param>
    /// <returns></returns>
    public static bool VerifyOrThrow<__Type>(__Type __object, ICollection<__Type> __collection) {
        if(__collection.Contains(__object)) return true;

        Debug.LogError("Collection does not contain object!", ["ObjectType"], 
            [__object.GetType().Name]);

        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Requires that the index fits a given collection
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class CollectionDoesntContain : Attribute
{


    /// <summary>
    /// Verifies if the value fits a range
    /// </summary>
    /// <param name="__componentDocker"></param>
    /// <returns></returns>
    public static bool VerifyOrThrow<__Type>(__Type __object, ICollection<__Type> __collection) {
        if(!__collection.Contains(__object)) return true;

        Debug.LogError("Collection already contains object!", ["ObjectType"], 
            [__object.GetType().Name]);

        return Awperative.IgnoreErrors;
    }
}



/// <summary>
/// Shows that the given object is unsafe (ex. it doesn't check for null values and such, or it doesn't have guardrails based on cases).
/// This is just for internal/private methods to remind myself how to call it :) The reasoning is case by case, but most of the time,
/// it is because all of the exposing public methods already check, and double checks would only slow me down
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class UnsafeInternal : Attribute { }



/// <summary>
/// Shows that the given object (meant for external use) is calculated every time it is called! Good to know for performance heavy systems.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class CalculatedProperty() : Attribute { }



/// <summary>
/// Just a way to write how expensive a calculated property can be.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class CalculatedPropertyExpense(string Expense) : Attribute { }