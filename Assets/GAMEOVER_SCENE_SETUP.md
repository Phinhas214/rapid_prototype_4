# Game Over Scene Setup Guide

This guide will help you create a Game Over scene that displays the final score and provides options to return to the main menu or replay the game.

---

## Step 1: Create the Game Over Scene

1. In Unity, go to **File > New Scene**
2. Choose **2D** template
3. Click **Create**
4. Save the scene: **File > Save As**
   - Name it: `GameOver`
   - Save in: `Assets/Scenes/`
   - Click **Save**

---

## Step 2: Set Up the Camera

1. Select **Main Camera** in Hierarchy
2. In **Inspector**, set **Projection** to **Orthographic**
3. Set **Size** to `5`
4. Set **Position** to: X = `0`, Y = `0`, Z = `-10`
5. Set **Background** color to a nice color (e.g., dark blue: R=30, G=40, B=60)

---

## Step 3: Create Background (Optional but Recommended)

1. Right-click in **Hierarchy** > **Create Empty**
2. Name it: `GameOverBackground`
3. Add **Sprite Renderer** component
4. Drag `Background-Land.jpg` from `Assets/Images/` onto the **Sprite** field (or use a solid color)
5. Set **Scale** to fill screen (e.g., X = `15`, Y = `15`, Z = `1`)
6. Set **Order in Layer** to `-10` (behind everything)

---

## Step 4: Create the Canvas

1. Right-click in **Hierarchy** > **UI > Canvas**
2. A Canvas and EventSystem will be created
3. Select **Canvas** in Hierarchy
4. In **Canvas Scaler** component:
   - Set **UI Scale Mode** to **Scale With Screen Size**
   - Set **Reference Resolution** to: X = `1920`, Y = `1080`

---

## Step 5: Create Game Over Title Text

1. Right-click on **Canvas** > **UI > Text - TextMeshPro**
2. Name it: `GameOverTitleText`
3. In **Rect Transform**:
   - Set **Anchor** to **Top Center**
   - Set **Pos X** to `0`, **Pos Y** to `-100`
   - Set **Width** to `800`, **Height** to `100`
4. In **TextMeshPro** component:
   - Set **Text** to: `GAME OVER`
   - Set **Font Size** to `72`
   - Set **Alignment** to **Center**
   - Set **Color** to white or a bright color (e.g., yellow: R=255, G=255, B=100)
   - Enable **Bold** if desired

### Add Shadow Effect (Optional but Beautiful):

1. With **GameOverTitleText** selected, click **Add Component**
2. Add **Shadow** (under UI Effects)
3. Set **Effect Color** to black with some transparency (R=0, G=0, B=0, A=150)
4. Set **Effect Distance** to: X = `3`, Y = `-3`

---

## Step 6: Create Final Score Text

1. Right-click on **Canvas** > **UI > Text - TextMeshPro**
2. Name it: `FinalScoreText`
3. In **Rect Transform**:
   - Set **Anchor** to **Center Center**
   - Set **Pos X** to `0`, **Pos Y** to `50`
   - Set **Width** to `600`, **Height** to `80`
4. In **TextMeshPro** component:
   - Set **Text** to: `Final Score: 0` (will be updated by script)
   - Set **Font Size** to `48`
   - Set **Alignment** to **Center**
   - Set **Color** to white or gold (R=255, G=215, B=0)

### Add Shadow Effect:

1. With **FinalScoreText** selected, click **Add Component**
2. Add **Shadow** (UI Effects)
3. Set **Effect Color** to black (R=0, G=0, B=0, A=150)
4. Set **Effect Distance** to: X = `2`, Y = `-2`

---

## Step 7: Create Main Menu Button

1. Right-click on **Canvas** > **UI > Button - TextMeshPro**
2. Name it: `MainMenuButton`
3. In **Rect Transform**:
   - Set **Anchor** to **Center Center**
   - Set **Pos X** to `0`, **Pos Y** to `-50`
   - Set **Width** to `300`, **Height** to `80`
4. Select the **Text (TMP)** child object:
   - Set **Text** to: `MAIN MENU`
   - Set **Font Size** to `36`
   - Set **Alignment** to **Center**
   - Set **Color** to white
5. Select **MainMenuButton** again:
   - In **Image** component, set **Color** to blue (R=70, G=130, B=200)

### Add Shadow to Button:

1. With **MainMenuButton** selected, click **Add Component**
2. Add **Shadow** (UI Effects)
3. Set **Effect Color** to black (R=0, G=0, B=0, A=100)
4. Set **Effect Distance** to: X = `2`, Y = `-2`

---

## Step 8: Create Replay Button

1. Right-click on **Canvas** > **UI > Button - TextMeshPro**
2. Name it: `ReplayButton`
3. In **Rect Transform**:
   - Set **Anchor** to **Center Center**
   - Set **Pos X** to `0`, **Pos Y** to `-150`
   - Set **Width** to `300`, **Height** to `80`
4. Select the **Text (TMP)** child object:
   - Set **Text** to: `REPLAY`
   - Set **Font Size** to `36`
   - Set **Alignment** to **Center**
   - Set **Color** to white
