# 13 - iOS Launch Screen

On Startup, a Unity game will display the splash screen (if enabled) before loading the first scene. However, if you launch the game on iOS, you will notice the following bluescreen before the splash screen/first scene.

<img src="https://i.imgur.com/XuUQht6.png" width="200px"/>

This is an *iOS Launch Screen*, a screen that appears instantly when an iOS app lauches. As every app must supply a launch screen, when building an iOS Xcode project, Unity will supply a default as seen above.

> *The launch screen is quickly replaced with the first screen of your app, giving the impression that your app is fast and responsive. The launch screen isn’t an opportunity for artistic expression. It’s solely intended to enhance the perception of your app as quick to launch and immediately ready for use.* [Apple](https://developer.apple.com/ios/human-interface-guidelines/icons-and-images/launch-screen/)

So now that we know what a launch screen is, wouldn't it be nice to supply something better suited to the splash screen/first scene? This can be easily achieved in **Player Settings -> iOS -> Splash Screen**

![](https://i.imgur.com/oD6gShK.png)

There are two options: **1)** Supply static images of different sizes for different devices (iOS7+) or **2)** Generate an XIB file which is adapted to each device screen size (iOS8+).

In the launch screen seen above, Unity automatically creates iPhone and iPad XIB files (*Default*). There are also *Image and Background (relative size)*, *Image and Background (constant size)* and *Custom XIB* options, the first two which create XIB files based on a background color and an overlay image, while the latter uses a supplied XIB file.

For a simple, single-colored launch screen with no text or images, either of the three options will suffice, however I personally prefer to supply a custom XIB created in Xcode as **1)** The file size is smaller and **2)** Unity's created XIB uses a constrained UIImage which is unnecessary as the base UIView's background color can be assigned.

<img src="https://i.imgur.com/xCy9c1h.png" width="200px"/>

Amazingly I have seen the default Unity launch screen in games with more than a million downloads! A custom launch screen will not only look aesthetically better, it will give the impression that the app is loading faster, good for older devices. Simply
* design a launch screen that’s as identical to the splash screen/first scene as possible
* avoid including text as localization isn't posible