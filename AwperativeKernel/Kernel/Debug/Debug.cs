using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AwperativeKernel;


public static class Debug
{
        
        
        
    /// <summary>
    /// True path of the log file Awperative dumps to.
    /// </summary>
    public static string LogFilePath { get; private set; }
        
        
        
    /// <summary>
    /// Target name of the log file
    /// </summary>
    public static string LogFileName { get; private set; } = "Log";


        
    /// <summary>
    /// If this is running or not
    /// </summary>
    private static bool Running = false;
        
        
        
    //Whether to throw error exceptions
    public static bool ThrowExceptions { get; set; } = false;
    //Whether to ignore/pass through errors or not
    public static bool IgnoreErrors { get; set; } = false;
    //Whether to debug errors at all
    public static bool DebugErrors { get; set; } = true;
        
        
        
    public static SafetyLevel safetyLevel {
        get => _safetyLevel; 
        set {
            ThrowExceptions = value is SafetyLevel.Extreme;
            IgnoreErrors = value is SafetyLevel.Low or SafetyLevel.None;
            DebugErrors = value is not SafetyLevel.None;
            _safetyLevel = value;
        }
    } private static SafetyLevel _safetyLevel;

    public enum SafetyLevel {
        Extreme, //Throw exceptions and stop the whole program
        Normal, //Just debug it to the console, and halt current process
        Low, //Push through tasks but debug error
        None, //Ignore most/all errors and do not debug it,
    }
        
        
        
    /// <summary>
    /// Sets up the Awperative debugger and finds the log file.
    /// </summary>
    public static void Start() {
        if(Running) return;

        Running = true;
            
        string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if(directoryPath == null) throw new Exception("Failed to get directory path!");
            
        LogFilePath = Path.Join(directoryPath, LogFileName + ".awlf");
            
        if(!Directory.GetFiles(directoryPath).Contains(LogFileName + ".awlf")) { File.Create(LogFilePath).Close(); }
    }

    public static void Stop() {
        Running = false;
    }

    public static void LogAction(string __message) {
        LogGeneric(__message, "ACT", [], [], false);
    }
        
    public static void LogAction(string __message, IEnumerable<string> __values, IEnumerable<string> __args) {
        LogGeneric(__message, "ACT", __values, __args, false);
    }
        
        
        
    public static void LogWarning(string __message) {
        LogGeneric(__message, "WRN", [], [], false);
    }
    
    public static void LogWarning(string __message, IEnumerable<string> __values, IEnumerable<string> __args) {
        LogGeneric(__message, "WRN", __values, __args, false);;
    }
        
        
        
    public static void LogError(string __message) {
        LogGeneric(__message, "ERR", [], [], true);
    }
    
    public static void LogError(string __message, IEnumerable<string> __values, IEnumerable<string> __args) {
        LogGeneric(__message, "ERR", __values, __args, true);
    }


    /// <summary>
    /// Writes the current message to the log file. With any given call sign.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__callSign"> Message identifier</param>
    /// <param name="__parameters"> Names of values to debug</param>
    /// <param name="__values"> Values to debug</param>
    /// <param name="__exception"> Should this throw an exception instead</param>
    public static void LogGeneric(string __message, string __callSign, IEnumerable<string> __parameters, IEnumerable<string> __values, bool __exception) {
        string output = "\n\n" + __callSign + "- \"" + __message + "\"\n         STK-" + new StackTrace();

        for (int i = 0; i < __parameters.Count() || i < __values.Count(); i++)
            output += "\n         " + __parameters.ElementAt(i) + "- " + __values.ElementAt(i);

        if (__exception && ThrowExceptions) throw new Exception(output);
            
        File.AppendAllText(LogFilePath, output);
    }
}