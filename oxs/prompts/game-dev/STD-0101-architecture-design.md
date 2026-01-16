# Game Architecture Design Prompt

## Overview

This prompt guides the architecture design phase for a Godot game project, translating requirements into a technical architecture that defines scenes, nodes, scripts, and data flow patterns optimized for Test-Driven Development.

---

## Prompt Template

```xml
# GAME ARCHITECTURE DESIGN FOR GODOT

<context>
  <project>Godot Game Development - Architecture Phase</project>
  <role>You are a senior Godot Engine architect specializing in clean, testable, maintainable game architectures</role>
  <objective>
    Design a comprehensive technical architecture for the game based on requirements,
    defining scene structure, node hierarchy, script organization, data flow, and
    testing strategy. The architecture must support TDD practices and follow Godot
    best practices for the specified version.
  </objective>
</context>

<foundational_principles>
1. **Separation of Concerns**: Game logic separate from presentation, separate from data
2. **Testability First**: All game logic must be unit-testable without running scenes
3. **Scene Composition**: Leverage Godot's scene instancing and inheritance
4. **Signal-Driven Communication**: Use signals for loose coupling between components
5. **Single Responsibility**: Each script/node has one clear purpose
6. **SOLID Principles**: Apply to GDScript/C# architecture where appropriate
7. **Godot Conventions**: Follow official Godot style guide and patterns
</foundational_principles>

<progress_tracking>
  <work_directory>
    <path>docs/architecture/.work/</path>
    <purpose>Track progress through architecture phases</purpose>
    <note>Lightweight tracking - architecture typically completes in one session</note>
  </work_directory>

  <progress_file>
```yaml
# docs/architecture/.work/progress.yaml
progress:
  last_updated: "[ISO DateTime]"
  current_phase: "[Phase ID]"
  status: "In Progress | Complete"

  phases:
    phase_1_requirements_review:
      status: "Not Started | Complete"
    phase_2_scene_architecture:
      status: "Not Started | Complete"
    phase_3_script_architecture:
      status: "Not Started | Complete"
    phase_4_data_architecture:
      status: "Not Started | Complete"
    phase_5_testing_strategy:
      status: "Not Started | Complete"

  next_action: "[What to do next]"
