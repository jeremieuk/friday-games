# Rubric Assessment for Game Development Prompts

## Date: 2026-01-16
## Project: Scissors-Paper-Rock (Godot 4.5)

---

## Executive Summary

The existing **STD-0001-prompt-creation-rubric.md** is excellent for large-scale analysis, QA, and verification tasks but requires adaptation for game development workflows. This assessment identifies what to keep, what to adapt, and what to add for game development prompts.

---

## Rubric Analysis

### ✅ Keep These Patterns

| Pattern | Reason | Application in Game Dev |
|---------|--------|-------------------------|
| **XML Tag Structure** | Aligns with Anthropic's guide; provides clarity | Structure requirements, architecture, GDD, tests |
| **Progress Tracking** | Good for multi-phase work | Track requirements → architecture → GDD → TDD phases |
| **Phase-Based Workflow** | Natural fit for sequential development | Requirements, Architecture, Design, Development |
| **Clear Next Actions** | Enables resumption | Useful if work is interrupted between phases |
| **Checkpoint Strategies** | Good practice for any work | Checkpoint after each phase completion |
| **Deliverables vs Work Files** | Separation of concerns | `.work/` for progress, `spr/` for game deliverables |

### 🔧 Adapt These Patterns

| Pattern | Original Purpose | Game Dev Adaptation |
|---------|------------------|---------------------|
| **Large File Handling** | Processing 100KB+ specs/docs | **Remove** - Creating small game, not analyzing large files |
| **Context Compaction Survival** | Long-running analysis (hours) | **Simplify** - Game dev phases are shorter, less compaction risk |
| **Source Discovery** | Cataloging existing files | **Replace** - Discovery of Godot project structure, not source analysis |
| **Avoiding Arbitrary Limits** | Processing ALL files in analysis | **Less relevant** - Not processing hundreds of files |
| **Memory Efficient Patterns** | Chunked reading of large files | **Not needed** - Creating code, not reading massive files |

### ➕ Add for Game Development

| Pattern | Purpose | Implementation |
|---------|---------|----------------|
| **Godot Project Structure** | Consistent scene/script organization | Define folder structure for game assets |
| **C# Coding Standards** | Code quality (already exists: STD-0002) | Reference and enforce in TDD prompt |
| **Game State Management** | Handle game states (menu, playing, result) | State machine pattern or similar |
| **Scene Architecture** | Node hierarchy best practices | Define how UI, game logic, audio are organized |
| **Test-First Development** | TDD workflow for game logic | Write GUT tests before implementation |
| **Asset Pipeline** | Managing sprites, audio, fonts | Define asset organization and naming |
| **Input Handling** | Mouse, touch, keyboard inputs | Define input abstraction layer |

---

## Adapted Core Principles for Game Development

| Principle | Adaptation |
|-----------|------------|
| **Disk Over Memory** | Write phase outputs to disk (requirements.yaml, architecture.md, GDD.md) |
| **Progress After Every Phase** | Update `progress.yaml` after requirements → architecture → GDD → TDD |
| **Complete Then Move** | Finish requirements before architecture; architecture before GDD; GDD before TDD |
| **Check Before Starting** | Check for existing phase outputs before starting |
| **Clear Next Action** | Document what comes next at end of each phase |
| **Separate Work from Deliverables** | `.work/` for progress tracking; `spr/` for game files; `docs/` for documentation |

---

## Recommended Directory Structure

```
friday-games/
├── .work/
│   └── spr-game/
│       └── progress.yaml          # Phase tracking
│
├── spr/                            # Game project root
│   ├── project.godot
│   ├── docs/
│   │   ├── requirements.md        # Phase 1 output
│   │   ├── architecture.md        # Phase 2 output
│   │   └── game-design-doc.md     # Phase 3 output
│   │
│   ├── scenes/                    # Godot scenes
│   │   ├── main_menu.tscn
│   │   ├── game.tscn
│   │   └── result.tscn
│   │
│   ├── scripts/                   # C# scripts
│   │   ├── game_logic/
│   │   ├── ui/
│   │   └── managers/
│   │
│   ├── tests/                     # GUT tests
│   │   └── unit/
│   │
│   └── assets/                    # Game assets
│       ├── sprites/
│       ├── audio/
│       └── fonts/
│
└── spr/prompts/
    └── game-dev/
        ├── 01-requirements-prompt.md
        ├── 02-architecture-prompt.md
        ├── 03-game-design-doc-prompt.md
        └── 04-tdd-development-prompt.md
```

---

## Prompt Suite Structure

### Phase 1: Requirements Gathering
**Output**: `spr/docs/requirements.md`

- User stories
- Functional requirements
- Non-functional requirements
- Success criteria
- Technical constraints (Godot 4.5, C#)

### Phase 2: Architecture Design
**Input**: `spr/docs/requirements.md`
**Output**: `spr/docs/architecture.md`

- High-level architecture (MVC, state machine, etc.)
- Scene hierarchy
- Component relationships
- Data flow
- Input handling strategy
- Testing strategy

### Phase 3: Game Design Document
**Input**: `spr/docs/requirements.md`, `spr/docs/architecture.md`
**Output**: `spr/docs/game-design-doc.md`

- Game mechanics (detailed)
- UI/UX design
- Visual design (sprites, colors, fonts)
- Audio design (SFX, music)
- Win/lose conditions
- Game flow diagrams

### Phase 4: TDD Development
**Input**: `spr/docs/requirements.md`, `spr/docs/architecture.md`, `spr/docs/game-design-doc.md`
**Output**: Working game in `spr/` with tests in `spr/tests/`

- Test-first development
- Red-Green-Refactor cycle
- GUT test framework
- Incremental development
- Continuous testing

---

## Progress Tracking Schema (Simplified)

```yaml
# .work/spr-game/progress.yaml
progress:
  last_updated: "2026-01-16T10:00:00Z"
  current_phase: "requirements"  # requirements | architecture | gdd | tdd | complete
  status: "In Progress"

  phases:
    requirements:
      status: "Complete"
      output_file: "spr/docs/requirements.md"
      completed_at: "2026-01-16T09:30:00Z"

    architecture:
      status: "In Progress"
      output_file: "spr/docs/architecture.md"

    game_design_doc:
      status: "Not Started"
      output_file: "spr/docs/game-design-doc.md"

    tdd_development:
      status: "Not Started"
      output_dir: "spr/"

  next_action: "Complete architecture design phase. Reference requirements.md and define scene hierarchy."
```

---

## Key Differences from Original Rubric

1. **No Large File Handling** - We're creating, not analyzing
2. **Simpler Progress Tracking** - 4 phases vs multi-level analysis
3. **Sequential not Parallel** - Each phase depends on previous
4. **Game-Specific Outputs** - GDD, scenes, scripts vs analysis reports
5. **Less Compaction Risk** - Shorter, focused phases
6. **Test-Driven Focus** - TDD is core to development phase

---

## Rubric Verdict

✅ **The rubric is suitable WITH adaptations**

The core principles (progress tracking, phase management, XML structure, deliverables separation) are excellent. We remove the analysis-specific patterns (large file handling, source discovery) and add game-development patterns (Godot structure, TDD, asset management).

---

## Next Steps

Create four prompts following this adapted rubric:
1. `01-requirements-prompt.md`
2. `02-architecture-prompt.md`
3. `03-game-design-doc-prompt.md`
4. `04-tdd-development-prompt.md`

Each prompt will:
- Use XML tags per Anthropic's guide
- Include simplified progress tracking
- Reference previous phase outputs
- Define clear deliverables
- Enforce C# coding standards (STD-0002) in TDD phase
