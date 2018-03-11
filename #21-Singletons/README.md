# 21 - Singletons

The *singleton pattern* is a design pattern that restricts the instantiation of a class to a single, globally accessible instance. This is particularly useful when a single instance is needed to coordinate actions across an entire project. The benefits of this approach are clear:
1. we have a global pointer which we do not need to tediously pass to all classes who need to reference it.
2. as the class is initialized at runtime, it can utilize runtime information (unlike static classes).
3. the class can be lazily instantiated, that is, only created once the instance is first needed. This can be quite helpful for resource-heavy classes. Static classes are created when first loaded.

The singleton pattern is often used in games as 'Managers', for instance GameManager, AudioManager, LocalizationManager, however it is often abused and overused due to lazyness, lack of OOP understanding or poor code design. There are tons of articles and discussions on this issue, with the general consensus of using the pattern *sparingly*.

When deciding whether to use this pattern, it is worthwhile to consider if **1)** a static class could be instead utilized or **2)** if the code could be incorporated into another class.

A class of constant variables that need to be global? Static members of a static class. An AnalyticsManager that sends custom analytic events? A static class with static methods.

Moreover, in the following example adapted from Robert Nystrom, we have a *Bullet* class and *BulletManager*. As the game has many bullets, we probably need a single-instance *BulletManager* right?

```C#
public class Bullet
{
  public int x { get; set; }
  public int y { get; set; }
}

public class BulletManager
{
  public Bullet Create(int x, int y)
  {
    Bullet bullet = new Bullet();
    bullet.x = x; bullet.y = y;
    return bullet;
  }

  public bool IsOnScreen(Bullet bullet)
  {
    return bullet.x >= 0 && bullet.x < Screen.width && bullet.y >= 0 && bullet.y < Screen.height;
  }

  public void Move(Bullet bullet)
  {
    bullet.x += 5;
  }
}
```

Actually no. *BulletManager* is simply a poorly designed helper class whose functionality could easily be incorporated into the *Bullet* class itself.

```C#
public class Bullet
{
  public int x { get; set; }
  public int y { get; set; }

  public bool isOnScreen
  {
    get { return x >= 0 && x < Screen.width && y >= 0 && y < Screen.height; }
  }

  public void Move()
  {
    x += 5;
  }
}
```

There are sometimes, however, when I utilize the singleton pattern, for instance when saving/loading player data to disk and when classes need to reference data (LocalizationManager, AudioManager etc.). A *MonoBehaviour* class can easily be turned into a singleton by extending

```C#
/// <summary>A base abstract class which can be extented to make a singleton component attachable to a game object.</summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
  /// <summary>A static instance which is created on first lauch and thereafter never destroyed.</summary>
  public static T instance { get; private set; }

  /// <summary>Callback when the instance is awoken.
  /// Ensure that there is only one instance of the class and that it cannot be destroyed.</summary>
  private void Awake()
  {
    if(instance == null) { instance = this as T; DontDestroyOnLoad(gameObject); instance.Init(); }
    else if(instance != this) { Destroy(gameObject); }
  }

  /// <summary>Init the specific inherited class.</summary>
  protected virtual void Init() {}
}
```

while a class saved to disk using [*BinarySerialization*](https://github.com/defuncart/50-unity-tips/tree/master/%2307-BinarySerialization) can be extended from

```C#
[System.Serializable]
public abstract class SerializableSingleton<T> where T : class
{
  /// <summary>The class's name.</summary>
  protected static string className
  {
    get { return typeof(T).Name; }
  }

  protected static T _instance; //backing variable for instance
  /// <summary>A computed property that returns a static instance of the class.
  /// If the instance hasn't already been loaded, then it is loaded from File.</summary>
  public static T instance
  {
    get { return _instance ?? (_instance = BinarySerializer.Load<T>(className)); }
  }

  /// <summary>As the object's constructor is private, this method allows the creation of
  /// the object. Only creates the object if one isn't already saved to disk.</summary>
  public static void Create()
  {
    if(!BinarySerializer.FileExists(className))
    {
      _instance = (T)System.Activator.CreateInstance(type: typeof(T), nonPublic: true);
    }
  }

  /// <summary>Saves the current instance to file.</summary>
  protected void Save()
  {
    BinarySerializer.Save(className, this);
  }
}
```

In short, although singletons are generally overused and abused, they are still sometimes a viable design pattern. In future tips I will show how I utilize them within my projects.

## Further Reading
[Singleton Pattern](https://en.wikipedia.org/wiki/Singleton_pattern)

[Game Programming Patterns: Singleton](http://gameprogrammingpatterns.com/singleton.html)

[Service Locator Pattern](https://en.wikipedia.org/wiki/Service_locator_pattern)

[What is so bad about singletons?](https://stackoverflow.com/a/138012)

[On Design Patterns: When to use the Singleton?](https://stackoverflow.com/a/228380)
