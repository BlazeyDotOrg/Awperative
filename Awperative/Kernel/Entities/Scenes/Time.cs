using Microsoft.Xna.Framework;


namespace Awperative;

public sealed partial class Scene
{

    public void Terminate() {
        foreach (Behavior behavior in behaviors) behavior.Terminate();
        foreach (Body body in bodies) body.Terminate();
    }

    public void Load() {
        foreach (Behavior behavior in behaviors) { behavior.Load(); }
        foreach (Body body in bodies) { body.Load(); }
    }

    public void Update(GameTime __gameTime) {
        foreach (Behavior behavior in behaviors) { behavior.Update(__gameTime); }
        foreach (Body body in bodies) { body.Update(__gameTime); }
    }

    public void Draw(GameTime __gameTime) {
        foreach (Behavior behavior in behaviors) { behavior.Draw(__gameTime); }
        foreach (Body body in bodies) { body.Draw(__gameTime); }
    }
}