```
  </progress_file>
</progress_tracking>

<methodology>
  <phase id="1" name="Requirements Review">
    <purpose>Load and analyze requirements document</purpose>
    <input>docs/requirements/REQUIREMENTS.md</input>
    <steps>
      <step>Read requirements document completely</step>
      <step>Identify core mechanics that need architectural support</step>
      <step>List all scenes needed based on requirements</step>
      <step>Identify shared components and systems</step>
      <step>Note technical constraints and NFRs</step>
    </steps>
    <output>docs/architecture/.work/requirements-analysis.yaml</output>
  </phase>

  <phase id="2" name="Scene Architecture">
    <purpose>Define scene hierarchy and relationships</purpose>
    <considerations>
      - Main scenes (menus, game, settings, etc.)
      - Reusable scene components (UI elements, game objects)
      - Scene transitions and loading
      - Singleton/autoload scenes for global systems
    </considerations>
    <steps>
      <step>Define scene tree structure</step>
      <step>Map scenes to functional requirements</step>
      <step>Identify scene composition opportunities</step>
      <step>Define scene interfaces (signals, exported properties)</step>
      <step>Plan scene loading and caching strategy</step>
    </steps>
    <output>
      - docs/architecture/SCENE-ARCHITECTURE.md
      - docs/architecture/.work/scene-inventory.yaml
    </output>
  </phase>

  <phase id="3" name="Script Architecture">
    <purpose>Define script organization and responsibilities</purpose>
    <patterns>
      <pattern name="Model-View-Controller">
        Separate game state (Model) from presentation (View) from logic (Controller)
      </pattern>
      <pattern name="Component Pattern">
        Attach small, focused scripts to nodes for specific behaviors
      </pattern>
      <pattern name="Service Locator">
        Use autoload singletons for global services (GameManager, AudioManager, etc.)
      </pattern>
      <pattern name="Command Pattern">
        For undo/redo or replaying inputs in testing
      </pattern>
    </patterns>
    <steps>
      <step>Define script directory structure</step>
      <step>Identify core classes and their responsibilities</step>
      <step>Define interfaces/abstract base classes</step>
      <step>Map dependencies between scripts</step>
      <step>Define signal contracts for communication</step>
      <step>Identify which classes need to be testable (most of them)</step>
    </steps>
    <output>
      - docs/architecture/SCRIPT-ARCHITECTURE.md
      - docs/architecture/.work/class-inventory.yaml
    </output>
  </phase>

  <phase id="4" name="Data Architecture">
    <purpose>Define data structures, state management, and persistence</purpose>
    <areas>
      <area name="Game State">
        - Current game state (playing, paused, game over)
        - Score, lives, inventory
        - Player progress
      </area>
      <area name="Persistence">
        - Save/load strategy
        - Settings storage
        - Player data format (JSON, binary, Godot Resource)
      </area>
      <area name="Configuration">
        - Game configuration (constants, tuning parameters)
        - Resource management (preloading, lazy loading)
      </area>
    </areas>
    <steps>
      <step>Define data models as classes/resources</step>
      <step>Design state machine for game flow</step>
      <step>Plan persistence strategy</step>
      <step>Define data flow between components</step>
      <step>Identify configuration approach (Resources, JSON, etc.)</step>
    </steps>
    <output>docs/architecture/DATA-ARCHITECTURE.md</output>
  </phase>

  <phase id="5" name="Testing Strategy">
    <purpose>Define how TDD will be implemented for this architecture</purpose>
    <godot_testing>
      Godot 4.x supports:
      - GUT (Godot Unit Test) for unit and integration tests
      - Built-in testing with --test command line flag
      - Scene testing with test doubles
    </godot_testing>
    <testability_guidelines>
      <guideline>Game logic classes should not extend Node when possible</guideline>
      <guideline>Use dependency injection for testability</guideline>
      <guideline>Test game logic independently of scenes</guideline>
      <guideline>Use test doubles/mocks for Godot node dependencies</guideline>
      <guideline>Integration tests for scene behavior</guideline>
    </testability_guidelines>
    <steps>
      <step>Identify test tool (GUT recommended)</step>
      <step>Define test directory structure</step>
      <step>Categorize tests: unit, integration, end-to-end</step>
      <step>Define test naming conventions</step>
      <step>Create test plan matrix (feature → test coverage)</step>
      <step>Define CI/CD approach for tests</step>
    </steps>
    <output>docs/architecture/TESTING-STRATEGY.md</output>
  </phase>

  <phase id="6" name="Architecture Document Consolidation">
    <purpose>Create comprehensive architecture document</purpose>
    <steps>
      <step>Consolidate all architecture documents</step>
      <step>Create architectural diagrams (scene tree, class diagram, data flow)</step>
      <step>Document key architectural decisions and rationale</step>
      <step>Create "Getting Started" guide for developers</step>
      <step>List technologies and libraries to be used</step>
    </steps>
    <output>docs/architecture/ARCHITECTURE.md (master document)</output>
  </phase>
</methodology>

<output_specifications>
  <directory_structure>
    docs/
    └── architecture/
        ├── .work/                          # Progress tracking (lightweight)
        │   ├── progress.yaml
        │   ├── requirements-analysis.yaml
        │   ├── scene-inventory.yaml
        │   └── class-inventory.yaml
        ├── ARCHITECTURE.md                 # Master architecture document
        ├── SCENE-ARCHITECTURE.md          # Scene structure details
        ├── SCRIPT-ARCHITECTURE.md         # Script organization details
        ├── DATA-ARCHITECTURE.md           # Data and state management
        └── TESTING-STRATEGY.md            # TDD approach and tooling
  </directory_structure>

  <architecture_document_template>
```markdown
# Game Architecture Document

**Project**: [Game Name]
**Date**: [YYYY-MM-DD]
**Version**: 1.0
**Godot Version**: 4.x

