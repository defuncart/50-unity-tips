# 02 - Extension Methods

*Extension Methods* allow us to add functionality to existing classes. Say for instance we want to set the X position of a Transform. We could write a private method within our class MyGameObject but as this is probably something that we could use across multiple classes, it makes more sense to create an Extension Method

```C#
public static void SetX(this Transform transform, float x)
{
  Vector3 position = transform.position;
  position.x = x;
  transform.position = position;
}
```

which can be called on any Transform in any class.

```C#
Transform myTransform.
myTransform.SetX(100);
```

As *transform.position* is a property and not a variable, Extension Methods like *SetX*, *SetY*, *MoveToInTime* etc. are extremely useful as they reduce duplicated lines of code, while making the code more understandable.

One issue with Extension Methods is that there are no automatic null checks. So if myTransform happens to be null, then a **NullReferenceException** will be thrown when trying to call SetX. Thus we need to explicitly check for null, if applicable.

```C#
public static void SetX(this Transform transform, float x)
{
  if(transform == null) { Debug.LogError("transform is null"); return; }

  Vector3 position = transform.position;
  position.x = x;
  transform.position = position;
}
```

```C#
public static bool IsNullOrEmpty(this string value)
{
  return string.IsNullOrEmpty(value);
}
```

If you are unfamiliar with Extension Methods, then I suggest you take a look [here](https://msdn.microsoft.com/pl-pl/library/windows/desktop/bb383977(v=vs.100).aspx), [here](https://unity3d.com/learn/tutorials/topics/scripting/extension-methods) and [here](http://www.alanzucconi.com/2015/08/05/extension-methods-in-c/), and start utilizing them within your projects!

Note that Extension Methods can be written inside any static class. Some developers like to have a single *ExtensionMethods.cs* file, others like to break the methods out into separate files, for instance *StringExtentions.cs*, *TransformExtensions.cs* etc.