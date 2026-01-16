# Phase 4: TDD Development for Scissors-Paper-Rock Game

<context>
<project>Scissors-Paper-Rock game built with Godot 4.5 and C#</project>
<role>You are a Godot game developer implementing the game using Test-Driven Development (TDD) with the GUT framework</role>
<objective>Build a fully functional, tested scissors-paper-rock game following TDD principles, C# coding standards, and architectural designs</objective>
<phase>Phase 4 of 4: Requirements → Architecture → Game Design Document → **TDD Development**</phase>
<input_files>
- spr/docs/requirements.md (Phase 1)
- spr/docs/architecture.md (Phase 2)
- spr/docs/game-design-doc.md (Phase 3)
- spr/prompts/rubrics/STD-0002-csharp-rubric.md (Coding standards)
</input_files>
</context>

---

## Foundational Principles

1. **Test First, Always** - Write failing test → Write minimal code to pass → Refactor
2. **Red-Green-Refactor** - See test fail (Red) → Make it pass (Green) → Improve code (Refactor)
3. **Small Steps** - One test, one feature at a time
4. **C# Standards** - Follow STD-0002-csharp-rubric.md rigorously
5. **Architecture Adherence** - Implement exactly as designed in Phase 2
6. **Minimal Code** - Write the minimum code to pass tests; no speculative features
7. **Continuous Testing** - Run tests frequently; all tests must pass before moving forward
8. **Clean Code** - Refactor continuously; keep code simple and readable

---

## Progress Tracking

<progress_tracking>
This is Phase 4 of a 4-phase process - the final implementation phase.

**Before starting**: Check for existing progress
```bash
cat .work/spr-game/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

**Prerequisites**:
- Phase 1 complete (requirements.md exists)
- Phase 2 complete (architecture.md exists)
- Phase 3 complete (game-design-doc.md exists)
- Godot 4.5 project initialized in `spr/`
- GUT framework installed

**During development**: Update progress.yaml after completing each component

**After completion**: Update progress.yaml with:
- Phase 4 status: Complete
- All tests passing
- Game fully functional
</progress_tracking>

---

## TDD Methodology

<tdd_process>

### TDD Cycle (Red-Green-Refactor)

```
1. RED: Write a failing test
   - Test describes desired behavior
   - Test fails because feature doesn't exist yet
   - Verify test actually fails

2. GREEN: Write minimal code to make test pass
   - Only write enough code to pass the test
   - Don't worry about perfection
   - Get to green as quickly as possible

3. REFACTOR: Improve the code
   - Eliminate duplication
   - Improve names
   - Apply design patterns
   - Follow C# standards (STD-0002)
   - ALL tests must still pass

4. REPEAT: Next test
```

### Test Naming Convention

```gdscript
# GUT test files (GDScript)
# tests/unit/test_<component_name>.gd

# Test method naming:
# test_<method>_<scenario>_<expected_result>

func test_determine_winner_rock_vs_scissors_returns_rock_wins():
    # Test Rock beats Scissors

func test_determine_winner_same_choice_returns_draw():
    # Test same choice results in draw

func test_add_score_valid_player_increments_score():
    # Test score increment works
```

### What to Test

**DO Test**:
- Core game logic (GameLogic.cs)
- Business rules (win conditions, score calculation)
- State transitions (GameManager state changes)
- Data transformations (Choice enum, Result calculations)
- Public interfaces of components

**DON'T Test**:
- UI rendering (visual testing is manual)
- Godot framework itself (trust the engine)
- Private methods (test through public interface)
- Simple getters/setters with no logic

### GUT Framework Basics

```gdscript
# tests/unit/test_game_logic.gd
extends GutTest

# Reference to the C# class being tested
var GameLogic = load("res://scripts/game_logic/GameLogic.cs")
var _game_logic

func before_each():
    # Setup - runs before each test
    _game_logic = GameLogic.new()

func after_each():
    # Teardown - runs after each test
    _game_logic = null

