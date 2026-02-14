# Awperative Behaviors

---


Behaviors are the main innovation in Awperative's take on a modern, unbiased
**ECS** [(Entity Component System)](https://en.wikipedia.org/wiki/Entity_component_system); traditionally, 
an Entity Component system involves 2/3 types of data.

    Parent Scene/World :
        
        Children GameObjects/Actors :
            
            Children Components/Scripts

 - **Components** are what we actually care about, shockingly, they are the
"Component" in Entity Component System, and you can think about them as the actual
scripts or program in your Game, but a little more object oriented.

   
 - **GameObjects** are the parents components are glued to, in most Game Libraries and Engines
they are also treated as physical objects; and often given properties like **Transform** and **Tags**. 
GameObjects can also commonly nest in each other. I find it useful *(Especially in Awperative)* to view
these as Directories for components.

   

 - **Scenes** are basically the worlds these objects inhabit. Common *synonyms* are Worlds or Levels. If GameObjects
are files in our analogy, then scenes are separate hard drives. The scene's jobs mainly center around being a
container. *(Rather than an object of functionality)*

**Keep in mind** that this is a general description of **other** Game Development Platforms. It's impossible to summarize
every single **ECS** in the world. 

## How Awperative Differs

As of this current version, Awperative's **ECS** has taken on a different form than most.

    Parent Scene/World

        Behaviors :

            Children Behaviors

One of the main **Awperative Principles** is **Generalization**; and during development it became clear
GameObjects are unnecessary, which caused them to be replaced by: the **Behavior**.

**Behaviors** are a combination of the **GameObjects** and **Components** we discussed earlier. Awperative 
does not implement many fancy features out of the box; because of that the traditionally useful GameObjects became
obsolete. Objects are also not built to be flexible like Components, leaving empty, nearly static objects floating in
our Scenes.

Because of this it was decided to make a more flexible type of entity that can act as GameObject and Component at once.
However, Behaviors still do not implement many features out of the box, instead we use their **expandability** to our advantage.

## How To Use

Behaviors are rather easy to control, similar to Engines like Unity, you can make your own custom script by inheriting the abstract "**Behavior**" class.

    public class MyScript : Behavior {}

On the surface level, Behaviors provide you with your current scene and parents.

They also give very handy Game Events which are present in 99% of Game Development Platforms.

    public virtual void Load();
    public virtual void Unload();

    public virtual void Update();
    public virtual void Draw();

    public virtual void Create();
    public virtual void Destroy();

 - Load and Unload provides a call when the Game is opened and closed respectively, please be wary: these will not call under a force close.



 - Update and Draw both trigger each frame, starting with Update. It is recommended to put your non-graphics related
   code there; and vice versa for Draw.


 - Finally Create and Destroy are called when the Behavior is spawned In/Out. It should be noted that Create is called **after** the constructor.
 If you try to make certain references or calls in the constructor it may not fully work, as the object is half instantiated. It is recommended to use Create when possible. Also, Destroy will not be called if the program is closed, just Unload.

If you want to hook onto any of these methods it is quite simple using the override keyword.

    public override void Update() {}
   
Putting this code inside any Behavior inheriting class, will create a method that gets called every single frame, just like that!

For any further documentation, please refer to the API section of our glorious website!

## Examples and Good Practice

First let's see how we can recreate typical GameObject behavior. I would most recommend using **Nested Behaviors** to acheive this.
If we pretend we have implemented a few modules for basic transform profiles and sprite management, then we can easily make a basic movable sprite object.
Like so :

    Parent Scene/World
        
        Empty Behavior :

            Transform Behavior

            Sprite Behavior

We can expand upon this easily as well. Say we want to make it into a moveable player character, we can modify the Empty behavior to
carry some additional functionality.

    Parent Scene/World
        
        Player Controller Behavior : <--

            Transform Behavior

            Sprite Behavior

If we want to give it a hitbox.

    Parent Scene/World
        
        Player Controller Behavior :

            Transform Behavior

            Sprite Behavior

            Hitbox Behavior <--

And maybe let's say we want to scale or offset that

    Parent Scene/World
        
        Player Controller Behavior :

            Transform Behavior

            Sprite Behavior

            Hitbox Behavior

                Transform Behavior <--

Of course, there is some additional programming that would be needed between each step.
**(Ex. Hitboxes listening to the child transform)**, but you can see how this data structure
builds intuitively. 

I would recommend compartmentalizing any repeating pieces of code or types into a **Behavior**. It is also
not immediately obvious at first, But I would say one of the largest utilities from an **object free component** system is the ability
to function at a high level in the scene.

Often times in component-object **ECS**'  you will have a static object/s that stores important one off Behaviors, such as the *Camera*,
or the *Game's Asset Loader*. I've always found this to be unsatisfying. Luckily because Behaviors can operate as standalone objects
you can instead insert a standalone Camera Behavior into the scene. Which makes more logical and grammatical sense.

    Parent Scene/World
        
        Camera

        Asset Loader

## Under the Hood

As you've seen part of what makes **Behaviors** so great is the fact that they can nest within themselves infinitely.
This is possible because of an essential piece known as the **Docker**.

Dockers are seen everywhere in Awperative. They are built to store   child Behaviors. The Behavior class carries a Docker like so.

    Behavior : Docker

Dockers also provide the Add, Get and Remove functions we all know and love, along with the list of child Behaviors. 
It is also responsible for Awperative Events being passed through the Scene, for Example, an Update call would look like this

    Scene    -> Docker        -> Component(Docker's child)
    Update() -> ChainUpdate() -> Update()

Of course, the Update call would be redirected to a different spot if you were to override it, but the idea stays the same.

For more details, please look at Docker in the API and the file in Awperative!
## Specialized Behaviors

Because of its focus on **Expandability**, Awperative also allows **third-party** Behaviors to enter the Scene.
This allows for specialized behavior or streamlined development, if you are willing to make assumptions.

Let's imagine you are making an enemy for an RPG and your Behavior Layout looks somewhat like this

    Parent Scene/World
        
        Enemy Pathfinding Behavior :

            Transform Behavior

            Sprite Behavior

            Hitbox Behavior

            Health Behavior

            UNIQUE BEHAVIOR

In this diagram, **UNIQUE BEHAVIOR** is just a placeholder for any future or enemy specific behaviors, and it is critical to our example.
And let's imagine that any time you add an enemy, then you will probably be adding some sort of "UNIQUE Behavior" there; And this unique behavior often
uses aspects from the others present.

In a scenario like this, it would likely behoove you to create a specialized behavior type. As mentioned earlier, Behaviors are identified by inheriting the
abstract Behavior class. But it is possible to build an in-between class for additional functionality.

 - Traditional Pattern

    
    Your Script : Behavior : Docker

 - Example Specialized Pattern


    Your Script : EnemyBehavior : Behavior : Docker

As you can see we inherited through EnemyBehavior rather than Behavior. This is perfectly legal; and intended!
You can do virtually anything in your specialized behavior in-between. I most recommend making use of **lambda** in situations like this.

For instance, you can provide simpler access to another Behavior like so

    int Health => Parent.Get<Health>().Health;

Any future EnemyBehaviors can simply put "**Health**" and it will correctly retrieve the dynamic value.

I should mention that this power comes with **great responsibility**. In this case: using EnemyBehavior without a Health Behavior
will cause logged errors, and possibly a runtime error/halt.

---

---
# End Of Documentation
### Written By Avery Norris
