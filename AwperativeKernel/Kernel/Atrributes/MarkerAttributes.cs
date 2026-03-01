using System;


namespace AwperativeKernel;


public static class MarkerAttributes
{
    
    /// <summary>
    /// Shows that the given object is unsafe (ex. it doesn't check for null values and such, or it doesn't have guardrails based on cases).
    /// This is just for internal/private methods to remind myself how to call it :) The reasoning is case by case, but most of the time,
    /// it is because all of the exposing public methods already check, and double checks would only slow me down
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class UnsafeInternal : Attribute { }



    /// <summary>
    /// Shows that the given object (meant for external use) is calculated every time it is called! Good to know for performance heavy systems.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CalculatedProperty() : Attribute { }



    /// <summary>
    /// Just a way to write how expensive a calculated property can be.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CalculatedPropertyExpense(string Expense) : Attribute { }
}