using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Awperative;

/// <summary>
/// Initiating class of Awperative. Call Start() to start the kernel.
/// </summary>
/// <author> Avery Norris </author>
public static class Awperative
{
    
    
    
    /// <summary>
    /// Bottom class of Awperative. Contains the MonoGame instance.
    /// </summary>
    public static Base Base { get; internal set; }
    
    
    
    
    
    /// <summary>
    /// Handles graphics settings through MonoGame.
    /// </summary>
    public static GraphicsDeviceManager GraphicsDeviceManager { get; internal set; }
    
    
    
    /// <summary>
    /// Handles drawing sprites to the screen through MonoGame.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; internal set; }
    
    
    
    /// <summary>
    /// Handles loading content through MonoGame.
    /// </summary>
    public static ContentManager ContentManager { get; internal set; }





    /// <summary>
    /// List of all scenes currently loaded in the kernel. 
    /// </summary>
    public static List<Scene> LoadedScenes => _loadedScenes.ToList();
    internal static HashSet<Scene> _loadedScenes { get; private set; }= [];
    
    
    
    /// <summary>
    /// List of all event hooks currently loaded in the kernel.
    /// </summary>
    public static List<AwperativeHook> EventHooks => _eventHooks.ToList();
    internal static HashSet<AwperativeHook> _eventHooks { get; private set; } = [];
    

    
    
    
    /// <summary>
    /// Start() begins the game; and begins communication with all event hooks.
    /// </summary>
    /// <param name="__hooks"> List of all event hooks you wish to use. </param>
    /// <remarks> You cannot add new hooks later; so make sure to register all of them in the Start() method.</remarks>
    public static void Start(List<AwperativeHook> __hooks) {
        
        _eventHooks = new HashSet<AwperativeHook>(__hooks);
        
        Base = new Base();
        Base.Run();
    }
    
    
    
    
}