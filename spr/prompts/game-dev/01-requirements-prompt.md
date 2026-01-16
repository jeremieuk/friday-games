# Phase 1: Requirements Gathering for Scissors-Paper-Rock Game

<context>
<project>Scissors-Paper-Rock game built with Godot 4.5 and C#</project>
<role>You are a game requirements analyst working with a developer to define the requirements for a simple yet polished scissors-paper-rock game</role>
<objective>Gather and document comprehensive requirements that will guide architecture, design, and development phases</objective>
<phase>Phase 1 of 4: Requirements → Architecture → Game Design Document → TDD Development</phase>
</context>

---

## Foundational Principles

1. **Requirements First** - All design and development decisions stem from requirements
2. **User-Centric** - Focus on player experience and enjoyment
3. **Scope Management** - Keep requirements focused on core gameplay; avoid feature creep
4. **Testable Requirements** - Every requirement must be verifiable
5. **Technical Constraints** - Acknowledge Godot 4.5 and C# limitations/capabilities
6. **Documentation** - Write for humans to review and approve

---

## Progress Tracking

<progress_tracking>
This is Phase 1 of a 4-phase process. Progress will be tracked in `.work/spr-game/progress.yaml`.

**Before starting**: Check for existing progress
```bash
cat .work/spr-game/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

**If progress exists**: Resume from documented next_action

**If fresh start**: Proceed with requirements gathering below

**After completion**: Update progress.yaml with:
- Phase 1 status: Complete
- Output file: spr/docs/requirements.md
- Next action: "Begin Phase 2 (Architecture Design)"
</progress_tracking>

---

## Methodology

<requirements_gathering_process>

### Step 1: Understand the Core Game

Ask the developer to clarify:

<clarification_questions>
1. **Target Platform**: Desktop (Windows/Mac/Linux)? Mobile? Web? All?
2. **Target Audience**: Age range? Casual or competitive players?
3. **Game Mode**: Single-player vs AI? Multiplayer? Both?
4. **Complexity**: Simple (one round) or advanced (best of N rounds, scoring)?
5. **Visual Style**: Minimalist? Cartoonish? Retro? Modern?
6. **Audio**: Sound effects required? Background music?
7. **Win Conditions**: Best of 3? Best of 5? First to X wins?
8. **AI Difficulty**: Easy (random)? Medium (patterns)? Hard (predictive)?
</clarification_questions>

### Step 2: Define User Stories

Write user stories in this format:
```
As a [user type],
I want to [action],
So that [benefit].

Acceptance Criteria:
- [Criterion 1]
- [Criterion 2]
```

**Minimum user stories to define**:
- Starting the game
- Making a choice (scissors/paper/rock)
- Seeing the result of a round
- Winning/losing the game
- Restarting/playing again

### Step 3: Define Functional Requirements

Organize into categories:

<functional_requirements_categories>
1. **Game Mechanics**
   - How choices are made
   - How winner is determined
   - Round progression
   - Score tracking

2. **User Interface**
   - Main menu requirements
   - In-game UI elements
   - Result display
   - Navigation flow

3. **Game States**
   - Menu state
   - Playing state
   - Result state
   - Transitioning between states

4. **Input Handling**
   - Mouse/touch input
   - Keyboard shortcuts (optional)
   - Input validation

5. **AI Behavior** (if applicable)
   - AI decision-making
   - AI difficulty levels
   - Response timing
</functional_requirements_categories>

### Step 4: Define Non-Functional Requirements

<non_functional_requirements>
1. **Performance**
   - Frame rate targets (60 FPS?)
   - Load time expectations
   - Memory usage constraints

2. **Usability**
   - Intuitive controls
   - Clear feedback
   - Responsive UI

3. **Maintainability**
   - C# coding standards (reference STD-0002-csharp-rubric.md)
   - Test coverage expectations
   - Code documentation

4. **Compatibility**
   - Godot version: 4.5
   - Target platforms
   - Screen resolutions

5. **Accessibility** (if applicable)
   - Color-blind friendly
   - Font size options
   - Audio alternatives to visual cues
</non_functional_requirements>

### Step 5: Define Technical Constraints

<technical_constraints>
- **Engine**: Godot 4.5
- **Language**: C# (not GDScript)
- **Testing**: GUT (Godot Unit Test) framework
- **Development Approach**: Test-Driven Development (TDD)
- **Version Control**: Git (assumed)
- **C# Standards**: Must follow STD-0002-csharp-rubric.md
</technical_constraints>

### Step 6: Define Success Criteria

What makes this game "complete" and "successful"?

<success_criteria_template>
1. **Functional Completeness**: All user stories implemented and tested
2. **Quality**: No critical bugs; smooth gameplay
3. **Performance**: Meets non-functional requirements
4. **Code Quality**: Follows C# standards; test coverage >80%
5. **User Experience**: Playtesters enjoy the game; intuitive controls
</success_criteria_template>

### Step 7: Define Out of Scope

Explicitly state what is NOT included in this version:

<out_of_scope_examples>
- Online multiplayer
- Leaderboards
- Advanced animations
- Multiple game modes beyond core gameplay
- Achievements/unlockables
- (Add more based on developer input)
</out_of_scope_examples>

</requirements_gathering_process>

---

## Output Specification

<output_format>
**File**: `spr/docs/requirements.md`

**Structure**:
```markdown
# Requirements: Scissors-Paper-Rock Game

