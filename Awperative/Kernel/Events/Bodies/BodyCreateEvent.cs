namespace Gravity.Kernel;

public sealed record BodyCreateEvent
{
    public readonly Body body;
    public readonly Scene scene;

    internal BodyCreateEvent() {}

    internal BodyCreateEvent(Body __body, Scene __scene)
    {
        body = __body;
        scene = __scene;
    }
}