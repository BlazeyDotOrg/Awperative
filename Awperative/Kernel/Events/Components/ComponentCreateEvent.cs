namespace Gravity.Kernel;

public sealed record ComponentCreateEvent
{
    public readonly Component component;
    public readonly Body body;
    public readonly Scene scene;

    internal ComponentCreateEvent() {}

    internal ComponentCreateEvent(Component __component, Body __body, Scene __scene)
    {
        component = __component;
        body = __body;
        scene = __scene;
    }
}