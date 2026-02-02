using System.Linq;
using Microsoft.Xna.Framework;


namespace Awperative;

public sealed partial class Scene
{

    public void Unload() {
        foreach (Behavior behavior in behaviors.ToList()) behavior.Unload();
        foreach (Body body in bodies.ToList()) body.Unload();
    }

    public void Load() {
        foreach (Behavior behavior in behaviors.ToList()) { behavior.Load(); }
        foreach (Body body in bodies.ToList()) { body.Load(); }
    }

    public void Update(GameTime __gameTime) {
        foreach (Behavior behavior in behaviors.ToList()) { behavior.Update(__gameTime); }
        foreach (Body body in bodies.ToList()) { body.Update(__gameTime); }
    }

    public void Draw(GameTime __gameTime) {
        foreach (Behavior behavior in behaviors.ToList()) { behavior.Draw(__gameTime); }
        foreach (Body body in bodies.ToList()) { body.Draw(__gameTime); }
    }
}