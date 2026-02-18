using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;



namespace AwperativeKernel;

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
    /// List of all scenes currently loaded in the kernel. 
    /// </summary>
    public static ImmutableArray<Scene> Scenes => [.._scenes];
    internal static HashSet<Scene> _scenes { get; private set; } = [];





    /// <summary>
    /// Creates a new Scene
    /// </summary>
    public static Scene CreateScene(string __name) {
        if (!ContainsScene(__name)) {
            Scene newScene = new Scene(__name);
            _scenes.Add(newScene);
            return newScene;
        } else Debug.LogError("Awperative already has a Scene with that name!", ["Scene", "Name"], [GetScene(__name).GetHashCode().ToString(), __name]); return null;
    }
    
    

    /// <summary>
    /// Finds a Scene from a given name
    /// </summary>
    /// <param name="__name"> Name to search for</param>
    /// <returns></returns>
    public static Scene GetScene(string __name) => _scenes.FirstOrDefault(scene => scene.Name == __name, null);
    
    
    
    /// <summary>
    /// Returns bool based on whether there a scene with the given name or not.
    /// </summary>
    /// <param name="__name"> Name of the Scene</param>
    /// <returns></returns>
    public static bool ContainsScene(string __name) => _scenes.Any(scene => scene.Name == __name);



    /// <summary>
    /// Closes a Scene
    /// </summary>
    /// <param name="__scene"> Scene to close</param>
    public static void CloseScene(Scene __scene) => Scenes.Remove(__scene);
    
    
    
    /// <summary>
    /// Closes a Scene
    /// </summary>
    /// <param name="__name"> Name of the scene</param>
    public static void CloseScene(string __name) => Scenes.Remove(GetScene(__name));
    

    
    
    
    /// <summary>
    /// Gets Awperative ready to roll!
    /// </summary>
    /// <param name="__hooks"> List of all event hooks you wish to use. </param>
    /// <remarks> You cannot add new hooks later; so make sure to register all of them in the Start() method.</remarks>
    public static void Start() {
        Debug.Initiate();
    }


    
    /// <summary>
    /// Starts Awperative up! This method runs forever.
    /// </summary>
    public static void Run() {
        Base = new Base();
        Base.Run();
    }
    
    
    
    
}