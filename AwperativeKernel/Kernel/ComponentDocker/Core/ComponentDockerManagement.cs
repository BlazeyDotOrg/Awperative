using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;


namespace AwperativeKernel;


public abstract partial class ComponentDocker
{

    
    
    /// <summary>
    /// Adds a previously instantiated Component to the Docker 
    /// </summary>
    /// <param name="__component">The Component to add</param>
    /// <remarks> This is NOT meant to transfer a Component from one docker to another (Please use Move() for that).
    /// And If you are just instantiating an Empty Component, also consider instead using Add&lt;Type&gt;();</remarks>
    /// <author> Avery Norris</author>
    #nullable enable
    public void Add([ComponentNotNull,OrphanComponent] Component __component) {
        if(!ComponentNotNull.VerifyOrThrow(__component)) return;
        if(!OrphanComponent.VerifyOrThrow(__component)) return;
        
        //Component has already been added to another docker
        if (__component.ComponentDocker != null) { Debug.LogError("You cannot use add if the Component already belongs to a Docker, use Component.Transfer();"); return; }
        
        InitiateComponent(__component);
    }
    
    
    
    /// <summary>
    /// Add a new Component to the Docker; and returns a reference to it!
    /// </summary>
    /// <typeparam name="__Type"> Type of Component to instantiate</typeparam>
    /// <remarks>Component cannot have a Constructor</remarks>
    /// <author> Avery Norris</author>
    public __Type Add<__Type>() where __Type : Component, new() { Component newComponent = new __Type(); InitiateComponent(newComponent); return (__Type)newComponent; }



    /// <summary>
    /// Add a new Component to the Docker; and returns a reference to it!
    /// </summary>
    /// <typeparam name="__Type"> Type of Component to instantiate</typeparam>
    /// <remarks>Component cannot have a Constructor</remarks>
    /// <author> Avery Norris</author>
    public __Type Add<__Type>(string name = null, int priority = 0, Collection<string> tags = null) where __Type : Component, new() {
        Component newComponent = new __Type(); 
        newComponent.Name = name ??= typeof(__Type).Name;
        newComponent._tags = [..tags ??= []];
        newComponent._priority = priority;
        
        InitiateComponent(newComponent); return (__Type)newComponent;
    }


    
    
    
    /// <summary>
    /// Sets important Component variables; and tries to send the Create() event.
    /// </summary>
    /// <param name="__component"></param>
    /// <typeparam name="__Type"></typeparam>
    private void InitiateComponent(Component __component) {
        //add to Component Docker's lists
        AddComponentToLists(__component);
        
        __component.ComponentDocker = this;
        //create event
        __component.TryEvent(4); __component.ChainEvent(4);
    }
    
    
    
    
    
    /// <summary>
    /// Transfers a Component to another Docker
    /// </summary>
    /// <param name="__component"> Component to move</param>
    /// <param name="__componentDocker"> Docker to move component to</param>
    /// <remarks> Components cannot transfer themselves with this Method!</remarks>
    public void Move([ComponentNotNull,DockerOwns] Component __component, [DockerNotNull,DifferentDocker] ComponentDocker __componentDocker) {
        if(!ComponentNotNull.VerifyOrThrow(__component)) return;
        if(!DockerOwns.VerifyOrThrow(this, __component)) return;
        if(!DockerNotNull.VerifyOrThrow(__componentDocker)) return;
        if(!DifferentDocker.VerifyOrThrow(this, __componentDocker)) return;
        
        if (!Contains(__component)) {
            Debug.LogError("Docker does not have ownership over Component!", ["ComponentType"], [__component.GetType().Name]); return;
        }

        if (__componentDocker == this) {
            Debug.LogError("Docker already has Component!", ["ComponentType"], [__component.GetType().Name]); return;
        }

        var type = __component.GetType();
        
        //Modify collections on both Dockers
        RemoveComponentFromLists(__component);
        __componentDocker.AddComponentToLists(__component);
        
        __component.ComponentDocker = __componentDocker;
    }
    
    
    
    /// <summary>
    /// Transfers the first found Component of a specific type to another Docker
    /// </summary>
    /// <param name="__componentDocker"> Docker to move the Component to</param>
    /// <typeparam name="__Type"> Type of component</typeparam>
    public void Move<__Type>([DockerNotNull,DifferentDocker] ComponentDocker __componentDocker) where __Type : Component => Move(Get<__Type>(), __componentDocker);

    
    
    /// <summary>
    /// Transfers all Components in a collection to another Docker.
    /// </summary>
    /// <param name="__Components"> The Components that need to be transferred</param>
    /// <param name="__componentDocker"> Docker to move Component to</param>
    public void MoveAll([ComponentNotNull,DockerOwns] IEnumerable<Component> __Components, [DockerNotNull,DifferentDocker] ComponentDocker __componentDocker) { foreach (Component Component in (Component[])[..__Components]) Move(Component, __componentDocker); }
    
    
    
    /// <summary>
    /// Transfers all Components of a type to another Docker.
    /// </summary>
    /// <param name="__componentDocker"> Target Docker</param>
    /// <typeparam name="__Type"> Type of Components to transfer</typeparam>
    public void MoveAll<__Type>([DockerNotNull, DifferentDocker] ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>(), __componentDocker);
    
    
    
    
        
    /// <summary>
    /// Destroys a Component attached to the Docker
    /// </summary>
    /// <param name="__component"></param>
    public void Destroy([ComponentNotNull,DockerOwns] Component __component) {
        if(!ComponentNotNull.VerifyOrThrow(__component)) return;
        if(!DockerOwns.VerifyOrThrow(this, __component)) return;

        __component.TryEvent(5);
        __component.ChainEvent(5);  
        
        RemoveComponentFromLists(__component);
        __component.ComponentDocker = null;
        
        __component.Dispose();
    }
    
    
        
    /// <summary>
    /// Destroys the first found Component of a given type
    /// </summary>
    /// <typeparam name="__Type"> Type of Component to destroy</typeparam>
    public void Destroy<__Type>() where __Type : Component => Destroy(Get<__Type>());



    /// <summary>
    /// Destroys all Components from a given collection.
    /// </summary>
    /// <param name="__Components"></param>
    public void DestroyAll([ComponentNotNull, DockerOwns] Collection<Component> __Components) { for (var i = 0; i < __Components.Count; i++) Destroy(__Components[i]); }

    
    
    /// <summary>
    /// Destroys all Components of a given type
    /// </summary>
    /// <typeparam name="__Type"></typeparam>
    public void DestroyAll<__Type>() where __Type : Component => DestroyAll(GetAll<__Type>());



    /// <summary>
    /// Destroys all Components attached to Docker
    /// </summary>
    public void DestroyAll() { for(var i = 0; i < _Components.Count; i++) Destroy(_Components[i]); }
}