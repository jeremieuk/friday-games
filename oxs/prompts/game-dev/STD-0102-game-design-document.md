# Game Design Document Creation Prompt

## Overview

This prompt guides the creation of a comprehensive Game Design Document (GDD) for a Godot game project, translating requirements and architecture into detailed game design specifications covering mechanics, UI, content, progression, and assets.

---

## Prompt Template

```xml
# GAME DESIGN DOCUMENT CREATION

<context>
  <project>Godot Game Development - Game Design Phase</project>
  <role>You are an experienced game designer who creates comprehensive, actionable Game Design Documents</role>
  <objective>
    Create a complete Game Design Document that bridges requirements and architecture
    with detailed game design specifications. The GDD must provide clear guidance
    for developers implementing the game using TDD, specifying exact mechanics,
    UI layouts, content, and asset requirements.
  </objective>
</context>

<foundational_principles>
1. **Player-Centric Design**: Every design decision serves player experience
2. **Implementation-Ready**: Specifications must be clear enough to implement directly
3. **Consistency**: Maintain consistent terminology and concepts throughout
4. **Traceability**: Link design elements back to requirements
5. **Visual Communication**: Use diagrams, mockups, and examples liberally
6. **Testable Design**: Specify behaviors in ways that can be verified through testing
7. **Iterative Refinement**: GDD evolves as design is validated through implementation
</foundational_principles>

<context_compaction_survival>
  <critical_warning>
  GDD creation may span multiple sessions and involve numerous sections.
  You MUST track progress to avoid repeating work if context is compacted.
  </critical_warning>

  <work_tracking_directory>
    <path>docs/gdd/.work/</path>
    <purpose>Persistent state for GDD creation progress</purpose>
    <critical>Create this directory FIRST before any GDD work</critical>

    <required_files>
      <file name="progress.yaml">
        <purpose>Track which GDD sections are complete</purpose>
        <updated>After EVERY section completion</updated>
        <critical>Your resumption lifeline across sessions</critical>
      </file>

      <file name="section-checklist.yaml">
        <purpose>Inventory of all GDD sections and their status</purpose>
        <created>At start of GDD work</created>
        <updated>As sections are completed</updated>
      </file>

      <file name="design-decisions.yaml">
        <purpose>Record key design decisions and rationale</purpose>
        <created>Throughout GDD creation</created>
        <format>Decision log with context and alternatives considered</format>
      </file>
    </required_files>
  </work_tracking_directory>

  <progress_tracking_schema>
```yaml
# docs/gdd/.work/progress.yaml - UPDATE AFTER EVERY SECTION
progress:
  last_updated: "[ISO DateTime]"
  current_section: "[Section name]"
  status: "In Progress | Complete"

  sections:
    executive_summary:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"
    core_mechanics:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"
    game_flow:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"
    ui_ux_design:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"
    content_specification:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"
    progression_systems:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"
    asset_requirements:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"
    technical_design:
      status: "Not Started | In Progress | Complete"
      completed_at: "[DateTime if complete]"

  work_completed:
    - section: "[Section name]"
      completed_at: "[DateTime]"

  work_in_progress:
    - section: "[Current section]"
      status: "[Progress within section]"

  work_remaining:
    - "[List of pending sections]"

  next_action: "Continue [section] OR Start [next section] OR Finalize GDD"
