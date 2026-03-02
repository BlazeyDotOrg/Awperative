using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;


namespace AwperativeKernel;


public abstract partial class ComponentDocker
{


    /// <summary> Attaches a preexisting component to the docker, this is not transferring the component, the method will throw an error if the component is already attached to a docker</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Add([DebugAttributes.ComponentNotNull, DebugAttributes.OrphanComponent] Component __component) {
        if (!DebugAttributes.ComponentNotNull.VerifyOrThrow(__component)) return;
        if (!DebugAttributes.OrphanComponent.VerifyOrThrow(__component)) return;

        InitiateComponent(__component);
    }


    /// <summary> Creates a new instance of that type of component and attaches it to the docker, and returns a reference to it.</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public __Type Add<__Type>() where __Type : Component, new() {
        Component newComponent = new __Type();
        InitiateComponent(newComponent);
        return (__Type)newComponent;
    }


    /// <summary> Creates a new instance of that type of component and attaches it to the docker, and returns a reference to it.</summary>    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public __Type Add<__Type>(string name = null, [DebugAttributes.ValueFitsRange] int priority = 0, Collection<string> tags = null) where __Type : Component, new() {
        Component newComponent = new __Type();
        newComponent.Name = name ??= typeof(__Type).Name;
        newComponent._tags = [..tags ??= []];
        newComponent.Priority = priority;

        InitiateComponent(newComponent);
        return (__Type)newComponent;
    }


    /// <summary> Initiates a component into the docker. </summary>
    [MarkerAttributes.UnsafeInternal]
    private void InitiateComponent(Component __component) {
        //add to Component Docker's lists
        AddComponentToLists(__component);

        __component.ComponentDocker = this;
        //create event
        __component.TryEvent(4);
        __component.ChainEvent(4);
    }





    /// <summary> Destroys a component attached to the Docker </summary>
    /// <param name="__component"></param>
    public void Destroy([DebugAttributes.ComponentNotNull, DebugAttributes.DockerOwns] Component __component) {
        if (!DebugAttributes.ComponentNotNull.VerifyOrThrow(__component)) return;
        if (!DebugAttributes.DockerOwns.VerifyOrThrow(this, __component)) return;

        __component.TryEvent(5);
        __component.ChainEvent(5);

        RemoveComponentFromLists(__component);
        __component.ComponentDocker = null;
    }



    /// <summary> Destroys all the components in a given list </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)] 
    public void DestroyAll() => DestroyAll(GetAll());



    /// <summary> Destroys all the components in a given list </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll([DebugAttributes.EnumerableNotNull, DebugAttributes.DockerOwns] IEnumerable<Component> __Components) { foreach (Component component in __Components.ToArray()) Destroy(component); }



    /// <summary> Destroys the first component with a given Type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Destroy<__Type>() where __Type : Component => Destroy(Get<__Type>());



    /// <summary> Destroys all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll<__Type>() where __Type : Component => DestroyAll(GetAll<__Type>());



    /// <summary> Destroys all components that have all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags) => DestroyAll(GetAll(__tags));




    /// <summary> Destroys all Components that have the given type, and all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll<__Type>([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags) where __Type : Component => DestroyAll(GetAll<__Type>(__tags));




    /// <summary> Destroys all the components with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll([DebugAttributes.NotNull] string __tag) => DestroyAll(GetAll([__tag]));



    /// <summary> Destroys all the components that have a certain type, and a certain tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void DestroyAll<__Type>([DebugAttributes.NotNull] string __tag) where __Type : Component => DestroyAll(GetAll<__Type>([__tag]));



    /// <summary> Destroys the first component with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Destroy([DebugAttributes.NotNull] string __tag) => Destroy(GetAll([__tag]).FirstOrDefault());



    /// <summary> Destroys the Destroys component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void Destroy<__Type>([DebugAttributes.NotNull] string __tag) where __Type : Component => Destroy(GetAll<__Type>(__tag).FirstOrDefault());

}