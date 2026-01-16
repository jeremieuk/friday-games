# Godot Game Development Prompt Series

## Overview

This directory contains a comprehensive prompt series for developing games using Godot Engine with Test-Driven Development (TDD) methodology. The prompts guide you from initial requirements through to deployable game, following best practices from both Anthropic's prompt engineering guidelines and Godot development conventions.

---

## Prompt Series

### Phase Flow

```
Requirements → Architecture → Game Design Document → TDD Development → Deployment
    (1-2h)        (2-3h)            (3-5h)                (weeks)
```

### STD-0100: Requirements Gathering
**File**: `STD-0100-requirements-gathering.md`
**Duration**: 30-60 minutes
**Output**: `docs/requirements/REQUIREMENTS.md`

Conversational prompt that helps gather:
- Game concept and vision
- Functional requirements (mechanics, UI, content)
- Non-functional requirements (performance, usability, technical)
- Success criteria and scope boundaries

**When to use**: At project start, or when adding major features

---

### STD-0101: Architecture Design
**File**: `STD-0101-architecture-design.md`
**Duration**: 1-2 hours
**Input**: `docs/requirements/REQUIREMENTS.md`
**Output**: `docs/architecture/` directory with ARCHITECTURE.md

Creates technical architecture covering:
- Scene hierarchy and structure
- Script organization and class design
- Data architecture and state management
- Testing strategy for TDD

**When to use**: After requirements are approved

---

### STD-0102: Game Design Document
**File**: `STD-0102-game-design-document.md`
**Duration**: 2-4 hours (more for complex games)
**Input**: Requirements + Architecture
**Output**: `docs/gdd/GDD.md`

Creates detailed game design specifications:
- Core mechanics (precise, testable specifications)
- Game flow and state transitions
- UI/UX designs with mockups
- Content specifications
- Asset requirements
- Technical implementation guidance

**Features**:
- Context compaction survival (can resume across sessions)
- Section-by-section progress tracking
- Links design to architecture

**When to use**: After architecture is approved

---

### STD-0103: TDD Development
**File**: `STD-0103-tdd-development.md`
**Duration**: Multiple weeks
**Input**: Requirements + Architecture + GDD
**Output**: Complete, tested, deployable game

Implements game using strict TDD methodology:
- Red-Green-Refactor cycle for all components
- GUT (Godot Unit Test) integration
- Unit, integration, and E2E testing
- Component-by-component implementation
- Asset integration and polish
- Build and deployment

**Features**:
- Full context compaction survival (essential for multi-week projects)
- Comprehensive progress tracking
- Component inventory and test coverage tracking
- Safe resumption after any interruption

**When to use**: After GDD is approved, throughout development

---

## Key Features

### 1. Context Compaction Survival

All prompts (especially STD-0102 and STD-0103) implement patterns to survive Claude's context compaction:

- **Progress Tracking**: `.work/` directories with `progress.yaml`
- **Resumption Protocol**: Check for existing progress, resume from `next_action`
- **Incremental Work**: Complete one unit before starting another
- **Persistent State**: All critical information written to disk, not kept in context

### 2. XML Structuring

All prompts follow Anthropic's official XML tag guidelines:
- Clear sections with semantic tags
- Nested hierarchies for complex content
- Consistent naming conventions
- Easy to parse and modify

### 3. Godot-Specific Guidance

Each prompt includes Godot 4.x-specific patterns:
- Scene composition over monoliths
- Signal-driven communication
- Resource-based data models
- Autoload singletons for global systems
- GDScript/C# best practices

### 4. TDD Integration

Architecture through development emphasize Test-Driven Development:
- Design for testability
- Separate logic from presentation
- GUT (Godot Unit Test) integration
- Comprehensive test coverage (>80% goal)

---

## Directory Structure (After All Phases)

```
your-game-project/
├── .work/
│   └── tdd-development/          # TDD progress tracking
│       ├── progress.yaml
│       ├── component-inventory.yaml
│       ├── test-coverage.yaml
│       ├── integration-checklist.yaml
│       ├── asset-integration.yaml
│       └── build-status.yaml
│
├── docs/
│   ├── requirements/
│   │   └── REQUIREMENTS.md       # From STD-0100
│   │
│   ├── architecture/
│   │   ├── .work/                # Architecture progress (lightweight)
│   │   ├── ARCHITECTURE.md       # From STD-0101 (master)
│   │   ├── SCENE-ARCHITECTURE.md
│   │   ├── SCRIPT-ARCHITECTURE.md
│   │   ├── DATA-ARCHITECTURE.md
│   │   └── TESTING-STRATEGY.md
│   │
│   └── gdd/
│       ├── .work/                # GDD progress tracking
│       │   ├── progress.yaml
│       │   ├── section-checklist.yaml
│       │   ├── design-decisions.yaml
│       │   └── asset-inventory.yaml
│       └── GDD.md                # From STD-0102
│
├── res://                        # Godot project (from STD-0103)
│   ├── addons/
│   │   └── gut/                  # GUT testing framework
│   │
│   ├── scenes/
│   │   ├── Main.tscn
│   │   ├── MainMenu.tscn
│   │   └── Game.tscn
│   │
│   ├── scripts/
│   │   ├── core/
│   │   ├── game/
│   │   ├── ui/
│   │   └── utils/
│   │
│   ├── tests/
│   │   ├── unit/
│   │   ├── integration/
│   │   └── e2e/
│   │
│   ├── assets/
│   │   ├── sprites/
│   │   ├── audio/
│   │   └── fonts/
│   │
│   └── project.godot
│
└── builds/                       # Export builds
```

