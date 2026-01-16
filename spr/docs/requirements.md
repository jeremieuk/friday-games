# Requirements: Scissors-Paper-Rock Game

**Version**: 1.0
**Date**: 2026-01-16
**Project**: SPR Game (Scissors-Paper-Rock Card Battler Foundation)
**Phase**: 1 of 4 (Requirements → Architecture → Game Design Document → TDD Development)

---

## 1. Executive Summary

A desktop card-battler style game featuring the classic Rock-Paper-Scissors gameplay with three distinct game modes: Single Match (best of 3), Tournament (8 or 16 opponents), and Survival (roguelike continuous battles with HP system). The game will be built with Godot 4.5 using C# and Test-Driven Development, featuring cartoon-style artwork on cards similar to Pokémon, Star Wars Unlimited, and Magic: The Gathering aesthetics.

**Key Innovation**: While starting with traditional 3-choice Rock-Paper-Scissors, the architecture will be designed to support future expansion into a full card battler with multiple choices, choice durability systems, deck building, and hand selection mechanics.

---

## 2. Project Context

### 2.1 Target Platform
- **Primary**: Desktop (Windows, Mac, Linux)
- **Resolution**: 1280x720 (HD)
- **Future**: Mobile and Web considered for later versions

### 2.2 Target Audience
- **Demographic**: Casual players, all ages
- **Playstyle**: Quick sessions (2-5 minutes for Single Match, 10-20 minutes for Tournament/Survival)
- **Experience Level**: No prior game experience required

### 2.3 Game Modes
1. **Single Match**: Quick best of 3 against AI
2. **Tournament**: Bracket-style competition (8 or 16 AI opponents)
3. **Survival**: Roguelike mode with HP system, continuous battles until defeat
4. **Local Multiplayer**: Hot-seat mode for 2 players

### 2.4 Development Approach
- **Engine**: Godot 4.5
- **Language**: C# (not GDScript)
- **Methodology**: Test-Driven Development (TDD)
- **Testing**: GUT (Godot Unit Test) framework
- **Standards**: STD-0002-csharp-rubric.md

---

## 3. User Stories

### US-001: Start Single Match
**As a** casual player,
**I want to** quickly start a best of 3 match against AI,
**So that** I can play a quick game without commitment.

**Acceptance Criteria**:
- [ ] Main menu displays "Single Match" option
- [ ] Clicking launches game with player vs AI
- [ ] First to win 2 rounds wins the match
- [ ] Match completes in under 2 minutes

### US-002: Play Tournament Mode
**As a** competitive player,
**I want to** compete in a bracket-style tournament,
**So that** I can feel progression through multiple opponents.

**Acceptance Criteria**:
- [ ] Option to select 8-player or 16-player tournament
- [ ] Bracket-style progression (Quarters → Semis → Finals for 8-player)
- [ ] All opponents are AI-controlled
- [ ] Each match is best of 3
- [ ] Winner is crowned after final match

### US-003: Play Survival Mode
**As a** challenge-seeking player,
**I want to** battle continuously until I'm defeated,
**So that** I can test my endurance and track high scores.

**Acceptance Criteria**:
- [ ] Player starts with 10 HP
- [ ] Each opponent has 1 HP
- [ ] Each battle is best of 3 (first to 2 wins)
- [ ] Losing a battle costs player 1 HP
- [ ] Opponents defeated counter tracked
- [ ] High score displayed (most opponents defeated in session)
- [ ] Game over at 0 HP

### US-004: Make a Choice
**As a** player,
**I want to** select Rock, Paper, or Scissors using card-style UI,
**So that** the game feels engaging and visually appealing.

**Acceptance Criteria**:
- [ ] Three cards displayed: Rock, Paper, Scissors
- [ ] Cards feature cartoon artwork
- [ ] Clicking a card locks in choice
- [ ] Visual feedback on selection
- [ ] Choice cannot be changed once locked

### US-005: See Round Result
**As a** player,
**I want to** see a dramatic reveal of both choices and the winner,
**So that** each round feels exciting and clear.

**Acceptance Criteria**:
- [ ] Player choice revealed first
- [ ] AI choice revealed with 1 second delay (dramatic reveal)
- [ ] Clear indication of round winner (visual highlight)
- [ ] Result displayed: "You Win", "You Lose", or "Draw"
- [ ] Round score updated (e.g., "2-1")
- [ ] 3 second pause before next round

### US-006: Track Score
**As a** player,
**I want to** see my score and opponent's score,
**So that** I know how close I am to winning.

