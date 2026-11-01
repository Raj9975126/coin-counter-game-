# 🎮 Coin Catcher 2D Game (Unity & C#)

A beginner-friendly 2D game project built for college submissions and learning Unity game development fundamentals.

---

## 📁 Project Structure

All ready-to-use C# scripts are located in `Assets/Scripts/`:
- [`PlayerController.cs`](file:///Users/rajkumar/Downloads/coin%20earning%20game%20/Assets/Scripts/PlayerController.cs) - Moves the basket horizontally with boundary clamping.
- [`Coin.cs`](file:///Users/rajkumar/Downloads/coin%20earning%20game%20/Assets/Scripts/Coin.cs) - Moves coin downward, detects catch vs. miss.
- [`CoinSpawner.cs`](file:///Users/rajkumar/Downloads/coin%20earning%20game%20/Assets/Scripts/CoinSpawner.cs) - Spawns coins at random positions along the top.
- [`GameManager.cs`](file:///Users/rajkumar/Downloads/coin%20earning%20game%20/Assets/Scripts/GameManager.cs) - Manages Score, Lives, Game Over state, Audio, and UI.

---

## 🚀 Step-by-Step Implementation Guide in Unity

### Step 1: Create the Unity Project
1. Open **Unity Hub**.
2. Click **New Project** (Top-right).
3. Select **2D (Core)** template.
4. Set **Project Name**: `CoinCatcher`.
5. Click **Create Project**.
6. Copy the `Assets/Scripts/` folder from this directory into your Unity project's `Assets/` folder.

---

### Step 2: Create the Player (Basket)
1. In the **Hierarchy** window, right-click $\rightarrow$ **2D Object** $\rightarrow$ **Sprites** $\rightarrow$ **Square** (or Capsule/Sprite).
2. Rename it to `Player`.
3. In the **Inspector**:
   - Set **Tag** to `"Player"` *(Click Tag dropdown $\rightarrow$ select "Player")*.
   - **Position**: `X = 0`, `Y = -4`, `Z = 0`.
   - **Scale**: `X = 2.0`, `Y = 0.5`, `Z = 1` *(Looks like a wide basket or tray)*.
   - **Color**: Choose any color (e.g., Brown, Green, or Blue).
4. Click **Add Component** $\rightarrow$ add **Box Collider 2D**.
5. Click **Add Component** $\rightarrow$ add **Rigidbody 2D**:
   - Set **Body Type** to `Kinematic`.
6. Click **Add Component** $\rightarrow$ add **`PlayerController`**.
   - Set `Move Speed = 10`.
   - Set `X Boundary = 7.5`.

---

### Step 3: Create the Coin Prefab
1. In the **Hierarchy**, right-click $\rightarrow$ **2D Object** $\rightarrow$ **Sprites** $\rightarrow$ **Circle**.
2. Rename it to `Coin`.
3. In the **Inspector**:
   - **Scale**: `X = 0.6`, `Y = 0.6`, `Z = 1`.
   - **Color**: Yellow / Gold (`#FFD700`).
4. Click **Add Component** $\rightarrow$ add **Circle Collider 2D**:
   - ✅ Check **Is Trigger** to **true**.
5. Click **Add Component** $\rightarrow$ add **Rigidbody 2D**:
   - Set **Body Type** to `Kinematic`.
6. Click **Add Component** $\rightarrow$ add **`Coin`**.
   - Set `Fall Speed = 5`.
   - Set `Score Value = 1`.
   - Set `Bottom Limit = -5.5`.
7. **Make it a Prefab**:
   - Drag `Coin` from the **Hierarchy** down into your **Assets** window.
   - Delete `Coin` from the Hierarchy (now safely saved as a prefab).

---

### Step 4: Create the Coin Spawner
1. In the **Hierarchy**, right-click $\rightarrow$ **Create Empty**.
2. Rename it to `CoinSpawner`.
3. Position: `(0, 6, 0)`.
4. Click **Add Component** $\rightarrow$ add **`CoinSpawner`**.
5. Drag your `Coin` prefab from the Assets folder into the **Coin Prefab** field.
6. Set:
   - `Spawn Interval = 1.2`
   - `X Range = 7.5`
   - `Spawn Y = 6.0`

---

### Step 5: Setup UI (Canvas, Score, Lives & Game Over)
1. In the **Hierarchy**, right-click $\rightarrow$ **UI** $\rightarrow$ **Canvas**.
2. Select the `Canvas`, in the Inspector:
   - **UI Scale Mode**: `Scale With Screen Size`.
   - **Reference Resolution**: `1920 x 1080`.
3. **Score Text**:
   - Right-click `Canvas` $\rightarrow$ **UI** $\rightarrow$ **Text - TextMeshPro**.
   - Rename to `ScoreText`.
   - Anchor to **Top-Left**, position at `X = 150`, `Y = -50`.
   - Set text to: `Score: 0`, Font Size `36`, Color `White`.
4. **Lives Text**:
   - Right-click `Canvas` $\rightarrow$ **UI** $\rightarrow$ **Text - TextMeshPro**.
   - Rename to `LivesText`.
   - Anchor to **Top-Right**, position at `X = -150`, `Y = -50`.
   - Set text to: `Lives: 3`, Font Size `36`, Color `Red` or `White`.
5. **Game Over Panel**:
   - Right-click `Canvas` $\rightarrow$ **UI** $\rightarrow$ **Panel**.
   - Rename to `GameOverPanel`.
   - Set background color to semi-transparent black.
   - Inside `GameOverPanel`, add:
     - **Title**: TextMeshPro $\rightarrow$ `"GAME OVER"` (Size 60, Centered).
     - **Final Score**: TextMeshPro $\rightarrow$ name it `FinalScoreText` (Size 36).
     - **Restart Button**: Right-click `GameOverPanel` $\rightarrow$ **UI** $\rightarrow$ **Button - TextMeshPro**.
       - Set Button text to `"RESTART"`.

---

### Step 6: Create the GameManager & Audio
1. In the **Hierarchy**, right-click $\rightarrow$ **Create Empty**.
2. Rename it to `GameManager`.
3. Click **Add Component** $\rightarrow$ add **Audio Source**.
4. Click **Add Component** $\rightarrow$ add **`GameManager`**.
5. Connect the Inspector fields:
   - Drag `ScoreText` $\rightarrow$ **Score Text**.
   - Drag `LivesText` $\rightarrow$ **Lives Text**.
   - Drag `GameOverPanel` $\rightarrow$ **Game Over Panel**.
   - Drag `FinalScoreText` $\rightarrow$ **Final Score Text**.
   - Drag `GameManager` (itself) $\rightarrow$ **Audio Source**.
   - *(Optional)* Attach audio clips to **Coin Catch Sound**, **Coin Miss Sound**, and **Game Over Sound**.
6. **Hook up the Restart Button**:
   - Select the `Restart Button` under `GameOverPanel`.
   - In Inspector, find **On Click ()** $\rightarrow$ Click **`+`**.
   - Drag `GameManager` into the object slot.
   - In the dropdown, choose **`GameManager` $\rightarrow$ `RestartGame()`**.

---

### Step 7: Play and Test! 🕹️
1. Click the **Play** button at the top of Unity.
2. Use **A / D** or **Left / Right Arrow keys** to steer your basket.
3. Catch coins to increase your score!
4. If a coin falls below the screen, you lose 1 life.
5. When lives reach 0, the Game Over panel displays your final score and lets you restart with one click!
