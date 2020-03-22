# Hand Tracking Pack (Oculus Quest)
Unity project with some things I've been working on:

* Matching technique for getting 2D haptic feedback (virtual-real table)
* Turn your table into a whiteboard (fingers to paint, fist to erase)
* Detect space-coupled convex 3D gestures (applied on drawing detection)
* Fast hand gesture recognition (working with controllers too!)
* Grab things with your fist
* Optimized 3D scanned room
* Virtual touchscreen
* Virtual piano
* VR shopping (POC)

![Matching technique](https://i.imgur.com/KZMWlEu.gif)

![Painting with fingers](https://i.imgur.com/SPXv96b.gif)

## Getting Started

How can I open this project?

1. ```git clone https://github.com/jorgejgnz/HandTrackingPack-HapticFeedback```
2. Open [UnityHub](https://unity3d.com/es/get-unity/download)
3. Press Add and select the folder where you cloned this repo (Unity 2019.3.4f1)
4. Set Build Settings / Texture Compression to ASTC (it will take long, be patient)

If you're having the following error: ´´´.../il2cpp.exe did not run properly!´´´ on building, try the following:
1. Set Project Settings / Player / Other settings / Scripting backend to Mono
2. Set ARM v7 enabled
3. Build
4. Set Project Settings / Player / Other settings / Scripting backend to ILCPP
5. Set ARMv7 disabled
6. Set ARM64 enabled
7. Build (without errors)


## Built With

This Unity project uses the following:

* [Unity 2019.3.4f1](https://unity3d.com/es/get-unity/download/archive)
* [Oculus Integration 13.0](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022)
* [VRTK Prefabs Package 1.1.3](https://www.npmjs.com/package/io.extendreality.vrtk.prefabs)
* [Drawing in VR](https://github.com/MarekMarchlewicz/Painting)

3D Models:

* [Trainer - kirkbear](https://sketchfab.com/3d-models/7th-trainer-scan-2nd-upload-29710ee05c82496684df2d4ab454b2bc)
* [Dinning Room - The Hallwyl Museum (Hallwylska museet)](https://sketchfab.com/3d-models/the-dining-room-da3e970d523a46e4974b2357b6a9538e)

Oculus Integration and VRTK package are already imported and ready to go in this project.

## Author

* **Jorge Juan González** - *HCI Researcher at I3A (UCLM)* - [GitHub](https://github.com/jormaje) - [LinkedIn](https://www.linkedin.com/in/jorgejgnz/) - [ResearchGate](https://www.researchgate.net/profile/Jorge_Juan_Gonzalez)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