```
  </progress_tracking_schema>

  <resumption_protocol>
  WHEN RESUMING WORK:

  1. IMMEDIATELY check for existing progress:
     ```bash
     cat docs/gdd/.work/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
     ```

  2. IF progress file exists:
     - Read current_section and next_action
     - Check which sections are complete
     - Resume from next_action
     - Do NOT restart from beginning
     - Read existing GDD.md to understand what's done

  3. IF no progress file (fresh start):
     - Create docs/gdd/.work/ directory
     - Initialize progress.yaml
     - Begin with prerequisites (load requirements and architecture)

  4. After completing each section:
     - Update progress.yaml immediately
     - Append section to docs/gdd/GDD.md
     - Document next_action clearly
  </resumption_protocol>

  <compaction_safe_practices>
    <practice>Write progress.yaml after EVERY section completion</practice>
    <practice>Build GDD incrementally - write each section as completed</practice>
    <practice>Reference requirements and architecture docs, don't duplicate</practice>
    <practice>Complete one section fully before starting another</practice>
    <practice>Update next_action with specific section to work on next</practice>
  </compaction_safe_practices>
</context_compaction_survival>

<methodology>
  <phase id="0" name="Prerequisites">
    <purpose>Load and understand inputs</purpose>
    <steps>
      <step>Read docs/requirements/REQUIREMENTS.md</step>
      <step>Read docs/architecture/ARCHITECTURE.md</step>
      <step>Create docs/gdd/.work/ directory</step>
      <step>Initialize progress.yaml and section-checklist.yaml</step>
      <step>Create GDD.md with title and table of contents</step>
    </steps>
    <output>
      - docs/gdd/.work/ directory structure
      - docs/gdd/GDD.md (initial with TOC)
    </output>
  </phase>

  <phase id="1" name="Executive Summary">
    <purpose>High-level overview of the game</purpose>
    <content>
      - Game concept (2-3 paragraphs)
      - Target audience
      - Unique selling points
      - Platform and technical summary
      - Development approach (TDD)
    </content>
    <output>Executive Summary section in GDD.md</output>
  </phase>

  <phase id="2" name="Core Mechanics">
    <purpose>Detailed specification of game mechanics</purpose>
    <content>
      - Primary gameplay loop
      - Player actions and controls
      - Game rules (win conditions, lose conditions)
      - Game state and state transitions
      - Physics and collision (if applicable)
      - Examples and edge cases
    </content>
    <format>
      For each mechanic:
      - **Name**: Clear identifier
      - **Description**: What it does
      - **Player Interaction**: How player engages
      - **Rules**: Precise algorithmic specification
      - **Examples**: Concrete scenarios
      - **Edge Cases**: Boundary conditions
      - **Tests**: How to verify behavior
    </format>
    <output>Core Mechanics section in GDD.md</output>
  </phase>

  <phase id="3" name="Game Flow">
    <purpose>Define the flow between game states and screens</purpose>
    <content>
      - State diagram (Main Menu → Game → Game Over → etc.)
      - Scene transitions
      - Pause/resume behavior
      - Win/lose flows
      - Restart/retry flows
    </content>
    <format>
      - Visual flowchart (ASCII or description for diagram)
      - Transition triggers
      - Data that persists across transitions
    </format>
    <output>Game Flow section in GDD.md</output>
  </phase>

  <phase id="4" name="UI/UX Design">
    <purpose>Specify user interface and user experience</purpose>
    <content>
      For each screen/scene:
      - **Screen Name**: Identifier
      - **Purpose**: What player does here
      - **Layout**: ASCII mockup or detailed description
      - **Elements**: List all UI elements (buttons, labels, etc.)
      - **Interactions**: Click/touch behaviors
      - **Feedback**: Visual/audio responses
      - **Accessibility**: Considerations for usability
    </content>
    <screens>
      - Main Menu
      - Game Screen
      - Pause Menu
      - Game Over Screen
      - Settings Screen
      - [Any others from requirements]
    </screens>
    <output>UI/UX Design section in GDD.md</output>
  </phase>

  <phase id="5" name="Content Specification">
    <purpose>Define all game content</purpose>
    <content>
      - Levels/stages (if applicable)
      - Characters/entities
      - Items/pickups (if applicable)
      - Dialog/text content
      - Tutorial content
    </content>
    <format>
      For each content type:
      - **Quantity**: How many needed
      - **Specifications**: Detailed requirements
      - **Progression**: How content unlocks/appears
      - **References**: Link to architecture (which scenes/scripts)
    </format>
    <output>Content Specification section in GDD.md</output>
  </phase>

  <phase id="6" name="Progression Systems">
    <purpose>Define how game progresses and scales</purpose>
    <content>
      - Difficulty progression
      - Scoring system (if applicable)
      - Achievements/unlocks (if applicable)
      - Player progression (if applicable)
      - Replayability features
    </content>
    <output>Progression Systems section in GDD.md</output>
  </phase>

  <phase id="7" name="Asset Requirements">
    <purpose>Enumerate all assets needed</purpose>
    <categories>
      <category name="Visual Assets">
        - Sprites/textures
        - UI elements
        - Animations
        - Fonts
        - Particles/effects
      </category>
      <category name="Audio Assets">
        - Music tracks
        - Sound effects
        - UI sounds
      </category>
      <category name="Data Assets">
        - Configuration files
        - Level data
        - Localization strings (if applicable)
      </category>
    </categories>
    <format>
      For each asset:
      - **Asset ID**: Unique identifier
      - **Type**: Sprite, sound, etc.
      - **Specifications**: Size, format, duration, etc.
      - **Purpose**: Where/how used
      - **Source**: Created, purchased, placeholder
      - **Status**: Not Started | In Progress | Complete
    </format>
    <output>
      - Asset Requirements section in GDD.md
      - docs/gdd/.work/asset-inventory.yaml
    </output>
  </phase>

  <phase id="8" name="Technical Design">
    <purpose>Technical implementation guidance</purpose>
    <content>
      - Reference to architecture document
      - Scene-to-GDD mapping (which scenes implement which mechanics)
      - Script responsibilities summary
      - Data flow for key mechanics
      - Performance targets (from NFRs)
      - Testing approach (reference to architecture)
    </content>
    <note>
      This section bridges GDD to architecture, helping developers
      understand how design maps to implementation.
    </note>
    <output>Technical Design section in GDD.md</output>
  </phase>

  <phase id="9" name="Finalization">
    <purpose>Review, polish, and complete GDD</purpose>
    <steps>
      <step>Review all sections for completeness</step>
      <step>Check traceability to requirements</step>
      <step>Verify consistency of terminology</step>
      <step>Add any missing diagrams or examples</step>
      <step>Create version history section</step>
      <step>Add glossary if needed</step>
      <step>Update progress.yaml to "Complete"</step>
    </steps>
    <output>Final docs/gdd/GDD.md</output>
  </phase>
</methodology>

<output_specifications>
  <directory_structure>
    docs/
    └── gdd/
        ├── .work/                      # Progress tracking
        │   ├── progress.yaml
        │   ├── section-checklist.yaml
        │   ├── design-decisions.yaml
        │   └── asset-inventory.yaml
        └── GDD.md                      # Master Game Design Document
  </directory_structure>

  <gdd_template>
```markdown
# Game Design Document

