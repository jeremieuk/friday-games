# SPR Card Battler - Setup Instructions

## Prerequisites
- Godot 4.3+ with .NET support
- .NET 8 SDK (for C# compilation)

## Initial Setup

### 1. Install .NET 8 SDK
If not already installed, download from: https://dotnet.microsoft.com/download/dotnet/8.0

### 2. Open Project in Godot
1. Open Godot Editor
2. Click "Import" and select this directory
3. The editor will automatically:
   - Generate .godot/ folder
   - Build C# solution
   - Import assets

### 3. Install GUT Testing Framework
1. In Godot Editor, go to AssetLib tab
2. Search for "GUT" (Godot Unit Test)
3. Install GUT framework
4. Enable the plugin in: Project → Project Settings → Plugins → GUT (check enabled)

### 4. Run Tests
After GUT is installed:
- Bottom panel: Click "GUT" tab
- Click "Run All" to execute tests
- Or use CLI: `godot --path . --script res://addons/gut/gut_cmdln.gd -gdir=res://tests/unit/`

## Project Structure
```
spr/
├── project.godot          # Godot project file
├── SprGame.csproj        # C# project
├── .gutconfig.json       # GUT test configuration
├── scripts/
│   ├── core/             # Pure C# game logic (zero Godot deps)
│   ├── managers/         # Service layer
│   ├── ui/              # Presentation layer
│   └── ai/              # AI opponent
├── scenes/               # Godot scene files
├── tests/
│   └── unit/            # GUT test files
└── assets/              # Graphics, audio, fonts
```

## Running the Game
- Press F5 in Godot Editor
- Or CLI: `godot --path . res://scenes/main_menu/MainMenu.tscn`

## Development Workflow (TDD)
1. Write test in `tests/unit/test_*.gd` (RED)
2. Run tests - should fail
3. Implement code in `scripts/**/*.cs` (GREEN)
4. Run tests - should pass
5. Refactor if needed
6. Commit

## Code Standards
- See `.editorconfig` for STD-0002 C# coding standards
- Egyptian braces (opening on same line)
- Pattern matching preferred
- No magic strings
