# Quick Start Guide for Mobile Adventure

## Step 1: Setting Up Unity
1. Download and install Unity Hub from the [Unity website](https://unity.com/).
2. Launch Unity Hub and log in or create a new account.
3. Install the latest version of Unity with the necessary modules for mobile development (iOS/Android).

## Step 2: Creating Scenes
1. Open Unity and create a new project.
2. Navigate to the `File` menu and select `New Scene` to create your first scene.
3. Save the scene in the `Scenes` folder of your project.

## Step 3: Adding the Player
1. Create a new GameObject by right-clicking in the Hierarchy window and selecting `3D Object > Capsule` (this will represent the player).
2. Rename the object to `Player`.
3. Add the `PlayerController` script to the `Player` object by dragging the script onto it in the Inspector window.

## Step 4: Configuring Ground Platforms
1. Create a ground platform by right-clicking in the Hierarchy and choosing `3D Object > Cube`.
2. Scale the cube to create a platform (e.g., set the scale to (5, 0.2, 5)).
3. Position the platform beneath the Player object.
4. You can duplicate the platform for more challenging configurations.

## Step 5: Testing the Game
1. Attach the `TimeLoopManager` script to an empty GameObject (create one if needed).
2. Set any necessary parameters in the `TimeLoopManager` script via the Inspector.
3. Click the `Play` button in Unity to start testing the game.
4. Use the arrow keys or joystick to control the player and observe the interactions with the ground.

## Troubleshooting
- If the player does not move, ensure that the `PlayerController` script is properly configured and the Input settings are correct.
- Check the console for any errors that may provide hints on script issues or missing components.

## Conclusion
Congratulations! You have set up your Unity environment and created your first mobile game scene. Happy gaming!