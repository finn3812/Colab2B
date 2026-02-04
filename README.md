# Colab2B

A Unity project built with Unity 2022.3.52f1.

## Getting Started

### Prerequisites

- Unity 2022.3.52f1
- JetBrains Rider 2025.3.2 (recommended IDE)

### Setup

1. Clone this repository
2. Open the project in Unity 2022.3.52f1
3. Follow the [Rider Setup Guide](RIDER_SETUP.md) to configure JetBrains Rider as your external script editor

### IDE Configuration

This project is configured to work with JetBrains Rider. See [RIDER_SETUP.md](RIDER_SETUP.md) for detailed setup instructions.

The project includes:
- JetBrains Rider Editor package (com.unity.ide.rider v3.0.34)
- Pre-configured project settings for Unity development
- Universal Render Pipeline (URP)

## Project Structure

```
.
├── Assets/          # Unity project assets
├── Packages/        # Unity package manifest
├── ProjectSettings/ # Unity project settings
└── UserSettings/    # User-specific settings (gitignored)
```

## Development

### Opening Scripts

Once Rider is configured as the external script editor:
1. Double-click any C# script in Unity's Project window
2. The script will open in Rider automatically
3. Rider provides Unity-specific features like event function auto-completion and Unity debugging

### Generating Project Files

If you need to regenerate the Unity solution files:
- In Unity Editor: **Edit** → **Preferences** → **External Tools** → "Regenerate project files"
- Or in Rider: Right-click solution → **Unity** → "Generate project files"

## Additional Documentation

- [Rider Setup Guide](RIDER_SETUP.md) - Detailed instructions for configuring JetBrains Rider

## License

[Add license information here]
