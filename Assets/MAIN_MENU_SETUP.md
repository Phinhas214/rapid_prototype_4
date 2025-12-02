# Main Menu Scene Setup Guide

This guide will help you create a beautiful main menu scene with Start and Exit buttons.

---

## Step 1: Create the Main Menu Scene

1. In Unity, go to **File > New Scene**
2. Choose **2D** template
3. Click **Create**
4. Save the scene: **File > Save As**
   - Name it: `MainMenu`
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

### Option A: Use Your Background Image

1. Right-click in **Hierarchy** > **Create Empty**
2. Name it: `MenuBackground`
3. Add **Sprite Renderer** component
4. Drag `Background-Land.jpg` from `Assets/Images/` onto the **Sprite** field
5. Set **Scale** to fill screen (e.g., X = `15`, Y = `15`, Z = `1`)
6. Set **Order in Layer** to `-10` (behind everything)

### Option B: Use a Solid Color Background

1. Right-click in **Hierarchy** > **Create Empty**
2. Name it: `MenuBackground`
3. Add **Sprite Renderer** component
4. Set **Color** to a nice gradient color (e.g., dark blue-purple: R=40, G=30, B=60)
5. Set **Scale** to: X = `20`, Y = `20`, Z = `1`
6. Set **Order in Layer** to `-10`

---

## Step 4: Create the Canvas

1. Right-click in **Hierarchy** > **UI > Canvas**
2. A Canvas and EventSystem will be created
3. Select **Canvas** in Hierarchy
4. In **Canvas Scaler** component:
   - Set **UI Scale Mode** to **Scale With Screen Size**
   - Set **Reference Resolution** to: X = `1920`, Y = `1080`

---

## Step 5: Create Title Text

1. Right-click on **Canvas** > **UI > Text - TextMeshPro**
2. Name it: `TitleText`
3. In **Rect Transform**:
   - Set **Anchor** to **Top Center**
   - Set **Pos X** to `0`, **Pos Y** to `-80`
   - Set **Width** to `800`, **Height** to `100`
4. In **TextMeshPro** component:
   - Set **Text** to: `SEED SHOOTER` (or your game name)
   - Set **Font Size** to `72`
   - Set **Alignment** to **Center**
   - Set **Color** to white or a bright color (e.g., yellow: R=255, G=255, B=100)
   - Enable **Bold** if desired

### Add Shadow Effect (Optional but Beautiful):

1. With **TitleText** selected, click **Add Component**
2. Add **Shadow** (under UI Effects)
3. Set **Effect Color** to black with some transparency (R=0, G=0, B=0, A=150)
4. Set **Effect Distance** to: X = `3`, Y = `-3`

---

## Step 6: Create Subtitle/Description (Optional)

1. Right-click on **Canvas** > **UI > Text - TextMeshPro**
2. Name it: `SubtitleText`
3. In **Rect Transform**:
   - Set **Anchor** to **Top Center**
   - Set **Pos X** to `0`, **Pos Y** to `-180`
   - Set **Width** to `600`, **Height** to `40`
4. In **TextMeshPro**:
   - Set **Text** to: `Aim and shoot seeds at targets!`
   - Set **Font Size** to `24`
   - Set **Alignment** to **Center**
   - Set **Color** to light gray (R=200, G=200, B=200)

---

## Step 7: Create Start Button

1. Right-click on **Canvas** > **UI > Button - TextMeshPro**
2. Name it: `StartButton`
3. In **Rect Transform**:
   - Set **Anchor** to **Center Center**
   - Set **Pos X** to `0`, **Pos Y** to `50`
   - Set **Width** to `300`, **Height** to `80`
4. Select the **Text (TMP)** child object:
   - Set **Text** to: `START`
   - Set **Font Size** to `36`
   - Set **Alignment** to **Center**
   - Set **Color** to white
5. Select **StartButton** again:
   - In **Image** component, set **Color** to green (R=50, G=200, B=50)
   - Or use a nice blue: R=70, G=130, B=200

### Make Button Look Better:

1. With **StartButton** selected, click **Add Component**
2. Add **Shadow** (UI Effects)
3. Set **Effect Color** to black (R=0, G=0, B=0, A=100)
4. Set **Effect Distance** to: X = `2`, Y = `-2`

---

## Step 8: Create Exit Button

1. Right-click on **Canvas** > **UI > Button - TextMeshPro**
2. Name it: `ExitButton`
3. In **Rect Transform**:
   - Set **Anchor** to **Center Center**
   - Set **Pos X** to `0`, **Pos Y** to `-50`
   - Set **Width** to `300`, **Height** to `80`
