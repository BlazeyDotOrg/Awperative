using System.Collections.Generic;
using System.Collections.Immutable;

namespace Awperative;

public abstract partial class Behavior : Docker
{
    
    
    
    public Scene Scene => __QueryScene();
    private Scene __QueryScene() {
        if (Docker is Scene scene) return scene;
        if (Docker is Behavior behavior) return behavior.__QueryScene();
            
        return null;
    }
    
    
    
    

    public ImmutableArray<Docker> Dockers => __QueryDockers();
    private ImmutableArray<Docker> __QueryDockers() {
        List<Docker> returnValue = [];
        Docker currentDocker = Docker;
        
        while (!(currentDocker is Scene))
            if (currentDocker is Behavior behavior) {
                returnValue.Add(currentDocker); 
                currentDocker = behavior.Docker;
            }
        returnValue.Add(currentDocker);
        
        return ImmutableArray.Create<Docker>(returnValue.ToArray());
    }
    
    
    
    

    public Behavior Parent => __QueryParent();
    private Behavior __QueryParent() {
        if (Docker is Behavior behavior)
            return behavior;
        return null;
    }

    
    
    
    
    public ImmutableArray<Behavior> Parents => __QueryBehaviors();
    private ImmutableArray<Behavior> __QueryBehaviors() {
        List<Behavior> returnValue = [];
        Docker currentDocker = Docker;
        
        while (!(currentDocker is Scene))
            if (currentDocker is Behavior behavior) {
                returnValue.Add(behavior); 
                currentDocker = behavior.Docker;
            }
        return ImmutableArray.Create<Behavior>(returnValue.ToArray());
    }
    
    
    
}