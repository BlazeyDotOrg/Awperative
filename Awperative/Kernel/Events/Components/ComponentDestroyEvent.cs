namespace Gravity.Kernel;

public sealed record ComponentDestroyEvent
{
    public readonly Component component;
    public readonly Body body;
    public readonly Scene scene;

    internal ComponentDestroyEvent() {}

    internal ComponentDestroyEvent(Component __component, Body __body, Scene __scene)
    {
        component = __component;
        body = __body;
        scene = __scene;
    }
}