using System;


namespace Gravity.Kernel;

public sealed partial class Body
{
    //todo: add component events to scene in v5
    
    public event EventHandler<ComponentCreateEvent> ComponentCreatedEvent;
    public event EventHandler<ComponentDestroyEvent> ComponentDestroyedEvent;
}