# Architecture: Scissors-Paper-Rock Game

**Version**: 1.2
**Date**: 2026-01-16
**Last Updated**: 2026-01-16 (v1.2 - Two-stage multiplayer transitions for shared reveal experience)
**Project**: SPR Game (Scissors-Paper-Rock Card Battler Foundation)
**Phase**: 2 of 4 (Requirements → **Architecture** → Game Design Document → TDD Development)

---

## 1. Introduction

### 1.1 Purpose

This document defines the technical architecture for the Scissors-Paper-Rock (SPR) game built with Godot 4.5 and C#. It provides a comprehensive blueprint for implementation, covering component design, data flow, state management, and testing strategy. The architecture is designed to support Test-Driven Development (TDD) while maintaining flexibility for future expansion into a full card battler game.

### 1.2 Scope

This architecture covers:
- High-level architectural patterns and principles
- Scene hierarchy and component organization
- Data flow and state management
- Testing strategy and approach
- File/folder structure
- Key interfaces and contracts
- Cross-cutting concerns (logging, error handling, performance)

This architecture **does not** cover:
- Implementation details (covered in Phase 4: TDD Development)
- Game design specifics (covered in Phase 3: Game Design Document)
- Asset creation or art style (handled separately)

### 1.3 References

- **requirements.md** - Phase 1 output defining functional and non-functional requirements
- **STD-0002-csharp-rubric.md** - C# coding standards for Godot 4.5

---

## 2. Architectural Overview

### 2.1 High-Level Pattern

The architecture uses a **Hybrid Approach** combining three complementary patterns:

1. **State Machine Pattern** (Game Flow)
   - Manages high-level game states (MainMenu → Playing → RoundResult → GameOver)
   - Explicit state transitions triggered by user actions or game events
   - Centralized in GameManager component

2. **Component-Based Architecture** (Godot Integration)
   - Leverages Godot's scene system and node architecture
   - Reusable components with single responsibilities
   - Loose coupling via Godot signals

3. **Three-Layer Separation** (Testability)
   - **Core Layer**: Pure C# game logic (zero Godot dependencies)
   - **Service Layer**: Managers and orchestration (minimal Godot dependencies)
   - **Presentation Layer**: Godot scenes and UI (full Godot integration)

**Rationale**: This hybrid approach balances Godot best practices with TDD requirements. The state machine provides clear game flow, components enable reusability, and layer separation ensures testability.

### 2.2 Design Principles

