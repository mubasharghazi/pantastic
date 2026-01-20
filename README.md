# 🎮 Pantastic - 2D Physics Puzzle Game

**Pantastic** is a 2D strategy puzzle game built with the **Unity Engine**. Inspired by the classic *Angry Birds* mechanics, players use a slingshot to launch birds, destroy structures, and defeat enemies (Pigs) using realistic physics.

---

## ✨ Features

* **Physics-Based Gameplay:** Realistic gravity, collisions, and destruction mechanics.
* **Trajectory Prediction:** Visual dotted line that calculates flight path using vector mathematics.
* **Level System:** 4 progressively challenging levels with an **Unlocking System** (saves progress using `PlayerPrefs`).
* **Parallax Scrolling:** Dynamic background layers that move at different speeds for a 3D depth effect.
* **Score & UI:** Real-time scoring, Pause Menu, and "Level Cleared/Game Over" panels.
* **Responsive Camera:** Smooth camera follow and **Pinch-to-Zoom** support for mobile devices.

---

## 🕹️ Controls

| Action | Input (PC) | Input (Mobile) |
| :--- | :--- | :--- |
| **Aim** | Click & Drag Mouse | Touch & Drag |
| **Shoot** | Release Mouse Button | Release Finger |
| **Zoom** | Scroll Wheel | Pinch In/Out |
| **Pause** | Click Pause Button (Top Right) | Tap Pause Button |

---

## 📸 Screenshots

*(Add your screenshots here by dragging images into the GitHub editor)*

> *Tip: Upload images of the Main Menu, Gameplay (Aiming), and Level Selection Screen.*

---

## 🛠️ Technical Details

This project utilizes **C# scripts** to handle game logic and physics interactions:

* **`GameManager.cs`**: The core logic handler. Manages game states (Win/Loss), scoring, UI panels, and level unlocking.
* **`SlingShot.cs`**: Handles input detection and calculating the launch vector. Uses `LineRenderer` to draw the trajectory.
* **`Bird.cs`**: Controls the projectile behavior, trail effects, and collision detection.
* **`Pig.cs` & `Brick.cs`**: Handles health and damage logic based on collision impact force.
* **`CameraFollow.cs`**: Smoothly tracks the bird's movement within level boundaries.

---

## 🚀 How to Run the Project

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/mubasharghazi/pantastic.git](https://github.com/mubasharghazi/pantastic.git)
    ```
2.  **Open in Unity:**
    * Open **Unity Hub**.
    * Click **Add** and select the cloned `pantastic` folder.
    * Open the project (Recommended Unity Version: 6000.0.26f1 or later).
3.  **Play:**
    * Open `Scenes/MainMenu` and press the **Play** button.

---

## 🔮 Future Improvements

* [ ] Add new bird types (Speed Bird, Bomb Bird).
* [ ] Implement a Global Leaderboard.
* [ ] Add complex obstacles like TNT and Stone blocks.

---

## 👨‍💻 Developer
Developed by **Mubashar Ghazi**.

---
*Made with ❤️ and Unity.*
