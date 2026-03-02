using System.Collections.Generic;
using System.Linq;


namespace AwperativeKernel;


public abstract partial class ComponentDocker
{

    
    
    /// <summary> Moves a component belonging to the docker to another docker</summary>
    public void Move([DebugAttributes.ComponentNotNull,DebugAttributes.DockerOwns] Component __component, [DebugAttributes.DockerNotNull,DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) {
        if(!DebugAttributes.ComponentNotNull.VerifyOrThrow(__component)) return;
        if(!DebugAttributes.DockerOwns.VerifyOrThrow(this, __component)) return;
        if(!DebugAttributes.DockerNotNull.VerifyOrThrow(__componentDocker)) return;
        if(!DebugAttributes.DifferentDocker.VerifyOrThrow(this, __componentDocker)) return;
        
        RemoveComponentFromLists(__component);
        __componentDocker.AddComponentToLists(__component);
        
        __component.ComponentDocker = __componentDocker;
    }
        

    /// <summary> Moves all components in a list to another docker</summary>
    public void MoveAll([DebugAttributes.EnumerableNotNull, DebugAttributes.DockerOwns] IEnumerable<Component> __Components, [DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) {
        if(!DebugAttributes.EnumerableNotNull.VerifyOrThrow(__Components)) return;
        if(!DebugAttributes.DockerNotNull.VerifyOrThrow(__componentDocker)) return;
        if(!DebugAttributes.DifferentDocker.VerifyOrThrow(this, __componentDocker)) return;

        foreach (Component Component in __Components) {
            if(!DebugAttributes.DockerOwns.VerifyOrThrow(__componentDocker, Component)) return;
            Move(Component, __componentDocker);
        }
    }
    
    /// <summary> Gets all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)] 
    public void MoveAll([DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) => MoveAll(GetAll(), __componentDocker);

    /// <summary> Moves the first component with a given Type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Move<__Type>([DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) where __Type : Component => Move(Get<__Type>(), __componentDocker);



    /// <summary> Moves all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>([DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>(), __componentDocker);



    /// <summary> Moves all components that have all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags, [DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) => MoveAll(GetAll(__tags), __componentDocker);
    



    /// <summary> Moves all Components that have the given type, and all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>([DebugAttributes.EnumerableNotNull] IEnumerable<string> __tags, [DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>(__tags), __componentDocker);
    
    
    
    
    /// <summary> Moves all the components with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void MoveAll([DebugAttributes.NotNull] string __tag, [DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) => MoveAll(GetAll([__tag]), __componentDocker);
    
    
    
    /// <summary> Moves all the components that have a certain type, and a certain tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>([DebugAttributes.NotNull] string __tag, [DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>([__tag]), __componentDocker);
    
    

    /// <summary> Moves the first component with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Move([DebugAttributes.NotNull] string __tag, [DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) => Move(GetAll([__tag]).FirstOrDefault(), __componentDocker);


    
    /// <summary> Moves the moves component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void Move<__Type>([DebugAttributes.NotNull] string __tag, [DebugAttributes.DockerNotNull, DebugAttributes.DifferentDocker] ComponentDocker __componentDocker) where __Type : Component => Move(GetAll<__Type>(__tag).FirstOrDefault(), __componentDocker);
    
    
}