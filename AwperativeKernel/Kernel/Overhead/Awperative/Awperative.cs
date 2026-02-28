using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace AwperativeKernel;

/// <summary>
/// Initiating class of Awperative. Call Start() to start the kernel.
/// </summary>
/// <author> Avery Norris </author>
public static partial class Awperative
{

    
    /// <summary>
    /// Current Version of Awperative
    /// </summary>
    public static string Version = "1.2C";



    /// <summary>
    /// Bottom class of Awperative. Contains the OpenTK Instance.
    /// </summary>
    [NotNull, UnsafeInternal] private static Base Base;

    
    
    /// <summary>
    /// List of all scenes currently loaded in the kernel. 
    /// </summary>
    [CalculatedProperty, CalculatedPropertyExpense("Very Low")]
    public static IReadOnlyList<Scene> Scenes => [.._scenes];
    [UnsafeInternal] internal static HashSet<Scene> _scenes { get; private set; } = [];


    
    /// <summary> Displays if Awperative has Started or not </summary>
    public static bool IsStarted { get; private set; } = false;
    /// <summary> Displays if the update loop is active</summary>
    public static bool IsRunning { get; private set; } = false;


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
    public static void CloseScene(Scene __scene) => _scenes.Remove(__scene);
    
    
    
    /// <summary>
    /// Closes a Scene
    /// </summary>
    /// <param name="__name"> Name of the scene</param>
    public static void CloseScene(string __name) => _scenes.Remove(GetScene(__name));
    

    
    
    
    /// <summary>
    /// Gets Awperative ready to begin! Compiles Component functions etc. Please call before doing anything Awperative
    /// related!
    /// </summary>
    public static void Start() {
        if(IsStarted) return;
        IsStarted = true;
        
        Debug.Initiate();
        
        //Load in all Components nd find the associated types.
        Debug.LogAction("Evaluating Components!");
        foreach (Type type in Assembly.GetCallingAssembly().GetTypes()) {
            if (type.IsSubclassOf(typeof(Component))) {


                Action<Component>[] timeEvents = new Action<Component>[ComponentEvents.Count];

                byte eventProfile = 0;
                List<string> debugProfile = [];

                for(int i = 0; i < ComponentEvents.Count; i++) {
                    MethodInfo eventMethod = type.GetMethod(ComponentEvents[i]);

                    if (eventMethod != null) {
                        
                        debugProfile.Add(ComponentEvents[i]);
                        
                        var ComponentInstanceParameter = Expression.Parameter(typeof(Component), "__component");
                        var Casting = Expression.Convert(ComponentInstanceParameter, type);
                        var Call = Expression.Call(Casting, eventMethod);
                        var Lambda = Expression.Lambda<Action<Component>>(Call, ComponentInstanceParameter);
                        timeEvents[i] = Lambda.Compile();
                        
                    } else timeEvents[i] = null;
                }
                
                

                Debug.LogAction("Evaluated Component! ", ["Type", "Time Events", "Profile"], [type.Name, "[" + string.Join(", ", debugProfile.Select(x => x.ToString())) + "]", eventProfile.ToString()]);
                _TypeAssociatedTimeEvents.Add(type, timeEvents);
            }
        }
        
        
    }
    
    internal static Comparer<Component> _prioritySorter = Comparer<Component>.Create((a, b) => {
        int result = b.Priority.CompareTo(a.Priority); 
        return (result != 0) ? result : a.GetHashCode().CompareTo(b.GetHashCode());
    });


    
    /// <summary>
    /// Starts Awperative up! This method runs forever.
    /// </summary>
    public static void Run() {
        if(!IsStarted && IsRunning) return;
        IsRunning = true;
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
    internal static Dictionary<Type, Action<Component>[]> _TypeAssociatedTimeEvents = [];


    //What to do if there is an error, keep in mind low and none still can have errors, because you are turning off validation checking
}