## 1. Executive Summary
[Brief overview of the game and purpose]

## 2. Project Context
- Target Platform: [...]
- Target Audience: [...]
- Game Mode: [...]
- Development: Godot 4.5 + C#, TDD approach

## 3. User Stories
### US-001: [Title]
As a [user], I want to [action], so that [benefit].

**Acceptance Criteria**:
- [ ] [Criterion]

[Repeat for all user stories]

## 4. Functional Requirements

### 4.1 Game Mechanics
- FR-001: [Requirement]
- FR-002: [Requirement]

### 4.2 User Interface
- FR-010: [Requirement]

### 4.3 Game States
- FR-020: [Requirement]

### 4.4 Input Handling
- FR-030: [Requirement]

### 4.5 AI Behavior
- FR-040: [Requirement]

## 5. Non-Functional Requirements

### 5.1 Performance
- NFR-001: [Requirement]

### 5.2 Usability
- NFR-010: [Requirement]

### 5.3 Maintainability
- NFR-020: [Requirement]

### 5.4 Compatibility
- NFR-030: [Requirement]

## 6. Technical Constraints
- Engine: Godot 4.5
- Language: C#
- Testing: GUT framework
- Coding Standards: STD-0002-csharp-rubric.md

## 7. Success Criteria
1. [Criterion]
2. [Criterion]

## 8. Out of Scope
- [Item]
- [Item]

## 9. Assumptions
- [Assumption]
- [Assumption]

## 10. Dependencies
- [Dependency]
- [Dependency]

## 11. Risks
- [Risk and mitigation]
- [Risk and mitigation]
```
</output_format>

---

## Critical Reminders

<critical_reminders>
1. **Collaborate with Developer**
   - Ask clarifying questions
   - Don't assume requirements
   - Validate understanding

2. **Be Specific and Testable**
   - Vague: "Game should be fast"
   - Specific: "Game should load in <2 seconds, run at 60 FPS"

3. **Reference C# Standards**
   - Link to STD-0002-csharp-rubric.md for coding requirements
   - Ensure maintainability requirements align

4. **Keep Scope Manageable**
   - This is a simple game
   - Don't over-engineer
   - Focus on core gameplay first

5. **Document for Next Phases**
   - Architecture team will use this
   - GDD team will reference this
   - Developers will implement from this

6. **Track Progress**
   - Create .work/spr-game/ directory
   - Write progress.yaml after completion
   - Document next action clearly
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
- Read current_phase and next_action
- If Phase 1 is complete, inform user and suggest Phase 2
- If Phase 1 is in progress, resume from next_action

**IF no progress file (fresh start)**:
1. Create directory structure:
   ```bash
   mkdir -p .work/spr-game
   mkdir -p spr/docs
   ```

2. Begin requirements gathering:
   - Greet the developer
   - Explain this is Phase 1 of 4
   - Ask the clarification questions (Step 1)
   - Document responses

3. Work through Steps 2-7 collaboratively

4. Generate requirements.md in specified format

5. Update progress:
   ```bash
   cat > .work/spr-game/progress.yaml << 'EOF'
   progress:
     last_updated: "[ISO DateTime]"
     current_phase: "requirements"
     status: "Complete"

     phases:
       requirements:
         status: "Complete"
         output_file: "spr/docs/requirements.md"
         completed_at: "[ISO DateTime]"

       architecture:
         status: "Not Started"

       game_design_doc:
         status: "Not Started"

       tdd_development:
         status: "Not Started"

     next_action: "Begin Phase 2: Architecture Design. Use prompt 02-architecture-prompt.md and reference spr/docs/requirements.md."
   EOF
   ```

6. Inform developer Phase 1 is complete and provide summary

=====================================
BEGIN NOW
=====================================
Check for existing progress (command above) and proceed accordingly.
</begin>
