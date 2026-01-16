# Phase 3: Game Design Document for Scissors-Paper-Rock Game

<context>
<project>Scissors-Paper-Rock game built with Godot 4.5 and C#</project>
<role>You are a game designer creating a comprehensive game design document that bridges architecture and implementation</role>
<objective>Document the visual, audio, UX, and mechanical design details that developers will implement</objective>
<phase>Phase 3 of 4: Requirements → Architecture → **Game Design Document** → TDD Development</phase>
<input_files>
- spr/docs/requirements.md (Phase 1)
- spr/docs/architecture.md (Phase 2)
</input_files>
</context>

---

## Foundational Principles

1. **Player Experience First** - Every design decision serves the player experience
2. **Clarity and Feedback** - Players always know what's happening and what to do next
3. **Visual Consistency** - Unified style, color palette, and typography
4. **Responsive Design** - Immediate feedback for all actions
5. **Accessibility** - Usable by diverse players (color-blind, different skill levels)
6. **Implementation-Ready** - Specific enough for developers to implement without guessing
7. **Reference Architecture** - Design aligns with architectural decisions

---

## Progress Tracking

<progress_tracking>
This is Phase 3 of a 4-phase process.

**Before starting**: Check for existing progress
```bash
cat .work/spr-game/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

**Prerequisites**:
- Phase 1 must be complete (requirements.md exists)
- Phase 2 must be complete (architecture.md exists)

**After completion**: Update progress.yaml with:
- Phase 3 status: Complete
- Output file: spr/docs/game-design-doc.md
- Next action: "Begin Phase 4 (TDD Development)"
</progress_tracking>

---

## Methodology

<game_design_process>

### Step 1: Review Previous Phases

<phase_review>
1. **Read requirements.md**:
   - Extract functional requirements (game mechanics, UI, states)
   - Note target audience and platform
   - Identify win conditions and game modes

2. **Read architecture.md**:
   - Understand scene hierarchy
   - Note component architecture
   - Review state machine design
   - Check data flow patterns

3. **Identify Design Constraints**:
   - Technical limitations (Godot 4.5, performance targets)
   - Scope boundaries (out of scope items)
   - Platform constraints (screen sizes, input methods)
</phase_review>

### Step 2: Define Visual Design

<visual_design>

#### 2.1 Art Style
Choose a visual style:
- **Minimalist**: Clean shapes, limited colors, modern
- **Cartoonish**: Playful characters, bright colors, exaggerated features
- **Flat Design**: 2D, solid colors, no gradients/shadows
- **Pixel Art**: Retro, 8-bit or 16-bit style
- **Hand-Drawn**: Sketch-like, organic, personal

**Decision**: [Choose based on target audience and development resources]

#### 2.2 Color Palette
Define primary, secondary, and accent colors:
```
Primary Colors:
- Background: #XXXXXX
- UI Elements: #XXXXXX
- Text: #XXXXXX

Secondary Colors:
- Rock: #XXXXXX (e.g., gray/brown)
- Paper: #XXXXXX (e.g., white/cream)
- Scissors: #XXXXXX (e.g., silver/blue)

Accent Colors:
- Win: #XXXXXX (e.g., green)
- Lose: #XXXXXX (e.g., red)
- Draw: #XXXXXX (e.g., yellow/orange)

Accessibility:
- Ensure WCAG AA contrast ratios (4.5:1 for text)
- Consider color-blind friendly palettes
```

#### 2.3 Typography
Define fonts and text styles:
```
Font Family: [e.g., "Roboto", "Comic Sans", "Press Start 2P"]

Heading 1: [Size]px, [Weight], [Color]
Heading 2: [Size]px, [Weight], [Color]
Body Text: [Size]px, [Weight], [Color]
Button Text: [Size]px, [Weight], [Color]