1. **Separation of Concerns**: Game logic independent of UI and Godot framework
2. **Testability First**: All business logic unit testable without Godot runtime
3. **SOLID Principles**: Single Responsibility, Dependency Inversion, Interface Segregation
4. **Godot Conventions**: Scenes for composition, signals for communication, nodes for structure
5. **STD-0002 Compliance**: Egyptian braces, pattern matching, strongly-typed IDs, functional composition
6. **YAGNI (You Aren't Gonna Need It)**: Don't over-engineer for hypothetical future features
7. **Immutability by Default**: Use records and readonly where possible

### 2.3 Architectural Diagram

```
┌──────────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                             │
│  ┌──────────┐  ┌────────────┐  ┌────────────┐  ┌──────────┐    │
│  │ MainMenu │  │SingleMatch │  │ Tournament │  │ Survival │    │
│  │ (Scene)  │  │  (Scene)   │  │  (Scene)   │  │ (Scene)  │    │
│  └────┬─────┘  └─────┬──────┘  └─────┬──────┘  └────┬─────┘    │
│       │              │               │              │            │
│       │  ┌───────────┴───────────────┴──────────────┴─────┐     │
│       │  │         Multiplayer (Scene)                     │     │
│       │  │         ResultScene (Scene)                     │     │
│       │  └─────────────────────────────────────────────────┘     │
│       │ Signals       │ Signals       │ Signals       │          │
└───────┼───────────────┼───────────────┼───────────────┼──────────┘
          │                 │                 │
┌─────────┼─────────────────┼─────────────────┼──────────────┐
│         │      SERVICE LAYER (Managers)     │               │
│         ▼                 ▼                 ▼               │
│  ┌──────────────────────────────────────────────────┐      │
│  │          GameManager (State Machine)             │      │
│  │  - Orchestrates game flow                        │      │
│  │  - Manages state transitions                     │      │
│  │  - Coordinates components                        │      │
│  └────┬─────────────┬──────────────┬────────────────┘      │
│       │             │              │                        │
│  ┌────▼──────┐ ┌───▼────────┐ ┌──▼──────────┐            │
│  │ScoreTracker│ │ModeManager │ │AudioManager │            │
│  └────┬───────┘ └───┬────────┘ └─────────────┘            │
│       │             │                                       │
│       │      ┌──────▼────────┐                             │
│       │      │ TournamentMgr │                             │
│       │      │ SurvivalMgr   │                             │
│       │      └───────────────┘                             │
└───────┼─────────────┼───────────────────────────────────────┘
        │             │
┌───────┼─────────────┼───────────────────────────────────────┐
│       │   CORE LAYER (Pure C# - No Godot)  │               │
│       ▼             ▼                       ▼               │
│  ┌────────────┐ ┌──────────┐ ┌──────────────────────┐     │
│  │ GameLogic  │ │AIOpponent│ │  Data Types          │     │
│  │ - Winner   │ │- Random  │ │  - Choice (record)   │     │
│  │   logic    │ │  choice  │ │  - RoundResult       │     │
│  └────────────┘ └──────────┘ │  - GameMode (enum)   │     │
│                               │  - PlayerId (record) │     │
│                               └──────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
```

**Flow**: User interacts with Presentation → Signals notify Service Layer → Services call Core Logic → Results flow back up

---

## 3. Scene Hierarchy

### 3.1 Scene Structure

```
Main.tscn (Root Scene - Never unloaded)
├── GameManager (Node - C# script)
│   └── Manages game state, coordinates all systems
│
├── AudioManager (Node - C# script)
│   └── Handles SFX and music playback
│
└── SceneContainer (Node)
    └── (Current scene loaded here dynamically)
        │
        ├── MainMenu.tscn (Scene instance)
        │   ├── Control (VBoxContainer)
        │   │   ├── TitleLabel (Label)
        │   │   ├── PlayButton (Button) → signal: pressed
        │   │   └── QuitButton (Button) → signal: pressed
        │   └── MainMenuController (Node - C# script)
        │
        ├── ModeSelection.tscn (Scene instance)
        │   ├── Control (VBoxContainer)
        │   │   ├── SingleMatchButton (Button)
        │   │   ├── TournamentButton (Button)
        │   │   ├── SurvivalButton (Button)
        │   │   ├── MultiplayerButton (Button)
        │   │   └── BackButton (Button)
        │   └── ModeSelectionController (Node - C# script)
        │
        ├── SingleMatchScene.tscn (Scene instance - Single Match mode)
        │   ├── UI (Control)
        │   │   ├── TopBar (HBoxContainer)
        │   │   │   ├── PlayerNameLabel (Label)
        │   │   │   ├── ScoreLabel (Label)
        │   │   │   └── OpponentNameLabel (Label)
        │   │   │
        │   │   ├── ChoiceArea (CenterContainer)
        │   │   │   ├── ChoiceCards (HBoxContainer)
        │   │   │   │   ├── RockButton (TextureButton)
        │   │   │   │   ├── PaperButton (TextureButton)
        │   │   │   │   └── ScissorsButton (TextureButton)
        │   │   │   └── ChoiceController (Node - C# script)
        │   │   │
        │   │   └── RevealArea (Control - Initially hidden)
        │   │       ├── PlayerChoiceDisplay (TextureRect)
        │   │       ├── OpponentChoiceDisplay (TextureRect)
        │   │       └── ResultLabel (Label)
        │   │
        │   └── SingleMatchController (Node - C# script)
        │
        ├── TournamentScene.tscn (Scene instance - Tournament mode)
        │   ├── UI (Control)
        │   │   ├── TopBar (HBoxContainer)
        │   │   │   ├── PlayerNameLabel (Label)
        │   │   │   ├── ScoreLabel (Label)
        │   │   │   └── OpponentNameLabel (Label)
        │   │   │
        │   │   ├── TournamentInfo (VBoxContainer)
        │   │   │   ├── BracketDisplay (Custom Control)
        │   │   │   └── RoundLabel (Label) - "Quarterfinals", etc.
        │   │   │
        │   │   ├── ChoiceArea (CenterContainer)
        │   │   │   ├── ChoiceCards (HBoxContainer)
        │   │   │   │   ├── RockButton (TextureButton)
        │   │   │   │   ├── PaperButton (TextureButton)
        │   │   │   │   └── ScissorsButton (TextureButton)
        │   │   │   └── ChoiceController (Node - C# script)
        │   │   │
        │   │   └── RevealArea (Control - Initially hidden)
        │   │       ├── PlayerChoiceDisplay (TextureRect)
        │   │       ├── OpponentChoiceDisplay (TextureRect)
        │   │       └── ResultLabel (Label)
        │   │
        │   └── TournamentSceneController (Node - C# script)
        │
        ├── SurvivalScene.tscn (Scene instance - Survival mode)
        │   ├── UI (Control)
        │   │   ├── TopBar (HBoxContainer)
        │   │   │   ├── PlayerNameLabel (Label)
        │   │   │   ├── ScoreLabel (Label)
        │   │   │   └── OpponentNameLabel (Label)
        │   │   │
        │   │   ├── SurvivalInfo (VBoxContainer)
        │   │   │   ├── HPBar (ProgressBar)
        │   │   │   ├── DefeatedLabel (Label) - "Defeated: X"
        │   │   │   └── HighScoreLabel (Label) - "High Score: X"
        │   │   │
        │   │   ├── ChoiceArea (CenterContainer)
        │   │   │   ├── ChoiceCards (HBoxContainer)
        │   │   │   │   ├── RockButton (TextureButton)
        │   │   │   │   ├── PaperButton (TextureButton)
        │   │   │   │   └── ScissorsButton (TextureButton)
        │   │   │   └── ChoiceController (Node - C# script)
        │   │   │
        │   │   └── RevealArea (Control - Initially hidden)
        │   │       ├── PlayerChoiceDisplay (TextureRect)
        │   │       ├── OpponentChoiceDisplay (TextureRect)
        │   │       └── ResultLabel (Label)
        │   │
        │   └── SurvivalSceneController (Node - C# script)
        │
        ├── MultiplayerScene.tscn (Scene instance - Local multiplayer)
        │   ├── UI (Control)
        │   │   ├── TopBar (HBoxContainer)
        │   │   │   ├── Player1NameLabel (Label)
        │   │   │   ├── ScoreLabel (Label)
        │   │   │   └── Player2NameLabel (Label)
        │   │   │
        │   │   ├── ChoiceArea (CenterContainer)
        │   │   │   ├── ChoiceCards (HBoxContainer)
        │   │   │   │   ├── RockButton (TextureButton)
        │   │   │   │   ├── PaperButton (TextureButton)
        │   │   │   │   └── ScissorsButton (TextureButton)
        │   │   │   └── ChoiceController (Node - C# script)
        │   │   │
        │   │   ├── TransitionScreen (Control - Shows between turns and before reveal)
        │   │   │   ├── MessageLabel (Label) - Dynamic message:
        │   │   │   │   - "[Player 2]'s Turn - Ready?" (after P1 choice)
        │   │   │   │   - "Ready for Reveal?" (after P2 choice)
        │   │   │   └── ReadyButton (Button) - "Ready"
        │   │   │
        │   │   └── RevealArea (Control - Initially hidden)
        │   │       ├── Player1ChoiceDisplay (TextureRect)
        │   │       ├── Player2ChoiceDisplay (TextureRect)
        │   │       └── ResultLabel (Label)
        │   │
        │   └── MultiplayerSceneController (Node - C# script)
        │       └── Flow: P1 choice → Transition 1 → P2 choice → Transition 2 → Reveal
        │
        └── ResultScene.tscn (Scene instance)
            ├── Control (VBoxContainer)
            │   ├── ResultLabel (Label) - "You Win!" / "You Lose!"
            │   ├── FinalScoreLabel (Label)
            │   ├── StatsLabel (Label) - Mode-specific stats
            │   ├── PlayAgainButton (Button)
            │   └── MainMenuButton (Button)
            └── ResultSceneController (Node - C# script)
```

### 3.2 Scene Transitions

**State-Driven Scene Loading**:

```
GameManager.ChangeState(GameState newState, GameMode mode) triggers scene changes:

MainMenu state       → Load MainMenu.tscn
ModeSelection state  → Load ModeSelection.tscn
Playing state        → Load mode-specific scene:
                       - GameMode.SingleMatch   → SingleMatchScene.tscn
                       - GameMode.Tournament    → TournamentScene.tscn
                       - GameMode.Survival      → SurvivalScene.tscn
                       - GameMode.Multiplayer   → MultiplayerScene.tscn
GameOver state       → Load ResultScene.tscn
```

**Scene Loading Strategy**:
- Use `ResourceLoader.LoadThreaded*` for async loading (future optimization)
- For MVP: `PackedScene.Instantiate()` with immediate loading
- Old scene freed before new scene instantiated (memory efficiency)
- Transition fade: 0.3s black screen between scenes (optional polish)

### 3.3 Scene Responsibilities

| Scene | Responsibility | Input Handling | State Changes Triggered |
|-------|---------------|----------------|------------------------|
| **MainMenu.tscn** | Display title, start button, quit option | Play button, Quit button | MainMenu → ModeSelection |
| **ModeSelection.tscn** | Display game mode options | Mode selection buttons | ModeSelection → Playing |
| **SingleMatchScene.tscn** | Single match gameplay, score display | Choice buttons (Rock/Paper/Scissors) | Playing → GameOver |
| **TournamentScene.tscn** | Tournament gameplay, bracket display | Choice buttons, bracket navigation | Playing → GameOver |
| **SurvivalScene.tscn** | Survival gameplay, HP bar, high score | Choice buttons | Playing → GameOver |
| **MultiplayerScene.tscn** | Hot-seat multiplayer with manual transition | Choice buttons, Ready button | Playing → GameOver |
| **ResultScene.tscn** | Display final results, offer replay or return to menu | Play Again, Main Menu buttons | GameOver → Playing OR MainMenu |

---

## 4. Component Architecture

### 4.1 GameManager (Service Layer)

**File**: `scripts/managers/GameManager.cs`

**Responsibility**:
- Centralized game state machine
- Orchestrates scene transitions
- Coordinates communication between managers
- Manages game mode lifecycle

**Signals** (Godot):
```csharp
[Signal] public delegate void StateChangedEventHandler(GameState newState);
[Signal] public delegate void GameModeStartedEventHandler(GameMode mode);
[Signal] public delegate void RoundCompletedEventHandler(RoundResult result);
[Signal] public delegate void MatchCompletedEventHandler(MatchResult result);
```

**Public Methods**:
```csharp
void ChangeState(GameState newState);
void StartGame(GameMode mode, GameConfig config);
void ProcessPlayerChoice(PlayerId playerId, Choice choice);
void ReturnToMainMenu();
void QuitGame();
```

**Dependencies**:
- ScoreTracker
- ModeManager (factory for mode-specific managers)
- AudioManager
- GameLogic (core)

**State Management**:
```csharp
private GameState _currentState = GameState.MainMenu;
private Dictionary<GameState, Action> _stateHandlers;
```

### 4.2 GameLogic (Core Layer)

**File**: `scripts/core/GameLogic.cs`

**Responsibility**:
- Pure game rules (Rock beats Scissors, etc.)
- Determine round winners
- Validate choices
- **Zero Godot dependencies** (100% unit testable)

**Public Methods**:
```csharp
RoundResult DetermineWinner(Choice playerChoice, Choice opponentChoice);
bool IsValidChoice(Choice choice);
Choice GetWinningChoice(Choice losingChoice);
Choice GetLosingChoice(Choice winningChoice);
```

**Implementation Notes**:
- Static class (no state)
- Uses pattern matching (STD-0002 compliance)
- Returns Result<T> for error cases (no exceptions)

**Example**:
```csharp
public static RoundResult DetermineWinner(Choice player, Choice opponent) =>
    (player, opponent) switch {
        var (p, o) when p == o => RoundResult.Draw(p, o),
        (Choice.Rock, Choice.Scissors) => RoundResult.Win(player, opponent),
        (Choice.Scissors, Choice.Paper) => RoundResult.Win(player, opponent),
        (Choice.Paper, Choice.Rock) => RoundResult.Win(player, opponent),
        _ => RoundResult.Loss(player, opponent)
    };
```

### 4.3 ScoreTracker (Service Layer)

**File**: `scripts/managers/ScoreTracker.cs`

**Responsibility**:
- Track scores for current match
- Determine match winner based on win threshold
- Reset scores between matches
- Calculate statistics (win rate, rounds played)

**Public Methods**:
```csharp
void RecordRoundResult(RoundResult result);
int GetScore(PlayerId playerId);
void ResetScores();
bool HasMatchWinner(int winThreshold, out PlayerId? winnerId);
ScoreState GetCurrentScores();
```

**State**:
```csharp
private Dictionary<PlayerId, int> _scores = new();
private List<RoundResult> _roundHistory = new();
```

### 4.4 AIOpponent (Core Layer)

**File**: `scripts/core/AIOpponent.cs`

**Responsibility**:
- Generate random choice for AI opponent
- Future: Support multiple AI strategies (not in MVP)

**Public Methods**:
```csharp
Choice MakeChoice();
Choice MakeChoice(IRandom random); // Dependency injection for testing
```

**Implementation**:
```csharp
private static readonly Choice[] Choices = [Choice.Rock, Choice.Paper, Choice.Scissors];

public Choice MakeChoice(IRandom random) =>
    Choices[random.Next(0, Choices.Length)];
```

**Testing Notes**:
- Inject mock `IRandom` for deterministic tests
- Verify distribution over large samples (statistical tests)

### 4.5 ModeManager (Service Layer)

**File**: `scripts/managers/ModeManager.cs`

**Responsibility**:
- Factory for mode-specific managers
- Delegates mode-specific logic to specialized managers
- Provides unified interface for GameManager

**Public Methods**:
```csharp
IModeController CreateController(GameMode mode, GameConfig config);
```

**Mode Controllers**:
- **SingleMatchController**: Manages best-of-3 match
- **TournamentController**: Manages bracket progression
- **SurvivalController**: Manages HP system and continuous battles
- **MultiplayerController**: Manages hot-seat turn transitions

### 4.6 TournamentController (Service Layer)

**File**: `scripts/modes/TournamentController.cs`

**Responsibility**:
- Manage tournament bracket structure (8 or 16 players)
- Track player progression through rounds
- Determine next opponent
- Handle tournament completion

**Public Methods**:
```csharp
void Initialize(int playerCount); // 8 or 16
MatchResult ProcessMatchResult(PlayerId winner);
OpponentId GetNextOpponent();
TournamentState GetCurrentState();
bool IsTournamentComplete();
```

**State**:
```csharp
private TournamentBracket _bracket;
private int _currentRound;
private int _currentMatchInRound;
```

### 4.7 SurvivalController (Service Layer)

**File**: `scripts/modes/SurvivalController.cs`

**Responsibility**:
- Track player HP (starts at 10)
- Count opponents defeated
- Manage continuous battle flow
- Track high score for session

**Public Methods**:
```csharp
void Initialize(int startingHp);
void ProcessMatchResult(MatchResult result);
int GetCurrentHp();
int GetOpponentsDefeated();
int GetSessionHighScore();
bool IsGameOver();
```

**State**:
```csharp
private int _playerHp = 10;
private int _opponentsDefeated = 0;
private int _sessionHighScore = 0; // Resets when game closes
```

### 4.8 ChoiceController (Presentation Layer)

**File**: `scripts/ui/ChoiceController.cs`

**Responsibility**:
- Handle player input on choice buttons
- Animate card selection
- Disable input after choice made
- Emit signal with selected choice

**Signals**:
```csharp
[Signal] public delegate void ChoiceMadeEventHandler(Choice choice);
```

**Public Methods**:
```csharp
void EnableInput();
void DisableInput();
void HighlightChoice(Choice choice);
void ResetChoices();
```

### 4.9 AudioManager (Service Layer)

**File**: `scripts/managers/AudioManager.cs`

**Responsibility**:
- Play sound effects for game events
- Manage background music (optional)
- Control volume levels

**Public Methods**:
```csharp
void PlaySFX(SoundEffect sfx);
void PlayMusic(MusicTrack track);
void StopMusic();
void SetSFXVolume(float volume);
void SetMusicVolume(float volume);
```

**Supported SFX**:
```csharp
public enum SoundEffect {
    ButtonClick,
    ChoiceSelected,
    CardReveal,
    RoundWin,
    RoundLose,
    Draw,
    MatchWin,
    MatchLose
}
```

---

## 5. Data Flow

### 5.1 Player Choice Flow

**Scenario**: Player clicks "Rock" button in Single Match mode

```
1. User clicks Rock button (GameScene.tscn)
   ↓
2. ChoiceController._OnRockButtonPressed() fires
   ↓
3. ChoiceController validates input is enabled
   ↓
4. ChoiceController disables further input
   ↓
5. ChoiceController emits signal: ChoiceMade(Choice.Rock)
   ↓
6. GameSceneController receives signal
   ↓
7. GameSceneController forwards to GameManager.ProcessPlayerChoice(PlayerId.Human, Choice.Rock)
   ↓
8. GameManager stores player choice
   ↓
9. GameManager calls AIOpponent.MakeChoice() → returns Choice.Paper
   ↓
10. GameManager stores opponent choice
    ↓
11. GameManager calls GameLogic.DetermineWinner(Choice.Rock, Choice.Paper)
    ↓
12. GameLogic returns RoundResult.Loss(Choice.Rock, Choice.Paper)
    ↓
13. GameManager passes result to ScoreTracker.RecordRoundResult(result)
    ↓
14. ScoreTracker updates scores: Player: 0, AI: 1
    ↓
15. GameManager emits signal: RoundCompleted(result)
    ↓
16. GameSceneController receives signal
    ↓
17. GameSceneController triggers reveal animation:
    - Show player choice card (Rock)
    - Wait 1 second
    - Show opponent choice card (Paper)
    - Display "You Lose!" text
    - Update score display: "0 - 1"
    ↓
18. AudioManager plays RoundLose SFX
    ↓
19. After 3 second pause, GameSceneController emits ReadyForNextRound signal
    ↓
20. GameManager checks ScoreTracker.HasMatchWinner(winThreshold: 2)
    ↓
21. If no winner: GameManager resets round, re-enables input (back to step 1)
    If winner: GameManager transitions to GameOver state → load ResultScene.tscn
```

### 5.2 State Transition Flow

**State Machine Transitions**:

```
[MainMenu State]
  │
  ├─ User clicks "Play" button
  │  → GameManager.ChangeState(GameState.ModeSelection)
  │  → Load ModeSelection.tscn
  ↓
[ModeSelection State]
  │
  ├─ User clicks "Single Match"
  │  → GameManager.StartGame(GameMode.SingleMatch, config)
  │  → GameManager.ChangeState(GameState.Playing)
  │  → Load GameScene.tscn
  ↓
[Playing State]
  │
  ├─ Round completes (both choices made, winner determined)
  │  → ScoreTracker.HasMatchWinner() checked
  │  │
  │  ├─ No winner yet: Stay in Playing state, reset round
  │  │
  │  └─ Winner determined:
  │     → GameManager.ChangeState(GameState.GameOver)
  │     → Load ResultScene.tscn
  ↓
[GameOver State]
  │
  ├─ User clicks "Play Again"
  │  → GameManager.StartGame(same mode, reset config)
  │  → GameManager.ChangeState(GameState.Playing)
  │
  ├─ User clicks "Main Menu"
  │  → GameManager.ReturnToMainMenu()
  │  → GameManager.ChangeState(GameState.MainMenu)
  │
  └─ User clicks "Quit"
     → GameManager.QuitGame()
     → Application exits
```

### 5.3 Score Update Flow

```
1. RoundResult determined by GameLogic
   ↓
2. GameManager → ScoreTracker.RecordRoundResult(result)
   ↓
3. ScoreTracker updates internal dictionary:
   - If result.Winner == PlayerId.Human: _scores[PlayerId.Human]++
   - If result.Winner == PlayerId.AI: _scores[PlayerId.AI]++
   - If result.Winner == null: no score change (draw)
   ↓
4. ScoreTracker adds result to _roundHistory
   ↓
5. ScoreTracker returns updated ScoreState
   ↓
6. GameManager emits RoundCompleted signal with result and scores
   ↓
7. GameSceneController receives signal
   ↓
8. GameSceneController calls UI update:
   - ScoreLabel.Text = $"{scores.PlayerScore} - {scores.OpponentScore}"
   - Brief highlight animation (0.5s scale pulse)
   ↓
9. GameSceneController checks for match winner
   ↓
10. If winner: Trigger celebration animation, then transition to GameOver state
```

---

## 6. State Management

### 6.1 Game States

```csharp
public enum GameState {
    MainMenu,       // Player at main menu, no game active
    ModeSelection,  // Player selecting game mode
    Playing,        // Active gameplay (choice selection, reveals, rounds)
    GameOver        // Match complete, showing results
}
```

**State Descriptions**:

| State | Description | Valid Transitions | Scene |
|-------|-------------|-------------------|-------|
| **MainMenu** | Initial state, game logo and play button | → ModeSelection | MainMenu.tscn |
| **ModeSelection** | Choosing game mode (Single/Tournament/Survival/Multiplayer) | → Playing, → MainMenu | ModeSelection.tscn |
| **Playing** | Active match (rounds in progress) | → GameOver, → MainMenu (quit) | GameScene.tscn |
| **GameOver** | Match finished, results displayed | → Playing (replay), → MainMenu | ResultScene.tscn |

### 6.2 State Transitions

**Transition Diagram**:

```
    ┌─────────────┐
    │  MainMenu   │ ◄──────────────┐
    └──────┬──────┘                │
           │                       │
           │ "Play"                │ "Main Menu"
           ▼                       │
    ┌─────────────┐                │
    │   Mode      │                │
    │  Selection  │ ───────────────┤
    └──────┬──────┘    "Back"      │
           │                       │
           │ "Select Mode"         │
           ▼                       │
    ┌─────────────┐                │
    │   Playing   │ ───────────────┤
    │             │    "Quit"      │
    └──────┬──────┘                │
           │                       │
           │ "Match Over"          │
           ▼                       │
    ┌─────────────┐                │
    │  GameOver   │ ───────────────┘
    └─────────────┘   "Play Again" (loops to Playing)
```

**Transition Rules**:

```csharp
private readonly Dictionary<(GameState From, GameState To), bool> _validTransitions = new() {
    { (GameState.MainMenu, GameState.ModeSelection), true },
    { (GameState.ModeSelection, GameState.Playing), true },
    { (GameState.ModeSelection, GameState.MainMenu), true },
    { (GameState.Playing, GameState.GameOver), true },
    { (GameState.Playing, GameState.MainMenu), true },
    { (GameState.GameOver, GameState.Playing), true },
    { (GameState.GameOver, GameState.MainMenu), true },
};

public bool IsValidTransition(GameState from, GameState to) =>
    _validTransitions.ContainsKey((from, to)) && _validTransitions[(from, to)];
```

### 6.3 State Machine Implementation

**Approach**: Simple switch-based state machine (no State Pattern classes)

**Rationale**:
- Game has only 4 states (low complexity)
- State logic is minimal (mostly scene transitions)
- Switch expressions align with STD-0002 standards
- Easier to test than State Pattern classes

**Implementation**:

```csharp
public void ChangeState(GameState newState) {
    if (!IsValidTransition(_currentState, newState)) {
        GD.PushError($"Invalid state transition: {_currentState} → {newState}");
        return;
    }

    ExitState(_currentState);
    _currentState = newState;
    EnterState(newState);
    EmitSignal(SignalName.StateChanged, (int)newState);
}

private void EnterState(GameState state) => state switch {
    GameState.MainMenu => LoadScene(ScenePaths.MainMenu),
    GameState.ModeSelection => LoadScene(ScenePaths.ModeSelection),
    GameState.Playing => InitializeGameplay(),
    GameState.GameOver => ShowResults(),
    _ => throw new ArgumentOutOfRangeException(nameof(state))
};

private void ExitState(GameState state) => state switch {
    GameState.Playing => CleanupGameplay(),
    _ => Task.CompletedTask // Most states need no cleanup
};
```

**Sub-States** (within Playing):

For more granular control within gameplay, a sub-state enum is used:

```csharp
public enum PlayingSubState {
    WaitingForChoice,     // Player can select Rock/Paper/Scissors
    ChoiceLocked,         // Player made choice, waiting for opponent
    RevealingChoices,     // Animated reveal of both choices
    ShowingResult,        // Displaying round result (3 second pause)
    TransitioningRound    // Brief transition to next round or game over
}
```

**Rationale for Sub-States**:
- Prevents input during animations
- Enables granular UI updates
- Simplifies testing (each sub-state has clear entry/exit conditions)

---

## 7. Testing Strategy

### 7.1 Unit Testing Approach

**Principle**: Test core logic in isolation without Godot runtime.

**Core Layer Tests** (100% coverage target):

1. **GameLogic Tests** (`tests/unit/test_game_logic.gd`)
   ```gdscript
   # Test all win conditions
   - Rock beats Scissors
   - Scissors beats Paper
   - Paper beats Rock

   # Test draw conditions
   - Rock vs Rock = Draw
   - Paper vs Paper = Draw
   - Scissors vs Scissors = Draw

   # Test invalid inputs
   - Null choices handled gracefully
   - Invalid enum values rejected
   ```

2. **AIOpponent Tests** (`tests/unit/test_ai_opponent.gd`)
   ```gdscript
   # Test random distribution (statistical)
   - 1000 choices: ~33% each for Rock/Paper/Scissors
   - Chi-square test for uniformity

   # Test deterministic behavior (with mock Random)
   - Seeded random produces expected sequence
   - Same seed = same choices
   ```

3. **ScoreTracker Tests** (`tests/unit/test_score_tracker.gd`)
   ```gdscript
   # Test score updates
   - Win increments winner's score
   - Loss increments loser's score
   - Draw doesn't change scores

   # Test match winner detection
   - Best of 3: First to 2 wins
   - No winner if tied 1-1
   - Winner correctly identified

   # Test reset
   - ResetScores() clears all scores
   - Round history preserved (or cleared based on requirement)
   ```

4. **TournamentController Tests** (`tests/unit/test_tournament_controller.gd`)
   ```gdscript
   # Test bracket initialization
   - 8-player bracket: 3 rounds (QF, SF, F)
   - 16-player bracket: 4 rounds (R16, QF, SF, F)

   # Test progression
   - Winner advances to next round
   - Loser is eliminated
   - Correct opponent pairing

   # Test completion
   - Tournament complete after final match
   - Correct champion determined
   ```

5. **SurvivalController Tests** (`tests/unit/test_survival_controller.gd`)
   ```gdscript
   # Test HP system
   - Start with 10 HP
   - Losing match: -1 HP
   - Winning match: HP unchanged
   - Game over at 0 HP

   # Test opponent counter
   - Increments after each match (win or loss)
   - High score updated when surpassed
   ```

**Service Layer Tests** (80%+ coverage target):

6. **GameManager Tests** (`tests/unit/test_game_manager.gd`)
   ```gdscript
   # Test state transitions
   - Valid transitions succeed
   - Invalid transitions rejected (logged, not crashed)

   # Test game flow
   - StartGame() initializes correctly
   - ProcessPlayerChoice() triggers correct sequence
   - ReturnToMainMenu() cleans up state
   ```

### 7.2 Integration Testing Approach

**Integration Tests** simulate real game flows:

1. **Full Match Flow** (`tests/integration/test_single_match_flow.gd`)
   ```gdscript
   # Simulate complete match
   1. Start single match
   2. Player chooses Rock (wins)
   3. Verify score: 1-0
   4. Player chooses Paper (loses)
   5. Verify score: 1-1
   6. Player chooses Scissors (wins)
   7. Verify match complete, player winner
   ```

2. **Tournament Flow** (`tests/integration/test_tournament_flow.gd`)
   ```gdscript
   # Simulate 8-player tournament
   1. Initialize tournament
   2. Win quarterfinal
   3. Verify advancement to semifinal
   4. Win semifinal
   5. Win final
   6. Verify champion status
   ```

3. **Survival Flow** (`tests/integration/test_survival_flow.gd`)
   ```gdscript
   # Simulate survival mode
   1. Start with 10 HP
   2. Win 3 matches (HP: 10, Defeated: 3)
   3. Lose 1 match (HP: 9, Defeated: 4)
   4. Continue until 0 HP
   5. Verify game over, high score recorded
   ```

4. **Scene Transition Tests** (`tests/integration/test_scene_transitions.gd`)
   ```gdscript
   # Verify scene loading
   1. MainMenu → ModeSelection
   2. ModeSelection → Playing
   3. Playing → GameOver
   4. GameOver → MainMenu
   - Assert correct scene loaded
   - Assert GameManager state matches
   ```

### 7.3 Test Coverage Goals

| Layer | Component | Coverage Target | Rationale |
|-------|-----------|----------------|-----------|
| **Core** | GameLogic | 100% | Critical business logic, pure functions |
| **Core** | AIOpponent | 100% | Deterministic with mocked random |
| **Service** | ScoreTracker | 100% | Core scoring logic |
| **Service** | TournamentController | 90% | Complex branching, edge cases |
| **Service** | SurvivalController | 90% | HP logic critical |
| **Service** | GameManager | 80% | State machine coverage |
| **Service** | AudioManager | 50% | Primarily Godot API calls |
| **Presentation** | UI Controllers | 60% | Heavy Godot dependencies |
| **Overall** | All | >80% | Industry standard |

### 7.4 Testing Tools

**Primary Framework**: GUT (Godot Unit Test)
- Version: Latest compatible with Godot 4.5
- Language: GDScript (for tests), C# (for production code)
- Run Command: `godot --headless --script res://addons/gut/gut_cmdln.gd`

**Mocking Strategy**:
- **Core Layer**: No mocking needed (pure functions)
- **Service Layer**: Mock Godot nodes using GUT's `double()` and `stub()`
- **Randomness**: Inject `IRandom` interface for deterministic tests

**Test Organization**:
```
tests/
├── unit/
│   ├── test_game_logic.gd
│   ├── test_ai_opponent.gd
│   ├── test_score_tracker.gd
│   ├── test_tournament_controller.gd
│   └── test_survival_controller.gd
│
├── integration/
│   ├── test_single_match_flow.gd
│   ├── test_tournament_flow.gd
│   └── test_survival_flow.gd
│
└── .gutconfig.json  # GUT configuration
```

**CI/CD Integration** (Future):
- Run tests on every commit
- Block merge if tests fail
- Generate coverage reports

---

## 8. File/Folder Structure

```
spr/
├── project.godot                  # Godot project file
├── .gutconfig.json                # GUT test runner config
│
├── docs/                          # Phase outputs
│   ├── requirements.md            # Phase 1 ✅
│   ├── architecture.md            # Phase 2 (this file)
│   └── game-design-doc.md         # Phase 3 (pending)
│
├── scenes/                        # Godot .tscn files
│   ├── Main.tscn                  # Root scene (never unloaded)
│   ├── MainMenu.tscn              # Main menu UI
│   ├── ModeSelection.tscn         # Mode selection UI
│   ├── SingleMatchScene.tscn      # Single match gameplay scene
│   ├── TournamentScene.tscn       # Tournament gameplay scene
│   ├── SurvivalScene.tscn         # Survival gameplay scene
│   ├── MultiplayerScene.tscn      # Local multiplayer gameplay scene
│   └── ResultScene.tscn           # Game over / match results
│
├── scripts/                       # C# source code
│   │
│   ├── core/                      # Core Layer (pure C#, no Godot)
│   │   ├── GameLogic.cs           # Win/loss determination
│   │   ├── AIOpponent.cs          # Random choice generation
│   │   ├── Choice.cs              # Choice enum or record
│   │   ├── RoundResult.cs         # Result record (winner, choices)
│   │   ├── GameMode.cs            # Game mode enum
│   │   ├── PlayerId.cs            # Strongly-typed player ID
│   │   └── Result.cs              # Result<T> type for error handling
│   │
│   ├── managers/                  # Service Layer
│   │   ├── GameManager.cs         # State machine, game orchestration
│   │   ├── ScoreTracker.cs        # Score tracking and match winner detection
│   │   ├── ModeManager.cs         # Factory for mode controllers
│   │   └── AudioManager.cs        # SFX and music playback
│   │
│   ├── modes/                     # Mode-specific controllers
│   │   ├── IModeController.cs     # Interface for mode controllers
│   │   ├── SingleMatchController.cs
│   │   ├── TournamentController.cs
│   │   ├── SurvivalController.cs
│   │   └── MultiplayerController.cs
│   │
│   ├── ui/                        # Presentation Layer (Godot-heavy)
│   │   ├── MainMenuController.cs             # MainMenu scene controller
│   │   ├── ModeSelectionController.cs        # Mode selection scene controller
│   │   ├── SingleMatchSceneController.cs     # Single match scene controller
│   │   ├── TournamentSceneController.cs      # Tournament scene controller
│   │   ├── SurvivalSceneController.cs        # Survival scene controller
│   │   ├── MultiplayerSceneController.cs     # Multiplayer scene controller
│   │   ├── ResultSceneController.cs          # Result scene controller
│   │   ├── ChoiceController.cs               # Choice button input handler (reusable)
│   │   ├── ScoreDisplay.cs                   # Score UI component (reusable)
│   │   └── RevealAnimator.cs                 # Reveal animation component (reusable)
│   │
│   └── common/                    # Shared utilities
│       ├── ScenePaths.cs          # Const strings for scene paths
│       ├── IRandom.cs             # Random interface (for testing)
│       ├── RandomAdapter.cs       # System.Random → IRandom adapter
│       └── Extensions.cs          # Extension methods (if needed)
│
├── tests/                         # GUT test files (GDScript)
│   ├── unit/
│   │   ├── test_game_logic.gd
│   │   ├── test_ai_opponent.gd
│   │   ├── test_score_tracker.gd
│   │   ├── test_tournament_controller.gd
│   │   ├── test_survival_controller.gd
│   │   └── test_game_manager.gd
│   │
│   └── integration/
│       ├── test_single_match_flow.gd
│       ├── test_tournament_flow.gd
│       ├── test_survival_flow.gd
│       └── test_scene_transitions.gd
│
└── assets/                        # Art, audio, fonts
    ├── sprites/                   # Card artwork, UI elements
    │   ├── cards/
    │   │   ├── rock.png
    │   │   ├── paper.png
    │   │   └── scissors.png
    │   ├── ui/
    │   │   ├── button_normal.png
    │   │   ├── button_hover.png
    │   │   └── button_pressed.png
    │   └── backgrounds/
    │       └── game_bg.png
    │
    ├── audio/                     # Sound effects and music
    │   ├── sfx/
    │   │   ├── button_click.wav
    │   │   ├── choice_select.wav
    │   │   ├── card_reveal.wav
    │   │   ├── round_win.wav
    │   │   ├── round_lose.wav
    │   │   ├── draw.wav
    │   │   ├── match_win.wav
    │   │   └── match_lose.wav
    │   └── music/
    │       └── background_loop.ogg (optional for MVP)
    │
    └── fonts/
        └── main_font.ttf          # UI font (open-source)
```

**Naming Conventions**:
- **C# files**: PascalCase (e.g., `GameManager.cs`)
- **GDScript files**: snake_case (e.g., `test_game_logic.gd`)
- **Scene files**: PascalCase (e.g., `MainMenu.tscn`)
- **Asset folders**: lowercase (e.g., `sprites/`, `audio/`)
- **Asset files**: snake_case (e.g., `rock.png`, `button_click.wav`)

---

## 9. Key Interfaces

### 9.1 Core Interfaces

```csharp
// File: scripts/core/Choice.cs
public enum Choice {
    Rock,
    Paper,
    Scissors
}

// File: scripts/core/RoundResult.cs
public record RoundResult(
    PlayerId? Winner,      // null if draw
    Choice PlayerChoice,
    Choice OpponentChoice
) {
    public bool IsDraw => Winner == null;

    public static RoundResult Win(Choice player, Choice opponent) =>
        new(PlayerId.Human, player, opponent);

    public static RoundResult Loss(Choice player, Choice opponent) =>
        new(PlayerId.AI, player, opponent);

    public static RoundResult Draw(Choice player, Choice opponent) =>
        new(null, player, opponent);
}

// File: scripts/core/PlayerId.cs
public record PlayerId(string Value) {
    public static PlayerId Human { get; } = new("human");
    public static PlayerId AI { get; } = new("ai");
    public static PlayerId Create(string name) => new(name);
}

// File: scripts/core/GameMode.cs
public enum GameMode {
    SingleMatch,
    Tournament,
    Survival,
    Multiplayer
}

// File: scripts/core/Result.cs
public abstract record Result<T> {
    public record Success(T Value) : Result<T>;
    public record Failure(string Error) : Result<T>;

    public bool IsSuccess => this is Success;
    public bool IsFailure => this is Failure;

    public T ValueOrDefault(T defaultValue) => this switch {
        Success(var value) => value,
        _ => defaultValue
    };
}
```

### 9.2 Service Layer Interfaces

```csharp
// File: scripts/modes/IModeController.cs
public interface IModeController {
    void Initialize(GameConfig config);
    void ProcessMatchResult(MatchResult result);
    bool IsGameOver();
    void Reset();
}

// File: scripts/common/IRandom.cs
public interface IRandom {
    int Next(int minValue, int maxValue);
    int Next(int maxValue);
}

// File: scripts/common/RandomAdapter.cs
public class RandomAdapter : IRandom {
    private readonly Random _random = new();

    public int Next(int minValue, int maxValue) =>
        _random.Next(minValue, maxValue);

    public int Next(int maxValue) =>
        _random.Next(maxValue);
}
```

### 9.3 Data Transfer Objects (DTOs)

```csharp
// File: scripts/core/GameConfig.cs
public record GameConfig(
    GameMode Mode,
    string PlayerName = "Player",
    string OpponentName = "CPU",
    int WinThreshold = 2,                    // Best of 3 = first to 2
    int TournamentSize = 8,                  // 8 or 16
    int SurvivalStartingHp = 10
);

// File: scripts/core/MatchResult.cs
public record MatchResult(
    PlayerId Winner,
    ScoreState FinalScores,
    List<RoundResult> RoundHistory
);

// File: scripts/core/ScoreState.cs
public record ScoreState(
    int PlayerScore,
    int OpponentScore,
    int RoundsPlayed
) {
    public int LeadingScore => Math.Max(PlayerScore, OpponentScore);
    public int TrailingScore => Math.Min(PlayerScore, OpponentScore);
    public bool IsTied => PlayerScore == OpponentScore;
}

// File: scripts/modes/TournamentState.cs
public record TournamentState(
    int CurrentRound,            // 1 = Quarterfinals, 2 = Semifinals, 3 = Finals
    int TotalRounds,             // 3 for 8-player, 4 for 16-player
    int MatchesInRound,
    int CurrentMatchInRound,
    string RoundName            // "Quarterfinals", "Semifinals", "Finals"
);

// File: scripts/modes/SurvivalState.cs
public record SurvivalState(
    int CurrentHp,
    int StartingHp,
    int OpponentsDefeated,
    int SessionHighScore
) {
    public float HpPercentage => (float)CurrentHp / StartingHp;
    public bool IsAlive => CurrentHp > 0;
}
```

---

## 10. Cross-Cutting Concerns

### 10.1 Logging

**Strategy**: Use Godot's `GD.Print()` for development, structured logging for production.

```csharp
public static class Logger {
    public static void Info(string message) =>
        GD.Print($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");

    public static void Warning(string message) =>
        GD.PushWarning($"[WARN] {DateTime.Now:HH:mm:ss} - {message}");

    public static void Error(string message) =>
        GD.PushError($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
}
```

**Logging Levels**:
- **Info**: State transitions, game events
- **Warning**: Invalid user input, recoverable errors
- **Error**: Unrecoverable errors, exceptions

**Examples**:
```csharp
Logger.Info($"State changed: {oldState} → {newState}");
Logger.Warning($"Invalid choice: {choice}");
Logger.Error($"Failed to load scene: {scenePath}");
```

### 10.2 Error Handling

**Principles**:
1. **No exceptions for expected failures** (use Result<T> pattern)
2. **Validate all external input** (user clicks, file loads)
3. **Fail gracefully** (never crash, always log)
4. **Provide meaningful feedback** (user-facing error messages)

**Examples**:

```csharp
// Use Result<T> for expected failures
public Result<Scene> LoadScene(string path) {
    if (!ResourceLoader.Exists(path))
        return new Result<Scene>.Failure($"Scene not found: {path}");

    var scene = ResourceLoader.Load<PackedScene>(path);
    return scene == null
        ? new Result<Scene>.Failure($"Failed to load: {path}")
        : new Result<Scene>.Success(scene.Instantiate());
}

// Validate user input
public void ProcessPlayerChoice(Choice choice) {
    if (!GameLogic.IsValidChoice(choice)) {
        Logger.Warning($"Invalid choice: {choice}");
        return; // Ignore invalid input, don't crash
    }

    // Process valid choice...
}

// Graceful degradation
public void PlaySFX(SoundEffect sfx) {
    if (!_sfxCache.ContainsKey(sfx)) {
        Logger.Warning($"SFX not loaded: {sfx}. Continuing without sound.");
        return; // Game continues even if sound fails
    }

    _audioPlayer.Stream = _sfxCache[sfx];
    _audioPlayer.Play();
}
```

### 10.3 Performance Considerations

**Target**: Maintain 60 FPS on minimum spec hardware (Intel Core i3, 4GB RAM, integrated graphics).

**Optimization Strategies**:

1. **Avoid Frame-by-Frame Updates**
   - Use signals for event-driven updates
   - Don't poll input in `_Process()` (use signals)
   - Cache UI references (don't use `GetNode()` in _Process)

2. **Object Pooling** (if needed)
   - Not critical for MVP (minimal object creation)
   - Consider for particle effects or card animations (future)

3. **Scene Caching**
   - Preload common scenes (MainMenu, GameScene)
   - Avoid repeated `ResourceLoader.Load()` calls

4. **Asset Optimization**
   - Card images: Max 512x512 pixels
   - Compress textures (lossy for JPG, lossless for PNG with transparency)
   - Use Ogg Vorbis for audio (smaller than WAV)

5. **Profiling**
   - Use Godot's built-in profiler
   - Identify bottlenecks before optimizing
   - Measure before/after optimization changes

**Anti-Patterns to Avoid**:
```csharp
// ❌ BAD: Polling in _Process
public override void _Process(double delta) {
    if (Input.IsActionJustPressed("ui_select"))
        OnChoiceSelected();
}

// ✅ GOOD: Signal-driven
public override void _Ready() {
    GetNode<Button>("RockButton").Pressed += () => OnChoiceSelected(Choice.Rock);
}
```

### 10.4 Accessibility

**MVP Requirements**:
1. **Visual Clarity**
   - Card artwork distinct and recognizable
   - Minimum 18pt font size (readable at 1280x720)
   - High contrast text (white on dark background)

2. **Keyboard Navigation** (optional for MVP)
   - Tab to cycle through choices
   - Enter to confirm selection
   - Escape for back/menu

3. **Audio Feedback**
   - SFX for all user actions (click, select, win, lose)
   - Distinct sounds for win vs. loss

**Future Enhancements** (post-MVP):
- Colorblind-friendly palette (icon shapes in addition to colors)
- Screen reader support (text-to-speech for card names)
- Adjustable text size
- Remappable controls

---

## 11. Technology Stack

| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| **Game Engine** | Godot | 4.5.x | Core engine, rendering, scenes |
| **Language** | C# | .NET 8 | Game logic, scripting |
| **Test Framework** | GUT (Godot Unit Test) | Latest (4.5 compatible) | Unit and integration testing |
| **Version Control** | Git | Latest | Source control |
| **IDE** | Visual Studio Code / Rider | Latest | C# development |
| **Asset Creation** | Krita / Aseprite / Figma | N/A | Sprite creation (external) |
| **Audio Tools** | Audacity / LMMS | N/A | SFX creation (external) |

**External Libraries**:
- None required for MVP (pure Godot + C#)

**Platform Targets**:
- Windows 10/11 (64-bit)
- macOS 10.15+ (Catalina)
- Linux (Ubuntu 20.04+, Fedora 35+)

---

## 12. Design Decisions (ADR-style)

### Decision 1: Three-Layer Architecture (Core/Service/Presentation)

**Context**: Need to support TDD with 80%+ test coverage while using Godot framework.

**Decision**: Separate code into three layers:
- Core Layer: Pure C# (no Godot dependencies)
- Service Layer: Light Godot dependencies (Node, signals)
- Presentation Layer: Full Godot integration (scenes, UI)

**Rationale**:
- Core logic 100% unit testable without Godot runtime
- Aligns with SOLID principles (Dependency Inversion)
- Enables future code reuse (e.g., port to different engine)

**Consequences**:
- ✅ High test coverage achievable
- ✅ Clear separation of concerns
- ⚠️ More files and boilerplate
- ⚠️ Must maintain discipline (no Godot in Core)

**Alternatives Considered**:
- Single-layer (all code in Godot nodes): Rejected due to poor testability
- Pure ECS (Entity-Component-System): Overkill for simple game

---

### Decision 2: State Machine in GameManager (Not State Pattern)

**Context**: Need explicit state management for game flow (MainMenu → Playing → GameOver).

**Decision**: Use simple enum + switch expression in GameManager, not separate State Pattern classes.

**Rationale**:
- Only 4 states (low complexity)
- Switch expressions align with STD-0002 standards
- Easier to test and maintain for small state space
- Less boilerplate (no abstract State classes)

**Consequences**:
- ✅ Simple, readable code
- ✅ Easy to debug (single file)
- ⚠️ Switch statement grows if many states added (future risk)

**Alternatives Considered**:
- State Pattern (separate class per state): Rejected as over-engineering for 4 states
- No explicit state management: Rejected due to implicit state bugs

**Threshold for Reconsideration**: If state count exceeds 8, refactor to State Pattern.

---

### Decision 3: Separate Scenes for Each Game Mode

**Context**: Four game modes (Single Match, Tournament, Survival, Multiplayer) with similar but distinct UI needs.

**Decision**: Separate scene files for each mode (SingleMatchScene.tscn, TournamentScene.tscn, SurvivalScene.tscn, MultiplayerScene.tscn).

**Rationale**:
- Cleaner separation of concerns (each mode is self-contained)
- Easier to maintain (changes to Tournament don't affect Survival)
- Simpler scene files (no conditional show/hide logic)
- Multiplayer has unique flow (transition screen, Ready button)
- Core gameplay can still be shared via reusable components (ChoiceController)

**Consequences**:
- ✅ Clean scene files (no conditional UI)
- ✅ Mode-specific logic isolated to its scene
- ✅ Easier to test individual modes
- ⚠️ Some UI duplication (TopBar, ChoiceArea, RevealArea appear in all)
- ⚠️ Four scene files instead of one

**Alternatives Considered**:
- Single shared GameScene.tscn: Rejected (more complex, harder to maintain, awkward for Multiplayer's unique flow)
- Inheritance/scene composition: Considered for future if duplication becomes problematic

---

### Decision 4: Records for Data Objects (Not Classes)

**Context**: Need data structures for RoundResult, ScoreState, GameConfig, etc.

**Decision**: Use C# records (immutable value types) instead of classes.

**Rationale**:
- Aligns with STD-0002 standards (prefer immutability)
- Value semantics (equality by value, not reference)
- Concise syntax (positional records)
- Compiler-generated Equals, GetHashCode, ToString

**Consequences**:
- ✅ Immutable data (fewer bugs)
- ✅ Better testability (equality by value)
- ✅ Less boilerplate
- ⚠️ Requires .NET 9+ (Godot 4.5 supports this)

**Alternatives Considered**:
- Classes: Rejected (mutable, reference semantics)
- Structs: Rejected (boxing issues, no inheritance)

---

### Decision 5: AI Random Only (No Difficulty Levels)

**Context**: AI opponent needs decision-making logic for MVP.

**Decision**: AI always chooses randomly (33.3% each choice), no difficulty levels.

**Rationale**:
- Requirement FR-003 explicitly states "No difficulty levels"
- Simplifies testing (deterministic with seeded random)
- Rock-Paper-Scissors is inherently luck-based
- Pattern detection AI doesn't add value to MVP

**Consequences**:
- ✅ Simple implementation
- ✅ Easy to test
- ⚠️ May feel repetitive for experienced players (acceptable for MVP)

**Future Enhancement**: Add "Smart AI" mode that detects player patterns.

---

### Decision 6: Hot-Seat Multiplayer with Two-Stage Manual Transition

**Context**: Local multiplayer requirement (FR-013) needs player choice hiding between turns, plus both players should watch the reveal together.

**Decision**: Sequential turns with **two manual transitions**:
1. Player 1 chooses → "Player 2's Turn - Ready?" → Player 1 looks away → Player 2 clicks Ready
2. Player 2 chooses → "Ready for Reveal?" → Player 1 turns back → Either player clicks Ready
3. Both choices revealed simultaneously

**Rationale**:
- First transition: Allows Player 1 to look away (privacy for Player 2)
- Second transition: Allows Player 1 to turn back and watch reveal together (shared experience)
- Manual control at both stages respects player autonomy (no rushed timing)
- Creates suspense and social moment when both players watch the reveal

**Consequences**:
- ✅ Both players experience the reveal together (better social experience)
- ✅ No forced timing (players control transitions)
- ✅ Prevents accidental peeking
- ✅ Builds suspense with two-stage flow
- ⚠️ Two extra button clicks per round (acceptable trade-off)

**Alternatives Considered**:
- Single transition only: Rejected (Player 1 wouldn't see the reveal, misses social moment)
- Automatic timed transitions: Rejected (too rushed, awkward timing)
- Simultaneous hidden input: Rejected (requires multiple devices)
- Network multiplayer: Out of scope for MVP

---

### Decision 7: No Persistence (Session-Only High Scores)

**Context**: Survival mode high score requirement (US-010).

**Decision**: High score stored in memory only (resets when game closes).

**Rationale**:
- Requirement NFR-030 explicitly states "No persistence"
- Simplifies architecture (no file I/O, no save system)
- MVP focus on core gameplay

**Consequences**:
- ✅ No serialization complexity
- ✅ No save file corruption risks
- ⚠️ Players lose high scores on exit (expected behavior for MVP)

**Future Enhancement**: Add save system for persistent high scores, unlocks, settings.

---

### Decision 8: GUT Framework for Testing (Not NUnit)

**Context**: Need testing framework compatible with Godot 4.5 and C#.

**Decision**: Use GUT (Godot Unit Test) with GDScript test files.

**Rationale**:
- GUT designed for Godot, integrates with editor
- Can test C# code from GDScript tests
- Official Godot community recommendation
- Supports mocking Godot nodes

**Consequences**:
- ✅ Good Godot integration
- ✅ Active community support
- ⚠️ Tests in GDScript (production code in C#)
- ⚠️ Less mature than NUnit/xUnit

**Alternatives Considered**:
- NUnit: Rejected (poor Godot integration, can't mock nodes)
- Manual testing: Rejected (doesn't meet NFR-021 test coverage requirement)

---

## 13. Dependencies

### 13.1 Runtime Dependencies

- **Godot Engine** 4.5.x
  - Rendering, scene management, input handling
  - Signal system, node architecture
  - Resource loading

- **.NET Runtime** 8.0+
  - C# language features (records, pattern matching)
  - Standard library (LINQ, collections)

### 13.2 Development Dependencies

- **GUT Framework** (latest for Godot 4.5)
  - Unit testing
  - Integration testing
  - Mocking Godot nodes

- **Git** (version control)
  - Source code management
  - Branching for features
  - Commit history

### 13.3 Asset Dependencies

- **Card Artwork**: 3 images (Rock, Paper, Scissors)
  - Format: PNG with transparency
  - Dimensions: 512x512 pixels (downscaled as needed)
  - Style: Cartoon, colorful (Pokémon/MTG-inspired)

- **UI Sprites**: Buttons, backgrounds, icons
  - Button states: normal, hover, pressed, disabled
  - Background: Simple gradient or texture

- **Sound Effects**: 8 audio files (WAV or OGG)
  - button_click.wav
  - choice_select.wav
  - card_reveal.wav
  - round_win.wav
  - round_lose.wav
  - draw.wav
  - match_win.wav
  - match_lose.wav

- **Font**: Single TrueType font (TTF)
  - Open-source license (e.g., Google Fonts)
  - Readable at 18pt+

### 13.4 External Resources

- **STD-0002-csharp-rubric.md**: Coding standards document
- **requirements.md**: Phase 1 output (functional requirements)

---

## 14. Risks and Mitigations

| Risk | Impact | Likelihood | Mitigation |
|------|--------|------------|------------|
| **Godot 4.5 + C# + GUT incompatibility** | High (blocks testing) | Low | Validate GUT framework early in Phase 4; use stable Godot release |
| **TDD slows development pace** | Medium (delayed MVP) | Medium | Accept slower start; trust process; tests pay off in debugging time |
| **Over-engineering for future card game** | Medium (wasted effort) | High | **Strict YAGNI adherence**; only add what's needed for 3 choices; refactor later |
| **Tournament bracket logic complexity** | Medium (bugs, delays) | Medium | Start with 8-player only; add 16-player after testing; comprehensive unit tests |
| **Hot-seat multiplayer awkward UX** | Low (playability issue) | Medium | User test with real players; improve transitions if needed; acceptable for MVP |
| **Asset creation delays** | Low (blocked polish) | Medium | Use placeholder art (colored rectangles with text); commission art in parallel |
| **State machine bugs (invalid transitions)** | Medium (crashes, soft locks) | Low | Validate transitions; log invalid attempts; comprehensive state tests |
| **Score tracking edge cases** | Low (incorrect results) | Low | 100% test coverage for ScoreTracker; test draws, ties, boundary conditions |
| **Random AI feels boring** | Low (player engagement) | Medium | Accept for MVP; document "Smart AI" feature for v2.0 |
| **Performance issues at 60 FPS** | Low (on min spec hardware) | Low | Profile early; avoid frame-by-frame updates; cache references; minimal animations |

**Risk Response Strategy**:
1. **High Impact + High Likelihood**: Immediate mitigation (none currently)
2. **High Impact + Low Likelihood**: Monitor and prepare contingency (Godot/GUT compatibility)
3. **Medium Impact + High Likelihood**: Active mitigation (over-engineering → YAGNI)
4. **Low Impact**: Accept risk, document for awareness

---

## 15. Future Considerations

This architecture is designed for the **MVP** (3-choice Rock-Paper-Scissors). The following enhancements would require architectural changes:

### 15.1 Expanded Choice System (4+ Choices)

**Current**: Hard-coded 3 choices (Rock, Paper, Scissors) in enum.

**Future**: Data-driven choice system with configurable win/loss relationships.

**Architectural Changes Needed**:
```csharp
// Current (MVP)
public enum Choice { Rock, Paper, Scissors }

// Future (Expandable)
public record ChoiceData(
    ChoiceId Id,
    string Name,
    string IconPath,
    List<ChoiceId> Defeats  // What this choice beats
);

public class ChoiceRegistry {
    private Dictionary<ChoiceId, ChoiceData> _choices = new();

    public void RegisterChoice(ChoiceData choice) { ... }
    public RoundResult DetermineWinner(ChoiceId player, ChoiceId opponent) { ... }
}
```

**Impact**: Core Layer (GameLogic), Presentation Layer (UI generation).

---

### 15.2 Deck Building and Hand Selection

**Current**: Player always has access to all 3 choices.

**Future**: Player builds deck of choices, draws hand each round, selects from hand.

**Architectural Changes Needed**:
- **DeckManager**: Manages player's deck (collection of choices)
- **HandManager**: Draws random subset from deck (e.g., 5 cards)
- **UI Changes**: Display hand instead of all choices
- **State Changes**: Add "DrawHand" sub-state before "WaitingForChoice"

**Impact**: Service Layer (new managers), Presentation Layer (hand UI), State Machine (new sub-states).

---

### 15.3 Choice Durability/HP System

**Current**: Choices have no HP (infinite uses).

**Future**: Each choice in hand has HP, loses HP when used, removed when HP = 0.

**Architectural Changes Needed**:
```csharp
public record ChoiceInstance(
    ChoiceId ChoiceId,
    int CurrentHp,
    int MaxHp
);
```

- **HandManager**: Tracks ChoiceInstance objects (not just ChoiceId)
- **GameLogic**: Deduct HP after each use
- **UI Changes**: Display HP on cards

**Impact**: Core Layer (data model), Service Layer (hand management), Presentation Layer (HP display).

---

### 15.4 Online Multiplayer

**Current**: Local only (hot-seat or AI).

**Future**: Network multiplayer with matchmaking.

**Architectural Changes Needed**:
- **NetworkManager**: Handle connections, messages, synchronization
- **MatchmakingService**: Find opponents, lobby system
- **Input Changes**: Asynchronous input (waiting for network)
- **State Changes**: Add "WaitingForOpponent" sub-state
- **Security**: Server-authoritative game logic (prevent cheating)

**Impact**: Massive (entire new layer), requires rearchitecting game flow.

---

### 15.5 Persistent Progression (Unlocks, Stats)

**Current**: No persistence (session-only).

**Future**: Save player profile, high scores, unlocks, statistics.

**Architectural Changes Needed**:
- **SaveManager**: Serialize/deserialize player data
- **ProfileData**: Player name, high scores, unlocks, stats
- **Storage**: Local file (JSON/binary) or cloud (backend service)

**Impact**: Service Layer (new manager), Security (save file integrity), UI (profile display).

---

### 15.6 Refactoring Strategy for Future

**When expanding to card game mechanics**:

1. **Phase 1**: Add Choice Registry (data-driven choices)
   - Refactor GameLogic to use ChoiceRegistry
   - Migrate hard-coded enum to data files
   - Minimal UI changes (still 3 choices visible)

2. **Phase 2**: Add Deck and Hand Systems
   - Implement DeckManager and HandManager
   - Update UI to show hand (not all choices)
   - Add "Draw Hand" state

3. **Phase 3**: Add Choice Durability
   - Change ChoiceId to ChoiceInstance
   - Track HP per instance
   - Update UI to display HP

4. **Phase 4**: Online Multiplayer (if needed)
   - Implement NetworkManager
   - Refactor state machine for asynchronous input
   - Add server-side validation

**Principle**: Refactor in small steps, maintain test coverage at each step.

---

## Appendix A: Architectural Checklist

Use this checklist during Phase 4 (TDD Development) to ensure architecture is followed:

- [ ] All core logic (GameLogic, AIOpponent) has **zero Godot dependencies**
- [ ] All core logic has **100% unit test coverage**
- [ ] Game state managed by explicit state machine (GameManager)
- [ ] Invalid state transitions **logged and rejected** (not crashed)
- [ ] All data objects use **records** (immutable value types)
- [ ] All enums used instead of magic strings
- [ ] Pattern matching used extensively (switch expressions)
- [ ] Signals used for loose coupling (not direct method calls across layers)
- [ ] Scene transitions triggered by state changes (not UI buttons directly)
- [ ] All public methods have **clear, self-documenting names**
- [ ] No functions longer than **30 lines**
- [ ] No files longer than **300 lines**
- [ ] STD-0002 standards followed (Egyptian braces, etc.)
- [ ] All tests pass before moving to next feature
- [ ] Overall test coverage exceeds **80%**

---

## Appendix B: Glossary

| Term | Definition |
|------|------------|
| **Core Layer** | Pure C# code with zero Godot dependencies, fully unit testable |
| **Service Layer** | Managers and orchestration logic with minimal Godot dependencies (Node, signals) |
| **Presentation Layer** | Godot scenes and UI controllers with full Godot integration |
| **State Machine** | Pattern for managing game states (MainMenu, Playing, GameOver) with explicit transitions |
| **Sub-State** | Granular state within a main state (e.g., WaitingForChoice within Playing) |
| **Signal** | Godot's event system for loose coupling between nodes |
| **Record** | C# immutable value type with value-based equality |
| **Result<T>** | Functional pattern for error handling without exceptions |
| **Hot-Seat** | Multiplayer mode where players take turns on the same computer/screen |
| **YAGNI** | "You Aren't Gonna Need It" - principle to avoid over-engineering for hypothetical features |
| **TDD** | Test-Driven Development - write test first, then implement to pass test |
| **GUT** | Godot Unit Test framework for testing Godot games |
| **ADR** | Architecture Decision Record - document explaining why architectural choice was made |

---

**End of Architecture Document**

**Next Phase**: Game Design Document (Phase 3)
**Prompt**: `spr/prompts/game-dev/03-game-design-doc-prompt.md`
**Status**: Ready for Phase 3 after architecture approval
