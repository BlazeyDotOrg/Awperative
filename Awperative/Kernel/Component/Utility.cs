using System.Collections.Generic;
using System.Collections.Immutable;

namespace Awperative;

public abstract partial class Component : Container
{
    
    
    
    public Scene Scene => __QueryScene();
    private Scene __QueryScene() {
        if (Container is Scene scene) return scene;
        if (Container is Component behavior) return behavior.__QueryScene();
            
        return null;
    }
    
    
    
    

    public ImmutableArray<Container> Dockers => __QueryDockers();
    private ImmutableArray<Container> __QueryDockers() {
        List<Container> returnValue = [];
        Container currentContainer = Container;
        
        while (!(currentContainer is Scene))
            if (currentContainer is Component behavior) {
                returnValue.Add(currentContainer); 
                currentContainer = behavior.Container;
            }
        returnValue.Add(currentContainer);
        
        return ImmutableArray.Create<Container>(returnValue.ToArray());
    }
    
    
    
    

    public Component Parent => __QueryParent();
    private Component __QueryParent() {
        if (Container is Component behavior)
            return behavior;
        return null;
    }

    
    
    
    
    public ImmutableArray<Component> Parents => __QueryBehaviors();
    private ImmutableArray<Component> __QueryBehaviors() {
        List<Component> returnValue = [];
        Container currentContainer = Container;
        
        while (!(currentContainer is Scene))
            if (currentContainer is Component behavior) {
                returnValue.Add(behavior); 
                currentContainer = behavior.Container;
            }
        return ImmutableArray.Create<Component>(returnValue.ToArray());
    }
    
    
    
}