Special:
- Score Display: [Large, bold, prominent]
- Result Text: [Extra large, animated entry]
```

#### 2.4 Sprites and Graphics
List all visual assets needed:

| Asset | Description | Size (px) | Format | Notes |
|-------|-------------|-----------|--------|-------|
| rock_icon.png | Rock choice icon | 128x128 | PNG | Transparent BG |
| paper_icon.png | Paper choice icon | 128x128 | PNG | Transparent BG |
| scissors_icon.png | Scissors choice icon | 128x128 | PNG | Transparent BG |
| rock_button.png | Rock button (normal) | 256x256 | PNG | With hover/pressed states |
| paper_button.png | Paper button (normal) | 256x256 | PNG | With hover/pressed states |
| scissors_button.png | Scissors button (normal) | 256x256 | PNG | With hover/pressed states |
| background.png | Game background | 1920x1080 | PNG/JPG | Scalable/tiled |
| logo.png | Game title logo | 512x128 | PNG | Transparent BG |

**Visual States**:
- Normal
- Hover (lighter/glow)
- Pressed (darker/scale down)
- Disabled (grayed out)
- Selected (highlighted border)

</visual_design>

### Step 3: Define UI/UX Design

<ux_design>

#### 3.1 Screen Layouts

**Main Menu Screen**:
```
┌─────────────────────────────────────┐
│                                     │
│         [GAME LOGO]                 │
│    Scissors Paper Rock              │
│                                     │
│      [START GAME BUTTON]            │
│      [OPTIONS BUTTON]               │
│      [EXIT BUTTON]                  │
│                                     │
│                                     │
└─────────────────────────────────────┘
```

**Game Screen**:
```
┌─────────────────────────────────────┐
│  Round: X/Y    Player: A  AI: B     │  ← Header
├─────────────────────────────────────┤
│                                     │
│   [YOUR CHOICE]    [AI CHOICE]      │  ← Choice Display
│      (Icon)           (Icon)        │
│                                     │
├─────────────────────────────────────┤
│  [ ROCK ]  [ PAPER ]  [ SCISSORS ]  │  ← Player Input
│                                     │
│       [BACK TO MENU]                │
└─────────────────────────────────────┘
```

**Result Screen**:
```
┌─────────────────────────────────────┐
│                                     │
│         YOU WIN! / YOU LOSE!        │  ← Result
│         (Animated entrance)         │
│                                     │
│   Player: [Choice Icon]             │
│   AI:     [Choice Icon]             │
│                                     │
│   Final Score: P: X - AI: Y         │
│                                     │
│      [PLAY AGAIN]                   │
│      [MAIN MENU]                    │
└─────────────────────────────────────┘
```

#### 3.2 UI Components

Define each UI element:

**Button Component**:
- States: Normal, Hover, Pressed, Disabled
- Animation: Scale on press (0.95x), bounce on hover
- Sound: Click SFX on press
- Feedback: Visual highlight on hover

**Score Display Component**:
- Position: Top of screen
- Format: "Player: X - AI: Y"
- Update: Smooth number count-up animation
- Highlight: Briefly flash winner's score

**Choice Button Component**:
- Size: Large, easy to click/tap
- Icon: Centered, clear
- Label: Optional text below icon
- Highlight: Border glow when hovered
- Disabled: Gray out after choice made

**Round Indicator**:
- Format: "Round X of Y" or "First to Z wins"
- Position: Top-left or center-top
- Update: Smooth transition between rounds

#### 3.3 User Interactions

**Interaction Flow**:
1. **Main Menu**:
   - User sees logo and buttons
   - Hover: Button highlights
   - Click "Start Game": Fade to game screen

2. **Game Screen**:
   - User sees 3 choice buttons
   - Hover: Button highlights, sound (optional)
   - Click choice: Button scales down, choice confirmed
   - All buttons disabled, AI makes choice
   - Reveal: AI choice slides in or fades in
   - Result shown: Text appears, score updates
   - Wait 2 seconds: Automatically next round or show result screen

3. **Result Screen**:
   - Result text animates in (scale up + fade)
   - Choices displayed side-by-side
   - Final score shown
   - User clicks "Play Again" or "Main Menu"

**Animation Timings**:
- Button hover: 0.1s ease-in-out
- Button press: 0.05s ease-out
- Choice reveal: 0.3s ease-out
- Result text: 0.5s elastic ease-out
- Score count-up: 0.3s linear
- Scene transitions: 0.5s fade

#### 3.4 Feedback Mechanisms

Provide immediate, clear feedback:

| User Action | Visual Feedback | Audio Feedback | Haptic (Mobile) |
|-------------|-----------------|----------------|-----------------|
| Button hover | Highlight, scale +5% | Soft tick (optional) | - |
| Button click | Scale -5%, flash | Click sound | Short vibration |
| Make choice | Button selected, others dim | Choice sound | - |
| Win round | Green flash, +1 to score | Win jingle | Success pattern |
| Lose round | Red flash, +1 to AI score | Lose sound | - |
| Draw round | Yellow flash | Draw sound | - |
| Win game | Confetti/particles, big text | Victory music | Celebration |
| Lose game | Dim screen, text | Defeat music | - |

</ux_design>

### Step 4: Define Audio Design

<audio_design>

#### 4.1 Sound Effects (SFX)

| Sound | Trigger | Description | Duration | Format |
|-------|---------|-------------|----------|--------|
| button_click.wav | Any button press | Short, crisp click | 0.1s | WAV |
| button_hover.wav | Button hover (optional) | Subtle tick | 0.05s | WAV |
| choice_made.wav | Player makes choice | Confirmation sound | 0.2s | WAV |
| choice_reveal.wav | AI choice revealed | Swoosh or pop | 0.3s | WAV |
| win_round.wav | Player wins round | Positive chime | 0.5s | WAV |
| lose_round.wav | Player loses round | Negative tone | 0.5s | WAV |
| draw_round.wav | Round is a draw | Neutral beep | 0.3s | WAV |
| win_game.wav | Player wins game | Fanfare/jingle | 2s | WAV |
| lose_game.wav | Player loses game | Sad trombone | 1.5s | WAV |

**Volume Levels**:
- Master: 100%
- SFX: 80%
- UI sounds: 60%
- Result sounds: 90%

#### 4.2 Background Music

| Track | Context | Description | Loop | Format |
|-------|---------|-------------|------|--------|
| menu_theme.ogg | Main menu | Upbeat, inviting | Yes | OGG |
| game_theme.ogg | During gameplay | Energetic, tension-building | Yes | OGG |
| victory_theme.ogg | Win screen | Triumphant, celebratory | No | OGG |
| defeat_theme.ogg | Lose screen | Somber but hopeful | No | OGG |

**Music Transitions**:
- Fade duration: 1s
- Cross-fade between themes
- Reduce volume during SFX playback (ducking)

</audio_design>

### Step 5: Define Game Mechanics (Detailed)

<game_mechanics>

#### 5.1 Core Rules

**Winning Combinations**:
```
Rock beats Scissors (Rock crushes Scissors)
Scissors beats Paper (Scissors cuts Paper)
Paper beats Rock (Paper covers Rock)