func test_rock_beats_scissors():
    # Arrange
    var player_choice = GameLogic.Choice.Rock
    var ai_choice = GameLogic.Choice.Scissors

    # Act
    var result = _game_logic.DetermineWinner(player_choice, ai_choice)

    # Assert
    assert_eq(result, GameLogic.Result.PlayerWins, "Rock should beat Scissors")

func test_scissors_beats_paper():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Scissors,
        GameLogic.Choice.Paper)
    assert_eq(result, GameLogic.Result.PlayerWins)

func test_paper_beats_rock():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Paper,
        GameLogic.Choice.Rock)
    assert_eq(result, GameLogic.Result.PlayerWins)

func test_same_choice_draws():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Rock,
        GameLogic.Choice.Rock)
    assert_eq(result, GameLogic.Result.Draw)
```

</tdd_process>

---

## Development Sequence

<development_sequence>

### Phase 4.0: Project Setup

**Tasks**:
1. Verify Godot project structure exists
2. Install GUT framework
3. Create directory structure from architecture.md
4. Initialize git repository (if not already done)
5. Create initial progress.yaml for Phase 4

**Completion Criteria**:
- [ ] Godot project opens without errors
- [ ] GUT framework installed and runnable
- [ ] Directory structure matches architecture.md
- [ ] Can run empty test suite

**No tests yet** - This is setup

---

### Phase 4.1: Core Game Logic (Pure C#)

**Component**: `scripts/game_logic/GameLogic.cs`

This is pure logic with NO Godot dependencies - fully testable.

#### Test 4.1.1: Choice Enum

**Test File**: `tests/unit/test_game_logic.gd`

```gdscript
extends GutTest

var GameLogic = load("res://scripts/game_logic/GameLogic.cs")

func test_choice_enum_has_rock():
    assert_true(GameLogic.Choice.has("Rock"))

func test_choice_enum_has_paper():
    assert_true(GameLogic.Choice.has("Paper"))

func test_choice_enum_has_scissors():
    assert_true(GameLogic.Choice.has("Scissors"))
```

**Implementation**: Create `Choice` enum in GameLogic.cs

```csharp
// scripts/game_logic/GameLogic.cs
public enum Choice {
    Rock,
    Paper,
    Scissors
}
```

**Run Tests**: All tests should pass ✅

#### Test 4.1.2: Result Enum

```gdscript
func test_result_enum_has_player_wins():
    assert_true(GameLogic.Result.has("PlayerWins"))

func test_result_enum_has_ai_wins():
    assert_true(GameLogic.Result.has("AIWins"))

func test_result_enum_has_draw():
    assert_true(GameLogic.Result.has("Draw"))
```

**Implementation**: Create `Result` enum

```csharp
public enum Result {
    PlayerWins,
    AIWins,
    Draw
}
```

**Run Tests**: All tests should pass ✅

#### Test 4.1.3: DetermineWinner Method

```gdscript
var _game_logic

func before_each():
    _game_logic = GameLogic.new()

func test_rock_beats_scissors():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Rock,
        GameLogic.Choice.Scissors)
    assert_eq(result, GameLogic.Result.PlayerWins)

func test_scissors_beats_paper():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Scissors,
        GameLogic.Choice.Paper)
    assert_eq(result, GameLogic.Result.PlayerWins)

func test_paper_beats_rock():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Paper,
        GameLogic.Choice.Rock)
    assert_eq(result, GameLogic.Result.PlayerWins)

func test_rock_loses_to_paper():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Rock,
        GameLogic.Choice.Paper)
    assert_eq(result, GameLogic.Result.AIWins)

func test_scissors_loses_to_rock():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Scissors,
        GameLogic.Choice.Rock)
    assert_eq(result, GameLogic.Result.AIWins)

func test_paper_loses_to_scissors():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Paper,
        GameLogic.Choice.Scissors)
    assert_eq(result, GameLogic.Result.AIWins)

func test_same_choice_rock_draws():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Rock,
        GameLogic.Choice.Rock)
    assert_eq(result, GameLogic.Result.Draw)