**Acceptance Criteria**:
- [ ] Score displayed prominently during match
- [ ] Format: "Player: X - AI: Y" or "Player Name: X - Opponent: Y"
- [ ] Score updates immediately after each round
- [ ] Visual emphasis on score change (brief highlight)

### US-007: Play Local Multiplayer
**As a** player with a friend,
**I want to** play against another person on the same computer,
**So that** we can compete directly.

**Acceptance Criteria**:
- [ ] "Multiplayer (Local)" option in main menu
- [ ] Both players can enter their names
- [ ] Hot-seat mode: Player 1 chooses → Screen transition (2 seconds, show "[Player 2]'s turn") → Player 2 chooses
- [ ] Player 1's choice hidden during Player 2's turn
- [ ] Best of 3 format
- [ ] Winner announced at end

### US-008: Enter Player Name
**As a** player,
**I want to** enter my name,
**So that** the game feels personalized.

**Acceptance Criteria**:
- [ ] Name entry field before starting game
- [ ] Name displayed during match instead of "Player"
- [ ] Name displayed in results screen
- [ ] Default name: "Player" (if no name entered)
- [ ] Multiplayer: Both players can enter names

### US-009: Return to Main Menu
**As a** player,
**I want to** return to the main menu from any game state,
**So that** I can start a different game mode.

**Acceptance Criteria**:
- [ ] "Main Menu" button accessible during and after games
- [ ] Current game state discarded on return
- [ ] No unsaved progress warnings (no persistence)

### US-010: View Survival High Score
**As a** survival mode player,
**I want to** see my best run (most opponents defeated),
**So that** I can track my improvement.

**Acceptance Criteria**:
- [ ] High score displayed during Survival mode
- [ ] Format: "High Score: X opponents defeated"
- [ ] High score updates when surpassed
- [ ] High score resets when game closes (no persistence)

---

## 4. Functional Requirements

### 4.1 Game Mechanics

#### FR-001: Core Game Rules
The game shall implement standard Rock-Paper-Scissors rules:
- Rock beats Scissors
- Scissors beats Paper
- Paper beats Rock
- Identical choices result in a Draw (no winner)

#### FR-002: Best of 3 Format
All matches (Single, Tournament, Survival) shall use best of 3 format:
- First player to win 2 rounds wins the match
- Maximum 3 rounds per match
- Minimum 2 rounds if one player wins first 2

#### FR-003: AI Decision Making
AI shall make random choices with equal probability (33.3% each):
- No pattern detection
- No difficulty levels
- Purely random selection

#### FR-004: Choice Data Structure
Each choice (Rock, Paper, Scissors) shall be represented as a data structure supporting:
- Name (string)
- Icon/Artwork path (string)
- HP value (int, default 1 for MVP)
- Wins against (list of choice names)
- Future: Additional stats (Attack, Defense, etc.)

### 4.2 Game Modes

#### FR-010: Single Match Mode
- Player vs AI
- Best of 3 rounds
- Match completes in under 2 minutes
- Winner determined and displayed

#### FR-011: Tournament Mode
- Player selects 8-player or 16-player tournament
- Bracket-style progression:
  - 8-player: Quarterfinals (4 matches) → Semifinals (2 matches) → Finals (1 match)
  - 16-player: Round of 16 → Quarters → Semis → Finals
- All opponents AI-controlled (for MVP)
- Each match is best of 3
- Player advances through bracket on wins
- Tournament ends on player loss or player wins finals

#### FR-012: Survival Mode
- Player starts with 10 HP
- AI opponents have 1 HP each
- Each battle is best of 3 against one opponent
- Winning a battle: Opponent eliminated, player continues
- Losing a battle: Player loses 1 HP, opponent eliminated, player continues
- Game over at 0 HP
- High score = total opponents defeated
- High score tracked during current session only (no persistence)

#### FR-013: Local Multiplayer Mode
- Two human players on same computer
- Hot-seat style:
  1. Player 1 enters name and makes choice
  2. Screen hides Player 1's choice with transition (2 seconds)
  3. Message displays: "[Player 2 Name]'s turn"
  4. Player 2 enters name (if first round) and makes choice
  5. Both choices revealed
- Best of 3 format
- Winner announced

### 4.3 User Interface

#### FR-020: Main Menu
Main menu shall display:
- Game title/logo
- Single button to enter game mode selection
- Navigation to: Single Match, Tournament, Survival, Multiplayer (Local)

