# Colab2B - JetBrains Rider Setup Guide

This project is configured to use JetBrains Rider as the external script editor.

## Prerequisites

- Unity 2022.3.52f1 or compatible version
- JetBrains Rider 2025.3.2 or later

## Setup Instructions

### 1. Verify Rider Package Installation

The project already includes the JetBrains Rider Editor package (`com.unity.ide.rider` version 3.0.34) in the Package Manager. You can verify this by:

1. Open Unity Editor
2. Go to **Window** → **Package Manager**
3. Look for "JetBrains Rider Editor" in the list
4. It should show as installed

If for any reason it's not installed:
1. In Package Manager, click the **+** button
2. Select "Add package by name..."
3. Enter: `com.unity.ide.rider`
4. Click "Add"

### 2. Set Rider as External Script Editor

#### Option A: Through Unity Editor (Recommended)

1. Open the Unity project
2. Go to **Edit** → **Preferences** (Windows/Linux) or **Unity** → **Preferences** (macOS)
3. Select **External Tools** from the left sidebar
4. In the **External Script Editor** dropdown, select:
   - If Rider is installed: Choose "JetBrains Rider 2025.3.2" or your installed version
   - If not visible in the dropdown: Click "Browse..." and navigate to your Rider installation:
     - Windows: `C:\Program Files\JetBrains\JetBrains Rider 2025.3.2\bin\rider64.exe`
     - macOS: `/Applications/Rider.app`
     - Linux: `/opt/rider/bin/rider.sh` (or your installation path)
5. Optionally, check **"Generate .csproj files for:"** options if you want to customize which project files are generated
6. Click the **"Regenerate project files"** button

#### Option B: Open Through .sln File

1. First, generate the solution files:
   - In Unity Editor: Go to **Edit** → **Preferences** → **External Tools**
   - Click **"Regenerate project files"**
2. Close Unity Editor
3. Navigate to the project root directory
4. Double-click on the `.sln` file (e.g., `Colab2B.sln` or `Assembly-CSharp.sln`)
5. This should open the project in Rider (if Rider is set as the default application for .sln files)

### 3. Configure Rider for Unity

When you first open the project in Rider:

1. Rider should automatically detect it's a Unity project
2. Allow Rider to install the Unity Support plugin if prompted
3. The Unity editor should appear in the Rider toolbar
4. You can now use Rider's Unity-specific features like:
   - Unity event functions auto-completion
   - Unity-specific inspections and quick-fixes
   - Debugging Unity applications
   - Unity Explorer tool window

### 4. Verify the Setup

To verify everything is working correctly:

1. In Unity Editor, double-click any C# script in the Project window
2. Rider should open automatically with that script
3. In Rider, you should see Unity-specific features in the toolbar
4. You should be able to navigate to Unity assets and see Unity events auto-complete

## Troubleshooting

### Rider doesn't appear in External Script Editor dropdown

- Make sure Rider 2025.3.2 is properly installed
- Try restarting Unity Editor
- Manually browse to the Rider executable using the "Browse..." option

### Scripts open in another editor

- Check the External Script Editor preference in Unity (Edit → Preferences → External Tools)
- Make sure .cs files are associated with Rider in your operating system

### Project files are not up to date

- In Unity Editor: Edit → Preferences → External Tools → "Regenerate project files"
- Or in Rider: Right-click on solution → Unity → Generate project files

## Additional Resources

- [JetBrains Rider for Unity Documentation](https://www.jetbrains.com/help/rider/Unity.html)
- [Unity External Tools Documentation](https://docs.unity3d.com/Manual/Preferences.html#External-Tools)

---

**Note**: The `UserSettings` folder contains user-specific preferences including the external editor selection. This folder is gitignored and should not be committed to version control. Each developer needs to configure their own external editor preferences.
