# Phase 2: Architecture Design for Scissors-Paper-Rock Game

<context>
<project>Scissors-Paper-Rock game built with Godot 4.5 and C#</project>
<role>You are a game architect designing the technical architecture for the scissors-paper-rock game based on approved requirements</role>
<objective>Create a comprehensive architecture document that defines structure, components, data flow, and patterns for implementation</objective>
<phase>Phase 2 of 4: Requirements → **Architecture** → Game Design Document → TDD Development</phase>
<input_file>spr/docs/requirements.md (from Phase 1)</input_file>
</context>

---

## Foundational Principles

1. **Requirements-Driven** - Every architectural decision traces back to a requirement
2. **Godot Best Practices** - Leverage Godot's scene system, signals, and node architecture
3. **Separation of Concerns** - Game logic separate from UI, data separate from presentation
4. **Testability** - Architecture must support unit testing with GUT framework
5. **Maintainability** - Follow SOLID principles and C# standards (STD-0002)
6. **Scalability** - Design allows for future enhancements without major refactoring
7. **Simplicity** - Don't over-engineer; this is a simple game

---

## Progress Tracking

<progress_tracking>
This is Phase 2 of a 4-phase process.

**Before starting**: Check for existing progress
```bash
cat .work/spr-game/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

**Prerequisites**:
- Phase 1 must be complete
- spr/docs/requirements.md must exist

**After completion**: Update progress.yaml with:
- Phase 2 status: Complete
- Output file: spr/docs/architecture.md
- Next action: "Begin Phase 3 (Game Design Document)"
</progress_tracking>

---

## Methodology

<architecture_design_process>

### Step 1: Review Requirements

<requirements_review>
1. Read spr/docs/requirements.md thoroughly
2. Identify key functional requirements that drive architecture:
   - Game mechanics
   - State transitions
   - Input handling
   - AI behavior
   - UI requirements
3. Note non-functional requirements:
   - Performance targets
   - Testability requirements
   - Maintainability standards
4. Extract technical constraints:
   - Godot 4.5 limitations/features
   - C# language patterns
   - Testing framework (GUT)
</requirements_review>

### Step 2: Define High-Level Architecture Pattern

<architecture_patterns>
Choose appropriate pattern(s) for the game:

**Option 1: MVC (Model-View-Controller)**
- **Model**: Game state (scores, choices, round data)
- **View**: Godot scenes and UI nodes
- **Controller**: Game logic, state transitions

**Option 2: Component-Based (Godot's preferred)**
- **Components**: Reusable nodes with specific responsibilities
- **Composition**: Scenes composed of components
- **Communication**: Signals and direct references

**Option 3: State Machine**
- **States**: Menu, Playing, Result, GameOver
- **Transitions**: User actions trigger state changes
- **State Behavior**: Each state handles its own logic

**Recommendation**: Hybrid approach
- State machine for game flow (Menu → Playing → Result)
- Component-based for UI and game logic
- Separation of concerns for testability
</architecture_patterns>

### Step 3: Define Scene Hierarchy

<scene_hierarchy>
Define the Godot scene structure:

```
Root Scene: Main.tscn
├── GameManager (Node - C# script)
│   └── Manages state transitions, game flow
│
├── MainMenu.tscn (Scene instance)
│   ├── UI elements
│   └── Signal connections to GameManager
│
├── GameScene.tscn (Scene instance)
│   ├── PlayerChoice (Component - UI for player input)
│   ├── OpponentChoice (Component - Shows opponent choice)
│   ├── GameLogic (Node - C# script for game rules)
│   └── UI (Score, round counter, etc.)
│
├── ResultScene.tscn (Scene instance)
│   ├── Result display (Win/Lose/Draw)
│   ├── Score summary
│   └── Play Again button
│
└── AudioManager (Node - C# script)
    └── Manages SFX and music
```

**Key decisions**:
- How scenes are loaded/unloaded
- Parent-child relationships
- Which components are reusable
- Scene transition mechanisms
</scene_hierarchy>

### Step 4: Define Component Architecture

<component_architecture>
List all major components with responsibilities:

**Core Components**:
1. **GameManager**
   - Responsibility: Overall game state, scene transitions
   - Signals: `game_started`, `game_ended`, `state_changed`
   - Methods: `StartGame()`, `EndGame()`, `ChangeState(GameState)`

2. **GameLogic**
   - Responsibility: Determine round winner, validate choices
   - Methods: `DetermineWinner(Choice, Choice)`, `IsValidChoice(Choice)`
   - Pure logic (no UI, fully testable)

3. **ScoreManager**
   - Responsibility: Track scores across rounds
   - Methods: `AddScore(Player)`, `GetScore(Player)`, `ResetScores()`
   - Data persistence (if needed)

4. **AIPlayer** (if applicable)
   - Responsibility: AI decision-making
   - Methods: `MakeChoice()`, `SetDifficulty(Level)`
   - Algorithms: Random, pattern-based, predictive

5. **InputHandler**
   - Responsibility: Process user input (mouse, touch, keyboard)
   - Signals: `choice_made`, `menu_action`
   - Validation and event routing

6. **UIController**
   - Responsibility: Update UI based on game state
   - Methods: `ShowChoice(Choice)`, `UpdateScore(int, int)`, `ShowResult(Result)`
   - Binds data to UI elements

7. **AudioManager**
   - Responsibility: Play sounds and music
   - Methods: `PlaySFX(SoundType)`, `PlayMusic(Track)`, `SetVolume(float)`

**Design principles**:
- Single Responsibility
- Loose coupling via signals
- High cohesion within components
</component_architecture>

### Step 5: Define Data Flow

<data_flow>
Map how data moves through the system:

**Example Flow: Player Makes a Choice**
```
1. User clicks "Rock" button
   ↓
2. InputHandler receives input signal
   ↓
3. InputHandler emits `choice_made(Choice.Rock)`
   ↓
4. GameManager receives signal, stores player choice
   ↓
5. GameManager triggers AI opponent (if applicable)
   ↓
6. AIPlayer.MakeChoice() returns Choice
   ↓
7. GameManager calls GameLogic.DetermineWinner(playerChoice, aiChoice)
   ↓
8. GameLogic returns Result (Win/Lose/Draw)
   ↓
9. GameManager updates ScoreManager
   ↓
10. GameManager emits `round_completed(Result)`
    ↓
11. UIController updates display
    ↓
12. AudioManager plays appropriate SFX
    ↓
13. GameManager checks win condition
    ↓
14. If game over: Transition to ResultScene
    Else: Reset for next round
```

**Diagram this flow** in the architecture document.
</data_flow>

### Step 6: Define State Management

<state_management>
**Game States**:
```csharp
public enum GameState
{
    MainMenu,      // Initial state, show menu
    Playing,       // In-game, waiting for/processing choices
    RoundResult,   // Showing result of a round
    GameOver,      // Final result, option to play again
}
```

**State Transitions**:
```
MainMenu → Playing: User clicks "Start Game"
Playing → RoundResult: Both players made choices, winner determined
RoundResult → Playing: Next round (if game not over)
RoundResult → GameOver: Win condition met
GameOver → MainMenu: User clicks "Main Menu"
GameOver → Playing: User clicks "Play Again"
```

**State Machine Implementation**:
- Option A: Simple switch statement in GameManager
- Option B: State pattern (separate class per state)
- Recommendation for simple game: Option A
</state_management>

### Step 7: Define Testing Strategy

<testing_strategy>
**Unit Testing with GUT**:

1. **GameLogic Tests**
   - Test all win/lose/draw combinations
   - Test choice validation
   - Mock-free (pure logic)

2. **ScoreManager Tests**
   - Test score tracking
   - Test reset functionality
   - Test edge cases (negative scores, overflow)

3. **AIPlayer Tests**
   - Test random choice distribution
   - Test pattern detection (if applicable)
   - Mock randomness for deterministic tests

4. **GameManager Tests**
   - Test state transitions
   - Test round flow
   - Mock dependencies (ScoreManager, GameLogic)

5. **InputHandler Tests**
   - Test input validation
   - Test signal emissions
   - Mock Godot input events

**Integration Testing**:
- Test scene transitions
- Test signal propagation
- Test UI updates based on data changes

**Test Coverage Goal**: >80% for game logic, 100% for core rules
</testing_strategy>

### Step 8: Define File/Folder Structure

<file_structure>
```
spr/
├── project.godot
│
├── docs/
│   ├── requirements.md        # Phase 1 output
│   └── architecture.md         # Phase 2 output (this file)
│
├── scenes/
│   ├── Main.tscn              # Root scene
│   ├── MainMenu.tscn          # Main menu
│   ├── GameScene.tscn         # Main game
│   └── ResultScene.tscn       # Result display
│
├── scripts/
│   ├── game_logic/
│   │   ├── GameLogic.cs       # Core game rules
│   │   ├── Choice.cs          # Enum or class for choices
│   │   └── Result.cs          # Enum or class for results
│   │
│   ├── managers/
│   │   ├── GameManager.cs     # Main game controller
│   │   ├── ScoreManager.cs    # Score tracking
│   │   └── AudioManager.cs    # Audio playback
│   │
│   ├── ai/
│   │   └── AIPlayer.cs        # AI opponent
│   │
│   ├── ui/
│   │   ├── UIController.cs    # UI updates
│   │   ├── ChoiceButton.cs    # Reusable button component
│   │   └── ScoreDisplay.cs    # Score UI component
│   │
│   └── input/
│       └── InputHandler.cs    # Input processing
│
├── tests/
│   ├── unit/
│   │   ├── test_game_logic.gd      # GameLogic tests
│   │   ├── test_score_manager.gd   # ScoreManager tests
│   │   ├── test_ai_player.gd       # AIPlayer tests
│   │   └── test_game_manager.gd    # GameManager tests
│   │
│   └── integration/
│       └── test_game_flow.gd       # End-to-end tests
│
└── assets/
    ├── sprites/
    │   ├── rock.png
    │   ├── paper.png
    │   └── scissors.png
    │
    ├── audio/
    │   ├── sfx/
    │   │   ├── choice.wav
    │   │   ├── win.wav
    │   │   └── lose.wav
    │   └── music/
    │       └── background.ogg
    │
    └── fonts/
        └── main_font.ttf
```

**Naming Conventions**:
- PascalCase for C# files and classes
- snake_case for GDScript test files
- Lowercase for asset folders
- Descriptive names (no abbreviations unless obvious)
</file_structure>

### Step 9: Define Key Interfaces/APIs

<key_interfaces>
Define critical interfaces that will be implemented:

```csharp
// IGameLogic.cs - Interface for game rules
public interface IGameLogic
{
    Result DetermineWinner(Choice playerChoice, Choice opponentChoice);
    bool IsValidChoice(Choice choice);
}

// IPlayer.cs - Interface for player (human or AI)
public interface IPlayer
{
    Choice MakeChoice();
    string GetName();
}

// IScoreManager.cs - Interface for score tracking
public interface IScoreManager
{
    void AddScore(string playerName);
    int GetScore(string playerName);
    void ResetScores();
    bool HasWinner(int winThreshold);
}
```

**Why interfaces?**
- Enable dependency injection for testing
- Allow multiple implementations (e.g., different AI strategies)
- Follow SOLID principles (Dependency Inversion)
</key_interfaces>

### Step 10: Define Cross-Cutting Concerns

<cross_cutting_concerns>
**Logging**:
- Use Godot's `GD.Print()` for development
- Consider custom logger for production

**Error Handling**:
- Validate all user input
- Handle null references gracefully
- Provide meaningful error messages

**Performance**:
- Avoid frame-by-frame updates where not needed
- Use signals instead of polling
- Pool objects if many instances created/destroyed

**Accessibility**:
- Support keyboard navigation
- Provide audio cues for actions
- Ensure color contrast for visibility
</cross_cutting_concerns>

</architecture_design_process>

---

## Output Specification

<output_format>
**File**: `spr/docs/architecture.md`

**Structure**:
```markdown
# Architecture: Scissors-Paper-Rock Game

## 1. Introduction
### 1.1 Purpose
### 1.2 Scope
### 1.3 References
- requirements.md
- STD-0002-csharp-rubric.md

## 2. Architectural Overview
### 2.1 High-Level Pattern
[MVC, Component-Based, State Machine, or Hybrid]

### 2.2 Design Principles
- [Principle 1]
- [Principle 2]

### 2.3 Architectural Diagram
[ASCII or description of main components and relationships]

## 3. Scene Hierarchy
### 3.1 Scene Structure
[Tree diagram of scenes and major nodes]

### 3.2 Scene Transitions
[Diagram or description of how scenes change]

### 3.3 Scene Responsibilities
[What each scene does]

## 4. Component Architecture
### 4.1 GameManager
- **Responsibility**: [...]
- **Signals**: [...]
- **Public Methods**: [...]
- **Dependencies**: [...]

[Repeat for all major components]

## 5. Data Flow
### 5.1 Player Choice Flow
[Step-by-step flow from input to result]

### 5.2 State Transition Flow
[How game states change]

### 5.3 Score Update Flow
[How scores are calculated and displayed]

## 6. State Management
### 6.1 Game States
[Enum definition and description]

### 6.2 State Transitions
[Diagram or table of valid transitions]

### 6.3 State Machine Implementation
[Chosen approach and rationale]

## 7. Testing Strategy
### 7.1 Unit Testing Approach
[What will be unit tested and how]

### 7.2 Integration Testing Approach
[What will be integration tested and how]

### 7.3 Test Coverage Goals
[Target percentages and rationale]

### 7.4 Testing Tools
- GUT framework
- Mock strategies

## 8. File/Folder Structure
```
[Full directory tree from Step 8]
```

## 9. Key Interfaces
```csharp
[Interface definitions from Step 9]
```

## 10. Cross-Cutting Concerns
### 10.1 Logging
### 10.2 Error Handling
### 10.3 Performance Considerations
### 10.4 Accessibility

## 11. Technology Stack
- **Engine**: Godot 4.5
- **Language**: C#
- **Testing**: GUT
- **Version Control**: Git

## 12. Design Decisions (ADR-style)
### Decision 1: [Title]
- **Context**: [Why we needed to decide]
- **Decision**: [What we chose]
- **Rationale**: [Why we chose it]
- **Consequences**: [Impact of this choice]

[Repeat for major decisions]

## 13. Dependencies
- Godot 4.5
- GUT framework
- [Any other dependencies]

## 14. Risks and Mitigations
| Risk | Impact | Mitigation |
|------|--------|------------|
| [Risk] | [Impact] | [Mitigation] |

## 15. Future Considerations
[What would need to change for future features]
```
</output_format>

---

## Critical Reminders

<critical_reminders>
1. **Trace to Requirements**
   - Every component must map to requirements
   - Don't add unnecessary complexity

2. **Godot-Specific Patterns**
   - Use scenes, not pure classes, where appropriate
   - Leverage signals for loose coupling
   - Follow Godot's node architecture

3. **Testability is Key**
   - Separate logic from presentation
   - Use interfaces for dependency injection
   - Design for GUT framework

4. **C# Standards**
   - Follow STD-0002-csharp-rubric.md
   - Use modern C# features (properties, LINQ, async if needed)
   - Proper XML doc comments for public APIs

5. **Keep It Simple**
   - Don't over-engineer
   - Prefer composition over inheritance
   - Start simple, refactor later if needed

6. **Document Decisions**
   - Explain why you chose patterns
   - Note alternative approaches considered
   - Justify trade-offs made
</critical_reminders>

---

## Begin

<begin>
=====================================
CRITICAL: CHECK FOR EXISTING PROGRESS
=====================================

**FIRST ACTION** - Check for existing progress:
```bash
cat .work/spr-game/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

**IF progress file exists**:
- Verify Phase 1 is complete
- If Phase 2 is complete, inform user and suggest Phase 3
- If Phase 2 is in progress, resume from next_action

**IF Phase 1 not complete**:
- Stop and inform user to complete Phase 1 first
- Provide command: "Use prompt 01-requirements-prompt.md"

**IF ready to start Phase 2**:
1. Read requirements.md:
   ```bash
   cat spr/docs/requirements.md
   ```

2. Work through Steps 1-10:
   - Review requirements (Step 1)
   - Choose architecture pattern (Step 2)
   - Design scene hierarchy (Step 3)
   - Define components (Step 4)
   - Map data flow (Step 5)
   - Design state management (Step 6)
   - Plan testing strategy (Step 7)
   - Define file structure (Step 8)
   - Create interfaces (Step 9)
   - Address cross-cutting concerns (Step 10)

3. Generate architecture.md in specified format

4. Update progress:
   ```bash
   cat > .work/spr-game/progress.yaml << 'EOF'
   progress:
     last_updated: "[ISO DateTime]"
     current_phase: "architecture"
     status: "Complete"

     phases:
       requirements:
         status: "Complete"
         output_file: "spr/docs/requirements.md"

       architecture:
         status: "Complete"
         output_file: "spr/docs/architecture.md"
         completed_at: "[ISO DateTime]"

       game_design_doc:
         status: "Not Started"

       tdd_development:
         status: "Not Started"

     next_action: "Begin Phase 3: Game Design Document. Use prompt 03-game-design-doc-prompt.md and reference requirements.md and architecture.md."
   EOF
   ```

5. Inform developer Phase 2 is complete and provide summary

=====================================
BEGIN NOW
=====================================
Check for existing progress and proceed accordingly.
</begin>
