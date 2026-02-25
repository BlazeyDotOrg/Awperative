using System.ComponentModel;
using System.Linq;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;



namespace AwperativeKernel;


/// <summary>
/// Base class of Awperative. Carries events from MonoGame into scenes and hooks.
/// </summary>
/// <author> Avery Norris </author>
public sealed class Base() : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { })
{
    
    

    /// <summary>
    /// LoadContent() is called when the program starts; right after Initialize(). Override Load() in scripting tools or use hooks to call from this event.
    /// </summary>
    /// <remarks> It is recommended to load content during LoadContent()</remarks>
    protected override void OnLoad() { foreach (Component component in Awperative._TimeBasedComponents[Awperative.TimeEvent.Load].ToList()) component.ChainEvent(Awperative.TimeEvent.Load); base.OnLoad(); }
    
    
    
    
    
    /// <summary>
    /// Update() is called every frame; before Draw(). Override Update() in scripting tools to call from this event.
    /// </summary>
    /// <remarks> Hooks are unable to receive both Update() and Draw()</remarks>
    protected override void OnUpdateFrame(FrameEventArgs __args) { foreach (Component component in Awperative._TimeBasedComponents[Awperative.TimeEvent.Update].ToList()) component.ChainEvent(Awperative.TimeEvent.Update); base.OnUpdateFrame(__args); }

    
    
    
    
    /// <summary>
    /// Draw() is called every frame; after Update(). Override Draw() in scripting tools to call from this event.
    /// </summary>
    /// <remarks> Hooks are unable to receive both Update() and Draw()</remarks>
    protected override void OnRenderFrame(FrameEventArgs __args) { foreach (Component component in Awperative._TimeBasedComponents[Awperative.TimeEvent.Draw].ToList()) component.ChainEvent(Awperative.TimeEvent.Draw); base.OnRenderFrame(__args); }





    /// <summary>
    /// EndRun() is called if the program closes. Override Terminate() in scripting tools or use hooks to call from this event.
    /// </summary>
    /// <remarks> This event may not trigger if the program is force closed.</remarks>
    protected override void OnClosing(CancelEventArgs __args) { foreach (Component component in Awperative._TimeBasedComponents[Awperative.TimeEvent.Unload].ToList()) component.ChainEvent(Awperative.TimeEvent.Unload); base.OnClosing(__args); }
    
    
    
    
}