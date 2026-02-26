using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;


namespace AwperativeKernel;

/// <summary>
/// Initiating class of Awperative. Call Start() to start the kernel.
/// </summary>
/// <author> Avery Norris </author>
public static class Awperative
{

    
    /// <summary>
    /// Current Version of Awperative
    /// </summary>
    public static string Version = "1.2B";
    
    
    
    /// <summary>
    /// Bottom class of Awperative. Contains the MonoGame instance.
    /// </summary>
    public static Base Base { get; internal set; }
    

    
    /// <summary>
    /// List of all scenes currently loaded in the kernel. 
    /// </summary>
    public static ImmutableArray<Scene> Scenes => [.._scenes];
    internal static HashSet<Scene> _scenes { get; private set; } = [];



    public static bool IsRunning { get; private set; } = false;
    public static bool IsStarted { get; private set; } = false;



    public static bool DebugMode = false;
    
    
            
            
            
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
        
        //Load in all Components nd find the associated types.
        Debug.LogAction("Evaluating Components!");
        foreach (Type type in Assembly.GetCallingAssembly().GetTypes()) {
            if (type.IsSubclassOf(typeof(Component))) {

                byte eventProfile = 0;
                List<string> debugProfile = [];

                for(int i = 0; i < ComponentEvents.Count; i++) {
                    if (type.GetMethod(ComponentEvents[i]) != null) {
                        eventProfile |= (byte)(1 << i);
                        debugProfile.Add(ComponentEvents[i]);
                    }
                }
                
                

                Debug.LogAction("Evaluated Component! ", ["Type", "Time Events", "Profile"], [type.Name, "[" + string.Join(", ", debugProfile.Select(x => x.ToString())) + "]", eventProfile.ToString()]);
                _TypeAssociatedTimeEvents.Add(type, eventProfile);
            }
        }
    }


    
    /// <summary>
    /// Starts Awperative up! This method runs forever.
    /// </summary>
    public static void Run() {
        Base = new Base();
        Base.Run();
    }
    
    //Load, 0
    //Unload, 1
    //Update, 2
    //Draw 3
    //Create, 4
    //Destroy, 5
    
    // 0000 0000
    //


    internal static ReadOnlyCollection<string> ComponentEvents = new(["Load", "Unload", "Update", "Draw", "Create", "Destroy"]);



    /// <summary>
    /// List of all type of components and the associated time events
    /// Each event is a 0 or 1 based on true or false, stored at their index in the byte
    /// </summary>
    internal static Dictionary<Type, byte> _TypeAssociatedTimeEvents = [];
}