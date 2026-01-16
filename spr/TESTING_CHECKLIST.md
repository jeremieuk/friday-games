# Testing Checklist - Phase 4A MVP

## Prerequisites
- [ ] Godot 4.3+ installed with .NET support
- [ ] .NET 8 SDK installed
- [ ] GUT framework installed via AssetLib
- [ ] Project opens without errors in Godot editor
- [ ] C# solution builds successfully

## Unit Tests (GUT Framework)

Run tests via: `Project → Tools → GUT` or CLI: `godot --path . --script res://addons/gut/gut_cmdln.gd`

### Core Game Logic Tests
- [ ] `test_game_logic.gd` - All 9 tests pass
  - [ ] Rock beats Scissors
  - [ ] Scissors beats Paper
  - [ ] Paper beats Rock
  - [ ] Same choice is Draw
  - [ ] All losing conditions work
  - [ ] Deterministic behavior verified

### Score Tracking Tests
- [ ] `test_score_tracker.gd` - All 14 tests pass
  - [ ] Initial scores are zero
  - [ ] Recording wins updates scores
  - [ ] Best-of-3 logic works
  - [ ] Draws don't count toward score
  - [ ] Match completion detection
  - [ ] Winner determination

### AI Opponent Tests
- [ ] `test_ai_opponent.gd` - All 5 tests pass
  - [ ] Returns valid choices
  - [ ] Randomness verified
  - [ ] Distribution is balanced
  - [ ] Multiple instances are independent

### Game Manager Tests
- [ ] `test_game_manager.gd` - All 11 tests pass
  - [ ] State machine transitions
  - [ ] Round outcomes calculated correctly
  - [ ] Match completion logic
  - [ ] Score tracking integration
  - [ ] AI integration works

### Test Coverage Goals
- [ ] Core Layer: 100% coverage (GameLogic.cs)
- [ ] Service Layer: 80%+ coverage (Managers, AI)
- [ ] All tests pass without errors
- [ ] No warnings in test output

## Manual Testing (Gameplay)

### Main Menu
- [ ] Game opens to Main Menu
- [ ] Title displays correctly: "SCISSORS PAPER ROCK"
- [ ] Play button navigates to Single Match
- [ ] Exit button closes game
- [ ] UI is centered and readable
- [ ] Colors match design (deep blue, gold)

### Single Match Mode
- [ ] Scene loads successfully
- [ ] Score displays: "You: 0 | AI: 0"
- [ ] Three choice buttons visible (Rock, Paper, Scissors)
- [ ] Buttons display emojis correctly

### Gameplay Flow (10 Full Matches)
For each match, verify:
- [ ] Clicking Rock/Paper/Scissors triggers round
- [ ] Round result displays immediately
- [ ] Score updates correctly after each round
- [ ] Draws don't increment scores
- [ ] Player can win matches (play until 2 wins)
- [ ] AI can win matches
- [ ] Match completes at 2 wins
- [ ] "Play Again" button appears after match
- [ ] Choice buttons disable after match
- [ ] "Play Again" resets everything
- [ ] Back button returns to main menu
- [ ] No crashes or errors during gameplay

### Best-of-3 Scenarios
- [ ] Match 1: Player wins 2-0
- [ ] Match 2: AI wins 2-0
- [ ] Match 3: Player wins 2-1
- [ ] Match 4: AI wins 2-1
- [ ] Match 5: Multiple draws before win
- [ ] Match 6: Alternating wins (P-A-P)
- [ ] Match 7: Alternating wins (A-P-A)
- [ ] Match 8-10: Random play

### UI/UX Quality
- [ ] All text is readable
- [ ] Buttons respond to clicks
- [ ] No visual glitches
- [ ] Colors are consistent
- [ ] Layout is centered
- [ ] Result messages are clear
- [ ] Victory message is celebratory
- [ ] Defeat message is appropriate

### Audio (If files added)
- [ ] Button click sounds play
- [ ] Choice reveal sound plays
- [ ] Round win/lose sounds play
- [ ] Match victory/defeat sounds play
- [ ] Gameplay music loops
- [ ] Volume levels are appropriate

### Performance
- [ ] No lag or stuttering
- [ ] Smooth transitions between scenes
- [ ] Instant response to button clicks
- [ ] No memory leaks (play 20+ matches)

## Code Quality Review

### STD-0002 Compliance
- [ ] Egyptian braces used (opening on same line)
- [ ] Pattern matching preferred over if/else
- [ ] No magic strings in code
- [ ] No magic numbers (const values used)
- [ ] Private fields use underscore prefix
- [ ] Public members use PascalCase
- [ ] Methods use PascalCase
- [ ] Strong typing throughout

### Architecture Verification
- [ ] Core Layer has zero Godot dependencies
  - [ ] `GameLogic.cs` - pure C#
- [ ] Service Layer has minimal Godot dependencies
  - [ ] `ScoreTracker.cs` - RefCounted only
  - [ ] `AIOpponent.cs` - RandomNumberGenerator only
  - [ ] `GameManager.cs` - Node base class
  - [ ] `AudioManager.cs` - AudioStreamPlayer only
- [ ] Presentation Layer uses full Godot
  - [ ] `SingleMatchController.cs` - Control base class
  - [ ] Scene files (.tscn)

### Documentation
- [ ] README_SETUP.md exists and is complete
- [ ] README_AUDIO.md exists with asset list
- [ ] TESTING_CHECKLIST.md (this file) complete
- [ ] Code comments are clear and helpful
- [ ] TODO comments mark future work

## Integration Testing

### Scene Transitions
- [ ] Main Menu → Single Match works
- [ ] Single Match → Main Menu (back button) works
- [ ] No orphaned nodes after transitions
- [ ] No memory leaks between scenes

### C# ↔ GDScript Integration
- [ ] GDScript tests can load C# classes
- [ ] C# enums work in GDScript tests
- [ ] Dictionary returns from C# work in GDScript
- [ ] Signal connections work (UI buttons)

## Acceptance Criteria

### Functional Requirements (Must Pass)
- ✅ Player can play best-of-3 matches against random AI
- ✅ Scores track correctly (first to 2 wins)
- ✅ Draws don't count toward score
- ✅ Match ends when someone reaches 2 wins
- ✅ UI is functional and responsive
- ✅ Audio framework integrated (files pending)

### Technical Requirements (Must Pass)
- ✅ All unit tests pass (expect 39+ tests total)
- ✅ Tests run via GUT framework successfully
- ✅ Code follows STD-0002 C# coding standards
- ✅ Three-layer architecture maintained
- ✅ Zero Godot dependencies in Core layer
- ✅ Game runs without errors or warnings

### Quality Gates
- [ ] All GUT unit tests pass: ___/39 tests
- [ ] Manual playtest: 10 consecutive matches without issues
- [ ] Code review: STD-0002 compliance verified
- [ ] No console errors during gameplay
- [ ] No console warnings during gameplay

## Known Limitations (MVP)
- Audio files not yet sourced (framework ready)
- No Tournament mode (deferred to Phase 4B)
- No Survival mode (deferred to Phase 4B)
- No Multiplayer mode (deferred to Phase 4B)
- No advanced animations (deferred to Phase 4B)
- No card artwork (emojis used as placeholders)

## Next Steps After Testing
1. Address any failing tests
2. Fix any bugs discovered during manual testing
3. Source audio files and integrate
4. Commit to git with message: "feat(mvp): Complete Single Match mode with TDD"
5. Update progress.yaml with Phase 4A completion
6. Plan Phase 4B: Full Features (Tournament, Survival, Multiplayer)
