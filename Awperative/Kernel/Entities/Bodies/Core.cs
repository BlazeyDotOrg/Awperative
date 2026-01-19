using System;
using System.Collections.Generic;


namespace Gravity.Kernel;

public sealed partial class Body
{

    public readonly Scene scene;
    public readonly Transform transform = new Transform();

    public readonly List<string> tags = [];
    
    public readonly List<Component> components = [];
    

    public Body(Scene __scene) {
        scene = __scene;
    }

    public Body(Scene __scene, Transform __transform) {
        scene = __scene;
        transform = __transform;
    }
    

    //todo: make internal
}