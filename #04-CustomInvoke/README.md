# 04 - Custom Invoke

```public void Invoke(string methodName, float time)``` allows one to trigger a given method (via string methodName) after a delay of time seconds. However there are two notable issues:

1. Invoke uses reflection which can have a large overhead and should be avoided when possible.
2. Invoke methods are difficult to debug as methods called by name are hard to track in code.

A better Invoke can be acomplished by using Coroutines and Actions:

```c#
public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
{
  return monoBehaviour.StartCoroutine(InvokeImplementation(action, time));
}

private static IEnumerator InvokeImplementation(Action action, float time)
{
  yield return new WaitForSeconds(time);
  action();
}
```

which can be easily called within any class that extends MonoBehaviour

```C#
private void Test()
{
  Debug.Log("Test using custom invoke");
}

this.Invoke(Test2, 3f);

this.Invoke(() => {
  Debug.Log("Test using closure");
}, 4f);
```

This custom Invoke can easily be cancelled by holding a reference to its returned coroutine.

```C#
public static void CancelInvoke(this MonoBehaviour monoBehaviour, Coroutine coroutine)
{
  monoBehaviour.StopCoroutine(coroutine);
}

Coroutine coroutine = this.Invoke(Test, 10f);
this.CancelInvoke(coroutine);
```

Custom Invoke could be further extended with the ability to include parameters too, although this isn't something I've found a use for yet.

```C#
public static Coroutine Invoke<T>(this MonoBehaviour monoBehaviour, Action<T> action, T parameter, float time) where T : class
{
  return monoBehaviour.StartCoroutine(InvokeImplementation(action, parameter, time));
}

private static IEnumerator InvokeImplementation<T>(Action<T> action, T parameter, float time) where T : class
{
  yield return new WaitForSeconds(time);
  action(parameter);
}

private void TestWithParameter(string param)
{
  Debug.Log(param);
}
 
this.Invoke(TestWithParameter, "Test using custom invoke and param", 5f);
```

Finally, ```public void InvokeRepeating(string methodName, float time, float repeatRate)``` can be similarly implemented using Coroutines.

```C#
public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, float repeatRate)
{
return monoBehaviour.StartCoroutine(InvokeRepeatingImplementation(action, time, repeatRate));
}

private static IEnumerator InvokeRepeatingImplementation(Action action, float time, float repeatRate)
{
  yield return new WaitForSeconds(time);
  
  while(true)
  {
    action();
    yield return new WaitForSeconds(repeatRate);
  }
}
```

These custom invoke methods could also be added to a custom class inherited from MonoBehaviour which one would then always use as their base class.