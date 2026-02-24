# Neon Protocol - Project Setup

This folder contains the core scripts for the **Neon Protocol** Android FPS.

## Installation
1.  **Copy Files:** Drag the `Assets` folder into your Unity Project's `Assets` directory.
2.  **Dependencies:** Ensure you have the **New Input System** package installed via Package Manager.
3.  **Setup:**
    - **Input:** Attach `NeonInputHandler` to a persistent GameObject. Assign the `PlayerInput` asset.
    - **Movement:** Attach `NeonMovement` to your Player object (requires `CharacterController`).
    - **Pooling:** Attach `NeonPooler` to a Manager object. Populate the `Pools` list with Zombie/Bullet prefabs.
    - **Quests:** Attach `QuestManager` to a Manager object. Select the current `MapTheme`.

## Modules
- **Core/Input:** Handles Touch vs Gamepad switching.
- **Core/Movement:** Implements IW-style G-Slide and Bunny Hopping.
- **Core/Systems:** Object Pooling and Quest Management.
- **Maps:** Specific logic for Spaceland (UFO), Rave (Vision), Shaolin (Chi), Radioactive (Chemistry), and Beast (Cryptids).
