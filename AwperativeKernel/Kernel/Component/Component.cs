using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic;


namespace AwperativeKernel;




public abstract partial class Component : ComponentDocker
{



    /// <summary> Current parent of the Component. Can be either Scene or another Component.</summary>
    [MarkerAttributes.UnsafeInternal]
    public ComponentDocker ComponentDocker { get; set; } = null;


    /// <summary>
    /// Component name
    /// </summary>
    [NotNull]
    public string Name {
        get => _name;
        set { if (!DebugAttributes.NotNull.VerifyOrThrow(value)) return; _name = value; }
    } [MarkerAttributes.UnsafeInternal] private string _name;

    
    /// <summary> Represents the state of this Component, The largest bit represents if the Component is enabled or not, while the
    /// next 7 represent its priority </summary>
    [MarkerAttributes.UnsafeInternal]
    private byte OrderProfile;

    /// <summary> If the component receives time events or not. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.CalculatedPropertyExpense("Very Low")]
    public bool Enabled {
        get => (OrderProfile & 128) > 0;
        set => OrderProfile = (byte)((OrderProfile & 127) | (value ? 128 : 0));
    }
    
    /// <summary> Represents the Component's Update priority, can be set to any value ranging from -64 to 63; otherwise an error will throw! </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.CalculatedPropertyExpense("Very Low")]
    public int Priority {
        get => (sbyte)(OrderProfile << 1) >> 1;
        set {
            if(!DebugAttributes.ValueFitsRange.VerifyOrThrow(value, -64, 63)) return;
            OrderProfile = (byte)((OrderProfile & 0x80) | (value & 0x7F));
            ComponentDocker.UpdatePriority(this, value);
        }
    }
    
    
    
    

    /// <summary>
    /// Attempts to send an Event to the Component, terminates if the Component does not have the given Event
    /// </summary>
    /// <param name="__timeEvent">Type of Event</param>
    [MarkerAttributes.UnsafeInternal]
    internal void TryEvent(int __timeEvent) {
        Awperative._TypeAssociatedTimeEvents[GetType()][__timeEvent]?.Invoke(this);
    }

    
    
    /// <summary>
    /// Identifiers for Components.
    /// </summary>
    public IReadOnlyList<string> Tags => [.._tags];
    [MarkerAttributes.UnsafeInternal] internal HashSet<string> _tags = [];





    /// <summary>
    /// Adds a new tag to the Component
    /// </summary>
    /// <param name="__tag"> The tag to add</param>
    public void AddTag([DebugAttributes.NotNull, DebugAttributes.CollectionDoesntContain] string __tag)
    {
        if(!DebugAttributes.NotNull.VerifyOrThrow(__tag)) return;
        if(!DebugAttributes.CollectionDoesntContain.VerifyOrThrow(__tag, _tags)) return;
        
        _tags.Add(__tag);
        ComponentDocker.AddTagToComponent(__tag, this);
    }
    
    
    
    
    
    /// <summary>
    /// Adds a new tag to the Component
    /// </summary>
    /// <param name="__tag"> The tag to add</param>
    public void RemoveTag([DebugAttributes.NotNull,DebugAttributes.CollectionContains] string __tag)
    {
        if (!DebugAttributes.NotNull.VerifyOrThrow(__tag)) return;
        if(!DebugAttributes.CollectionContains.VerifyOrThrow(__tag, _tags)) return;
        
        _tags.Remove(__tag);
        ComponentDocker.RemoveTagFromComponent(__tag, this);
    }

    public override string ToString() {
        return this.Name;
    }
}