# 22 - Android Device Filter

Tucked away in **Player Settings - Android - Other Settings**, there is a *Device Filter* setting with the options **FAT (ARMv7 + x86)**, **ARMv7** and **x86**. There are many approaches to reducing the build size for Android, but one sure approach is to change the default device filter from **FAT** (which builds a combined executable compatible with both ARMv7 and x86 architectures), and build separate builds for **ARMv7** and **x86**, where the **x86** build has a higher build number than **ARMv7**. Although this requires building two builds, you will reduce the actual install sizes by about 10MB.

The Google Play Multiple APK Support state that:

> - All APKs you publish for the same application must have the same package name and be signed with the same certificate key.
> - Each APK must have a different version code, specified by the android:versionCode attribute.

For an empty project, **FAT** has an install size of 41.88Mb, while **ARMv7** had an install size of 32.23Mb. On other projects I've notice roughly a 10Mb difference also. It's basically a free trick.

Now you might be thinking that building multiple builds seems time consuming, however we can easy write a custom script to take care of that for us:

```c#
/// <summary>Builds the game for the Android platform using a menu item.</summary>
[MenuItem("Tools/Build/Android")]
public static void BuildForAndroid()
{
  ///arm
  BuildAndroidForDevice(AndroidTargetDevice.ARMv7);
  //x86
  BuildAndroidForDevice(AndroidTargetDevice.x86);
}

/// <summary>Builds the game for the Android platform using a given target device.</summary>
private static void BuildAndroidForDevice(AndroidTargetDevice device)
{
  PlayerSettings.Android.targetDevice = device;
  string androidPath = string.Format("{0}/Builds/{1} ({2}).apk", Path.GetDirectoryName(Application.dataPath), "My App", device.ToString());
  BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, androidPath, BuildTarget.Android, BuildOptions.None);
}
```

The last thing that we need to do is assign different version codes to each build. One approach is to define a large integer whose components reflect the builds major version, minor version, path version, build number and target device:

![](images/androidDeviceFilter1.png)

Thus version 1.2.3 with build version 17 for arm would be 10203170, while x86 would be 10203171. Notice that each x86 build has a higher version code than the corresponding arm build, while version 1.2.4 would have a higher version code than 1.2.3.

```c#
/// <summary>Determines the correct versionCode for Android.</summary>
/// <param name="major">The app's major version number (i.e. 1).</param>
/// <param name="minor">The app's minor version number (i.e. 0).</param>
/// <param name="patch">The app's patch version number (i.e. 0).</param>
/// <param name="build">The app's build version number (i.e. 99).</param>
/// <param name="x86">Whether it is an x86  build.</param>
private static int AndroidVersionCode(int major, int minor, int patch, int build, bool x86)
{
    return major*100000 + minor*10000 + patch*1000 + build*10 + (x86 ? 1 : 0);
}
```

## Further Reading

[Android Developer - Multiple APK Support](https://developer.android.com/google/play/publishing/multiple-apks.html)