#### FR-021: Choice Display
Choices shall be displayed as:
- Three cards arranged horizontally
- Each card shows cartoon artwork (Rock, Paper, or Scissors)
- Clear card borders and hover states
- Cards are clickable/selectable

#### FR-022: Score Display
Score shall be displayed:
- Format: "[Player Name]: X - [Opponent Name]: Y"
- Positioned at top of screen
- Updates immediately after each round
- Brief visual highlight on score change

#### FR-023: Result Display
After each round:
- Player choice card displayed on left
- Opponent choice card displayed on right (revealed after 1 second delay)
- Text indicates winner: "You Win!", "You Lose!", or "Draw!"
- Round score shown below
- 3 second pause before next round

#### FR-024: Tournament Bracket Display
Tournament mode shall show:
- Visual bracket representation
- Player's current position in bracket
- Current round name (Quarterfinals, Semifinals, Finals)
- Match number (e.g., "Match 3 of 7")

#### FR-025: Survival Stats Display
Survival mode shall show:
- Player HP: "HP: X/10" with visual HP bar
- Opponents Defeated: "Defeated: X"
- Current High Score: "High Score: X"

### 4.4 Input Handling

#### FR-030: Mouse Input
- Players shall select choices by clicking cards
- UI buttons respond to mouse clicks
- Hover effects on interactive elements

#### FR-031: Keyboard Input
- Enter key: Confirm selection (optional enhancement)
- Escape key: Return to main menu (with confirmation)
- Tab key: Navigate between UI elements (optional)

#### FR-032: Input Validation
- Only one choice can be selected per round
- Choice cannot be changed after confirmation
- Invalid inputs (clicking non-interactive areas) have no effect

### 4.5 Audio

#### FR-040: Sound Effects
Basic sound effects shall be implemented:
- Button click sound
- Choice selection sound
- Win round sound
- Lose round sound
- Draw sound
- Match win/lose sounds
- Card reveal sound

#### FR-041: Background Music
- Simple background music track for gameplay (optional for MVP)
- Can be toggled on/off (future enhancement)

---

## 5. Non-Functional Requirements

### 5.1 Performance

#### NFR-001: Frame Rate
The game shall maintain 60 FPS on desktop systems:
- Minimum spec: Intel Core i3 / AMD Ryzen 3, 4GB RAM, integrated graphics
- No frame drops during animations or transitions

#### NFR-002: Load Time
- Main menu shall load in under 2 seconds
- Scene transitions shall complete in under 1 second
- No loading screens for gameplay (all assets preloaded)

#### NFR-003: Memory Usage
- Game shall use less than 500MB RAM
- No memory leaks during extended play sessions

### 5.2 Usability

#### NFR-010: Intuitive Controls
- New players shall understand core gameplay within 30 seconds
- No tutorial required for basic Rock-Paper-Scissors mechanics
- UI elements clearly labeled and self-explanatory

#### NFR-011: Responsive Feedback
- All button clicks provide immediate visual feedback (< 50ms)
- Hover states visible on all interactive elements
- Clear indication of selected choice

#### NFR-012: Clear Visual Hierarchy
- Important information (score, HP) prominently displayed
- Game state always clear to player
- No visual clutter

### 5.3 Maintainability

#### NFR-020: Code Quality
Code shall follow STD-0002-csharp-rubric.md standards:
- Egyptian (cuddled) braces mandatory
- Pattern matching extensively used
- No magic strings (use enums and constants)
- Strongly-typed IDs and data structures
- Functional programming paradigm preferred
- Minimal code principle (no boilerplate)
- No XML documentation comments (self-documenting code)

#### NFR-021: Test Coverage
- Core game logic (GameLogic class) shall have 100% test coverage
- Overall test coverage shall exceed 80%
- All tests must pass before each commit

#### NFR-022: Code Organization
- Components shall follow single responsibility principle
- Game logic separated from presentation (Godot nodes)
- Core game logic shall have zero Godot dependencies
- Small, focused functions and classes

### 5.4 Compatibility

#### NFR-030: Platform Support
- Windows 10/11 (64-bit)
- macOS 10.15+ (Catalina and later)
- Linux (Ubuntu 20.04+, Fedora 35+)

#### NFR-031: Screen Resolution
- Native resolution: 1280x720 (HD)
- Shall scale appropriately for higher resolutions (1920x1080, 2560x1440)
- Maintain aspect ratio (16:9)

### 5.5 Accessibility

