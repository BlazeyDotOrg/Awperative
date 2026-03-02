using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Awperative.Kernel.Overhead.Reflection;


namespace AwperativeKernel;

/// <summary>
/// Initiating class of Awperative. Call Start() to start the kernel.
/// </summary>
/// <author> Avery Norris </author>
public static partial class Awperative
{

    
    /// <summary> Current Version of Awperative </summary>
    public static string Version = "1.2C";



    /// <summary> Bottom class of Awperative. Contains the OpenTK Instance. </summary>
    [DebugAttributes.NotNull, MarkerAttributes.UnsafeInternal] private static Base Base;

    
    
    /// <summary> List of all scenes currently loaded in the kernel. </summary>
    [MarkerAttributes.CalculatedProperty]
    public static IReadOnlySet<Scene> Scenes => _scenes;
    [MarkerAttributes.UnsafeInternal] internal static HashSet<Scene> _scenes { get; private set; } = [];


    
    /// <summary> Displays if Awperative has Started or not </summary>
    public static bool IsStarted { get; private set; } = false;
    /// <summary> Displays if the update loop is active</summary>
    public static bool IsRunning { get; private set; } = false;
    
    
    
    /// <summary> Awperative's debugger of choice, found from the module manager.</summary>
    [MarkerAttributes.UnsafeInternal, DependencyAttributes.RequiredModule]
    public static IDebugger Debug { get; internal set; }

    

    /// <summary> Creates a new Scene with the given name </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static Scene CreateScene([DebugAttributes.NotNull, DebugAttributes.SceneDoesNotExist] string __name) {
        if (!DebugAttributes.NotNull.VerifyOrThrow(__name)) return null;
        if (!DebugAttributes.SceneDoesNotExist.VerifyOrThrow(GetScene(__name))) return null;
        
        Scene newScene = new Scene(__name);
        _scenes.Add(newScene);
        return newScene;
    }
    
    
    
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static void AddScene([DebugAttributes.SceneNotNull, DebugAttributes.SceneDoesNotExist] Scene __scene) {
        if (!DebugAttributes.SceneNotNull.VerifyOrThrow(__scene)) return;
        if (!DebugAttributes.SceneDoesNotExist.VerifyOrThrow(__scene)) return;
        
        _scenes.Add(__scene);
    }



    /// <summary> Finds a Scene from a given name </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static Scene GetScene([DebugAttributes.NotNull] string __name) => !DebugAttributes.NotNull.VerifyOrThrow(__name) ? null : _scenes.FirstOrDefault(scene => scene.Name == __name, null);



    /// <summary> Returns bool based on whether there a scene with the given name or not. </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static bool ContainsScene(string __name) => _scenes.Any(scene => scene.Name == __name);



    /// <summary> Closes a Scene </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static void CloseScene(Scene __scene) => _scenes.Remove(__scene);
    
    
    
    /// <summary> Closes a Scene </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static void CloseScene(string __name) => _scenes.Remove(GetScene(__name));





    /// <summary> Gets Awperative ready to begin! Compiles Component functions etc. Please call before doing anything Awperative related! </summary>
    public static void Start(string moduleManagerPath) {
        if (IsStarted) return;
        IsStarted = true;

        ReflectionManager.ResolveModules(AppDomain.CurrentDomain.GetAssemblies());
        
        Debug.LogAction("Successfully Compiled Classes!");
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
    //Remove, 5
    
    // 0000 0000
    //

    public static void TestUpdate() {
        foreach (Scene scene in Scenes) {
            scene.ChainEvent(2);
        }
    }
    
    public static void TestDraw() {
        foreach (Scene scene in Scenes) {
            scene.ChainEvent(3);
        }
    }
    


    /// <summary>
    /// List of all type of components and the associated time events
    /// Each event is a 0 or 1 based on true or false, stored at their index in the byte
    /// </summary>


    //What to do if there is an error, keep in mind low and none still can have errors, because you are turning off validation checking
}