Same choice = Draw (no winner)
```

#### 5.2 Round Flow

1. **Round Start**:
   - Display round number
   - Enable player choice buttons
   - Clear previous choices from display

2. **Player Makes Choice**:
   - Player clicks Rock, Paper, or Scissors
   - Button selected, others dimmed
   - Choice locked in

3. **AI Makes Choice**:
   - AI algorithm runs (random, pattern-based, etc.)
   - Delay 0.5-1s (simulate "thinking")
   - AI choice determined

4. **Reveal Phase**:
   - Player choice shown on left
   - AI choice revealed on right (animate in)
   - Pause 0.5s for anticipation

5. **Determine Winner**:
   - Apply core rules
   - Show visual indicator (highlight winner's choice)
   - Play appropriate SFX

6. **Update Scores**:
   - Increment winner's score
   - Animate score display
   - Check win condition

7. **Round End**:
   - If win condition met: Transition to result screen
   - Else: Wait 2s, start next round

#### 5.3 Win Conditions

Define when game ends:
- **Best of X**: First to win X rounds (e.g., best of 3 = first to 2 wins)
- **Fixed Rounds**: Play X rounds, highest score wins
- **First to Y**: First player to reach Y score wins

**Example**: Best of 5 (first to 3 wins)

#### 5.4 AI Behavior

**Difficulty Levels** (if applicable):

**Easy**:
- Completely random selection
- No pattern detection

**Medium**:
- Slight pattern recognition (e.g., if player plays Rock 3 times, AI plays Paper)
- 70% random, 30% pattern-based

**Hard**:
- Advanced pattern recognition
- Predicts player's next move based on history
- 50% random, 50% predictive

**Implementation Note**: Start with Easy (random) for MVP, add difficulty later if time permits.

#### 5.5 Edge Cases

Handle special scenarios:
- **First Round**: No history, AI always random
- **All Draws**: If X consecutive draws, show message "It's a standoff!"
- **Disconnect/Crash**: (For multiplayer, out of scope for MVP)
- **Invalid Input**: Should be prevented by UI, but validate anyway

</game_mechanics>

### Step 6: Define Game Flow

<game_flow>

Create a detailed flowchart:

```
[Launch Game]
     ↓