4. Select the **Text (TMP)** child object:
   - Set **Text** to: `EXIT`
   - Set **Font Size** to `36`
   - Set **Alignment** to **Center**
   - Set **Color** to white
5. Select **ExitButton** again:
   - In **Image** component, set **Color** to red (R=200, G=50, B=50)
   - Or use dark gray: R=100, G=100, B=100

### Add Shadow to Exit Button:

1. With **ExitButton** selected, click **Add Component**
2. Add **Shadow** (UI Effects)
3. Set **Effect Color** to black (R=0, G=0, B=0, A=100)
4. Set **Effect Distance** to: X = `2`, Y = `-2`

---

## Step 9: Add Main Menu Controller

1. Create an empty GameObject: Right-click in **Hierarchy** > **Create Empty**
2. Name it: `MenuManager`
3. Click **Add Component**
4. Search for and add **Main Menu Controller**
5. In the **Main Menu Controller** component:
   - Set **Game Scene Name** to: `Stage-1` (or your game scene name)

---

## Step 10: Connect Buttons to Script

### Connect Start Button:

1. Select **StartButton** in Hierarchy
2. In **Inspector**, scroll down to **Button** component
3. Under **On Click ()**, click the **+** button
4. Drag **MenuManager** from Hierarchy into the object field
5. Click the dropdown that says **No Function**
6. Select: **MainMenuController > StartGame()**

### Connect Exit Button:

1. Select **ExitButton** in Hierarchy
2. In **Inspector**, find **Button** component
3. Under **On Click ()**, click the **+** button
4. Drag **MenuManager** from Hierarchy into the object field
5. Click the dropdown that says **No Function**
6. Select: **MainMenuController > ExitGame()**

---

## Step 11: Add Visual Enhancements (Optional but Recommended)

### Add Button Hover Effects:

**Option A: Color Tint (Built-in)**
1. Select **StartButton**
2. In **Button** component, find **Transition** dropdown
3. Set it to **Color Tint** (if not already)
4. Set **Normal Color** to your button color
5. Set **Highlighted Color** to a brighter version (e.g., lighter green)
6. Set **Pressed Color** to a darker version
7. Set **Selected Color** to match highlighted
8. Repeat for **ExitButton**

**Option B: Scale Animation (More Dynamic)**
1. Select **StartButton**
2. Click **Add Component**
3. Search for and add **Button Hover Effect**
4. Adjust **Hover Scale** (default: 1.1 = 10% bigger on hover)
5. Adjust **Animation Speed** (default: 5, higher = faster)
6. Repeat for **ExitButton**
7. Now when you hover over buttons, they'll smoothly scale up!

### Add Background Decoration (Optional):

1. Right-click on **Canvas** > **UI > Image**
2. Name it: `Decoration`
3. Set **Color** to a semi-transparent color
4. Position it behind buttons for visual interest
5. Set **Order in Layer** to be behind buttons

---

## Step 12: Set Main Menu as First Scene

1. Go to **File > Build Settings** (or **Edit > Project Settings > Editor > Scene Management**)
2. In **Scenes In Build**, make sure **MainMenu** is at index 0 (first scene)
3. If it's not there, click **Add Open Scenes**
4. Drag **MainMenu** scene to the top of the list
5. Your game scene (Stage-1) should be index 1

---

## Step 13: Test Your Menu!

1. Press **Play**
2. You should see:
   - Title at the top
   - Start and Exit buttons in the center
   - Nice background
3. Click **START** - should load your game scene
4. Click **EXIT** - should quit the game (or stop play in editor)

---

## Tips for Beautification:

1. **Colors**: Use a cohesive color scheme (e.g., blues and greens, or warm colors)
2. **Spacing**: Make sure buttons aren't too close together
3. **Fonts**: Use bold, readable fonts for titles
4. **Shadows**: Add shadows to text and buttons for depth
5. **Animations**: You can add simple animations later (button scale on hover, etc.)
6. **Background**: Use your game's background image or a gradient for visual interest

---

## Troubleshooting:

- **Buttons don't work**: Make sure MenuManager is assigned in the OnClick events
- **Scene doesn't load**: Check that the scene name in MainMenuController matches your game scene name exactly
- **Text doesn't show**: Make sure TextMeshPro is imported (Window > TextMeshPro > Import TMP Essential Resources)
- **Buttons look weird**: Check that Canvas Scaler is set to "Scale With Screen Size"

---

Your beautiful main menu is now ready! 🎮