func test_same_choice_paper_draws():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Paper,
        GameLogic.Choice.Paper)
    assert_eq(result, GameLogic.Result.Draw)

func test_same_choice_scissors_draws():
    var result = _game_logic.DetermineWinner(
        GameLogic.Choice.Scissors,
        GameLogic.Choice.Scissors)
    assert_eq(result, GameLogic.Result.Draw)
```

**Implementation**: Implement DetermineWinner logic

```csharp
public partial class GameLogic : RefCounted {
    public enum Choice { Rock, Paper, Scissors }
    public enum Result { PlayerWins, AIWins, Draw }

    public Result DetermineWinner(Choice playerChoice, Choice aiChoice) {
        if (playerChoice == aiChoice) {
            return Result.Draw;
        }

        return (playerChoice, aiChoice) switch {
            (Choice.Rock, Choice.Scissors) => Result.PlayerWins,
            (Choice.Scissors, Choice.Paper) => Result.PlayerWins,
            (Choice.Paper, Choice.Rock) => Result.PlayerWins,
            _ => Result.AIWins
        };
    }
}
```

**Run Tests**: All 9 tests should pass ✅

**Refactor**: Code is clean; follows C# standards ✅

**Update Progress**: Mark GameLogic component complete

---

### Phase 4.2: Score Manager

**Component**: `scripts/managers/ScoreManager.cs`

#### Tests 4.2.1-4.2.5: Score Management

```gdscript
extends GutTest

var ScoreManager = load("res://scripts/managers/ScoreManager.cs")
var _score_manager

func before_each():
    _score_manager = ScoreManager.new()

func test_initial_scores_are_zero():
    assert_eq(_score_manager.GetPlayerScore(), 0)
    assert_eq(_score_manager.GetAIScore(), 0)

func test_add_player_score_increments():
    _score_manager.AddPlayerScore()
    assert_eq(_score_manager.GetPlayerScore(), 1)

func test_add_ai_score_increments():
    _score_manager.AddAIScore()
    assert_eq(_score_manager.GetAIScore(), 1)

func test_multiple_scores_accumulate():
    _score_manager.AddPlayerScore()
    _score_manager.AddPlayerScore()
    _score_manager.AddAIScore()
    assert_eq(_score_manager.GetPlayerScore(), 2)
    assert_eq(_score_manager.GetAIScore(), 1)

func test_reset_scores_returns_to_zero():
    _score_manager.AddPlayerScore()
    _score_manager.AddAIScore()
    _score_manager.ResetScores()
    assert_eq(_score_manager.GetPlayerScore(), 0)
    assert_eq(_score_manager.GetAIScore(), 0)

func test_has_winner_returns_false_initially():
    assert_false(_score_manager.HasWinner(3))

func test_has_winner_returns_true_when_player_reaches_threshold():
    _score_manager.AddPlayerScore()
    _score_manager.AddPlayerScore()
    _score_manager.AddPlayerScore()
    assert_true(_score_manager.HasWinner(3))

func test_has_winner_returns_true_when_ai_reaches_threshold():
    _score_manager.AddAIScore()
    _score_manager.AddAIScore()
    _score_manager.AddAIScore()
    assert_true(_score_manager.HasWinner(3))

func test_has_winner_returns_false_below_threshold():
    _score_manager.AddPlayerScore()
    _score_manager.AddPlayerScore()
    assert_false(_score_manager.HasWinner(3))
```

**Implementation**:

```csharp
public partial class ScoreManager : RefCounted {
    private int _playerScore;
    private int _aiScore;

    public int GetPlayerScore() => _playerScore;
    public int GetAIScore() => _aiScore;

    public void AddPlayerScore() => _playerScore++;
    public void AddAIScore() => _aiScore++;

    public void ResetScores() {
        _playerScore = 0;
        _aiScore = 0;
    }

