# Game Design Document: Scissors-Paper-Rock

**Version**: 1.0
**Date**: 2026-01-16
**Project**: SPR Game (Scissors-Paper-Rock Card Battler Foundation)
**Phase**: 3 of 4 (Requirements → Architecture → **Game Design Document** → TDD Development)

---

## 1. Introduction

### 1.1 Game Overview

Scissors-Paper-Rock (SPR) is a desktop card-battler style implementation of the classic Rock-Paper-Scissors game. Players engage in quick, tactical matches against AI opponents or local human players, with three distinct game modes offering different levels of challenge and progression.

**Core Experience**:
- Fast-paced rounds (2-5 minutes per match)
- Cartoon card-based UI inspired by Pokémon, Star Wars Unlimited, and Magic: The Gathering
- Dramatic reveals and clear visual feedback
- Multiple game modes for varied experiences

### 1.2 Target Audience

- **Primary**: Casual gamers of all ages
- **Secondary**: Players looking for quick gaming sessions
- **Accessibility**: No prior gaming experience required
- **Platform**: Desktop users (Windows, Mac, Linux)

### 1.3 Platform

- **Primary Platform**: Desktop (Windows 10/11, macOS 10.15+, Linux Ubuntu 20.04+)
- **Resolution**: 1280x720 (HD), scales to higher resolutions
- **Input**: Mouse + Keyboard
- **Future**: Mobile and web versions considered

### 1.4 Design Goals

1. **Instant Clarity**: Players understand gameplay within 10 seconds
2. **Satisfying Feedback**: Every action feels responsive and rewarding
3. **Visual Polish**: Cartoon card aesthetic that's charming and professional
4. **Quick Sessions**: Complete matches in under 5 minutes
5. **Replay Value**: Multiple modes and quick "Play Again" flow
6. **Foundation for Growth**: Designed to expand into full card battler

### 1.5 References

- **requirements.md** (Phase 1) - Functional and non-functional requirements
- **architecture.md** (Phase 2) - Technical architecture and component design

---

## 2. Visual Design

### 2.1 Art Style

**Style**: **Cartoon Card-Battler**

**Characteristics**:
- Vibrant, colorful card artwork
- Playful character designs for Rock, Paper, and Scissors
- Clean, readable UI elements
- Inspired by Pokémon TCG, Magic: The Gathering, and Star Wars Unlimited
- 2D art with subtle depth (drop shadows, outlines)
- Friendly, approachable aesthetic

**Mood**:
- Fun and lighthearted
- Competitive but not serious
- Engaging without being overwhelming

**Visual References**:
- Pokémon TCG: Card borders, character artwork style
- MTG: Card layout, icon clarity
- Star Wars Unlimited: Modern UI, clean typography

### 2.2 Color Palette

#### Primary Colors

| Color Name | Hex Code | Usage |
|------------|----------|-------|
| **Deep Blue** | `#1E3A5F` | Background, primary UI surfaces |
| **Soft White** | `#F5F5F5` | Text, card backgrounds |
| **Dark Charcoal** | `#2C2C2C` | Secondary text, borders |
| **Accent Gold** | `#FFD700` | Highlights, selected states |

#### Choice Colors

| Choice | Primary | Secondary | Usage |
|--------|---------|-----------|-------|
| **Rock** | `#8B7355` (Brown) | `#A0826D` (Light Brown) | Rock card background, icon tint |
| **Paper** | `#E8E8E8` (Off-White) | `#FFFFFF` (White) | Paper card background, icon tint |
| **Scissors** | `#B0C4DE` (Steel Blue) | `#C8D8E8` (Light Steel) | Scissors card background, icon tint |

#### Feedback Colors

| Feedback | Color | Hex Code | Usage |
|----------|-------|----------|-------|
| **Win** | Green | `#4CAF50` | Win highlights, flashes, borders |
| **Lose** | Red | `#F44336` | Lose highlights, flashes, borders |
| **Draw** | Orange | `#FF9800` | Draw highlights, flashes, borders |
| **Neutral** | Gray | `#9E9E9E` | Disabled states, inactive elements |

#### Accessibility

- **Contrast Ratios**: All text meets WCAG AA standards (4.5:1 minimum)
- **Color-Blind Friendly**: Icons and patterns supplement color coding
- **High Contrast Mode**: Optional mode with increased contrast (future)

### 2.3 Typography

**Font Family**: **"Nunito"** (Open-source, Google Fonts)
- Friendly, rounded sans-serif
- Excellent readability at all sizes
- Supports bold weights for emphasis

**Fallback**: System sans-serif (`-apple-system, "Segoe UI", Arial, sans-serif`)

#### Type Scale

| Style | Size | Weight | Color | Usage |
|-------|------|--------|-------|-------|
| **Title** | 48px | Bold (700) | `#FFD700` (Gold) | Game logo, major headings |
| **Heading 1** | 32px | Bold (700) | `#F5F5F5` (White) | Screen titles, result text |
| **Heading 2** | 24px | SemiBold (600) | `#F5F5F5` (White) | Section headers, mode names |
| **Body** | 18px | Regular (400) | `#F5F5F5` (White) | General UI text, descriptions |
| **Button** | 20px | Bold (700) | `#2C2C2C` (Dark) | Button labels |
| **Score** | 36px | Bold (700) | `#FFD700` (Gold) | Score display |
| **Small** | 14px | Regular (400) | `#B0B0B0` (Light Gray) | Secondary info, hints |

### 2.4 Sprites and Graphics

#### Card Artwork

| Asset Name | Description | Size (px) | Format | States |
|------------|-------------|-----------|--------|--------|
| `rock_card.png` | Rock character card art | 512x512 | PNG | Full color + grayed out |
| `paper_card.png` | Paper character card art | 512x512 | PNG | Full color + grayed out |
| `scissors_card.png` | Scissors character card art | 512x512 | PNG | Full color + grayed out |
| `card_back.png` | Hidden card back design | 512x512 | PNG | Single state |

**Card Art Style**:
- Rock: Friendly boulder character with face, strong pose
- Paper: Flowing sheet character with expressive features
- Scissors: Sharp but playful scissor character, dynamic pose

#### UI Elements

| Asset Name | Description | Size (px) | Format | Notes |
|------------|-------------|-----------|--------|-------|
| `button_normal.png` | Default button background | 256x64 | PNG | 9-slice scalable |
| `button_hover.png` | Hovered button background | 256x64 | PNG | 9-slice scalable |
| `button_pressed.png` | Pressed button background | 256x64 | PNG | 9-slice scalable |
| `button_disabled.png` | Disabled button background | 256x64 | PNG | 9-slice scalable |
| `panel_bg.png` | UI panel background | 512x512 | PNG | 9-slice, tiled |
| `card_frame.png` | Card border/frame | 512x512 | PNG | Transparent overlay |
| `hp_bar_fill.png` | HP bar fill (Survival mode) | 256x32 | PNG | Green gradient |
| `hp_bar_bg.png` | HP bar background | 256x32 | PNG | Dark gray |

#### Icons

| Asset Name | Description | Size (px) | Format | Notes |
|------------|-------------|-----------|--------|-------|
| `icon_rock_small.png` | Small Rock icon | 64x64 | PNG | For UI, score display |
| `icon_paper_small.png` | Small Paper icon | 64x64 | PNG | For UI, score display |
| `icon_scissors_small.png` | Small Scissors icon | 64x64 | PNG | For UI, score display |
| `icon_settings.png` | Settings gear icon | 32x32 | PNG | Menu access |
| `icon_back.png` | Back arrow icon | 32x32 | PNG | Navigation |

#### Backgrounds

| Asset Name | Description | Size (px) | Format | Notes |
|------------|-------------|-----------|--------|-------|
| `bg_main_menu.png` | Main menu background | 1920x1080 | JPG | Subtle pattern, dark blue |
| `bg_game.png` | Game scene background | 1920x1080 | JPG | Arena/battle theme |
| `bg_result.png` | Result screen background | 1920x1080 | JPG | Celebratory/reflective |

#### Miscellaneous

| Asset Name | Description | Size (px) | Format | Notes |
|------------|-------------|-----------|--------|-------|
| `logo.png` | Game logo | 512x128 | PNG | Transparent background |
| `bracket_node.png` | Tournament bracket node | 128x64 | PNG | For bracket display |
| `bracket_line.png` | Tournament bracket connector | Varies | PNG | Connecting lines |

### 2.5 Visual States

#### Button States

1. **Normal**: Default appearance
   - Background: `button_normal.png`
   - Scale: 100%
   - Opacity: 100%

2. **Hover**: Mouse over button
   - Background: `button_hover.png`
   - Scale: 105% (smooth scale-up)
   - Opacity: 100%
   - Transition: 0.1s ease-in-out

3. **Pressed**: Mouse button down
   - Background: `button_pressed.png`
   - Scale: 95% (scale down)
   - Opacity: 100%
   - Transition: 0.05s ease-out

4. **Disabled**: Button not interactive
   - Background: `button_disabled.png`
   - Scale: 100%
   - Opacity: 60%
   - Cursor: Default (not pointer)

#### Card States

1. **Idle**: Card available for selection
   - Scale: 100%
   - Rotation: 0°
   - Glow: None
   - Brightness: 100%

2. **Hover**: Mouse over card
   - Scale: 110% (smooth scale-up)
   - Rotation: -3° (slight tilt)
   - Glow: Soft white outline (5px)
   - Brightness: 110%
   - Transition: 0.15s ease-out

3. **Selected**: Card chosen by player
   - Scale: 105%
   - Rotation: 0°
   - Glow: Gold outline (8px) `#FFD700`
   - Brightness: 120%
   - Transition: 0.2s ease-out

4. **Disabled**: Card not selectable
   - Scale: 100%
   - Rotation: 0°
   - Glow: None
   - Brightness: 50% (grayed out)
   - Opacity: 60%

