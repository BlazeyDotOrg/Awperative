using System.Collections.Generic;
using System.Collections.Immutable;
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
    public static ImmutableArray<Scene> Scenes => [.._scenes];
    internal static HashSet<Scene> _scenes { get; private set; } = [];

    
    
    

    /// <summary>
    /// Creates a new Scene
    /// </summary>
    public static void Create(string __name) { if (Contains(__name)) _scenes.Add(new Scene(__name)); else Debug.LogError("Awperative already has a Scene with that name!", ["Scene", "Name"], [Get(__name).GetHashCode().ToString(), __name]); }
    
    

    /// <summary>
    /// Finds a Scene from a given name
    /// </summary>
    /// <param name="__name"> Name to search for</param>
    /// <returns></returns>
    public static Scene Get(string __name) => _scenes.FirstOrDefault(scene => scene.Name == __name, null);
    
    
    
    /// <summary>
    /// Returns bool based on whether there a scene with the given name or not.
    /// </summary>
    /// <param name="__name"></param>
    /// <returns></returns>
    public static bool Contains(string __name) => _scenes.Any(scene => scene.Name == __name);



    /// <summary>
    /// Closes a Scene
    /// </summary>
    /// <param name="__scene"></param>
    public static void Close(Scene __scene) => Scenes.Remove(Get(__scene.Name));
    

    
    
    
    /// <summary>
    /// Start() begins the game; and begins communication with all event hooks.
    /// </summary>
    /// <param name="__hooks"> List of all event hooks you wish to use. </param>
    /// <remarks> You cannot add new hooks later; so make sure to register all of them in the Start() method.</remarks>
    public static void Start() {
        Base = new Base();
        Base.Run();
    }
    
    
    
    
}