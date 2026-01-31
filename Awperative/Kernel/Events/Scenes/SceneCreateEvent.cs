namespace Awperative;


public sealed record SceneCreateEvent
{
    public Scene scene;
    
    internal SceneCreateEvent() {}

    internal SceneCreateEvent(Scene __scene)
    {
        scene = __scene;
    }
}