    public bool HasWinner(int threshold) {
        return _playerScore >= threshold || _aiScore >= threshold;
    }
}
```

**Run Tests**: All tests pass ✅

**Update Progress**: Mark ScoreManager complete

---

### Phase 4.3: AI Player

**Component**: `scripts/ai/AIPlayer.cs`

#### Tests 4.3.1-4.3.3: Random AI

```gdscript
extends GutTest

var AIPlayer = load("res://scripts/ai/AIPlayer.cs")
var _ai_player

func before_each():
    _ai_player = AIPlayer.new()

func test_make_choice_returns_valid_choice():
    var choice = _ai_player.MakeChoice()
    assert_true(
        choice == AIPlayer.Choice.Rock or
        choice == AIPlayer.Choice.Paper or
        choice == AIPlayer.Choice.Scissors,
        "AI should return valid choice")

func test_make_choice_is_random():
    # Run 100 times, should get variety
    var choices = {}
    for i in range(100):
        var choice = _ai_player.MakeChoice()
        if not choices.has(choice):
            choices[choice] = 0
        choices[choice] += 1

    # Should have at least 2 different choices in 100 attempts
    assert_gte(choices.size(), 2, "AI should make varied choices")

func test_make_choice_never_returns_invalid():
    for i in range(50):
        var choice = _ai_player.MakeChoice()
        assert_ne(choice, null)
        assert_true(choice >= 0 and choice <= 2)
```

**Implementation**:

```csharp
public partial class AIPlayer : RefCounted {
    public enum Choice { Rock, Paper, Scissors }

    private readonly RandomNumberGenerator _rng = new();

    public AIPlayer() {
        _rng.Randomize();
    }

    public Choice MakeChoice() {
        return (Choice)_rng.RandiRange(0, 2);
    }
}
```

**Run Tests**: All tests pass ✅

**Update Progress**: Mark AIPlayer complete

---

### Phase 4.4: Game Manager (State Machine)

**Component**: `scripts/managers/GameManager.cs`

This is a Godot Node, so tests verify state transitions and game flow logic.

#### Tests 4.4.1-4.4.6: State Management

```gdscript
extends GutTest

var GameManager = load("res://scripts/managers/GameManager.cs")
var _game_manager

func before_each():
    _game_manager = GameManager.new()
    add_child(_game_manager)  # Required for Godot nodes

func after_each():
    remove_child(_game_manager)
    _game_manager.free()

func test_initial_state_is_main_menu():
    assert_eq(_game_manager.GetCurrentState(), GameManager.GameState.MainMenu)

func test_start_game_transitions_to_playing():
    _game_manager.StartGame()
    assert_eq(_game_manager.GetCurrentState(), GameManager.GameState.Playing)

func test_start_game_resets_scores():
    _game_manager.StartGame()
    # Scores should be 0-0 (test via signal or public getter)
    assert_eq(_game_manager.GetPlayerScore(), 0)
    assert_eq(_game_manager.GetAIScore(), 0)

func test_process_round_updates_state():
    _game_manager.StartGame()
    _game_manager.ProcessRound(GameManager.Choice.Rock, GameManager.Choice.Scissors)
    # Should transition to RoundResult or stay in Playing
    var state = _game_manager.GetCurrentState()
    assert_true(
        state == GameManager.GameState.Playing or
        state == GameManager.GameState.RoundResult)

func test_win_condition_triggers_game_over():
    _game_manager.StartGame()
    # Simulate player winning 3 rounds (if threshold is 3)
    _game_manager.ProcessRound(GameManager.Choice.Rock, GameManager.Choice.Scissors)
    _game_manager.ProcessRound(GameManager.Choice.Paper, GameManager.Choice.Rock)
    _game_manager.ProcessRound(GameManager.Choice.Scissors, GameManager.Choice.Paper)

    assert_eq(_game_manager.GetCurrentState(), GameManager.GameState.GameOver)

func test_return_to_menu_from_game_over():
    _game_manager.StartGame()
    # Force to game over
    _game_manager.EndGame()
    _game_manager.ReturnToMenu()
    assert_eq(_game_manager.GetCurrentState(), GameManager.GameState.MainMenu)