[Main Menu Screen]
     ↓ (Click "Start Game")
[Initialize Game]
  - Reset scores to 0-0
  - Set round to 1
  - Load game scene
     ↓
[Game Screen - Round Start]
  - Display round number
  - Enable choice buttons
     ↓
[Wait for Player Choice]
     ↓ (Player clicks choice)
[Process Player Choice]
  - Lock in choice
  - Disable buttons
  - Show player choice
     ↓
[AI Makes Choice]
  - Run AI algorithm
  - Simulate delay (0.5-1s)
     ↓
[Reveal Phase]
  - Animate AI choice reveal
  - Display both choices
     ↓
[Determine Winner]
  - Apply game rules
  - Calculate result
     ↓
[Show Round Result]
  - Highlight winner
  - Play SFX
  - Update scores
     ↓
[Check Win Condition]
     ├─ [Win Condition Met] → [Result Screen: Win/Lose]
     │                              ↓
     │                         [User chooses]
     │                         ├─ [Play Again] → [Initialize Game]
     │                         └─ [Main Menu] → [Main Menu Screen]
     └─ [Continue] → [Next Round] → [Game Screen - Round Start]
```

</game_flow>

### Step 7: Define Player Experience

<player_experience>

#### 7.1 First-Time Player Experience (FTUE)

**Goal**: Player understands game within 10 seconds

1. **Main Menu**:
   - Clear "Start Game" button (primary action)
   - Optional: Brief instructions ("Choose your weapon!")

2. **First Round**:
   - Clear instructions: "Choose Rock, Paper, or Scissors"
   - (Optional) Tutorial overlay showing rules
   - (Optional) Tooltip: "Rock beats Scissors"

3. **After First Round**:
   - Result clearly displayed
   - Score updated prominently
   - Player understands they need to win X rounds

#### 7.2 Emotional Journey

| Game Phase | Desired Emotion | Design Elements |
|------------|-----------------|-----------------|
| Main Menu | Excitement, anticipation | Bright colors, upbeat music, clear CTA |
| Making Choice | Tension, focus | Simple UI, clear options, satisfying click |
| Reveal | Suspense, anticipation | Slow reveal, dramatic pause |
| Win Round | Joy, satisfaction | Green flash, win sound, score +1 animation |
| Lose Round | Mild disappointment | Red flash, lose sound, quick recovery |
| Win Game | Triumph, accomplishment | Victory music, big text, confetti |
| Lose Game | Motivation to retry | "Try Again" prominent, encouraging message |

#### 7.3 Replay Value

Elements that encourage replay:
- Quick rounds (each game < 2 minutes)
- "Play Again" button immediately available
- (Optional) Track win/loss record
- (Optional) AI difficulty adjustment

</player_experience>

### Step 8: Define Accessibility Considerations

<accessibility>

**Visual**:
- Color-blind modes: Patterns/icons in addition to colors
- High contrast mode: Ensure text readable on all backgrounds
- Font size options: Small, Medium, Large

**Audio**:
- SFX on/off toggle
- Music on/off toggle
- Volume sliders (Master, SFX, Music)

**Input**:
- Keyboard shortcuts (1=Rock, 2=Paper, 3=Scissors, Enter=Confirm)
- Tab navigation through UI
- Large touch targets for mobile (min 44x44px)

**Cognitive**:
- Clear instructions
- Consistent UI patterns
- No time pressure (player-paced)

</accessibility>

</game_design_process>

---

## Output Specification

<output_format>
**File**: `spr/docs/game-design-doc.md`

**Structure**:
```markdown
# Game Design Document: Scissors-Paper-Rock

## 1. Introduction
### 1.1 Game Overview
### 1.2 Target Audience
### 1.3 Platform
### 1.4 Design Goals
### 1.5 References
- requirements.md
- architecture.md

## 2. Visual Design
### 2.1 Art Style
[Description and examples]

### 2.2 Color Palette
[Table of colors with hex codes]

### 2.3 Typography
[Font specifications]

### 2.4 Sprites and Graphics
[Table of all visual assets]

### 2.5 Visual States
[Button states, animations]

## 3. UI/UX Design
### 3.1 Screen Layouts
[Wireframes/mockups for each screen]

### 3.2 UI Components
[Detailed component specifications]