5. **Revealed**: Card shown after selection
   - Scale: 120% (dramatic entrance)
   - Rotation: 0°
   - Glow: Based on result (win=green, lose=red, draw=orange)
   - Brightness: 100%
   - Animation: Slide + scale (0.5s ease-out)

---

## 3. UI/UX Design

### 3.1 Screen Layouts

#### Main Menu Screen

```
┌────────────────────────────────────────────────────────┐
│                                                        │
│                   [GAME LOGO]                          │
│            SCISSORS • PAPER • ROCK                     │
│                                                        │
│                                                        │
│                  [  PLAY GAME  ]                       │  ← Primary CTA
│                                                        │
│                  [    QUIT     ]                       │
│                                                        │
│                                                        │
│              v1.0        [Settings Icon]               │
└────────────────────────────────────────────────────────┘
```

**Layout Details**:
- **Logo**: Top center, 512x128px
- **Play Button**: Center, large (300x80px)
- **Quit Button**: Below Play, medium (200x60px)
- **Settings Icon**: Bottom-right corner (32x32px)
- **Version**: Bottom-left corner, small text
- **Background**: `bg_main_menu.png` with subtle animated particles (optional)

#### Mode Selection Screen

```
┌────────────────────────────────────────────────────────┐
│  [←]                                                   │  ← Back button
│                                                        │
│              CHOOSE YOUR CHALLENGE                     │
│                                                        │
│  ┌───────────────┐  ┌───────────────┐                │
│  │ SINGLE MATCH  │  │  TOURNAMENT   │                │
│  │  Best of 3    │  │   8 or 16     │                │
│  └───────────────┘  └───────────────┘                │
│                                                        │
│  ┌───────────────┐  ┌───────────────┐                │
│  │   SURVIVAL    │  │  MULTIPLAYER  │                │
│  │  10 HP Mode   │  │   Hot-Seat    │                │
│  └───────────────┘  └───────────────┘                │
│                                                        │
└────────────────────────────────────────────────────────┘
```

**Layout Details**:
- **Back Button**: Top-left (32x32px icon)
- **Title**: Top center, Heading 2
- **Mode Cards**: 2x2 grid, 250x150px each
- **Mode Descriptions**: Subtitle below mode name (Body text)
- **Spacing**: 20px between cards, 40px margins

#### Single Match Screen

```
┌────────────────────────────────────────────────────────┐
│  Player: 1            ROUND 1/3             CPU: 0     │  ← Score bar
├────────────────────────────────────────────────────────┤
│                                                        │
│        YOUR CHOICE              CPU CHOICE             │
│       ┌──────────┐             ┌──────────┐           │  ← Reveal area
│       │   ???    │             │   ???    │           │  (initially hidden)
│       └──────────┘             └──────────┘           │
│                                                        │
│                 [RESULT TEXT]                          │  ← Win/Lose/Draw
│                                                        │
├────────────────────────────────────────────────────────┤
│              CHOOSE YOUR CARD                          │
│                                                        │
│   ┌─────────┐    ┌─────────┐    ┌─────────┐          │  ← Choice cards
│   │  ROCK   │    │  PAPER  │    │SCISSORS │          │
│   │  [IMG]  │    │  [IMG]  │    │  [IMG]  │          │
│   └─────────┘    └─────────┘    └─────────┘          │
│                                                        │
│                 [MAIN MENU]                            │  ← Back button
└────────────────────────────────────────────────────────┘
```

**Layout Details**:
- **Score Bar**: Top, full width, 60px height
  - Player name/score on left
  - Round indicator center
  - CPU score on right
- **Reveal Area**: Center-top, 400x300px zone
  - Two card displays side-by-side (180x240px each)
  - Result text between/below cards (Heading 1)
- **Choice Cards**: Bottom, 3 cards horizontal (200x280px each)
  - 20px spacing between cards
  - Hover/click interactions
- **Main Menu Button**: Bottom-center, small (150x50px)

#### Tournament Screen

```
┌────────────────────────────────────────────────────────┐
│  [←]   QUARTERFINALS • Match 1 of 4   Player: 1  CPU: 0│  ← Info bar
├────────────────────────────────────────────────────────┤
│  ┌───────────────┐                                     │
│  │   BRACKET     │                                     │  ← Bracket (left)
│  │   [8-Player]  │              YOUR CHOICE            │
│  │               │             ┌──────────┐            │
│  │   You vs CPU  │             │   ???    │            │
│  │      ...      │             └──────────┘            │
│  └───────────────┘                                     │
│                                  CPU CHOICE            │
│                                 ┌──────────┐           │
│                                 │   ???    │           │
│                                 └──────────┘           │
│                                                        │
│                     [RESULT TEXT]                      │
│                                                        │
├────────────────────────────────────────────────────────┤
│              CHOOSE YOUR CARD                          │
│   ┌─────────┐    ┌─────────┐    ┌─────────┐          │
│   │  ROCK   │    │  PAPER  │    │SCISSORS │          │
│   │  [IMG]  │    │  [IMG]  │    │  [IMG]  │          │
│   └─────────┘    └─────────┘    └─────────┘          │
└────────────────────────────────────────────────────────┘
```

**Layout Details**:
- **Info Bar**: Top, full width, shows round name and match number
- **Bracket Display**: Left sidebar, 250px wide
  - Visual bracket tree
  - Highlight current match
  - Grayed out completed/future matches
- **Game Area**: Right side, same as Single Match
- **Cards**: Bottom, same as Single Match

#### Survival Screen

```
┌────────────────────────────────────────────────────────┐
│  [←]   HP: ████████▒▒ 8/10   Defeated: 12   High: 15  │  ← Stats bar
├────────────────────────────────────────────────────────┤
│                                                        │
│        YOUR CHOICE              CPU CHOICE             │
│       ┌──────────┐             ┌──────────┐           │
│       │   ???    │             │   ???    │           │
│       └──────────┘             └──────────┘           │
│                                                        │
│                 [RESULT TEXT]                          │
│                                                        │
├────────────────────────────────────────────────────────┤
│              CHOOSE YOUR CARD                          │
│   ┌─────────┐    ┌─────────┐    ┌─────────┐          │
│   │  ROCK   │    │  PAPER  │    │SCISSORS │          │
│   │  [IMG]  │    │  [IMG]  │    │  [IMG]  │          │
│   └─────────┘    └─────────┘    └─────────┘          │
│                                                        │
│                 [MAIN MENU]                            │
└────────────────────────────────────────────────────────┘
```

**Layout Details**:
- **Stats Bar**: Top, full width
  - HP bar with visual fill (green gradient)
  - Defeated count
  - Session high score
- **Game Area**: Same as Single Match
- **HP Bar Visual**: Segmented (10 blocks), loses color as HP decreases
  - 10-7 HP: Green
  - 6-4 HP: Yellow
  - 3-1 HP: Red (pulsing animation)

#### Multiplayer Screen

**Phase 1: Player 1's Turn**
```
┌────────────────────────────────────────────────────────┐
│  Player 1: 1            ROUND 1/3         Player 2: 0  │
├────────────────────────────────────────────────────────┤
│                                                        │
│                 PLAYER 1 - MAKE YOUR CHOICE            │
│                                                        │
│   ┌─────────┐    ┌─────────┐    ┌─────────┐          │
│   │  ROCK   │    │  PAPER  │    │SCISSORS │          │
│   │  [IMG]  │    │  [IMG]  │    │  [IMG]  │          │
│   └─────────┘    └─────────┘    └─────────┘          │
│                                                        │
└────────────────────────────────────────────────────────┘
```

**Phase 2: Transition Screen (After P1 Choice)**
```
┌────────────────────────────────────────────────────────┐
│                                                        │
│                                                        │
│                                                        │
│                 PLAYER 2'S TURN                        │
│                                                        │
│                    [  READY  ]                         │
│                                                        │
│          (Player 1, please look away)                  │
│                                                        │
│                                                        │
└────────────────────────────────────────────────────────┘
```

**Phase 3: Player 2's Turn**
```
┌────────────────────────────────────────────────────────┐
│  Player 1: 1            ROUND 1/3         Player 2: 0  │
├────────────────────────────────────────────────────────┤
│                                                        │
│                 PLAYER 2 - MAKE YOUR CHOICE            │
│                                                        │
│   ┌─────────┐    ┌─────────┐    ┌─────────┐          │
│   │  ROCK   │    │  PAPER  │    │SCISSORS │          │
│   │  [IMG]  │    │  [IMG]  │    │  [IMG]  │          │
│   └─────────┘    └─────────┘    └─────────┘          │
│                                                        │
└────────────────────────────────────────────────────────┘
```

**Phase 4: Pre-Reveal Transition**
```
┌────────────────────────────────────────────────────────┐
│                                                        │
│                                                        │
│                                                        │
│                 READY FOR REVEAL?                      │
│                                                        │
│                    [  READY  ]                         │
│                                                        │
│          (Both players watch together!)                │
│                                                        │
│                                                        │
└────────────────────────────────────────────────────────┘
```

**Phase 5: Reveal**
```
┌────────────────────────────────────────────────────────┐
│  Player 1: 2            ROUND 2/3         Player 2: 0  │
├────────────────────────────────────────────────────────┤
│                                                        │
│      PLAYER 1 CHOICE           PLAYER 2 CHOICE         │
│       ┌──────────┐             ┌──────────┐           │
│       │   ROCK   │             │ SCISSORS │           │
│       │  [IMG]   │             │  [IMG]   │           │
│       └──────────┘             └──────────┘           │
│                                                        │
│                 PLAYER 1 WINS!                         │
│                                                        │
│                 [NEXT ROUND]                           │
└────────────────────────────────────────────────────────┘
```