**Game Title**: [Name]
**Version**: 1.0
**Date**: [YYYY-MM-DD]
**Author(s)**: [Names]
**Godot Version**: 4.x

---

## Table of Contents
1. [Executive Summary](#executive-summary)
2. [Core Mechanics](#core-mechanics)
3. [Game Flow](#game-flow)
4. [UI/UX Design](#ui-ux-design)
5. [Content Specification](#content-specification)
6. [Progression Systems](#progression-systems)
7. [Asset Requirements](#asset-requirements)
8. [Technical Design](#technical-design)
9. [Appendices](#appendices)

---

## 1. Executive Summary

### Game Concept
[2-3 paragraph description of the game]

### Target Audience
- **Primary**: [Who]
- **Secondary**: [Who]
- **Age Range**: [Range]
- **Player Count**: [Single/Multi]

### Unique Selling Points
1. [USP 1]
2. [USP 2]
3. [USP 3]

### Platform
- **Target Platforms**: [PC, Mobile, Web, etc.]
- **Engine**: Godot 4.x
- **Language**: [GDScript/C#]

### Development Approach
- **Methodology**: Test-Driven Development (TDD)
- **Architecture**: [Reference docs/architecture/ARCHITECTURE.md]

---

## 2. Core Mechanics

### Mechanic 1: [Name]

**Description**: [What this mechanic does]

**Player Interaction**: [How player engages with this]

**Rules**:
1. [Rule 1 - be algorithmic/precise]
2. [Rule 2]
3. [Rule 3]

**State Changes**:
- [What changes in game state]

**Examples**:
- **Example 1**: [Concrete scenario]
- **Example 2**: [Another scenario]

**Edge Cases**:
- [Edge case 1 and expected behavior]
- [Edge case 2 and expected behavior]

**Testing**:
- [ ] Test case 1
- [ ] Test case 2

### Mechanic 2: [Name]
[Repeat pattern...]

---

## 3. Game Flow

### State Diagram

```
[Splash Screen]
      ↓
[Main Menu] ←─────┐
      ↓           │
[Game] ──────→ [Pause Menu]
      ↓           │
[Game Over] ──────┘
      ↓
[Main Menu]
```

### Scene Transitions

#### Main Menu → Game
- **Trigger**: Player clicks "Play" button
- **Transition**: [Fade, instant, etc.]
- **Data Initialized**: [Game state, board, etc.]

[Continue for each transition...]

### Pause/Resume
- **Pause Trigger**: [ESC key, pause button]
- **Paused State Behavior**: [What's frozen, what's active]
- **Resume**: [How game resumes]

---

## 4. UI/UX Design

### Main Menu Screen

**Purpose**: Entry point, navigation to game modes and settings

**Layout**:
```
┌─────────────────────────────────┐
│                                 │
│         [Game Title]            │
│                                 │
│         [Play Button]           │
│       [Settings Button]         │
│         [Quit Button]           │
│                                 │
│      v1.0  |  [Audio Icon]      │
└─────────────────────────────────┘
```

**Elements**:
1. **Game Title**
   - Type: Label
   - Font: [Specify]
   - Size: [Size]
   - Position: [Center top]

2. **Play Button**
   - Type: Button
   - Size: [WxH]
   - Position: [Center]
   - Text: "Play"
   - On Click: Transition to Game scene
   - Hover Effect: [Describe]

[Continue for each element...]

**Interactions**:
- Mouse/touch on button: [Hover effect]
- Click: [Action + feedback]

**Accessibility**:
- Keyboard navigation: Tab through buttons
- High contrast mode: [If supported]

### Game Screen

[Repeat pattern for each screen...]

---

## 5. Content Specification

### [Content Type]: [Name]

**Quantity**: [How many]

**Specifications**:
- [Spec 1]
- [Spec 2]

**Details**:
[Detailed description or table]

[Repeat for each content type...]

---

## 6. Progression Systems

### [System Name]

**Description**: [What this system does]

**Mechanics**: [How it works]

**Progression Curve**: [How difficulty/rewards scale]

[Repeat for each system...]

---

## 7. Asset Requirements

### Visual Assets

| Asset ID | Type | Specifications | Purpose | Status |
|----------|------|----------------|---------|--------|
| SPR-001  | Sprite | 64x64px, PNG | Player | Not Started |
| SPR-002  | Sprite | 32x32px, PNG | Enemy | Not Started |

### Audio Assets

| Asset ID | Type | Specifications | Purpose | Status |
|----------|------|----------------|---------|--------|
| SFX-001  | Sound | MP3/OGG, <100KB | Button click | Not Started |
| MUS-001  | Music | MP3/OGG, 1-2min loop | Main menu | Not Started |

### Data Assets

| Asset ID | Type | Format | Purpose | Status |
|----------|------|--------|---------|--------|
| DAT-001  | Config | JSON | Game settings | Not Started |

---

## 8. Technical Design

### Architecture Reference
See: `docs/architecture/ARCHITECTURE.md`

### Scene-to-Mechanic Mapping

| Mechanic | Implemented In | Scripts |
|----------|---------------|---------|
| [Mechanic 1] | Game.tscn | Board.gd, Player.gd |
| [Mechanic 2] | Game.tscn | GameLogic.gd |

### Key Data Flows

#### [Flow Name]
1. [Step 1: Actor → Action → Data change]
2. [Step 2: Propagation]
3. [Step 3: UI update]

### Performance Targets
(From Requirements NFRs)
- Target FPS: [60]
- Max Load Time: [2 seconds]

### Testing Approach
(Reference architecture testing strategy)
- Unit tests for game logic
- Integration tests for scenes
- Manual testing for feel/polish

---

## 9. Appendices

### A. Glossary
- **Term 1**: Definition
- **Term 2**: Definition

### B. References
- Requirements: `docs/requirements/REQUIREMENTS.md`
- Architecture: `docs/architecture/ARCHITECTURE.md`
- Godot Docs: https://docs.godotengine.org

### C. Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | [Date] | Initial GDD |

### D. Design Decisions

#### Decision 1: [Title]
- **Context**: [Why this decision was needed]
- **Options Considered**:
  1. [Option 1] - [Pros/Cons]
  2. [Option 2] - [Pros/Cons]
- **Decision**: [What was chosen]
- **Rationale**: [Why]

[Continue for significant decisions...]
```
  </gdd_template>
</output_specifications>

<design_best_practices>
  <practice name="Be Specific">
    "Player clicks X" not "Player interacts"
    "Board is 3x3 grid" not "Board is a grid"
  </practice>

  <practice name="Use Examples">
    For every mechanic, provide concrete examples of play
  </practice>

  <practice name="Specify Edge Cases">
    "What happens if..." - answer these proactively
  </practice>

  <practice name="Visual Communication">
    ASCII mockups, diagrams, flowcharts - show, don't just tell
  </practice>

  <practice name="Testable Specifications">
    Write mechanics so they can be verified through tests
  </practice>

  <practice name="Consistent Terminology">
    Define terms once, use consistently throughout
  </practice>

  <practice name="Link to Architecture">
    Connect design elements to implementation components
  </practice>
</design_best_practices>

<critical_reminders>
================================================================================
                    CRITICAL REMINDERS
================================================================================

1. **PROGRESS TRACKING IS ESSENTIAL**
   - Update progress.yaml after EVERY section
   - GDD creation spans multiple sections
   - Context may compact between sections
   - next_action must be specific

2. **BUILD GDD INCREMENTALLY**
   - Write each section as completed
   - Don't wait to write everything at once
   - Append to GDD.md after each section
   - Makes resumption seamless

3. **SPECIFICATIONS MUST BE IMPLEMENTABLE**
   - Developers should understand exactly what to build
   - Mechanics should be algorithmic, not vague
   - UI layouts should be precise
   - Asset specs should be actionable

4. **MAINTAIN TRACEABILITY**
   - Every design element traces to requirements
   - Reference architecture for implementation
   - Link related sections within GDD

5. **COMPLETE SECTIONS FULLY**
   - Finish one section before starting another
   - Don't leave sections partially done
   - Update progress immediately upon completion

6. **USE VISUAL AIDS**
   - ASCII diagrams for layouts
   - Flowcharts for flows
   - Tables for asset lists
   - Examples for mechanics

</critical_reminders>

<begin>
=====================================
CRITICAL: CHECK FOR EXISTING PROGRESS
=====================================
GDD creation may have been started previously.

FIRST ACTION - Check for existing progress:
```bash
cat docs/gdd/.work/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

IF progress file exists:
- Read current_section and next_action
- Resume from where you left off
- Read existing GDD.md to see what's done
- Continue with next section

IF no progress file (fresh start):
- Create docs/gdd/.work/ directory
- Initialize progress.yaml
- Proceed with Phase 0 (Prerequisites)

=====================================
BEGIN GDD CREATION
=====================================

PROCESS:
1. Load requirements and architecture documents
2. Create directory structure and progress tracking
3. Work through sections 1-9 systematically
4. Update progress.yaml after EACH section
5. Build GDD.md incrementally
6. Finalize and review

VALIDATION:
- Does GDD cover all requirements?
- Are mechanics specified precisely enough to implement?
- Are UI layouts clear enough to build?
- Are assets enumerated completely?
- Is technical mapping to architecture clear?

OUTPUT:
- Complete docs/gdd/GDD.md
- Supporting .work/ files for tracking
- Clear handoff to TDD development phase

BEGIN NOW by checking for progress, then starting Phase 0.

</begin>
```

---

## Usage Notes

- **Timeframe**: 2-4 hours for typical game (more for complex games)
- **Dependencies**:
  - docs/requirements/REQUIREMENTS.md
  - docs/architecture/ARCHITECTURE.md
- **Output**: Complete GDD.md document
- **Next Phase**: STD-0103-tdd-development.md

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-01-16 | Initial GDD creation prompt for Godot games |

---

## Related Documents

- STD-0001-prompt-creation-rubric.md - Prompt patterns
- STD-0002-csharp-rubric.md - C# standards (if applicable)
- STD-0100-requirements-gathering.md - Requirements phase
- STD-0101-architecture-design.md - Architecture phase
- STD-0103-tdd-development.md - Next phase
