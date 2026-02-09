

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Awperative;

public sealed partial class Scene : DockerEntity
{
    
    public List<Body> bodies { get; private set; } = [];

    public void Unload() {
        foreach (Component component in _components) component.Unload();
        foreach (Body body in bodies.ToList()) body.Unload();
    }

    public void Load() {
        foreach (Component component in _components) component.Load();
        foreach (Body body in bodies.ToList()) { body.Load(); }
    }

    public void Update(GameTime __gameTime) {
        foreach (Component component in _components) component.Update(__gameTime);
        foreach (Body body in bodies.ToList()) { body.Update(__gameTime); }
    }

    public void Draw(GameTime __gameTime) {
        foreach (Component component in _components) component.Draw(__gameTime);
        foreach (Body body in bodies.ToList()) { body.Draw(__gameTime); }
    }
   
    //todo: add scene.destroy in v5
}
