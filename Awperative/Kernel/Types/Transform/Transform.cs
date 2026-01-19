using System;
using Microsoft.Xna.Framework;


namespace Gravity.Kernel;

public sealed class Transform
{
    
    public event EventHandler<TransformModifyEvent> OnTransformChangedEvent;
    
    
    public Vector2 Origin {
        get => _origin; set { 
            if(!value.Equals(_origin))
                OnTransformChangedEvent?.Invoke(this, TransformModifyEvent.FromPosition(this, value)); _origin = value;
        } 
    }
    private Vector2 _origin = Vector2.Zero;

    public Vector2 Position {
        get => _position; set { 
            if(!value.Equals(_position))
                OnTransformChangedEvent?.Invoke(this, TransformModifyEvent.FromPosition(this, value)); _position = value;
        } 
    } 
    private Vector2 _position = Vector2.Zero;
    
    public float Depth {
        get => _depth; set { 
            if(!value.Equals(_depth))
                OnTransformChangedEvent?.Invoke(this, TransformModifyEvent.FromDepth(this, value)); _depth = value;
        }
    } 
    private float _depth = 0f;
    
    public float Rotation {
        get => _rotation; set {  
            if(!value.Equals(_rotation)) 
                OnTransformChangedEvent?.Invoke(this, TransformModifyEvent.FromRotation(this, value)); _rotation = value;
        } 
    } 
    private float _rotation = 0f;
    
    public Vector2 Scale {
        get => _scale; set { 
            if(!value.Equals(_scale))
                OnTransformChangedEvent?.Invoke(this, TransformModifyEvent.FromScale(this, value)); _scale = value;
        } 
    } 
    private Vector2 _scale = Vector2.One;

    public void Set(Vector2 __origin, Vector2 __position, float __depth, float __rotation, Vector2 __scale)
    {
        //todo: rename to previous and check names`
        var previous = Clone();
        bool changed = false;

        if (!_origin.Equals(_origin)) { _origin = __origin; changed = true; }
        if (!_position.Equals(_position)) { _position = __position; changed = true; }
        if (!_depth.Equals(_depth)) { _depth = __depth; changed = true; }
        if (!_rotation.Equals(_rotation)) { _rotation = __rotation; changed = true; }
        if (!_scale.Equals(_scale)) { _scale = __scale; changed = true; }

        if (changed)
            OnTransformChangedEvent?.Invoke(this, TransformModifyEvent.FromTransforms(this, previous));
    }
    
    public Transform() {}

    public Transform(Vector2 __origin, Vector2 __position, float __depth, float __rotation, Vector2 __scale) {
        Origin = __origin; Position = __position; Depth = __depth; Rotation = __rotation; Scale = __scale;
    }

    
    //todo: operators?

    public Transform Clone()
    {
        return new Transform(Origin, Position, Depth, Rotation, Scale);
    }
    
    public Matrix ToMatrix()
    {
        return
            Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(new Vector3(Scale, 1f)) *
            Matrix.CreateTranslation(new Vector3(Origin, 0f));
    }
}