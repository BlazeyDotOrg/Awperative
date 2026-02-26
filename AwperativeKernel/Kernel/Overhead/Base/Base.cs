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
    protected override void OnLoad() { foreach(Scene scene in Awperative._scenes) scene.ChainEvent(0); base.OnLoad(); }
    


    /// <summary>
    /// EndRun() is called if the program closes. Override Terminate() in scripting tools or use hooks to call from this event.
    /// </summary>
    /// <remarks> This event may not trigger if the program is force closed.</remarks>
    protected override void OnClosing(CancelEventArgs __args) { foreach(Scene scene in Awperative._scenes) scene.ChainEvent(1); base.OnClosing(__args); }
    
    
    
    
    
    /// <summary>
    /// Update() is called every frame; before Draw(). Override Update() in scripting tools to call from this event.
    /// </summary>
    /// <remarks> Hooks are unable to receive both Update() and Draw()</remarks>
    protected override void OnUpdateFrame(FrameEventArgs __args) { foreach(Scene scene in Awperative._scenes) scene.ChainEvent(2); base.OnUpdateFrame(__args); }
    
    
    
    /// <summary>
    /// Draw() is called every frame; after Update(). Override Draw() in scripting tools to call from this event.
    /// </summary>
    /// <remarks> Hooks are unable to receive both Update() and Draw()</remarks>
    protected override void OnRenderFrame(FrameEventArgs __args) { foreach(Scene scene in Awperative._scenes) scene.ChainEvent(3); base.OnRenderFrame(__args); }
}