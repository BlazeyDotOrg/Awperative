using System.Collections.Generic;

namespace Awperative;

public abstract partial class Behavior : Docker
{
    public Scene Scene => __QueryScene();
    private Scene __QueryScene() {
        if (Docker is Scene scene) return scene;
        if (Docker is Behavior behavior) return behavior.__QueryScene();
            
        return null;
    }

    public Docker[] Dockers => __QueryDockers();

    private Docker[] __QueryDockers()
    {
        List<Docker> returnValue = [];

        Docker currentDocker = Docker;
        
        while (!(currentDocker is Scene))
        {
            if (currentDocker is Behavior behavior)
            {
                returnValue.Add(behavior);
                currentDocker = behavior.Docker;
            }
        }
        
        returnValue.Add(currentDocker);
        
        return returnValue.ToArray();
    }

    public Behavior[] Parents => __QueryParents();
    
}