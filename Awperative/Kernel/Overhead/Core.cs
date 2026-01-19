using System.Collections.Generic;


namespace Gravity.Kernel;


public static class Core
{
    public static Base Base;
    public static List<Scene> LoadedScenes => Base.LoadedScenes;
    
    public static List<AwperativeHook> ScriptingHooks;

    //hooks are called in order
    public static void Start(List<AwperativeHook> __hooks) {
        ScriptingHooks = __hooks;
        
        Base = new Base();
        Base.Run();
    }
}