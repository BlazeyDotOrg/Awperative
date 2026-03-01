using System.Collections.Generic;

namespace AwperativeKernel;

public abstract partial class Component
{
    /// <summary>   
    /// Scene the Component resides in.
    /// </summary>
    [CalculatedProperty, CalculatedPropertyExpense("Medium: O(Parents)")]
    public Scene Scene => __QueryScene();
    private Scene __QueryScene() {
        if (ComponentDocker is Scene scene) return scene;
        if (ComponentDocker is Component Component) return Component.__QueryScene();
            
        return null;
    }
    
    
    
    /// <summary>
    /// Returns the Parent Component. Will be null if the Component is at the base of a scene.
    /// </summary>
    [CalculatedProperty, CalculatedPropertyExpense("Very Low O(1)")]
    public Component Parent => __QueryParent();
    private Component __QueryParent() {
        if (ComponentDocker is Component Component)
            return Component;
        return null;
    }
    
    
    
    /// <summary>
    /// All parent Components and the parents of the parents up until the Scene. Will only list parents of parents, not uncle Components.
    /// </summary>
    [CalculatedProperty, CalculatedPropertyExpense("Medium O(Parents)")]
    public IReadOnlyList<Component> AllParents => __QueryComponents();
    private IReadOnlyList<Component> __QueryComponents() {
        List<Component> returnValue = [];
        ComponentDocker currentComponentDocker = ComponentDocker;
        
        while (!(currentComponentDocker is Scene))
            if (currentComponentDocker is Component Component) {
                returnValue.Add(Component); 
                currentComponentDocker = Component.ComponentDocker;
            }
        return [..returnValue];
    }

    
}