```

**Implementation**: Build incrementally to pass each test

```csharp
public partial class GameManager : Node {
    [Signal] public delegate void StateChangedEventHandler(GameState newState);
    [Signal] public delegate void RoundCompletedEventHandler(Result result);
    [Signal] public delegate void GameOverEventHandler(bool playerWon);

    public enum GameState { MainMenu, Playing, RoundResult, GameOver }
    public enum Choice { Rock, Paper, Scissors }
    public enum Result { PlayerWins, AIWins, Draw }

    private GameState _currentState = GameState.MainMenu;
    private ScoreManager _scoreManager;
    private GameLogic _gameLogic;
    private int _winThreshold = 3;  // Best of 5

    public override void _Ready() {
        _scoreManager = new ScoreManager();
        _gameLogic = new GameLogic();
    }

    public GameState GetCurrentState() => _currentState;
    public int GetPlayerScore() => _scoreManager.GetPlayerScore();
    public int GetAIScore() => _scoreManager.GetAIScore();

    public void StartGame() {
        _scoreManager.ResetScores();
        ChangeState(GameState.Playing);
    }

    public void ProcessRound(Choice playerChoice, Choice aiChoice) {
        var result = _gameLogic.DetermineWinner(
            (GameLogic.Choice)playerChoice,
            (GameLogic.Choice)aiChoice);

        if (result == (GameLogic.Result)Result.PlayerWins) {
            _scoreManager.AddPlayerScore();
        } else if (result == (GameLogic.Result)Result.AIWins) {
            _scoreManager.AddAIScore();
        }

        EmitSignal(SignalName.RoundCompleted, (int)result);

        if (_scoreManager.HasWinner(_winThreshold)) {
            var playerWon = _scoreManager.GetPlayerScore() >= _winThreshold;
            EndGame();
            EmitSignal(SignalName.GameOver, playerWon);
        }
    }

    public void EndGame() {
        ChangeState(GameState.GameOver);
    }

    public void ReturnToMenu() {
        ChangeState(GameState.MainMenu);
    }

