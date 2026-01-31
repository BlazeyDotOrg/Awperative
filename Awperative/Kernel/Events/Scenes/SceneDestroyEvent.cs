namespace Awperative;


public sealed record SceneDestroyEvent
{
    public Scene scene;
    
    internal SceneDestroyEvent() {}

    internal SceneDestroyEvent(Scene __scene)
    {
        scene = __scene;
    }
}