using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Awperative;

/// <summary>
/// Initiating class of Awperative. Call Start() to start the kernel.
/// </summary>
public static class Awperative
{
    //Inherits MonoGame and carries events.
    public static Base Base;
    public static List<Scene> LoadedScenes = [];
    
    //Handles, graphic Settings, drawing, and loading content respectively.
    public static GraphicsDeviceManager GraphicsDeviceManager { get; internal set; }
    public static SpriteBatch SpriteBatch { get; internal set; }
    public static ContentManager ContentManager { get; internal set; }
    
    //Entry points for code
    internal static List<AwperativeHook> EventHooks { get; private set; }

    /// <summary>
    /// Start() begins the game; and begins communication with all event hooks.
    /// </summary>
    /// <param name="__hooks"> List of all event hooks you wish to use. </param>
    /// <remarks> You cannot add new hooks later; so make sure to register all of them in the Start() method.</remarks>
    public static void Start(List<AwperativeHook> __hooks) {
        EventHooks = __hooks;
        
        Base = new Base();
        Base.Run();
    }
}