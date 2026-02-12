using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Awperative;


/// <summary>
/// Base class of Awperative. Carries events from MonoGame into scenes and hooks.
/// </summary>
/// <author> Avery Norris </author>
public sealed class Base : Game
{

    /// <summary>
    /// Start of Awperative. Please do not try to call this.
    /// </summary>
    internal Base() {
        Awperative.GraphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }
    
    
    
    
    
    /// <summary>
    /// Initialize() is called when the program starts. Goes before LoadContent(). And prepares the kernel for use.
    /// </summary>
    /// <remarks> It is recommended not to load content in Initialize()</remarks>
    protected override void Initialize() {
        Awperative.ContentManager = Content;
        Awperative.SpriteBatch = new SpriteBatch(GraphicsDevice);
        
        base.Initialize();
    }
    
    
    
    

    /// <summary>
    /// LoadContent() is called when the program starts; right after Initialize(). Override Load() in scripting tools or use hooks to call from this event.
    /// </summary>
    /// <remarks> It is recommended to load content during LoadContent()</remarks>
    protected override void LoadContent() {
        foreach (AwperativeHook hook in Awperative.EventHooks.ToList()) hook.Load();
        foreach(Scene scene in Awperative.LoadedScenes.ToList()) scene.ChainLoad();
    }
    
    
    
    
    
    /// <summary>
    /// Update() is called every frame; before Draw(). Override Update() in scripting tools to call from this event.
    /// </summary>
    /// <remarks> Hooks are unable to receive both Update() and Draw()</remarks>
    protected override void Update(GameTime __gameTime) {
        foreach(Scene scene in Awperative.LoadedScenes.ToList()) scene.ChainUpdate(__gameTime);
        base.Update(__gameTime);
    }

    
    
    
    
    /// <summary>
    /// Draw() is called every frame; after Update(). Override Draw() in scripting tools to call from this event.
    /// </summary>
    /// <remarks> Hooks are unable to receive both Update() and Draw()</remarks>
    protected override void Draw(GameTime __gameTime) {
        foreach(Scene scene in Awperative.LoadedScenes.ToList()) scene.ChainDraw(__gameTime);
        base.Draw(__gameTime);
    }
    
    
    
    

    /// <summary>
    /// EndRun() is called if the program closes. Override Terminate() in scripting tools or use hooks to call from this event.
    /// </summary>
    /// <remarks> This event may not trigger if the program is force closed.</remarks>
    protected override void EndRun() {
        foreach (AwperativeHook hook in Awperative.EventHooks.ToList()) hook.Unload();
        foreach (Scene scene in Awperative.LoadedScenes.ToList()) scene.ChainUnload();
    }
    
    
    
    
}