    private void ChangeState(GameState newState) {
        _currentState = newState;
        EmitSignal(SignalName.StateChanged, (int)newState);
    }
}
```

**Run Tests**: All tests pass ✅

**Refactor**: Clean up, follow C# standards ✅

**Update Progress**: Mark GameManager complete

---

### Phase 4.5: Scene and UI Implementation

**At this point**, core game logic is complete and fully tested. Now implement:

1. **Main Menu Scene** (MainMenu.tscn)
   - Manual testing (visual)
   - Wire up "Start Game" button to GameManager.StartGame()

2. **Game Scene** (GameScene.tscn)
   - Choice buttons (Rock, Paper, Scissors)
   - Score display
   - Wire up buttons to GameManager.ProcessRound()

3. **Result Scene** (ResultScene.tscn)
   - Show final result
   - "Play Again" / "Main Menu" buttons

4. **UI Controllers** (C# scripts for UI logic)
   - Bind data to UI elements
   - Listen to GameManager signals
   - Update displays

**Testing Strategy**:
- Unit tests for UI controllers (if they have logic)
- Manual testing for visual/interaction
- Integration testing for signal flow

---

### Phase 4.6: Integration and Polish

**Tasks**:
1. Connect all scenes together
2. Add animations (per game-design-doc.md)
3. Add audio (SFX, music)
4. Add visual feedback (highlights, color changes)
5. Test full game flow manually
6. Fix any bugs discovered

**Completion Criteria**:
- [ ] All unit tests pass
- [ ] Full game playable start to finish
- [ ] UI matches game-design-doc.md
- [ ] Audio plays as specified
- [ ] No critical bugs

</development_sequence>

---

## C# Coding Standards Enforcement

<coding_standards>

**Reference**: `spr/prompts/rubrics/STD-0002-csharp-rubric.md`

**Critical Rules**:

1. **Egyptian Braces** - MANDATORY
   ```csharp
   if (condition) {
       DoSomething();
   }
   ```

2. **Pattern Matching**
   ```csharp
   public Result DetermineWinner(Choice p, Choice ai) => (p, ai) switch {
       (Choice.Rock, Choice.Scissors) => Result.PlayerWins,
       (Choice.Scissors, Choice.Paper) => Result.PlayerWins,
       (Choice.Paper, Choice.Rock) => Result.PlayerWins,
       _ when p == ai => Result.Draw,
       _ => Result.AIWins
   };
   ```

3. **No Magic Strings**
   ```csharp
   // ❌ WRONG
   EmitSignal("state_changed", newState);

   // ✅ CORRECT
   EmitSignal(SignalName.StateChanged, newState);
   ```

4. **Strongly-Typed Everything**
   ```csharp
   // Use enums, not strings or ints
   public enum Choice { Rock, Paper, Scissors }
   ```

5. **Functional Style**
   ```csharp
   // Pure functions where possible
   public Result DetermineWinner(Choice p, Choice ai) {
       // No side effects, deterministic
   }
   ```

6. **Minimal Code**
   - Write the least code to pass tests
   - No speculative features
   - No boilerplate

7. **Early Returns**
   ```csharp
   public Result Validate(Request r) {
       if (r is null) return Result.Fail("Null");
       if (!r.IsValid) return Result.Fail("Invalid");
       return Result.Ok();
   }
   ```

8. **No XML Comments**
   - Code should be self-documenting
   - Only comment when logic is inherently unclear

**Review Checklist** (before committing code):
- [ ] Egyptian braces?
- [ ] Pattern matching used?
- [ ] No magic strings?
- [ ] Strongly typed?
- [ ] Minimal code?
- [ ] Early returns for guards?
- [ ] No deep nesting?
- [ ] No XML comments?

</coding_standards>

---

## Running Tests

<test_execution>

**Run All Tests**:
```bash
# From Godot editor:
# Scene → Run GUT Tests (or F5 with GUT configured)

# From command line:
godot --headless --script addons/gut/gut_cmdln.gd -gdir=res://tests/unit
```

**Run Specific Test File**:
```bash
godot --headless --script addons/gut/gut_cmdln.gd -gtest=res://tests/unit/test_game_logic.gd
```

**Continuous Testing**:
- Keep GUT panel open in editor
- Re-run tests after every code change
- All tests must pass before moving to next component

**Test Output**:
```
========== Running Tests ==========
Running test_game_logic.gd
  test_rock_beats_scissors: PASSED
  test_scissors_beats_paper: PASSED
  test_paper_beats_rock: PASSED
  test_same_choice_draws: PASSED