**Layout Details**:
- **Transition Screens**: Full-screen overlay
  - Large text (Heading 1)
  - Single "Ready" button (300x80px)
  - Instruction text below (Body, lighter color)
- **Transitions**:
  - After P1 choice: "Player 2's Turn - Ready?"
  - After P2 choice: "Ready for Reveal?"
- **Reveal**: Same as Single Match but with both player names

#### Result Screen

```
┌────────────────────────────────────────────────────────┐
│                                                        │
│                                                        │
│                    YOU WIN! 🎉                         │  ← Big result
│                  (or YOU LOSE!)                        │
│                                                        │
│       ┌──────────┐             ┌──────────┐           │
│       │  ROCK    │             │  PAPER   │           │  ← Final choices
│       │  [IMG]   │             │  [IMG]   │           │
│       └──────────┘             └──────────┘           │
│          YOU                        CPU                │
│                                                        │
│              FINAL SCORE: 2 - 1                        │  ← Score
│                                                        │
│  [Mode-Specific Stats]                                 │  ← Optional stats
│  (Tournament: "You won the tournament!")               │
│  (Survival: "Defeated 12 opponents!")                  │
│                                                        │
│                  [  PLAY AGAIN  ]                      │  ← Primary CTA
│                  [  MAIN MENU   ]                      │
│                                                        │
└────────────────────────────────────────────────────────┘
```

**Layout Details**:
- **Result Text**: Top-center, very large (Title size, 48px+)
  - Animated entrance (scale + fade, 0.5s)
- **Choice Cards**: Center, side-by-side (200x280px each)
  - Labels below: player names
- **Final Score**: Below cards, large (Score size, 36px)
- **Mode Stats**: Optional additional info (Body text)
- **Buttons**: Bottom-center, stacked
  - Play Again: Primary (300x80px)
  - Main Menu: Secondary (250x60px)

### 3.2 UI Components

#### Button Component

**Specifications**:
- **Minimum Size**: 150x50px (accessibility target: 44x44px)
- **Padding**: 20px horizontal, 15px vertical
- **Border Radius**: 8px (rounded corners)
- **Font**: Button style (20px, Bold)
- **Shadow**: Drop shadow (0px 4px 8px rgba(0,0,0,0.3))

**States**:
1. Normal: `button_normal.png` background
2. Hover: Scale 105%, `button_hover.png`
3. Pressed: Scale 95%, `button_pressed.png`
4. Disabled: Opacity 60%, `button_disabled.png`

**Animation**:
- Hover transition: 0.1s ease-in-out
- Press transition: 0.05s ease-out
- Scale origin: center

**Sound**:
- Hover: `button_hover.wav` (optional, subtle)
- Click: `button_click.wav`

#### Choice Card Component

**Specifications**:
- **Size**: 200x280px (card body 180x240px + label 20px)
- **Border**: 4px solid, color based on state
- **Border Radius**: 12px
- **Shadow**: 0px 6px 12px rgba(0,0,0,0.4)
- **Label**: Choice name below card (Body text)

**States**:
1. Idle: No border glow, 100% scale
2. Hover: White glow (5px), 110% scale, -3° tilt
3. Selected: Gold glow (8px), 105% scale, 0° tilt
4. Disabled: Grayed out (50% brightness), 60% opacity
5. Revealed: Result-based glow (green/red/orange), 120% scale

**Animation**:
- Hover: 0.15s ease-out
- Select: 0.2s ease-out with elastic bounce
- Reveal: 0.5s ease-out with slide-in from top

**Sound**:
- Hover: Subtle whoosh (optional)
- Click: `choice_made.wav`
- Reveal: `choice_reveal.wav`

#### Score Display Component

**Specifications**:
- **Format**: "[Player Name]: X  -  [Opponent Name]: Y"
- **Font**: Score style (36px, Bold, Gold)
- **Position**: Top bar, centered or spread
- **Spacing**: 40px between player scores
- **Icons**: Small choice icons next to scores (optional)

