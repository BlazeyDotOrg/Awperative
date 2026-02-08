using System.Diagnostics;
using System.IO;


namespace Awperative;


public static partial class Debug
{
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void LogState(string __message) => LogGeneric(__message, "STA", [], []);
    
    
    
    /// <summary>
    /// Writes the current message to the log file. With any given call sign.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__parameters"> Names of values to debug</param>
    /// <param name="__values"> Values to debug</param>
    public static void LogState(string __message, string[] __parameters, string[] __values) => LogGeneric(__message, "STA", __parameters, __values);

    
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void LogValue(string __message) => LogGeneric(__message, "VAL", [], []);
    
    
    
    /// <summary>
    /// Writes the current message to the log file. With any given call sign.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__parameters"> Names of values to debug</param>
    /// <param name="__values"> Values to debug</param>
    public static void LogValue(string __message, string[] __parameters, string[] __values) => LogGeneric(__message, "VAL", __parameters, __values);

    
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void LogWarning(string __message) => LogGeneric(__message, "WAR", [], []);
    
    
    
    /// <summary>
    /// Writes the current message to the log file. With any given call sign.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__parameters"> Names of values to debug</param>
    /// <param name="__values"> Values to debug</param>
    public static void LogWarning(string __message, string[] __parameters, string[] __values) => LogGeneric(__message, "WAR", __parameters, __values);
    
    
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    public static void LogError(string __message) => LogGeneric(__message, "ERR", [], []);
    
    
    
    /// <summary>
    /// Writes the current message to the log file. With any given call sign.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__parameters"> Names of values to debug</param>
    /// <param name="__values"> Values to debug</param>
    public static void LogError(string __message, string[] __parameters, string[] __values) => LogGeneric(__message, "ERR", __parameters, __values);
    
    
    
    /// <summary>
    /// Writes the current message to the log file.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__condition"> Condition to debug </param>
    public static void AssertError(bool __condition, string __message) => AssertGeneric(__condition, __message, "ERR", [], []);
//todo: add more asserts and overrides

    
    
    
    
    /// <summary>
    /// Writes the current message to the log file. With any given call sign.
    /// </summary>
    /// <param name="__message"> Message to debug</param>
    /// <param name="__callSign"> Message identifier</param>
    /// <param name="__parameters"> Names of values to debug</param>
    /// <param name="__values"> Values to debug</param>
    public static void LogGeneric(string __message, string __callSign, string[] __parameters, string[] __values) {
        string output = "\n\n" + __callSign + "- \"" + __message + "\"\n         STK-" + new StackTrace();

        for (int i = 0; i < __parameters.Length || i < __values.Length; i++)
            output += "\n         " + __parameters[i] + "- " + __values[i];
        
        File.AppendAllText(LogFilePath, output);
    }
    
    
    public static void AssertGeneric(bool __condition, string  __message, string __callSign, string[] __parameters, string[] __values) {
        if (!__condition) return;
        
        string output = "\n\n" + __callSign + "- \"" + __message + "\"\n         STK-" + new StackTrace();

        for (int i = 0; i < __parameters.Length || i < __values.Length; i++)
            output += "\n         " + __parameters[i] + "- " + __values[i];
        
        File.AppendAllText(LogFilePath, output);
    }
        
}