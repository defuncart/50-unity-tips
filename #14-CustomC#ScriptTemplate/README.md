# 14 - Custom C# Script Template

When one creates a new C# Script in Unity, they are presented with the following code automatically:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class #SCRIPTNAME# : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
}
```

Did you know that you can easily change this template? This is quite useful as
1. *Start* and *Update* methods, even when empty, are triggered once per frame for every game object. Often developers forget to remove unnecessary empty functions from their scripts - if it is never there in the first place, then that's one less thing to remove!
2. Can easily add #regions, ```<summary>```, custom code etc.

Simply go to **/Applications/Unity/Unity.app/Contents/Resources/ScriptTemplates** (or *C:\Program Files\Unity\Editor\Data\Resources\ScriptTemplates* on *Windows*) and edit *81-C# Script-NewBehaviourScript.cs.txt*.

```C#
/*
 *  Written by James Leahy. (c) 2017 DeFunc Art.
 */
using UnityEngine;

/// <summary>The #SCRIPTNAME# class.</summary>
public class #SCRIPTNAME# : MonoBehaviour
{
}
```

Other templates like shaders can be edited too. For more information see [here](https://support.unity3d.com/hc/en-us/articles/210223733-How-to-customize-Unity-script-templates). It is worth remembering that this is a version-specific alteration that one needs to do repeat for each new version.