**Updates**:
- Immediate number change
- Brief flash/pulse on score change (0.3s)
- Color flash based on result:
  - Win: Green flash (#4CAF50)
  - Lose: Red flash (#F44336)

**Animation**:
- Count-up: Smooth number interpolation (0.3s)
- Pulse: Scale to 110% and back (0.2s ease-out)

#### HP Bar Component (Survival Mode)

**Specifications**:
- **Size**: 300x40px
- **Segments**: 10 blocks (one per HP)
- **Fill Color**: Gradient based on HP
  - 10-7 HP: Green (#4CAF50 to #66BB6A)
  - 6-4 HP: Yellow (#FFB74D to #FFA726)
  - 3-1 HP: Red (#EF5350 to #E53935)
- **Background**: Dark gray (#424242)
- **Border**: 2px solid black
- **Label**: "HP: X/10" above bar

**Animation**:
- HP Loss: Block flashes red then fades (0.4s)
- Low HP (1-3): Pulsing glow effect (0.8s loop)

#### Round Indicator Component

**Specifications**:
- **Format**: "ROUND X/Y" or "ROUND X" (Survival)
- **Font**: Heading 2 (24px, SemiBold, White)
- **Position**: Top bar, center
- **Highlight**: Gold underline when new round starts

**Animation**:
- Round transition: Fade out old, fade in new (0.3s)

#### Bracket Display Component (Tournament)

**Specifications**:
- **Size**: 250px wide, variable height
- **Nodes**: 128x64px boxes
- **Lines**: 2px solid white
- **Current Match**: Gold highlight (#FFD700)
- **Completed Matches**: Grayed out (50% opacity)
- **Future Matches**: Dotted lines

**Layout**:
- Vertical tree structure
- 3 rounds (8-player): QF → SF → F
- 4 rounds (16-player): R16 → QF → SF → F

**Animation**:
- Match completion: Node fills with winner color
- Progression: Animate line drawing to next node (0.5s)

### 3.3 User Interactions

#### Main Menu Interactions

**Flow**:
1. User sees logo and buttons
2. Hover over "Play Game":
   - Button scales up (105%)
   - Subtle glow appears
   - Optional hover sound
3. Click "Play Game":
   - Button scales down (95%)
   - Click sound plays
   - Fade transition to Mode Selection (0.5s)

**Keyboard Shortcuts**:
- Enter: Start game
- Escape: Quit (with confirmation)

#### Mode Selection Interactions

**Flow**:
1. User sees 4 mode cards
2. Hover over mode card:
   - Card scales up (105%)
   - Border glow appears
   - Description text highlights
3. Click mode card:
   - Click sound
   - Card briefly scales down (95%)
   - Fade transition to game scene (0.5s)
   - Load appropriate scene based on mode

**Back Button**:
- Click to return to Main Menu
- Fade transition (0.3s)

#### Game Scene Interactions

**Choice Flow**:
1. **Round Start**:
   - Cards animate in from bottom (0.3s stagger)
   - Reveal area shows "???" placeholders
   - Instruction text: "Choose Your Card"

2. **Hover Card**:
   - Card scales up (110%)
   - Slight tilt (-3°)
   - White glow (5px)
   - Transition: 0.15s ease-out

3. **Click Card**:
   - `choice_made.wav` plays
   - Card scales to 105%, gold glow
   - Other cards dim (60% opacity)
   - All cards become disabled
   - Player choice appears in reveal area (left side)

4. **AI "Thinking"**:
   - Brief delay (0.5-1s)
   - Optional: Spinning indicator on CPU side

5. **AI Reveal**:
   - `choice_reveal.wav` plays
   - AI card slides in from top (0.5s ease-out)
   - Card scales to 120% then settles to 100%
   - Glow appears based on result

6. **Result Display**:
   - Result text animates in (scale + fade, 0.5s)
   - "YOU WIN!" / "YOU LOSE!" / "DRAW!"
   - Appropriate SFX plays
   - Winner's card gets result-colored glow
   - Score updates with animation

7. **Round End**:
   - Pause 2 seconds (let player see result)
   - Check win condition:
     - If game over: Fade to Result Screen (0.5s)
     - If continuing: Fade out cards, reset, next round

**Back to Menu**:
- Click "Main Menu" button at bottom
- Confirmation prompt: "Quit current game?"
- If yes: Fade to Main Menu (0.5s)

#### Tournament-Specific Interactions

- Bracket updates after each match:
  - Completed node fills with color
  - Line animates to next node (0.5s)
  - Next opponent highlighted
- Round name updates: "QUARTERFINALS" → "SEMIFINALS" → "FINALS"

#### Survival-Specific Interactions

- HP Bar updates after each match:
  - Loss: -1 HP, segment flashes red then fades (0.4s)
  - HP at 3 or below: Pulsing red glow (warning)
- Defeated counter increments with +1 animation
- High score updates if beaten (gold flash)

#### Multiplayer-Specific Interactions

**Two-Stage Transition Flow**:

**Stage 1: After Player 1 Choice**
1. Player 1 clicks choice card
2. Choice locked, cards disappear
3. Transition screen fades in (0.3s)
4. Screen shows: "PLAYER 2'S TURN - READY?"
5. Instruction: "(Player 1, please look away)"
6. Player 1 looks away from screen
7. Player 2 clicks "Ready" button
8. Transition screen fades out (0.3s)
9. Choice cards appear for Player 2

**Stage 2: After Player 2 Choice**
1. Player 2 clicks choice card
2. Choice locked, cards disappear
3. Transition screen fades in (0.3s)
4. Screen shows: "READY FOR REVEAL?"
5. Instruction: "(Both players watch together!)"
6. Player 1 returns to screen
7. Either player clicks "Ready" button
8. Transition screen fades out (0.3s)
9. Reveal animation plays (both choices shown)
10. Result displayed

**Transition Screen Specs**:
- Full-screen overlay with semi-transparent dark background
- Large centered text (Heading 1, 32px)
- Big "Ready" button (300x80px)
- Instruction text below (Body, 18px, lighter color)
- Fade in/out: 0.3s
- No timeout (manual control only)

#### Result Screen Interactions

**Flow**:
1. Screen fades in (0.5s)
2. Result text animates in:
   - Scale from 0% to 100% (0.5s elastic ease)
   - Color based on result (green/red)
3. Choice cards fade in (0.3s after text)
4. Score displays
5. Mode-specific stats show (if applicable)
6. Buttons appear (0.2s delay)

**Button Interactions**:
- **Play Again**:
  - Click → Restart same mode with same settings
  - Fade transition (0.5s)
- **Main Menu**:
  - Click → Return to Mode Selection
  - Fade transition (0.5s)

### 3.4 Feedback Mechanisms

#### Visual Feedback

| User Action | Visual Response | Duration | Details |
|-------------|----------------|----------|---------|
| Button hover | Scale 105%, highlight | 0.1s | Smooth ease-in-out |
| Button click | Scale 95%, press state | 0.05s | Quick press feedback |
| Card hover | Scale 110%, tilt -3°, glow | 0.15s | Engaging card lift |
| Card select | Gold glow, others dim | 0.2s | Clear selection state |
| Choice reveal | Slide + scale animation | 0.5s | Dramatic entrance |
| Win round | Green flash, score pulse | 0.3s | Positive reinforcement |
| Lose round | Red flash, score pulse | 0.3s | Clear loss indicator |
| Draw round | Orange flash | 0.3s | Neutral feedback |
| HP loss | Red flash on HP segment | 0.4s | Health warning |
| Score change | Number count-up, pulse | 0.3s | Smooth update |

#### Audio Feedback

| User Action | Sound Effect | Volume | Notes |
|-------------|-------------|--------|-------|
| Button hover | `button_hover.wav` | 60% | Optional, subtle |
| Button click | `button_click.wav` | 80% | Crisp, satisfying |
| Card hover | Soft whoosh | 50% | Optional, very subtle |
| Choice made | `choice_made.wav` | 85% | Confirmation tone |
| Choice reveal | `choice_reveal.wav` | 90% | Dramatic swoosh |
| Win round | `win_round.wav` | 90% | Positive chime |
| Lose round | `lose_round.wav` | 90% | Negative tone |
| Draw round | `draw_round.wav` | 85% | Neutral beep |
| Win game | `win_game.wav` | 100% | Triumphant fanfare |
| Lose game | `lose_game.wav` | 95% | Somber but hopeful |

#### Animation Timing Reference

| Animation | Duration | Easing Function |
|-----------|----------|-----------------|
| Button hover | 0.1s | ease-in-out |
| Button press | 0.05s | ease-out |
| Card hover | 0.15s | ease-out |
| Card select | 0.2s | elastic (bounce) |
| Choice reveal | 0.5s | ease-out |
| Result text | 0.5s | elastic |
| Score update | 0.3s | linear |
| Scene transition | 0.5s | fade (linear) |
| HP flash | 0.4s | ease-in-out |
| Bracket line | 0.5s | ease-in |

---

## 4. Audio Design

### 4.1 Sound Effects (SFX)

| Sound File | Trigger | Description | Duration | Format | Volume |
|------------|---------|-------------|----------|--------|--------|
| `button_hover.wav` | Mouse over button | Subtle tick/beep | 0.05s | WAV | 60% |
| `button_click.wav` | Button pressed | Crisp click sound | 0.1s | WAV | 80% |
| `choice_made.wav` | Player selects card | Confirmation chime | 0.2s | WAV | 85% |
| `choice_reveal.wav` | AI choice revealed | Dramatic swoosh/pop | 0.3s | WAV | 90% |
| `win_round.wav` | Player wins round | Positive ascending tone | 0.5s | WAV | 90% |
| `lose_round.wav` | Player loses round | Negative descending tone | 0.5s | WAV | 90% |
| `draw_round.wav` | Round is a draw | Neutral beep | 0.3s | WAV | 85% |
| `win_game.wav` | Player wins match | Triumphant fanfare | 2.0s | WAV | 100% |
| `lose_game.wav` | Player loses match | Sad trombone (lighthearted) | 1.5s | WAV | 95% |
| `hp_loss.wav` | HP decreases (Survival) | Hit sound, impact | 0.4s | WAV | 85% |
| `high_score.wav` | New high score (Survival) | Achievement jingle | 1.0s | WAV | 95% |
| `ready.wav` | Ready button clicked (Multiplayer) | Confirmation beep | 0.2s | WAV | 80% |

**SFX Characteristics**:
- **Style**: Cartoonish, playful, not overly serious
- **Mixing**: All SFX normalized, consistent volume levels
- **Ducking**: Background music reduces to 60% when SFX plays
- **Format**: 16-bit, 44.1kHz stereo WAV
- **Fallback**: Graceful degradation if SFX fails to load (silent, no errors)

### 4.2 Background Music

| Track File | Context | Description | Loop | Duration | Format | Volume |
|------------|---------|-------------|------|----------|--------|--------|
| `menu_theme.ogg` | Main Menu, Mode Selection | Upbeat, inviting, cheerful | Yes | 2:30 | OGG | 70% |
| `game_theme.ogg` | All gameplay scenes | Energetic, tension-building, rhythmic | Yes | 3:00 | OGG | 65% |
| `victory_theme.ogg` | Result Screen (Win) | Triumphant, celebratory | No | 0:30 | OGG | 80% |
| `defeat_theme.ogg` | Result Screen (Lose) | Somber but hopeful, encouraging | No | 0:30 | OGG | 75% |

**Music Characteristics**:
- **Style**: Chiptune or orchestral hybrid, upbeat and playful
- **Tempo**: 120-140 BPM (energetic but not frantic)
- **Instrumentation**: Synthesizers, light percussion, melody-focused
- **Mood**: Fun, competitive, lighthearted

**Music Transitions**:
- **Fade Duration**: 1.0s crossfade between tracks
- **Ducking**: Reduce to 60% volume during SFX playback
- **Loop**: Seamless loop points for menu and game themes

**Music Controls** (Future Enhancement):
- Master Volume slider (0-100%)
- Music On/Off toggle
- SFX On/Off toggle
- Individual volume sliders for Music and SFX

### 4.3 Audio Implementation Notes

**Godot Audio Setup**:
- Use `AudioStreamPlayer` for music (non-positional)
- Use `AudioStreamPlayer` for UI SFX (non-positional)
- Music bus: "Music" (for volume control)
- SFX bus: "SFX" (for volume control)

**Performance**:
- Preload all SFX on game start
- Stream music files (don't load entirely into memory)
- Limit simultaneous SFX to 8 channels (prevent audio clutter)

**Accessibility**:
- All audio optional (can play with audio off)
- Visual feedback supplements audio (never audio-only cues)

---

## 5. Game Mechanics

### 5.1 Core Rules

**Winning Combinations**:

| Player Choice | Beats | Reasoning |
|---------------|-------|-----------|
| **Rock** | Scissors | Rock crushes Scissors |
| **Scissors** | Paper | Scissors cuts Paper |
| **Paper** | Rock | Paper covers Rock |

**Draw Condition**:
- If both players choose the same option: **Draw** (no winner)

**Example Scenarios**:
- Player: Rock, CPU: Scissors → Player wins
- Player: Paper, CPU: Paper → Draw
- Player: Scissors, CPU: Rock → CPU wins

### 5.2 Round Flow

**Detailed Step-by-Step**:

**1. Round Initialization**
- Clear previous round UI elements
- Reset card states to idle (all selectable)
- Display round number: "ROUND X/Y" or "ROUND X"
- Show score: "Player: X - CPU: Y"
- Instruction text: "Choose Your Card"

**2. Player Input Phase**
- Cards animate in from bottom (0.3s stagger)
- Cards become interactive (hover states active)
- Player hovers and clicks a card:
  - Selected card: Gold glow, 105% scale
  - Other cards: Dim to 60% opacity
  - All cards disabled (no further input)
- Player's choice appears in left reveal area (faded)

**3. AI Decision Phase**
- Brief delay: 0.5-1.0 seconds (simulate "thinking")
- AI algorithm runs (see 5.4 AI Behavior)
- AI choice determined (random for MVP)
- Optional: Spinning indicator on CPU side

**4. Reveal Phase**
- `choice_reveal.wav` plays
- Player card fully appears on left (already visible, just brightens)
- CPU card slides in from top on right (0.5s ease-out animation)
- Both cards scale to 120%, then settle to 100%
- Brief pause: 0.5s (anticipation)

**5. Determine Winner**
- Apply core rules (Rock beats Scissors, etc.)
- Calculate result: Player Win, CPU Win, or Draw
- Prepare visual/audio feedback

**6. Display Result**
- Result text animates in: "YOU WIN!" / "YOU LOSE!" / "DRAW!"
  - Scale from 0% to 100% (0.5s elastic ease)
  - Color: Green (win), Red (lose), Orange (draw)
- Winner's card gets colored glow (green/red/orange)
- Appropriate SFX plays:
  - Win: `win_round.wav`
  - Lose: `lose_round.wav`
  - Draw: `draw_round.wav`

**7. Update Scores**
- Increment winner's score (or both on draw remain same)
- Score animates: Count-up (0.3s) + pulse (0.2s)
- Score display flashes result color briefly

**8. Check Win Condition**
- **Best of 3**: First to 2 wins
- If win condition met:
  - Wait 2 seconds
  - Transition to Result Screen
- Else:
  - Wait 3 seconds
  - Fade out cards and result text
  - Go to step 1 (next round)

### 5.3 Win Conditions

#### Single Match Mode
- **Format**: Best of 3 (first to 2 wins)
- **Rounds**: Minimum 2, maximum 3
- **Win**: First player to win 2 rounds
- **Example Scenarios**:
  - Player wins 2-0: Game ends after 2 rounds
  - Player wins 2-1: Game ends after 3 rounds
  - Tied 1-1 after 2 rounds: Play round 3 (tiebreaker)

#### Tournament Mode
- **Format**: Single-elimination bracket
- **Rounds per Match**: Best of 3 (first to 2 wins)
- **Tournament Rounds**:
  - 8-player: Quarterfinals (4 matches) → Semifinals (2 matches) → Finals (1 match)
  - 16-player: Round of 16 (8 matches) → Quarters → Semis → Finals
- **Win Condition**: Win all matches in bracket to become champion
- **Loss Condition**: Lose any match, eliminated from tournament

#### Survival Mode
- **Format**: Continuous battles until player reaches 0 HP
- **Player HP**: Starts with 10 HP
- **Opponent HP**: Each opponent has 1 HP (symbolic, always best of 3)
- **Rounds per Battle**: Best of 3 (first to 2 wins)
- **Losing a Battle**: Player loses 1 HP, continues to next opponent
- **Winning a Battle**: Player HP unchanged, continues to next opponent
- **Game Over**: Player reaches 0 HP
- **High Score**: Total opponents defeated in current session

**Survival HP Behavior**:
```
Start: Player HP = 10

Battle 1: Player wins 2-0 → HP = 10, Defeated = 1
Battle 2: Player wins 2-1 → HP = 10, Defeated = 2
Battle 3: Player loses 1-2 → HP = 9, Defeated = 3  ← Lost but still continues
Battle 4: Player wins 2-0 → HP = 9, Defeated = 4
...
Battle 15: Player loses 0-2 → HP = 0, Defeated = 15 → GAME OVER

High Score: 15 opponents defeated
```

#### Multiplayer Mode
- **Format**: Best of 3 (first to 2 wins)
- **Players**: 2 human players, hot-seat style
- **Win**: First player to win 2 rounds
- **Same as Single Match** but with two human players

### 5.4 AI Behavior

**MVP Implementation**: **Easy (Random) AI**

**Algorithm**:
```
function AIChooseMove():
    choices = [Rock, Paper, Scissors]
    randomIndex = Random(0, 2)  // 0, 1, or 2
    return choices[randomIndex]
```

**Characteristics**:
- Completely random selection
- 33.3% probability for each choice
- No pattern detection
- No memory of previous rounds
- Deterministic randomness (seeded for testing)

**AI "Thinking" Delay**:
- Random delay between 0.5-1.0 seconds
- Simulates human decision-making
- Prevents instant response (feels more natural)
- **Implementation**: `await get_tree().create_timer(randf_range(0.5, 1.0))`

**Future AI Enhancements** (Post-MVP):

**Medium AI**:
- 70% random, 30% pattern-based
- Detects if player uses same choice 3+ times in a row
- Responds with counter-choice
- Still beatable with mixed strategies

**Hard AI**:
- 50% random, 50% predictive
- Advanced pattern recognition (looks for cycles)
- Predicts player's next move based on last 5 rounds
- Occasionally makes "mistakes" (10% random overrides)

### 5.5 Edge Cases

**Scenario 1: All Draws**
- If 5 consecutive draws occur:
  - Display message: "It's a standoff! Try something different!"
  - Message fades after 2 seconds
  - Continue playing normally
- Draws don't count toward win condition (neither player gets point)

**Scenario 2: First Round of Game**
- AI has no history to analyze
- AI always makes random choice (even for future "smart" AI)
- Ensures fair start for all modes

**Scenario 3: Tournament Bracket Size Selection**
- Mode Selection should prompt: "8-Player or 16-Player Tournament?"
- Default: 8-player (shorter, more accessible)
- 16-player available for experienced players

**Scenario 4: Survival High Score Tie**
- If player defeats same number as previous high score:
  - Display: "Tied your high score!" (not "New high score!")
  - High score display shows same number (no change)

**Scenario 5: Multiplayer Ready Button**
- Both players can click Ready button (either player)
- Button disabled after first click (prevents double-trigger)
- Transition begins immediately on click

**Scenario 6: Invalid Input (Should Never Happen)**
- UI prevents invalid choices (buttons disabled after selection)
- If somehow invalid input reaches game logic:
  - Log error: "Invalid choice: [choice]"
  - Default to random valid choice
  - Continue game (graceful degradation)

**Scenario 7: Quit During Match**
- Click "Main Menu" button → Show confirmation dialog
- Dialog: "Quit current game? Progress will be lost."
- Buttons: "Yes, Quit" / "Cancel"
- If Yes: Return to Main Menu, discard game state
- If Cancel: Resume game

**Scenario 8: Game Over (Match Complete)**
- After final round, wait 2 seconds
- Automatic transition to Result Screen
- Result Screen shows:
  - Final result: "YOU WIN!" or "YOU LOSE!"
  - Final score: "2-1" (example)
  - Mode-specific stats (if applicable)
  - Buttons: "Play Again" / "Main Menu"

---

## 6. Game Flow

### 6.1 State Diagram

```
┌──────────────┐
│   LAUNCH     │
│     APP      │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│  MAIN MENU   │ ◄──────────────────────────┐
└──────┬───────┘                            │
       │ Click "Play Game"                  │
       ▼                                    │
┌──────────────┐                            │
│    MODE      │                            │
│  SELECTION   │                            │
└──────┬───────┘                            │
       │ Click mode                         │
       ▼                                    │
┌──────────────┐                            │
│ INITIALIZE   │                            │
│    GAME      │                            │
│ - Reset scores                            │
│ - Load scene                              │
└──────┬───────┘                            │
       │                                    │
       ▼                                    │
┌──────────────┐                            │
│ ROUND START  │ ◄──────────┐               │
│ - Display UI               │               │
│ - Enable cards             │               │
└──────┬───────┘             │               │
       │                     │               │
       ▼                     │               │
┌──────────────┐             │               │
│ WAIT PLAYER  │             │               │
│    CHOICE    │             │               │
└──────┬───────┘             │               │
       │ Player clicks       │               │
       ▼                     │               │
┌──────────────┐             │               │
│   PROCESS    │             │               │
│    PLAYER    │             │               │
│    CHOICE    │             │               │
│ - Lock choice              │               │
│ - Disable UI               │               │
└──────┬───────┘             │               │
       │                     │               │
       ▼                     │               │
┌──────────────┐             │               │
│ AI DECISION  │             │               │
│ - Delay 0.5s               │               │
│ - Calculate                │               │
└──────┬───────┘             │               │
       │                     │               │
       ▼                     │               │
┌──────────────┐             │               │
│    REVEAL    │             │               │
│    PHASE     │             │               │
│ - Show both                │               │
│ - Animate                  │               │
└──────┬───────┘             │               │
       │                     │               │
       ▼                     │               │
┌──────────────┐             │               │
│  DETERMINE   │             │               │
│   WINNER     │             │               │
│ - Apply rules              │               │
└──────┬───────┘             │               │
       │                     │               │
       ▼                     │               │
┌──────────────┐             │               │
│ SHOW RESULT  │             │               │
│ - Display win              │               │
│ - Play SFX                 │               │
│ - Update score             │               │
└──────┬───────┘             │               │
       │                     │               │
       ▼                     │               │
┌──────────────┐             │               │
│    CHECK     │             │               │
│     WIN      │             │               │
│  CONDITION   │             │               │
└──────┬───────┘             │               │
       │                     │               │
       ├─ Continue ──────────┘               │
       │   (No winner yet)                   │
       │                                     │
       └─ Game Over                          │
          (Winner determined)                │
          │                                  │
          ▼                                  │
       ┌──────────────┐                     │
       │   RESULT     │                     │
       │   SCREEN     │                     │
       │ - Show final                       │
       │ - Buttons                          │
       └──────┬───────┘                     │
              │                             │
              ├─ Play Again ────────────────┘
              │  (Restart mode)
              │
              └─ Main Menu ─────────────────┘
                 (Return to menu)
```

### 6.2 Scene Transitions

**Transition Matrix**:

| From Scene | To Scene | Trigger | Animation | Duration |
|------------|----------|---------|-----------|----------|
| Main Menu | Mode Selection | Click "Play Game" | Fade | 0.5s |
| Mode Selection | Main Menu | Click Back button | Fade | 0.3s |
| Mode Selection | Single Match | Click "Single Match" | Fade | 0.5s |
| Mode Selection | Tournament | Click "Tournament" | Fade | 0.5s |
| Mode Selection | Survival | Click "Survival" | Fade | 0.5s |
| Mode Selection | Multiplayer | Click "Multiplayer" | Fade | 0.5s |
| Any Game Scene | Result Screen | Win condition met | Fade | 0.5s |
| Result Screen | Same Game Scene | Click "Play Again" | Fade | 0.5s |
| Result Screen | Mode Selection | Click "Main Menu" | Fade | 0.5s |
| Any Scene | Main Menu | Click "Quit" (with confirm) | Fade | 0.5s |

**Transition Implementation**:
- Use Godot's `SceneTree.change_scene_to_file()` or custom fade overlay
- Fade overlay: Black screen, opacity 0% → 100% → 0%
- Fade-out: 0.25s, Fade-in: 0.25s (total 0.5s)
- Optional: Brief pause at 100% opacity (0.1s)

### 6.3 Navigation

**Navigation Map**:

```
Main Menu
    ↓
Mode Selection
    ↓
    ├─ Single Match ──→ [Game] ──→ Result ──┐
    ├─ Tournament   ──→ [Game] ──→ Result ──┤
    ├─ Survival     ──→ [Game] ──→ Result ──┼──→ Play Again (loop back)
    └─ Multiplayer  ──→ [Game] ──→ Result ──┘    or Main Menu (exit)
```

**Back Navigation**:
- Mode Selection → Main Menu: Back button (top-left)
- Any Game Scene → Main Menu: "Main Menu" button (with confirmation)
- Result Screen → Mode Selection: "Main Menu" button

**Keyboard Shortcuts** (Optional Future Enhancement):
- Escape: Back / Main Menu (with confirmation if in game)
- Enter: Confirm / Select primary action
- 1, 2, 3: Select Rock, Paper, Scissors (during gameplay)
- Tab: Cycle through interactive elements

---

## 7. Player Experience

### 7.1 First-Time User Experience (FTUE)

**Goal**: Player understands gameplay within 10 seconds of first launch

**FTUE Flow**:

**1. Launch (0-2 seconds)**
- Game logo appears
- Main menu loads quickly (< 2 seconds)
- Clear "Play Game" button (primary call-to-action)

**2. First Interaction (2-5 seconds)**
- Player hovers over "Play Game" → Button highlights (visual feedback)
- Player clicks → Smooth transition to Mode Selection
- Player sees 4 mode cards with descriptions

**3. Mode Selection (5-10 seconds)**
- Player understands there are 4 modes
- Descriptions clearly state what each mode is:
  - "Single Match - Best of 3"
  - "Tournament - 8 or 16 Players"
  - "Survival - 10 HP Mode"
  - "Multiplayer - Hot-Seat"
- Player clicks "Single Match" (most accessible option)

**4. First Round (10-20 seconds)**
- Scene loads, instruction text: "Choose Your Card"
- 3 large, clear cards displayed (Rock, Paper, Scissors)
- Player recognizes icons immediately (familiar game)
- Player clicks a card → Selection confirmed with visual/audio feedback
- AI choice revealed → Result shown → Score updated
- **Player now understands core gameplay loop**

**Optional Tutorial** (Future Enhancement):
- Brief overlay on first launch: "Choose your weapon to battle!"
- Tooltip on first card hover: "Rock beats Scissors"
- Can be skipped: "Don't show this again" checkbox

**FTUE Design Principles**:
- No forced tutorials (game is self-explanatory)
- Instructions via UI labels, not separate screens
- Immediate gameplay (no unnecessary delays)
- Familiar game (Rock-Paper-Scissors is universally known)

### 7.2 Emotional Journey

**Designed Emotional Arc Per Match**:

| Phase | Desired Emotion | Design Elements |
|-------|-----------------|-----------------|
| **Launch / Main Menu** | Excitement, Anticipation | Bright colors, upbeat music, inviting UI, clear CTA |
| **Mode Selection** | Curiosity, Decision | Clear options, variety, descriptions, smooth transitions |
| **Round Start** | Focus, Readiness | Clean UI, cards animate in, instruction text, calm before action |
| **Making Choice** | Tension, Strategy | Hover effects, card feedback, player in control, brief moment of decision |
| **AI "Thinking"** | Anticipation, Suspense | Short delay, optional spinner, builds tension |
| **Reveal** | Suspense, Peak Tension | Dramatic animation, slow reveal, pause before result |
| **Win Round** | Joy, Satisfaction | Green flash, positive SFX, score +1 animation, celebratory tone |
| **Lose Round** | Mild Disappointment, Determination | Red flash, negative tone, quick recovery, "try again" feeling |
| **Draw Round** | Mild Frustration, Amusement | Orange flash, neutral tone, "let's go again" |
| **Mid-Match (Tied)** | Tension, Engagement | Tight score display, excitement building |
| **Final Round** | High Stakes, Excitement | Clear indication "final round", increased tension |
| **Win Match** | Triumph, Accomplishment | Victory music, big text, confetti, score display, "Play Again" button |
| **Lose Match** | Mild Disappointment, Motivation | Somber music, encouraging text ("Try Again!"), prominent replay button |

**Emotional Design Goals**:
- Keep energy high (fast rounds, immediate feedback)
- Balance tension and relief (dramatic reveals, quick resets)
- Encourage replay (positive reinforcement, accessible "Play Again")
- Avoid frustration (no time pressure, clear rules, fair AI)

**Multiplayer Emotional Arc**:
- **P1 Chooses**: Tension (making the right choice)
- **Transition 1**: Anticipation (P1 looks away, P2's turn)
- **P2 Chooses**: Tension (matching P1's stakes)
- **Transition 2**: Excitement (both players return, about to see outcome)
- **Reveal**: Shared Suspense (watching together)
- **Result**: Shared Joy/Disappointment (social experience amplified)

### 7.3 Replay Value

**Elements Encouraging Replay**:

1. **Quick Sessions**
   - Single Match: 2-3 minutes
   - Tournament: 10-15 minutes
   - Survival: Until failure (variable, but engaging)
   - Multiplayer: 2-3 minutes
   - Low time commitment → Easy to "play one more"

2. **Immediate "Play Again" Button**
   - Result screen prominently features "Play Again"
   - No need to navigate back to menu
   - Restarts same mode instantly

3. **Multiple Game Modes**
   - Variety in gameplay (Single, Tournament, Survival, Multiplayer)
   - Different challenges (quick match vs. endurance)
   - Modes cater to different moods

4. **Survival Mode High Score**
   - Personal best tracking (session-based)
   - Incentive to "beat your record"
   - Endless progression (no hard cap)

5. **Tournament Progression**
   - Sense of achievement (winning bracket)
   - Challenge escalation (more matches)
   - Variety in opponent count (8 vs. 16)

6. **Social Experience (Multiplayer)**
   - Play with friends
   - Face-to-face competition
   - Shared moments (reveal together)

7. **Satisfying Feedback**
   - Every action feels good (animations, sounds)
   - Clear visual/audio reinforcement
   - Polished, responsive UI

**Future Replay Enhancements** (Post-MVP):
- Persistent high scores (across sessions)
- Player stats (total wins, win rate, favorite choice)
- AI difficulty levels (Easy, Medium, Hard)
- Unlockable card art (cosmetic rewards)
- Daily challenges (specific win conditions)

---

## 8. Accessibility

### 8.1 Visual Accessibility

**Color-Blind Considerations**:
- **Icons Supplement Colors**:
  - Rock: Brown + boulder icon
  - Paper: White + sheet icon
  - Scissors: Blue + blade icon
- **Result Indicators**:
  - Win: Green + "YOU WIN!" text + upward arrow
  - Lose: Red + "YOU LOSE!" text + downward arrow
  - Draw: Orange + "DRAW!" text + equals sign
- **Patterns on Cards** (Future):
  - Rock: Dotted texture
  - Paper: Lined texture
  - Scissors: Crosshatch texture

**Contrast Ratios**:
- All text meets WCAG AA standards (4.5:1 minimum)
- Heading text on dark background: White (#F5F5F5) on Deep Blue (#1E3A5F) = 9.2:1 ✓
- Body text: White (#F5F5F5) on Dark Charcoal (#2C2C2C) = 12.6:1 ✓
- Button text: Dark Charcoal (#2C2C2C) on Accent Gold (#FFD700) = 7.8:1 ✓

**Font Size Options** (Future Enhancement):
- Small: 16px body, 28px heading
- Medium: 18px body, 32px heading (default)
- Large: 22px body, 40px heading
- Adjustable in settings menu

**High Contrast Mode** (Future Enhancement):
- Increase contrast to maximum (black/white only)
- Remove subtle gradients
- Thicker borders (6px instead of 4px)

### 8.2 Audio Accessibility

**Audio Toggles**:
- **Master Volume**: 0-100% slider
- **Music On/Off**: Toggle switch
- **SFX On/Off**: Toggle switch
- Settings accessible from Main Menu (Settings icon)

**Visual-Only Play**:
- Game fully playable without audio
- All feedback has visual counterpart:
  - Win: Green flash (not just sound)
  - Lose: Red flash (not just sound)
  - Draw: Orange flash (not just sound)
  - Hover: Visual highlight (not just sound)
- No audio-only cues

**Captions** (Future Enhancement):
- SFX captions (e.g., "[Click]", "[Victory Fanfare]")
- Optional toggle in settings

### 8.3 Input Accessibility

**Mouse-Only Play** (Current):
- All interactions via mouse clicks
- Hover states for discoverability
- Large click targets (minimum 44x44px per WCAG)

**Keyboard Shortcuts** (Future Enhancement):
- **Gameplay**:
  - 1 or R: Select Rock
  - 2 or P: Select Paper
  - 3 or S: Select Scissors
  - Enter: Confirm (if needed)
- **Navigation**:
  - Tab: Cycle through buttons
  - Enter: Click focused button
  - Escape: Back / Main Menu
- **Settings**:
  - Remappable keys (future)

**Touch Targets** (Future Mobile Version):
- Minimum 44x44px (WCAG guideline)
- Generous spacing between buttons (20px+)
- No small, precise clicking required

**Controller Support** (Future Enhancement):
- D-pad: Navigate buttons/cards
- A button: Select/Confirm
- B button: Back/Cancel
- Vibration feedback (optional)

### 8.4 Cognitive Accessibility

**Clear Instructions**:
- Instruction text on gameplay screen: "Choose Your Card"
- Button labels clear and descriptive: "Play Game", "Main Menu", "Play Again"
- Round indicator: "ROUND 1/3" (clear progress)
- Score always visible: "Player: 2 - CPU: 1"

**Consistent UI Patterns**:
- Buttons always in same locations (bottom-center)
- Back button always top-left
- Score always top-center
- Predictable navigation flow

**No Time Pressure**:
- Player can take as long as needed to choose
- No countdown timers
- No turn limits
- Game is player-paced (not real-time)

**Forgiving Design**:
- No "game over" until HP = 0 (Survival) or match lost
- Multiple chances per match (best of 3)
- "Play Again" always available (no permanent loss)
- Can quit anytime (Main Menu button)

**Reduced Cognitive Load**:
- Simple rules (universally known game)
- Only 3 choices (not overwhelming)
- Clear win/lose indicators
- Minimal UI clutter

**Dyslexia-Friendly** (Considerations):
- Font: Nunito (sans-serif, rounded, readable)
- Line spacing: 1.5x (generous)
- Text alignment: Left-aligned (not justified)
- Font size: 18px+ (large enough)
- Avoid italics (harder to read)

---

## 9. Asset List

### 9.1 Graphics Assets

#### Card Artwork (Primary Assets)

| File Name | Description | Dimensions | Format | States | Priority |
|-----------|-------------|------------|--------|--------|----------|
| `rock_card.png` | Rock character art | 512x512 | PNG-24 | Normal, Grayscale | **CRITICAL** |
| `paper_card.png` | Paper character art | 512x512 | PNG-24 | Normal, Grayscale | **CRITICAL** |
| `scissors_card.png` | Scissors character art | 512x512 | PNG-24 | Normal, Grayscale | **CRITICAL** |
| `card_back.png` | Hidden card back | 512x512 | PNG-24 | Single | High |

#### UI Elements

| File Name | Description | Dimensions | Format | Notes | Priority |
|-----------|-------------|------------|--------|-------|----------|
| `button_normal.png` | Button normal state | 256x64 | PNG-24 | 9-slice scalable | **CRITICAL** |
| `button_hover.png` | Button hover state | 256x64 | PNG-24 | 9-slice scalable | **CRITICAL** |
| `button_pressed.png` | Button pressed state | 256x64 | PNG-24 | 9-slice scalable | **CRITICAL** |
| `button_disabled.png` | Button disabled state | 256x64 | PNG-24 | 9-slice scalable | High |
| `panel_bg.png` | UI panel background | 512x512 | PNG-24 | Tileable | High |
| `card_frame.png` | Card border/frame overlay | 512x512 | PNG-24 | Transparent | Medium |
| `hp_bar_fill.png` | HP bar fill (green gradient) | 256x32 | PNG-24 | Survival mode | High |
| `hp_bar_bg.png` | HP bar background | 256x32 | PNG-24 | Survival mode | High |

#### Icons

| File Name | Description | Dimensions | Format | Usage | Priority |
|-----------|-------------|------------|--------|-------|----------|
| `icon_rock.png` | Small Rock icon | 64x64 | PNG-24 | UI, scores | **CRITICAL** |
| `icon_paper.png` | Small Paper icon | 64x64 | PNG-24 | UI, scores | **CRITICAL** |
| `icon_scissors.png` | Small Scissors icon | 64x64 | PNG-24 | UI, scores | **CRITICAL** |
| `icon_settings.png` | Settings gear | 32x32 | PNG-24 | Menu | Medium |
| `icon_back.png` | Back arrow | 32x32 | PNG-24 | Navigation | High |

#### Backgrounds

| File Name | Description | Dimensions | Format | Compression | Priority |
|-----------|-------------|------------|--------|-------------|----------|
| `bg_main_menu.jpg` | Main menu background | 1920x1080 | JPEG | 80% quality | High |
| `bg_game.jpg` | Game scene background | 1920x1080 | JPEG | 80% quality | **CRITICAL** |
| `bg_result.jpg` | Result screen background | 1920x1080 | JPEG | 80% quality | Medium |

#### Logo & Branding

| File Name | Description | Dimensions | Format | Notes | Priority |
|-----------|-------------|------------|--------|-------|----------|
| `logo.png` | Game logo / title | 512x128 | PNG-24 | Transparent BG | **CRITICAL** |
| `logo_icon.png` | App icon (square) | 256x256 | PNG-24 | For desktop icon | High |

#### Tournament Mode

| File Name | Description | Dimensions | Format | Notes | Priority |
|-----------|-------------|------------|--------|-------|----------|
| `bracket_node.png` | Bracket match node | 128x64 | PNG-24 | Tournament | Medium |
| `bracket_line.png` | Bracket connector line | Varies | PNG-24 | Tournament | Medium |

### 9.2 Audio Assets

#### Sound Effects (SFX)

| File Name | Description | Duration | Format | Bit Rate | Priority |
|-----------|-------------|----------|--------|----------|----------|
| `button_hover.wav` | Button hover tick | 0.05s | WAV | 16-bit, 44.1kHz | Medium |
| `button_click.wav` | Button click | 0.1s | WAV | 16-bit, 44.1kHz | **CRITICAL** |
| `choice_made.wav` | Player selects card | 0.2s | WAV | 16-bit, 44.1kHz | **CRITICAL** |
| `choice_reveal.wav` | AI card revealed | 0.3s | WAV | 16-bit, 44.1kHz | **CRITICAL** |
| `win_round.wav` | Player wins round | 0.5s | WAV | 16-bit, 44.1kHz | **CRITICAL** |
| `lose_round.wav` | Player loses round | 0.5s | WAV | 16-bit, 44.1kHz | **CRITICAL** |
| `draw_round.wav` | Round is a draw | 0.3s | WAV | 16-bit, 44.1kHz | High |
| `win_game.wav` | Player wins match | 2.0s | WAV | 16-bit, 44.1kHz | **CRITICAL** |
| `lose_game.wav` | Player loses match | 1.5s | WAV | 16-bit, 44.1kHz | High |
| `hp_loss.wav` | HP decreases (Survival) | 0.4s | WAV | 16-bit, 44.1kHz | High |
| `high_score.wav` | New high score (Survival) | 1.0s | WAV | 16-bit, 44.1kHz | Medium |
| `ready.wav` | Ready button (Multiplayer) | 0.2s | WAV | 16-bit, 44.1kHz | High |

#### Background Music

| File Name | Description | Duration | Format | Loop | Priority |
|-----------|-------------|----------|--------|------|----------|
| `menu_theme.ogg` | Main menu music | 2:30 | OGG Vorbis | Yes | High |
| `game_theme.ogg` | Gameplay music | 3:00 | OGG Vorbis | Yes | **CRITICAL** |
| `victory_theme.ogg` | Win result music | 0:30 | OGG Vorbis | No | High |
| `defeat_theme.ogg` | Lose result music | 0:30 | OGG Vorbis | No | Medium |

**Total Audio File Count**: 16 files (12 SFX + 4 music tracks)

### 9.3 Font Assets

| Font Name | File Name | Weights | Format | License | Priority |
|-----------|-----------|---------|--------|---------|----------|
| Nunito | `Nunito-Regular.ttf` | 400 (Regular) | TTF | Open Font License | **CRITICAL** |
| Nunito | `Nunito-SemiBold.ttf` | 600 (SemiBold) | TTF | Open Font License | High |
| Nunito | `Nunito-Bold.ttf` | 700 (Bold) | TTF | Open Font License | **CRITICAL** |

**Font Source**: Google Fonts (https://fonts.google.com/specimen/Nunito)
**Fallback Font**: System sans-serif

### 9.4 Asset Summary

**Total Asset Count**:
- Graphics: 24 files (cards, UI, icons, backgrounds, logo, tournament)
- Audio: 16 files (12 SFX + 4 music)
- Fonts: 3 files (Regular, SemiBold, Bold)
- **Grand Total**: 43 assets

**Asset Priority Breakdown**:
- **CRITICAL** (Must-Have for MVP): 13 assets
- **High** (Important, should include): 17 assets
- **Medium** (Nice-to-Have, can defer): 13 assets

**Estimated Asset Creation Time** (Rough):
- Graphics: 3-5 days (assuming artist available)
- Audio: 2-3 days (SFX + music composition/sourcing)
- Fonts: 0 days (download from Google Fonts)
- **Total**: ~5-8 days for full asset pipeline

**Asset Placeholders** (For Development):
- Use colored rectangles with text labels for cards
- Use Godot's default UI theme for buttons
- Use royalty-free placeholder music (or silence)
- Use simple beep/blip sounds (or silence)

---

## 10. Implementation Notes

### 10.1 Animation Specifications

**Animation Library**:

| Animation Name | Target | Properties Animated | Duration | Easing | Trigger |
|----------------|--------|---------------------|----------|--------|---------|
| `button_hover` | Button | Scale (1.0 → 1.05) | 0.1s | ease-in-out | Mouse enter |
| `button_press` | Button | Scale (1.0 → 0.95) | 0.05s | ease-out | Mouse down |
| `card_hover` | Card | Scale (1.0 → 1.1), Rotation (0° → -3°), Glow (0 → 5px) | 0.15s | ease-out | Mouse enter |
| `card_select` | Card | Scale (1.0 → 1.05), Glow (5px → 8px gold) | 0.2s | elastic | Click |
| `card_dim` | Card | Opacity (1.0 → 0.6), Brightness (1.0 → 0.5) | 0.2s | ease-out | Other card selected |
| `card_reveal` | Card | Position (Y: +100 → 0), Scale (1.2 → 1.0) | 0.5s | ease-out | AI choice reveal |
| `result_text` | Label | Scale (0.0 → 1.0), Opacity (0.0 → 1.0) | 0.5s | elastic | Result determined |
| `score_pulse` | Label | Scale (1.0 → 1.1 → 1.0) | 0.3s | ease-in-out | Score change |
| `hp_flash` | HPBar | Color (normal → red → normal) | 0.4s | ease-in-out | HP loss |
| `fade_transition` | Scene | Opacity (0.0 → 1.0 → 0.0) | 0.5s | linear | Scene change |
| `card_enter` | Card | Position (Y: +100 → 0), Opacity (0.0 → 1.0) | 0.3s | ease-out | Round start |

**Godot Implementation**:
- Use `Tween` nodes for all animations
- `tween.tween_property(node, "scale", Vector2(1.1, 1.1), 0.15).set_ease(Tween.EASE_OUT)`
- Chain tweens with `.chain()` for sequential animations
- Use signals to detect animation completion: `tween.finished.connect()`

**Easing Functions**:
- `ease-in-out`: `Tween.EASE_IN_OUT`
- `ease-out`: `Tween.EASE_OUT`
- `elastic`: `Tween.TRANS_ELASTIC` (bounce effect)
- `linear`: `Tween.TRANS_LINEAR`

### 10.2 Timing Values

**Comprehensive Timing Reference**:

| Context | Timing | Value | Notes |
|---------|--------|-------|-------|
| **Button Interactions** | | | |
| Button hover transition | 0.1s | 100ms | Smooth, responsive |
| Button press transition | 0.05s | 50ms | Instant feedback |
| **Card Interactions** | | | |
| Card hover transition | 0.15s | 150ms | Engaging lift |
| Card select transition | 0.2s | 200ms | Satisfying bounce |
| Card dim transition | 0.2s | 200ms | Smooth fade |
| Card reveal animation | 0.5s | 500ms | Dramatic entrance |
| Card enter (round start) | 0.3s | 300ms | Staggered +0.1s each |
| **Result Display** | | | |
| Result text appearance | 0.5s | 500ms | Elastic bounce |
| Score update count-up | 0.3s | 300ms | Linear interpolation |
| Score pulse | 0.2s | 200ms | Brief emphasis |
| Result pause (before next) | 3.0s | 3000ms | Player reads result |
| **AI Behavior** | | | |
| AI "thinking" delay | 0.5-1.0s | 500-1000ms | Random, feels natural |
| AI choice reveal delay | 0.5s | 500ms | After player choice |
| **Scene Transitions** | | | |
| Fade out duration | 0.25s | 250ms | Half of total |
| Fade in duration | 0.25s | 250ms | Half of total |
| Total scene transition | 0.5s | 500ms | Smooth, not jarring |
| **Multiplayer Transitions** | | | |
| Transition screen fade in | 0.3s | 300ms | Quick, clear |
| Transition screen fade out | 0.3s | 300ms | After Ready click |
| **Survival Mode** | | | |
| HP flash animation | 0.4s | 400ms | Noticeable warning |
| HP low pulse loop | 0.8s | 800ms | Continuous loop |
| **Game Flow** | | | |
| Round result display time | 2.0s | 2000ms | Before auto-proceed |
| Match complete wait time | 2.0s | 2000ms | Before result screen |

### 10.3 Magic Numbers

**Constants to Extract** (for easy tuning):

```csharp
// Timing Constants
public static class GameTimings {
    public const float AI_THINKING_MIN = 0.5f;       // seconds
    public const float AI_THINKING_MAX = 1.0f;       // seconds
    public const float REVEAL_DELAY = 0.5f;          // seconds
    public const float RESULT_DISPLAY_TIME = 2.0f;   // seconds
    public const float ROUND_PAUSE_TIME = 3.0f;      // seconds
    public const float SCENE_TRANSITION = 0.5f;      // seconds
}

// Animation Constants
public static class AnimationDurations {
    public const float BUTTON_HOVER = 0.1f;          // seconds
    public const float BUTTON_PRESS = 0.05f;         // seconds
    public const float CARD_HOVER = 0.15f;           // seconds
    public const float CARD_SELECT = 0.2f;           // seconds
    public const float CARD_REVEAL = 0.5f;           // seconds
    public const float RESULT_TEXT = 0.5f;           // seconds
    public const float SCORE_PULSE = 0.3f;           // seconds
}

// Scale Constants
public static class ScaleValues {
    public const float BUTTON_HOVER_SCALE = 1.05f;
    public const float BUTTON_PRESS_SCALE = 0.95f;
    public const float CARD_HOVER_SCALE = 1.10f;
    public const float CARD_SELECT_SCALE = 1.05f;
    public const float CARD_REVEAL_SCALE = 1.20f;
    public const float CARD_DISABLED_OPACITY = 0.6f;
    public const float CARD_TILT_ANGLE = -3.0f;      // degrees
}

// Win Conditions
public static class WinConditions {
    public const int BEST_OF_THREE = 2;              // first to 2 wins
    public const int SURVIVAL_STARTING_HP = 10;
    public const int TOURNAMENT_SIZE_SMALL = 8;
    public const int TOURNAMENT_SIZE_LARGE = 16;
}

// Audio Constants
public static class AudioVolumes {
    public const float MASTER_VOLUME = 1.0f;         // 100%
    public const float SFX_VOLUME = 0.8f;            // 80%
    public const float UI_SOUNDS = 0.6f;             // 60%
    public const float RESULT_SOUNDS = 0.9f;         // 90%
    public const float MUSIC_VOLUME = 0.7f;          // 70%
    public const float MUSIC_DUCKING = 0.6f;         // 60% when SFX plays
}
```

**Rationale**:
- Centralizes all tunable values
- Easy to adjust during playtesting
- Avoids hardcoded magic numbers in code
- Improves code readability

---

## 11. Future Enhancements

**Post-MVP Features** (Out of Scope for v1.0):

### 11.1 Additional Game Modes

- **Campaign Mode**: Story-driven progression with boss battles
- **Challenge Mode**: Daily/weekly challenges with specific win conditions
- **Ranked Mode**: Online matchmaking with ELO rating (requires online)

### 11.2 Advanced Gameplay

- **Multiple Choices**: Expand beyond Rock-Paper-Scissors (add Lizard, Spock, etc.)
- **Choice Durability**: Cards have HP, lose HP when used
- **Deck Building**: Build custom deck of choices
- **Hand Selection**: Draw random hand from deck each round
- **Power-Ups**: Temporary boosts (double damage, shield, etc.)
- **Special Abilities**: Unique effects per choice (Rock stuns, Paper blocks, etc.)

### 11.3 AI Enhancements

- **Difficulty Levels**: Easy (random), Medium (pattern detection), Hard (predictive)
- **AI Personalities**: Different AI types with unique behaviors
- **Adaptive AI**: Learns player's patterns over time

### 11.4 Progression Systems

- **Persistent High Scores**: Saved across sessions
- **Player Stats**: Total wins, win rate, favorite choice, play time
- **Achievements**: Unlock badges for milestones ("Win 10 games", "Perfect Tournament")
- **Unlockable Content**: New card artwork, backgrounds, music tracks

### 11.5 Visual Enhancements

- **Advanced Animations**:
  - Card flip animations (3D rotation)
  - Particle effects (confetti on win, sparks on impact)
  - Per-choice victory animations (Rock smashes, Scissors cut, Paper wraps)
  - Camera shake on dramatic moments
- **Custom Themes**: Different visual styles (Dark Mode, Retro, Minimalist)
- **Animated Card Art**: Subtle idle animations on cards

### 11.6 Audio Enhancements

- **Dynamic Music**: Music intensity increases with tension (tied rounds)
- **Voice Acting**: Character voices for choices (optional)
- **Announcer**: "Round 1, Fight!" / "Player wins!" (fighting game style)

### 11.7 Multiplayer Enhancements

- **Online Multiplayer**:
  - Real-time 1v1 matches
  - Matchmaking system
  - Lobby system (create/join games)
  - Friend list
- **Spectator Mode**: Watch other players' matches
- **Replays**: Save and rewatch matches

### 11.8 Accessibility Enhancements

- **Full Keyboard Support**: Navigate entire game with keyboard
- **Controller Support**: Play with gamepad
- **Screen Reader**: Text-to-speech for visually impaired
- **Color-Blind Modes**: Multiple preset palettes
- **High Contrast Mode**: Maximum contrast option
- **Adjustable Text Size**: Small, Medium, Large, Extra Large
- **Captions**: Visual captions for all SFX

### 11.9 Settings & Options

- **Settings Menu**:
  - Volume controls (Master, Music, SFX)
  - Graphics quality (Low, Medium, High)
  - Screen resolution / fullscreen toggle
  - V-Sync toggle
  - FPS counter
  - Language selection (internationalization)
- **Keybind Remapping**: Customize keyboard shortcuts

### 11.10 Platform Expansion

- **Mobile Ports**: Android and iOS versions
- **Web Version**: HTML5 export (play in browser)
- **Console Ports**: Nintendo Switch, Xbox, PlayStation (future)

### 11.11 Social Features

- **Leaderboards**: Global and friend leaderboards
- **Share Results**: Screenshot and share win screens
- **Friend Invites**: Invite friends to play
- **Tournaments**: Player-organized tournaments

### 11.12 Monetization (If Applicable)

- **Cosmetic DLC**: Purchasable card art packs
- **Battle Pass**: Seasonal progression system
- **Free-to-Play Model**: Base game free, cosmetics purchasable
- **Ad-Supported**: Optional ad viewing for rewards

---

## Appendix A: Development Checklist

**Phase 3 Completion Checklist**:

- [x] Visual design defined (art style, color palette, typography)
- [x] All screen layouts designed (Main Menu, Mode Selection, all game modes, Result)
- [x] UI components specified (buttons, cards, score, HP bar, bracket)
- [x] User interactions documented (flows, animations, feedback)
- [x] Audio design complete (SFX list, music list, volumes)
- [x] Game mechanics detailed (rules, round flow, win conditions, AI behavior)
- [x] Game flow documented (state diagram, scene transitions)
- [x] Player experience designed (FTUE, emotional journey, replay value)
- [x] Accessibility considerations addressed (visual, audio, input, cognitive)
- [x] Complete asset list created (graphics, audio, fonts)
- [x] Animation specifications defined (all animations, timings, easing)
- [x] Magic numbers extracted (constants for easy tuning)
- [x] Future enhancements documented (out of scope items)

---

**End of Game Design Document**

**Next Phase**: TDD Development (Phase 4)
**Prompt**: `spr/prompts/game-dev/04-tdd-development-prompt.md`
**Status**: Ready for implementation with comprehensive design specification