5. Select **ReplayButton** again:
   - In **Image** component, set **Color** to green (R=50, G=200, B=50)

### Add Shadow to Replay Button:

1. With **ReplayButton** selected, click **Add Component**
2. Add **Shadow** (UI Effects)
3. Set **Effect Color** to black (R=0, G=0, B=0, A=100)
4. Set **Effect Distance** to: X = `2`, Y = `-2`

---

## Step 9: Add Game Over Controller

1. Create an empty GameObject: Right-click in **Hierarchy** > **Create Empty**
2. Name it: `GameOverManager`
3. Click **Add Component**
4. Search for and add **Game Over Controller**
5. In the **Game Over Controller** component:
   - Set **Main Menu Scene Name** to: `MainMenu`
   - Set **Replay Scene Name** to: `Stage-1`
   - Drag `GameOverTitleText` from Hierarchy to **Game Over Title Text** field
   - Drag `FinalScoreText` from Hierarchy to **Final Score Text** field
   - (Optional) Assign **Button Click Sound** from `Assets/Audio/` if you have one

---

## Step 10: Connect Buttons to Script

### Connect Main Menu Button:

1. Select **MainMenuButton** in Hierarchy
2. In **Inspector**, scroll down to **Button** component
3. Under **On Click ()**, click the **+** button
4. Drag **GameOverManager** from Hierarchy into the object field
5. Click the dropdown that says **No Function**
6. Select: **GameOverController > GoToMainMenu()**

### Connect Replay Button:

1. Select **ReplayButton** in Hierarchy
2. In **Inspector**, find **Button** component
3. Under **On Click ()**, click the **+** button
4. Drag **GameOverManager** from Hierarchy into the object field
5. Click the dropdown that says **No Function**
6. Select: **GameOverController > ReplayGame()**

---

## Step 11: Add Visual Enhancements (Optional but Recommended)

### Add Button Hover Effects:

1. Select **MainMenuButton**
2. Click **Add Component**
3. Search for and add **Button Hover Effect**
4. Adjust **Hover Scale** (default: 1.1 = 10% bigger on hover)
5. Adjust **Animation Speed** (default: 5, higher = faster)
6. Repeat for **ReplayButton**

### Add Button Color Tint:

1. Select **MainMenuButton**
2. In **Button** component, find **Transition** dropdown
3. Set it to **Color Tint** (if not already)
4. Set **Normal Color** to your button color
5. Set **Highlighted Color** to a brighter version
6. Set **Pressed Color** to a darker version
7. Repeat for **ReplayButton**

---

## Step 12: Update Stage 3 to Load Game Over Scene

1. Open the **Stage 3** scene
2. Find the **GameManger** GameObject (or the object with GameManger script)
3. In the **GameManger** component, you'll need to add a reference to the GameOver scene name
4. Open `Assets/Stage 3/Stage3 Script/GameManager.cs` in your code editor
5. Add this field at the top of the GameManger class:
   ```csharp
   [Header("Scene Settings")]
   [SerializeField] private string gameOverSceneName = "GameOver";
   ```
6. In the `GameOver()` method, replace the UI display code with:
   ```csharp
   // Load Game Over scene instead of showing in-scene UI
   if (!string.IsNullOrEmpty(gameOverSceneName))
   {
       SceneManager.LoadScene(gameOverSceneName);
   }
   ```

---

## Step 13: Add Game Over Scene to Build Settings

1. Go to **File > Build Settings** (or **Edit > Project Settings > Editor > Scene Management**)
2. In **Scenes In Build**, click **Add Open Scenes** (if GameOver scene is open)
3. Or drag the **GameOver** scene from `Assets/Scenes/` into the build list
4. Make sure the scene order is correct (GameOver should be after Stage 3)

---

## Step 14: Test Your Game Over Scene!

1. Press **Play**
2. Complete all 3 stages (or use the Game Over scene directly for testing)
3. You should see:
   - "GAME OVER" title at the top
   - Final score displayed in the center
   - Main Menu and Replay buttons
4. Click **MAIN MENU** - should load MainMenu scene and reset scores
5. Click **REPLAY** - should load Stage-1 scene and reset scores

---

## Tips for Beautification:

1. **Colors**: Use a cohesive color scheme (e.g., dark background with bright text)
2. **Spacing**: Make sure elements aren't too close together
3. **Fonts**: Use bold, readable fonts for the title
4. **Shadows**: Add shadows to text and buttons for depth
5. **Animations**: You can add simple animations later (fade in, scale up, etc.)
6. **Background**: Use your game's background image or a gradient for visual interest

---

## Troubleshooting:

- **Score doesn't show**: Make sure FinalScoreText is assigned in GameOverController
- **Buttons don't work**: Make sure GameOverManager is assigned in the OnClick events
- **Scene doesn't load**: Check that the scene name in GameOverController matches your scene name exactly
- **Score is 0**: Make sure MainMenuController.totalScore is being updated in all stages

---

Your Game Over scene is now ready! 🎮

