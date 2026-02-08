using System;


namespace Awperative;

public sealed partial class Body
{
    public event EventHandler<ComponentCreateEvent> ComponentCreatedEvent;
    public event EventHandler<ComponentDestroyEvent> ComponentDestroyedEvent;
}