#### NFR-040: Visual Accessibility
- Card artwork shall be distinct and recognizable
- Text shall be readable (minimum 14pt font)
- High contrast between UI elements and background
- Color-blind friendly palette (future enhancement)

#### NFR-041: Cognitive Accessibility
- No time pressure on player choices
- Clear instructions and feedback
- Consistent UI patterns throughout game

---

## 6. Technical Constraints

### 6.1 Technology Stack
- **Engine**: Godot 4.5.x
- **Language**: C# (.NET 8)
- **Testing Framework**: GUT (Godot Unit Test)
- **Version Control**: Git
- **Development Methodology**: Test-Driven Development (TDD)

### 6.2 Coding Standards
- **Must follow**: STD-0002-csharp-rubric.md
  - Egyptian braces
  - Pattern matching and switch expressions
  - Strongly-typed IDs (no primitive obsession)
  - Records for DTOs and value objects
  - Result pattern (no exceptions for expected failures)
  - Functional composition preferred
  - No magic strings
  - No anaemic entities (rich domain models)

### 6.3 Architecture Constraints
- **Separation of Concerns**: Core game logic independent of Godot
- **Testability**: All business logic unit testable
- **State Machine**: Game state management via explicit state machine
- **Signals**: Loose coupling between components via Godot signals

### 6.4 Development Constraints
- **TDD Workflow**: Write test → Fail → Implement → Pass → Refactor
- **Continuous Testing**: All tests must pass before moving to next feature
- **No Persistence**: No saving/loading data for MVP (deliberate constraint)

---

## 7. Success Criteria

### 7.1 Functional Completeness
- [ ] All 10 user stories implemented and tested
- [ ] All functional requirements (FR-001 to FR-041) implemented
- [ ] All game modes playable and bug-free

### 7.2 Quality Metrics
- [ ] 100% test coverage for core game logic (GameLogic, ScoreManager)
- [ ] Overall test coverage exceeds 80%
- [ ] Zero critical bugs
- [ ] Zero compiler warnings
- [ ] All code follows STD-0002 standards (verified by checklist)

### 7.3 Performance Targets
- [ ] Consistent 60 FPS on target hardware
- [ ] Load time under 2 seconds
- [ ] Scene transitions under 1 second
- [ ] No memory leaks (verified by profiling)

### 7.4 User Experience
- [ ] New player understands gameplay within 30 seconds
- [ ] All UI interactions feel responsive (< 50ms feedback)
- [ ] Game is enjoyable for 20+ minute play sessions
- [ ] No confusion about game state or rules

### 7.5 Code Quality
- [ ] Code passes STD-0002 checklist (Egyptian braces, pattern matching, etc.)
- [ ] All public methods have clear, self-documenting names
- [ ] No deep nesting (max 2 levels)
- [ ] No functions longer than 30 lines
- [ ] No files longer than 300 lines

---

## 8. Out of Scope

The following features are explicitly **OUT OF SCOPE** for Version 1.0 (MVP):

### 8.1 Advanced Visual Features
- ❌ Advanced animations (timer, hand placement animations, per-choice victory animations)
- ❌ Particle effects (confetti, explosions)
- ❌ Smooth camera transitions
- ❌ 3D card flipping animations

### 8.2 Expanded Gameplay
- ❌ More than 3 choices (Rock-Paper-Scissors-Lizard-Spock, etc.)
- ❌ Choice durability/HP system (choices losing HP over time)
- ❌ Deck building mechanics
- ❌ Hand selection (choosing from subset of available choices)
- ❌ Multiple AI difficulty levels
- ❌ Smart AI (pattern detection, predictive algorithms)

### 8.3 Multiplayer Features
- ❌ Online multiplayer
- ❌ Multiple local players in Tournament mode
- ❌ Spectator mode
- ❌ Replays

### 8.4 Progression & Persistence
- ❌ Saving/loading game state
- ❌ Persistent high scores (between sessions)
- ❌ Player profiles
- ❌ Unlockable content
- ❌ Achievements
- ❌ Statistics tracking across sessions

### 8.5 Settings & Customization
- ❌ Settings menu
- ❌ Volume controls
- ❌ Graphics options
- ❌ Key remapping
- ❌ Custom card artwork

### 8.6 Platform Support
- ❌ Mobile (Android/iOS)
- ❌ Web/HTML5 export
- ❌ Console platforms
- ❌ Touch controls

### 8.7 Advanced Features
- ❌ Tutorial mode
- ❌ Story/campaign mode
- ❌ Boss battles
- ❌ Leaderboards
- ❌ Social features (sharing, invites)

---