4 tests run, 4 passed, 0 failed
======================================
```

</test_execution>

---

## Critical Reminders

<critical_reminders>

1. **Test First, ALWAYS**
   - Write test before implementation
   - Watch test fail (RED)
   - Write code to pass (GREEN)
   - Refactor (keep GREEN)

2. **Minimal Code**
   - Only write code to pass current test
   - Don't anticipate future features
   - YAGNI (You Aren't Gonna Need It)

3. **Run Tests Frequently**
   - After every small change
   - Before committing
   - All tests must pass

4. **Follow C# Standards**
   - Reference STD-0002 constantly
   - Egyptian braces, pattern matching, strong typing
   - Review checklist before committing

5. **Pure Logic First**
   - Test core logic (GameLogic, ScoreManager) first
   - These have no Godot dependencies
   - Easiest to test

6. **Refactor Continuously**
   - After tests pass, improve code
   - Eliminate duplication
   - Improve naming
   - Tests must still pass after refactoring

7. **Update Progress**
   - Mark components complete in progress.yaml
   - Track test pass rate
   - Document next actions

8. **Integration Last**
   - Build components bottom-up
   - Test in isolation first
   - Integrate after components are solid

9. **Manual Testing for UI**
   - Unit test logic, manually test visuals
   - Play the game yourself
   - Get user feedback

10. **No Skipping Tests**
    - Don't comment out failing tests
    - Don't skip TDD cycle
    - If test is wrong, fix the test
</critical_reminders>

---

## Output Specification

<output_specification>

**Deliverables**:

1. **Complete Godot Project** in `spr/`
   - All scenes created and configured
   - All C# scripts implemented
   - All assets in place

2. **Test Suite** in `spr/tests/`
   - All unit tests passing
   - Test coverage >80% for core logic
   - Integration tests for critical paths

3. **Working Game**
   - Playable from main menu to result screen
   - All features from requirements implemented
   - Follows game-design-doc.md specifications

4. **Code Quality**
   - Follows STD-0002-csharp-rubric.md
   - Clean, readable, maintainable
   - No critical bugs

**Final Checklist**:
- [ ] All unit tests pass (100%)
- [ ] Game is playable end-to-end
- [ ] Follows requirements.md
- [ ] Matches architecture.md
- [ ] Implements game-design-doc.md
- [ ] Adheres to STD-0002 C# standards
- [ ] No critical bugs
- [ ] Code is clean and refactored
- [ ] Progress.yaml updated

</output_specification>

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
- Verify Phases 1, 2, and 3 are complete
- If Phase 4 is in progress, check which components are done
- Resume from documented next_action

**IF Phases 1, 2, or 3 not complete**:
- Stop and inform user to complete previous phases first

**IF ready to start Phase 4**:

1. **Verify Prerequisites**:
   ```bash
   # Check required files exist
   test -f spr/docs/requirements.md && echo "✅ Requirements" || echo "❌ Missing requirements"
   test -f spr/docs/architecture.md && echo "✅ Architecture" || echo "❌ Missing architecture"
   test -f spr/docs/game-design-doc.md && echo "✅ GDD" || echo "❌ Missing GDD"
   test -f spr/project.godot && echo "✅ Godot Project" || echo "❌ No Godot project"
   ```

2. **Read Previous Phase Outputs**:
   ```bash
   cat spr/docs/requirements.md
   cat spr/docs/architecture.md
   cat spr/docs/game-design-doc.md
   ```

3. **Setup Phase 4** (if not already done):
   - Install GUT framework
   - Create directory structure
   - Initialize progress tracking

4. **Begin TDD Development**:
   - Start with Phase 4.1: Core Game Logic
   - Write first test (test_choice_enum_has_rock)
   - Watch it fail (RED)
   - Implement minimal code (GREEN)
   - Refactor if needed
   - Continue through development sequence

5. **Update Progress After Each Component**:
   ```yaml
   progress:
     last_updated: "[ISO DateTime]"
     current_phase: "tdd_development"
     status: "In Progress"

     phases:
       requirements:
         status: "Complete"
       architecture:
         status: "Complete"
       game_design_doc:
         status: "Complete"
       tdd_development:
         status: "In Progress"
         components:
           game_logic:
             status: "Complete"
             tests_passing: 9
           score_manager:
             status: "In Progress"
             tests_passing: 3
           # ... etc

     next_action: "Continue Phase 4.2: Implement ScoreManager. Next test: test_reset_scores_returns_to_zero"
   ```

6. **Final Completion**:
   ```yaml
   progress:
     last_updated: "[ISO DateTime]"
     current_phase: "tdd_development"
     status: "Complete"

     phases:
       requirements:
         status: "Complete"
       architecture:
         status: "Complete"
       game_design_doc:
         status: "Complete"
       tdd_development:
         status: "Complete"
         total_tests: 45
         tests_passing: 45
         test_coverage: 85%
         completed_at: "[ISO DateTime]"

     next_action: "Project complete! Game is fully functional and tested. Play test and gather feedback."
   ```

=====================================
BEGIN NOW
=====================================

Check for existing progress and proceed accordingly.

**Remember**: Test First, Always. Red → Green → Refactor. All tests must pass.
</begin>