---

## Usage Guide

### For Your Noughts and Crosses Game

1. **Start with STD-0100** (Requirements Gathering)
   - Feed the prompt to Claude Code
   - Answer questions about your game vision
   - Receive `docs/requirements/REQUIREMENTS.md`

2. **Proceed to STD-0101** (Architecture Design)
   - Feed prompt with instruction to read requirements
   - Receive complete architecture documentation
   - Review and approve architecture

3. **Continue to STD-0102** (Game Design Document)
   - Feed prompt with instruction to read requirements and architecture
   - Claude will build GDD section by section
   - Can resume if interrupted (context compaction survival)
   - Receive complete GDD.md

4. **Implement with STD-0103** (TDD Development)
   - Feed prompt to begin TDD implementation
   - Claude will work through phases systematically
   - Work will span many sessions (days/weeks)
   - Can safely resume at any time (reads progress.yaml)
   - End result: Complete, tested game

### Customization

These prompts are designed for general Godot game development. For your specific project:

- **Requirements phase**: Provide your game-specific answers
- **Architecture phase**: Customize based on your game's complexity
- **GDD phase**: Add/remove sections based on game type
- **TDD phase**: Adjust component priorities based on your game

---

## Prompt Engineering Principles

These prompts follow principles from:

### 1. STD-0001 Prompt Creation Rubric
- Context compaction survival patterns
- Progress tracking schemas
- Output directory separation
- Large file handling (where applicable)

### 2. Anthropic XML Guidelines
- Clear, semantic XML tags
- Hierarchical nesting
- Consistent tag naming
- Separate instructions from content

### 3. Godot Best Practices
- Official Godot style guide compliance
- Scene composition patterns
- Signal-driven architecture
- Resource-based data

### 4. TDD Best Practices
- Red-Green-Refactor cycle
- Test independence
- High coverage goals
- Continuous integration

---

## Integration with Existing Rubrics

### STD-0001: Prompt Creation Rubric
These prompts implement patterns from STD-0001:
- **Lightweight** (STD-0100, STD-0101): Basic progress tracking
- **Medium-weight** (STD-0102): Section-level compaction survival
- **Full-weight** (STD-0103): Comprehensive compaction survival for long-running work

### STD-0002: C# Code Style Rubric
If using C# instead of GDScript:
- STD-0101 references C# architecture patterns
- STD-0103 follows C# coding standards
- All prompts note "if using C#" guidance

---

## Example: Noughts and Crosses (Tic-Tac-Toe)

For your noughts and crosses game, expect:

### Requirements (STD-0100)
- 2-player turn-based game
- 3x3 grid board
- Win detection (3 in a row)
- Draw detection
- Simple UI (grid, status, restart button)
- Target: PC (web export possible)

### Architecture (STD-0101)
- Scenes: Main, MainMenu, Game, GameOver
- Core classes: Board, GameLogic, Player, GameState
- UI: GridUI, StatusLabel, Buttons
- State management: Finite state machine
- Testing: GUT framework, high coverage

### GDD (STD-0102)
- Detailed grid interaction mechanics
- Turn-based flow specification
- UI layout with ASCII mockups
- Win/draw conditions precisely defined
- Asset requirements (minimal for noughts and crosses)

### TDD Implementation (STD-0103)
- ~10-15 core components to implement
- ~30-50 unit tests
- ~10 integration tests
- ~5 E2E tests
- Estimated: 2-4 weeks part-time development

---

## Tips for Success

### 1. Review and Approve Each Phase
Don't skip ahead. Each phase builds on the previous:
- Approve requirements before architecture
- Approve architecture before GDD
- Approve GDD before implementation

### 2. Trust the Progress Tracking
If context compacts during GDD or TDD:
- Don't panic
- Feed the same prompt again
- Claude will read progress.yaml and resume correctly

### 3. Modify Prompts as Needed
These are templates:
- Add game-specific sections
- Remove irrelevant parts
- Adjust phase durations based on experience

### 4. Use Git Throughout
- Commit after each prompt phase
- Commit frequently during TDD (as prompted)
- Git history becomes development diary

### 5. Run Tests Continuously
During TDD phase:
- Run tests after every code change
- Never commit failing tests
- Test suite is your safety net

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-01-16 | Initial game development prompt series |

---

## Related Documents

### Within This Repository
- `../rubrics/STD-0001-prompt-creation-rubric.md` - Prompt patterns
- `../rubrics/STD-0002-csharp-rubric.md` - C# coding standards

### External Resources
- [Anthropic XML Tags Guide](https://docs.anthropic.com/en/docs/build-with-claude/prompt-engineering/use-xml-tags)
- [Godot 4.x Documentation](https://docs.godotengine.org/en/stable/)
- [GUT Testing Framework](https://github.com/bitwes/Gut)
- [Godot TDD Best Practices](https://docs.godotengine.org/en/stable/tutorials/scripting/unit_testing.html)

---

## Support and Feedback

These prompts are living documents. As you use them:
- Note what works well
- Identify gaps or improvements
- Update prompts based on experience
- Contribute improvements back to the prompt library

---

## License

These prompts are part of the Friday Games project prompt library.
Use freely for your game development projects.

---

**Ready to build your game?**

Start with `STD-0100-requirements-gathering.md` and let Claude Code guide you through the journey from concept to playable game!