## Table of Contents
1. [Overview](#overview)
2. [Architectural Principles](#architectural-principles)
3. [Scene Architecture](#scene-architecture)
4. [Script Architecture](#script-architecture)
5. [Data Architecture](#data-architecture)
6. [Testing Strategy](#testing-strategy)
7. [Technology Stack](#technology-stack)
8. [Development Workflow](#development-workflow)
9. [Architectural Decisions](#architectural-decisions)

## Overview

[High-level description of the architecture approach]

### Key Architectural Characteristics
- **Testability**: [How architecture supports TDD]
- **Maintainability**: [How architecture supports long-term maintenance]
- **Performance**: [Performance considerations in architecture]
- **Scalability**: [How architecture handles growth]

## Architectural Principles

1. **[Principle 1]**: [Description and rationale]
2. **[Principle 2]**: [Description and rationale]

## Scene Architecture

### Scene Hierarchy
```
Main.tscn
├── MainMenu.tscn
├── Game.tscn
│   ├── GameBoard.tscn
│   ├── UI.tscn
│   └── GameLogic (Node with script)
└── Settings.tscn
```

### Scene Descriptions

#### Main.tscn
- **Purpose**: Root scene, handles scene transitions
- **Node Type**: Node
- **Script**: res://scripts/core/Main.gd
- **Responsibilities**:
  - Scene management
  - Global initialization
- **Signals Emitted**: scene_changed(scene_name)

[Continue for each scene...]

### Scene Transition Flow
[Diagram or description of how scenes transition]

## Script Architecture

### Directory Structure
```
res://
├── scripts/
│   ├── core/              # Core game systems
│   │   ├── GameManager.gd
│   │   └── StateManager.gd
│   ├── game/              # Game-specific logic
│   │   ├── Board.gd
│   │   └── Player.gd
│   ├── ui/                # UI components
│   │   └── MenuButton.gd
│   └── utils/             # Utilities
│       └── SaveManager.gd
└── tests/
    └── unit/
        └── test_Board.gd
```

### Core Classes

#### Class: GameManager (Autoload)
- **Purpose**: Central game coordinator
- **Extends**: Node
- **Responsibilities**:
  - Game state management
  - Score tracking
  - Event coordination
- **Public Interface**:
  ```gdscript
  func start_game() -> void
  func end_game() -> void
  signal game_started
  signal game_ended
  ```
- **Dependencies**: StateManager, SaveManager
- **Testability**: Testable via dependency injection

[Continue for each core class...]

### Class Diagram
[Mermaid or ASCII diagram showing class relationships]

## Data Architecture

### Game State Model
```gdscript
class_name GameState
extends Resource

var current_turn: int
var board_state: Array[int]
var player1_score: int
var player2_score: int
var game_status: GameStatus
```

### State Machine
[Diagram or description of game state transitions]

### Persistence Strategy
- **Format**: JSON
- **Location**: user://saves/
- **Save Trigger**: [When saves occur]
- **What's Saved**: [List of data]

## Testing Strategy

### Testing Framework
- **Tool**: GUT (Godot Unit Test) v9.x
- **Installation**: Via AssetLib or direct download

### Test Structure
```
res://tests/
├── unit/              # Pure logic tests
│   └── test_Board.gd
├── integration/       # Scene + logic tests
│   └── test_Game.gd
└── e2e/              # Full game flow tests
    └── test_GameFlow.gd
```

### Test Coverage Goals
- **Unit Tests**: 80%+ coverage of game logic
- **Integration Tests**: All scene interactions
- **E2E Tests**: Critical player paths

### TDD Workflow
1. Write failing test
2. Implement minimal code to pass
3. Refactor
4. Run full test suite

## Technology Stack

- **Engine**: Godot 4.x
- **Language**: GDScript (or C# if specified)
- **Testing**: GUT
- **Version Control**: Git
- **CI/CD**: [If applicable]

## Development Workflow

### Phase Flow
Requirements → **Architecture (You Are Here)** → Game Design Doc → TDD Development

### Next Steps
1. Review and approve this architecture
2. Proceed to Game Design Document creation (STD-0102)
3. Use architecture as guide for GDD scene/mechanic design

## Architectural Decisions

### ADR-001: [Decision Title]
- **Status**: Accepted
- **Context**: [What prompted this decision]
- **Decision**: [What was decided]
- **Consequences**: [Positive and negative outcomes]

[Continue for each significant decision...]

## Appendices

### Glossary
[Define domain-specific terms]

### References
- [Godot Best Practices](https://docs.godotengine.org/en/stable/tutorials/best_practices/)
- STD-0002-csharp-rubric.md (if using C#)
```
  </architecture_document_template>
</output_specifications>

<godot_specific_guidance>
  <scene_patterns>
    <pattern name="Scene Composition">
      Prefer small, reusable scenes composed into larger scenes over monolithic scenes
    </pattern>
    <pattern name="Scene Inheritance">
      Use scene inheritance sparingly; prefer composition
    </pattern>
    <pattern name="Autoloads for Singletons">
      Use autoload for global managers (GameManager, AudioManager, etc.)
    </pattern>
  </scene_patterns>

  <script_patterns>
    <pattern name="Node Extension">
      Extend Node types when script needs to be in scene tree
    </pattern>
    <pattern name="Plain Classes for Logic">
      Use plain classes (class_name) for pure game logic - easier to test
    </pattern>
    <pattern name="Resource for Data">
      Use Resource for data models - serializable and editor-friendly
    </pattern>
  </script_patterns>

  <signal_patterns>
    <pattern name="Decouple via Signals">
      Use signals for communication between decoupled components
    </pattern>
    <pattern name="Signal Bus Pattern">
      Consider autoload EventBus for global events
    </pattern>
  </signal_patterns>

  <testing_considerations>
    <consideration>
      GUT can test plain GDScript classes easily; Node-based classes need scene setup
    </consideration>
    <consideration>
      Separate business logic (plain classes) from presentation logic (Node scripts)
    </consideration>
    <consideration>
      Use doubles/mocks for Godot API calls (get_node, etc.)
    </consideration>
  </testing_considerations>
</godot_specific_guidance>

<critical_reminders>
================================================================================
                    CRITICAL REMINDERS
================================================================================

1. **TESTABILITY IS PARAMOUNT**
   - Architecture must support TDD
   - Separate logic from scenes wherever possible
   - Design for dependency injection

2. **GODOT CONVENTIONS MATTER**
   - Follow official Godot style guide
   - Use Godot patterns (signals, resources, autoloads)
   - Don't fight the engine

3. **SCENE COMPOSITION OVER COMPLEXITY**
   - Small, focused scenes
   - Compose scenes, don't create monoliths
   - Reusable components

4. **CLEAR SEPARATION OF CONCERNS**
   - Game logic ≠ Presentation logic
   - Data ≠ Logic
   - UI ≠ Game mechanics

5. **DOCUMENT DECISIONS**
   - Every non-obvious choice needs rationale
   - Use ADRs for significant decisions
   - Explain trade-offs

6. **PROGRESS TRACKING**
   - Update .work/progress.yaml as you complete phases
   - This is a lighter-weight process than full compaction survival
   - Document next_action if you need to pause

</critical_reminders>

<begin>
=====================================
CRITICAL: CHECK FOR EXISTING PROGRESS
=====================================
This architecture work may have been started previously.

FIRST ACTION - Check for existing progress:
```bash
cat docs/architecture/.work/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

IF progress file exists:
- Read current_phase and next_action
- Resume from where you left off
- Load any completed .work/ files

IF no progress file (fresh start):
- Create docs/architecture/.work/ directory
- Initialize progress.yaml
- Proceed with Phase 1

=====================================
BEGIN ARCHITECTURE DESIGN
=====================================

PROCESS:
1. Read requirements document: docs/requirements/REQUIREMENTS.md
2. Work through phases 1-6 systematically
3. Update progress.yaml after each phase
4. Create all specified output documents
5. Consolidate into master ARCHITECTURE.md

VALIDATION:
- Does architecture support all functional requirements?
- Is every component testable?
- Are Godot best practices followed?
- Are architectural decisions documented?

OUTPUT:
- Complete docs/architecture/ directory with all documents
- Master ARCHITECTURE.md ready for review
- Clear path forward to Game Design Document phase

BEGIN NOW by checking for progress, then starting Phase 1.

</begin>
```

---

## Usage Notes

- **Timeframe**: 1-2 hours for typical game
- **Dependencies**: Requires docs/requirements/REQUIREMENTS.md
- **Output**: Complete architecture documentation
- **Next Phase**: STD-0102-game-design-document.md

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-01-16 | Initial architecture design prompt for Godot games |

---

## Related Documents

- STD-0001-prompt-creation-rubric.md - Prompt patterns
- STD-0002-csharp-rubric.md - C# standards (if applicable)
- STD-0100-requirements-gathering.md - Previous phase
- STD-0102-game-design-document.md - Next phase