## 9. Assumptions

### 9.1 Technical Assumptions
- Godot 4.5 is stable and fully supports C# .NET 8
- GUT framework is compatible with Godot 4.5 and C#
- Target hardware meets minimum specifications
- Players have mouse for input (no touch/controller required)

### 9.2 Design Assumptions
- Cartoon artwork assets will be available or created
- Rock-Paper-Scissors rules are universally understood
- Players prefer random AI over smart AI for casual play
- 3 second result pause is appropriate pacing
- Hot-seat multiplayer is acceptable (no simultaneous hidden input needed)

### 9.3 Scope Assumptions
- MVP can be completed in reasonable timeframe (2-4 weeks development)
- Future versions will expand to card game mechanics
- No external networking libraries needed (local only)
- No monetization features needed for MVP

### 9.4 User Assumptions
- Players understand basic game UI conventions
- Players can read English text
- Players have basic computer literacy (mouse usage, window management)
- Players will play on desktop monitors (not tiny laptop screens)

---

## 10. Dependencies

### 10.1 Software Dependencies
- **Godot Engine**: 4.5.x (latest stable)
- **.NET SDK**: 8.0.x
- **GUT Framework**: Latest version compatible with Godot 4.5
- **Git**: For version control

### 10.2 Asset Dependencies
- Cartoon artwork for Rock, Paper, Scissors (3 card designs)
- Game title logo
- UI button sprites/styles
- Sound effects (7-10 basic SFX)
- Background music track (optional)
- Font file (for UI text)

### 10.3 Documentation Dependencies
- STD-0002-csharp-rubric.md (existing)
- Architecture document (Phase 2 output)
- Game Design Document (Phase 3 output)

### 10.4 Knowledge Dependencies
- Developer proficiency in C# (or learning curve accounted for)
- Understanding of Godot node system
- TDD methodology knowledge
- GUT framework usage

---

## 11. Risks

### 11.1 Technical Risks

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Godot 4.5 + C# compatibility issues | Low | High | Use stable release; monitor Godot GitHub issues |
| GUT framework bugs/limitations | Medium | Medium | Start with core logic tests (no Godot dependencies); validate GUT early |
| Performance issues with animations | Low | Low | Profile early; keep animations simple for MVP |
| C# learning curve slows development | Medium | Medium | Focus on core logic first (easier to learn); leverage STD-0002 examples |

### 11.2 Design Risks

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Random AI feels too easy/boring | Medium | Medium | Accept for MVP; plan smarter AI for v2.0 |
| 3-choice gameplay lacks depth | Medium | Low | Acknowledge this is foundation; card game mechanics coming |
| Hot-seat multiplayer awkward UX | Low | Low | Test with real users; improve transitions if needed |
| Cartoon art style doesn't match vision | Medium | Medium | Create mockups early; iterate on art direction |

### 11.3 Scope Risks

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Feature creep (adding out-of-scope items) | High | High | **Strict adherence to requirements**; document all "future" ideas separately |
| Tournament/Survival modes too complex | Medium | Medium | Implement Single Match first; validate complexity with user testing |
| TDD slows initial development | Medium | Low | Accept slower start for long-term quality; trust the process |
| Over-engineering for future card mechanics | High | Medium | YAGNI principle; only add what's needed for 3 choices; refactor later |

### 11.4 Asset Risks

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Cartoon artwork not available | High | High | Use placeholder art; create simple vector art; or commission artist |
| Audio assets missing | Medium | Low | Use free SFX libraries (freesound.org); silent gameplay acceptable for testing |
| Font licensing issues | Low | Low | Use open-source fonts (Google Fonts) |

### 11.5 Timeline Risks

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| TDD adds significant time | Medium | Medium | Trust TDD process; time invested upfront saves debugging time |
| Tournament mode takes longer than expected | Medium | Medium | Implement after Single Match and Survival are complete; cut if needed |
| Polish/animation time underestimated | High | Low | Keep animations basic for MVP; add polish in v2.0 |

---

## 12. Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2026-01-16 | Requirements Phase | Initial requirements document based on stakeholder interview |

---

## 13. Approval

This requirements document represents Phase 1 (Requirements Gathering) of the SPR Game development process.

**Next Phase**: Architecture Design (Phase 2)
**Prompt to Use**: `spr/prompts/game-dev/02-architecture-prompt.md`

**Stakeholder Approval**: ✅ Requirements gathered from user interview
**Ready for Architecture Phase**: Awaiting approval

---

*End of Requirements Document*
