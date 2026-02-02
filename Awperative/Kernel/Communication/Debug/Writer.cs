using System.Diagnostics;
using System.IO;


namespace Awperative;


public static partial class Debugger
{
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void DebugState(string __message) => DebugGeneric(__message, "STA");
    
    
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void DebugValue(string __message) => DebugGeneric(__message, "VAL");
    
    
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void DebugLog(string __message) => DebugGeneric(__message, "LOG");
    
    
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void DebugWarning(string __message) => DebugGeneric(__message, "WAR");
    
    
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void DebugError(string __message) => DebugGeneric(__message, "ERR");

    
    
    
    
    /// <summary>
    /// Writes the current message to the log file. With any given call sign.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__callSign"> Message identifier</param>
    public static void DebugGeneric(string __message, string __callSign) {
        File.AppendAllText(LogFilePath, "\n\n" + __callSign + "- \"" + __message + "\"\n         STK-" + new StackTrace());
    }
}