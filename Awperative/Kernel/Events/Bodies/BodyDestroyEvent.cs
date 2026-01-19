namespace Gravity.Kernel;

public sealed record BodyDestroyEvent
{
    public readonly Body body;
    public readonly Scene scene;

    internal BodyDestroyEvent() {}

    internal BodyDestroyEvent(Body __body, Scene __scene)
    {
        body = __body;
        scene = __scene;
    }
}