### 3.3 User Interactions
[Interaction flows and animations]

### 3.4 Feedback Mechanisms
[Table of feedback for each action]

## 4. Audio Design
### 4.1 Sound Effects
[Table of SFX with descriptions]

### 4.2 Background Music
[Table of music tracks]

### 4.3 Audio Settings
[Volume controls, toggles]

## 5. Game Mechanics
### 5.1 Core Rules
[Winning combinations]

### 5.2 Round Flow
[Step-by-step round progression]

### 5.3 Win Conditions
[How game ends]

### 5.4 AI Behavior
[AI algorithms and difficulty]

### 5.5 Edge Cases
[Special scenarios and handling]

## 6. Game Flow
### 6.1 State Diagram
[Flowchart of game states]

### 6.2 Scene Transitions
[How scenes change]

### 6.3 Navigation
[How player moves through game]

## 7. Player Experience
### 7.1 First-Time User Experience
[FTUE design]

### 7.2 Emotional Journey
[Desired emotions at each phase]

### 7.3 Replay Value
[Elements encouraging replay]

## 8. Accessibility
### 8.1 Visual Accessibility
[Color-blind, contrast, font size]

### 8.2 Audio Accessibility
[Toggles, volume controls]

### 8.3 Input Accessibility
[Keyboard, touch targets]

### 8.4 Cognitive Accessibility
[Clear instructions, pacing]

## 9. Asset List
### 9.1 Graphics Assets
[Complete list with sizes and formats]

### 9.2 Audio Assets
[Complete list with durations and formats]

### 9.3 Font Assets
[Fonts needed]

## 10. Implementation Notes
### 10.1 Animation Specifications
[Durations, easing functions]

### 10.2 Timing Values
[Delays, transitions, durations]

### 10.3 Magic Numbers
[Constants that might change: win threshold, AI delay, etc.]

## 11. Future Enhancements
[Features for future versions]
```
</output_format>

---

## Critical Reminders

<critical_reminders>
1. **Be Specific**
   - Not: "Nice colors"
   - Yes: "Background: #2C3E50, Text: #ECF0F1"

2. **Visual Clarity**
   - Include ASCII wireframes or descriptions
   - Be clear enough for developer to implement

3. **Reference Previous Phases**
   - Ensure design aligns with requirements
   - Match component names from architecture

4. **Implementation-Ready**
   - Developers shouldn't need to make design decisions
   - All assets listed with specs
   - All timings defined

5. **Player-Centric**
   - Every choice serves player experience
   - Think about what player sees, hears, feels

6. **Realistic Scope**
   - Don't over-design for MVP
   - Mark "nice-to-have" vs "must-have"
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
- Verify Phase 1 and 2 are complete
- If Phase 3 is complete, inform user and suggest Phase 4
- If Phase 3 is in progress, resume from next_action

**IF Phases 1 or 2 not complete**:
- Stop and inform user to complete previous phases first

**IF ready to start Phase 3**:
1. Read previous phase outputs:
   ```bash
   cat spr/docs/requirements.md
   cat spr/docs/architecture.md
   ```

2. Work through Steps 1-8:
   - Review previous phases (Step 1)
   - Design visual style (Step 2)
   - Design UI/UX (Step 3)
   - Design audio (Step 4)
   - Detail game mechanics (Step 5)
   - Map game flow (Step 6)
   - Design player experience (Step 7)
   - Plan accessibility (Step 8)

3. Generate game-design-doc.md in specified format

4. Update progress:
   ```bash
   cat > .work/spr-game/progress.yaml << 'EOF'
   progress:
     last_updated: "[ISO DateTime]"
     current_phase: "game_design_doc"
     status: "Complete"

     phases:
       requirements:
         status: "Complete"
         output_file: "spr/docs/requirements.md"

       architecture:
         status: "Complete"
         output_file: "spr/docs/architecture.md"

       game_design_doc:
         status: "Complete"
         output_file: "spr/docs/game-design-doc.md"
         completed_at: "[ISO DateTime]"

       tdd_development:
         status: "Not Started"

     next_action: "Begin Phase 4: TDD Development. Use prompt 04-tdd-development-prompt.md and implement game following TDD principles."
   EOF
   ```

5. Inform developer Phase 3 is complete and provide summary

=====================================
BEGIN NOW
=====================================
Check for existing progress and proceed accordingly.
</begin>
