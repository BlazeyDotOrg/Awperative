using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace AwperativeKernel;


/// <summary>
/// Holds a myriad of useful attributes which you are more than welcome to use! These attributes are not meant for reflection, but merely as a means of straightforward debugging.
/// Each attribute gives has a method called VerifyOrThrow() which can differ parameter wise. VerifyOrThrow() returns a bool based on if the condition is true or not.
/// If it is false it will try to throw an error, and returns false as well.The only time this behavior differs, is if Awperative is set to IgnoreErrors. In that case it will return true no matter what.
/// (However it will still debug unless that is disabled too).
/// </summary>
/// <usage>
/// The attributes have been designed to be used in methods like so : if(!Attribute.VerifyOrThrow()) return; This usage allows the attribute to control the flow of output, and halt any unsafe process.
/// However, nothing is stopping you from using them any other way, so go wild. Feel free to make more, or use these in your own code!
/// </usage>
/// <author> Avery Norris </author>
public static class DebugAttributes
{
    #region Docker/Entity
    
    /// <summary> Requires that any Component is owned by the Docker</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DockerOwns : Attribute
    {


        /// <summary> Returns true or false based on the given condition from the attribute, unless Debug.IgnoreErrors is true, in which it will always return true, but still try to throw errors. </summary>
        /// <usage> It is required to use VerifyOrThrow() to validate important conditions for methods within the kernel. You may also feel free to use this outside in any modules or games.
        /// It is easiest to use VerifyOrThrow like : (In your method) if(!Attribute.VerifyOrThrow()) return; That way the attribute can exit the code if the condition is false.</usage>
        public static bool VerifyOrThrow(ComponentDocker __docker, Component __component) {
            if (__docker.Contains(__component)) return true;

            Debug.LogError("Docker does not own the Component!",
                ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
                    __component.GetType().Name,
                    __component.Name,
                    __component.GetHashCode().ToString("N0"),
                    __docker.GetType().Name,
                    __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
                    __docker.GetHashCode().ToString("N0")
                ]);

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that the Docker does not own the given Component</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DockerDoesntOwn : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(ComponentDocker __docker, Component __component) {
            if (!__docker.Contains(__component)) return true;

            Debug.LogError("Docker owns the Component!",
                ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
                    __component.GetType().Name,
                    __component.Name,
                    __component.GetHashCode().ToString("N0"),
                    __docker.GetType().Name,
                    __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
                    __docker.GetHashCode().ToString("N0")
                ]);

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that the Component does not belong to a Docker</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class OrphanComponent : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(Component __component) {
            if (__component.ComponentDocker == null) return true;

            Debug.LogError("Component is already owned!",
                ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
                    __component.GetType().Name,
                    __component.Name,
                    __component.GetHashCode().ToString("N0"),
                    __component.ComponentDocker.GetType().Name,
                    __component.ComponentDocker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
                    __component.ComponentDocker.GetHashCode().ToString("N0")
                ]);

            return Debug.IgnoreErrors;
        }
    }
    
    
    
    /// <summary> Requires that a given Docker is not the same</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DifferentDocker : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(ComponentDocker __docker, ComponentDocker __other) {
            if (!__docker.Equals(__other)) return true;

            Debug.LogError("The dockers are the same!", ["DockerType", "DockerName", "DockerHash"], [
                __docker.GetType().Name,
                __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
                __docker.GetHashCode().ToString("N0")
            ]);

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that the Component is not null</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ComponentNotNull : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(Component __component) {
            if (__component != null) return true;

            Debug.LogError("Component is null!");

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that the Docker is not null</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DockerNotNull : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(ComponentDocker __componentDocker) {
            if (__componentDocker != null) return true;

            Debug.LogError("Docker is null!");

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that a given Scene is not null</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class SceneNotNull : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(Scene __scene) {
            if (__scene != null) return true;

            Debug.LogError("Scene is null!");

            return Debug.IgnoreErrors;
        }
    }
    
    #endregion

    #region Null/Collection

    /// <summary> Requires all elements in a Collection are not null </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CollectionNotNull : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(ICollection<object> __collection) {
            foreach (object obj in __collection) {
                if (obj == null) {
                    Debug.LogError("A given enumerator has null members!", ["Type"], [__collection.GetType().Name]);
                    return Debug.IgnoreErrors;
                }
            }

            return true;
        }
    }



    /// <summary> Requires all elements in an Enumerator are not null</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class EnumeratorNotNull : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(IEnumerable<object> __enumerator) {
            foreach (object obj in __enumerator) {
                if (obj == null) {
                    Debug.LogError("A given enumerator has null members!", ["Type"], [__enumerator.GetType().Name]);
                    return Debug.IgnoreErrors;
                }
            }

            return true;
        }
    }



    /// <summary> Requires a given object is not null </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class NotNull : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(Object __object) {
            if (__object != null) return true;

            Debug.LogError("A given object is null!");

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that an integer fits a range</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ValueFitsRange : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow(int __index, int __min, int __max) {
            if (__index >= __min && __index <= __max) return true;

            Debug.LogError("Value does not fit range!", ["Index"], [__index.ToString("N0")]);

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that a collection contains the given item</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CollectionContains : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow<__Type>(__Type __object, ICollection<__Type> __collection) {
            if (__collection.Contains(__object)) return true;

            Debug.LogError("Collection does not contain object!", ["ObjectType"], [__object.GetType().Name]);

            return Debug.IgnoreErrors;
        }
    }



    /// <summary> Requires that a collection does not contain the given item</summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CollectionDoesntContain : Attribute
    {


        /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
        public static bool VerifyOrThrow<__Type>(__Type __object, ICollection<__Type> __collection) {
            if (!__collection.Contains(__object)) return true;

            Debug.LogError("Collection already contains object!", ["ObjectType"], [__object.GetType().Name]);

            return Debug.IgnoreErrors;
        }
    }
    
    #endregion
}