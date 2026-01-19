using Microsoft.Xna.Framework;


namespace Gravity.Kernel;

public sealed record TransformModifyEvent
{
    public readonly Transform before;
    public readonly Transform after;

    internal TransformModifyEvent() {}
    
    internal TransformModifyEvent(Transform __before, Transform __after)
    {
        before = __before;
        after = __after;
    }
    
    internal static TransformModifyEvent FromTransforms(Transform __previous, Transform __after)
    {
        Transform before = __previous;
        Transform after = new Transform(__after.Origin, __after.Position, __after.Depth, __after.Rotation, __after.Scale);
        return new TransformModifyEvent(before, after);
    }

    internal static TransformModifyEvent FromOrigin(Transform __previous, Vector2 __origin)
    {
        Transform before = __previous;
        Transform after = new Transform(__origin, __previous.Position, __previous.Depth, __previous.Rotation, __previous.Scale);
        return new TransformModifyEvent(before, after);
    }
    
    internal static TransformModifyEvent FromPosition(Transform __previous, Vector2 __position)
    {
        Transform before = __previous;
        Transform after = new Transform(__previous.Origin, __position, __previous.Depth, __previous.Rotation, __previous.Scale);
        return new TransformModifyEvent(before, after);
    }
    
    internal static TransformModifyEvent FromDepth(Transform __previous, float __depth)
    {
        Transform before = __previous;
        Transform after = new Transform(__previous.Origin, __previous.Position, __depth, __previous.Rotation, __previous.Scale);
        return new TransformModifyEvent(before, after);
    }
    
    internal static TransformModifyEvent FromRotation(Transform __previous, float __rotation)
    {
        Transform before = __previous;
        Transform after = new Transform(__previous.Origin, __previous.Position, __previous.Depth, __rotation, __previous.Scale);
        return new TransformModifyEvent(before, after);
    }
    
    internal static TransformModifyEvent FromScale(Transform __previous, Vector2 __scale)
    {
        Transform before = __previous;
        Transform after = new Transform(__previous.Origin, __previous.Position, __previous.Depth, __previous.Rotation, __scale);
        return new TransformModifyEvent(before, after);
    }
}