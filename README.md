# Weak-Point Visualisation First-Person Shooter Game

**Note:** Due to file size limitations, this GitHub repository only has updated scripts and is not maintained for the rest of the assets. The full Unity project and a Windows build can be download from the links below:

## Downloads

**[Download Full Unity Project (~13.6 GB)](Placeholder)**

**[Download Windows Game Build (~200 MB)](Placeholder2)**

## Project Overview

This is a Unity-based FPS game created to investigate the influence of weak-point visualisation techniques inspired by common techniques utilised within the games industry. These techniques are employed to highlight weak-points on robotic enemies who the player fights against in a wave-based combat system. 

## Important Files
- **Assets/** - The root folder for game assets including scripts, prefabs, materials, models, art, etc.
- **Scenes/** - Contains the main menu scene and the game scene.
- **Scripts** - Contains all the C# scripts which game realises on for controlling gameplay, UI, and visualisation aspects.
- **Prefabs** - Contains various prefabs such as the new and old robotic model, and the bullet.
- **Robot** - Contains various elements relating the robot, although primarily has the animations used.


## Requirements 
- Unity Hub with Unity version 2022.3.47f1
- Visual Studio (installed via Unity Hub)
- OS: Only tested on windows 10
- Minimum 14 GB of free disk space (for full project download)
- Minimum of 200 MB of free disk space (Build only)

## Build Instructions 
1. Open the Unity Hub
2. Add the project folder. `Add -> Add project from disk`.
3. Open the project in Unity (May take a while for the first time, as requirments and dependencies will install)
4. Click `File -> Build Settings`
5. The target platform should be `Windows, Mac, Linux`
6. Click `Build and Run`, select a folder to save the build file to.

## Gameplay Instructions
You will be faced with the main menu, from here you can:

- Choose a visualisation technique to apply in gameplay
- Start the game
- End the game
- View performance metrics at the end of each round 
