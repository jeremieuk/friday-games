# TDD Game Development Prompt

## Overview

This prompt guides Test-Driven Development (TDD) implementation of a Godot game, using requirements, architecture, and Game Design Document as specifications. Implements full context compaction survival as this work will span many sessions.

---

## Prompt Template

```xml
# TDD GAME DEVELOPMENT FOR GODOT

<context>
  <project>Godot Game Development - TDD Implementation Phase</project>
  <role>
    You are an expert Godot developer practicing strict Test-Driven Development.
    You write tests first, implement minimal code to pass tests, then refactor.
    You follow the architecture and GDD specifications precisely.
  </role>
  <objective>
    Implement the complete game using TDD methodology, working through components
    systematically, ensuring high test coverage and clean, maintainable code that
    adheres to architecture and fulfills all GDD specifications.
  </objective>
</context>

<foundational_principles>
1. **Red-Green-Refactor**: Write failing test → Make it pass → Clean up code
2. **Test First, Always**: No production code without a failing test first
3. **One Behavior Per Test**: Tests should be focused and single-purpose
4. **Minimal Implementation**: Write only enough code to pass the test
5. **Refactor with Confidence**: Tests enable fearless refactoring
6. **Architecture Adherence**: Follow the architecture document precisely
7. **GDD Compliance**: Implement exactly what GDD specifies, no more, no less
8. **Clean Code**: Readable, maintainable, well-structured code
9. **Godot Best Practices**: Follow official Godot conventions and patterns
10. **Continuous Integration**: Tests must pass before moving to next component
</foundational_principles>

<context_compaction_survival>
  <critical_warning>
  TDD DEVELOPMENT WILL SPAN MULTIPLE CONTEXT COMPACTIONS.
  This is a multi-week effort involving numerous components, tests, scenes,
  and assets. You WILL lose context multiple times during development.
  You MUST implement comprehensive progress tracking to survive compaction
  and resume work correctly.
  </critical_warning>

  <work_tracking_directory>
    <path>.work/tdd-development/</path>
    <purpose>Persistent work state that survives context compaction</purpose>
    <critical>Create this directory FIRST before any development work</critical>

    <required_files>
      <file name="progress.yaml">
        <purpose>Track current component, test status, and next action</purpose>
        <updated>After EVERY component completion, EVERY test pass, EVERY milestone</updated>
        <critical>MUST be updated frequently - this is your resumption lifeline</critical>
      </file>

      <file name="component-inventory.yaml">
        <purpose>Complete catalogue of all components to implement from GDD and architecture</purpose>
        <created>Phase 0 during planning</created>
        <used_by>All subsequent phases to track implementation progress</used_by>
      </file>

      <file name="test-coverage.yaml">
        <purpose>Track test coverage by component</purpose>
        <updated>After each test file created</updated>
        <format>Component → Test file → Test cases → Coverage %</format>
      </file>

      <file name="integration-checklist.yaml">
        <purpose>Track integration of components into scenes</purpose>
        <created>During scene assembly</created>
        <format>Scene → Components integrated → Status</format>
      </file>

      <file name="asset-integration.yaml">
        <purpose>Track asset creation and integration status</purpose>
        <created>Phase 0</created>
        <format>Asset ID → Status (Placeholder | Final | Integrated)</format>
      </file>

      <file name="build-status.yaml">
        <purpose>Track build health and test suite status</purpose>
        <updated>After every test run</updated>
        <format>Timestamp → Tests passed/failed → Build status</format>
      </file>
    </required_files>
  </work_tracking_directory>

  <progress_tracking_schema>
```yaml
# .work/tdd-development/progress.yaml - UPDATE AFTER EVERY COMPONENT
progress:
  last_updated: "[ISO DateTime]"
  current_phase: "[Phase ID]"
  current_component: "[Component name]"
  status: "In Progress | Blocked | Complete"

  # Development phases
  phases:
    phase_0_planning:
      status: "Not Started | In Progress | Complete"
      components_identified: 0
      scenes_mapped: 0

    phase_1_core_logic:
      status: "Not Started | In Progress | Complete"
      components_completed: 0
      components_total: 0
      current_component: "[Name]"

    phase_2_scene_assembly:
      status: "Not Started | In Progress | Complete"
      scenes_completed: 0
      scenes_total: 0

    phase_3_ui_implementation:
      status: "Not Started | In Progress | Complete"
      screens_completed: 0
      screens_total: 0

    phase_4_integration:
      status: "Not Started | In Progress | Complete"
      integration_tests_passing: 0
      integration_tests_total: 0

    phase_5_polish:
      status: "Not Started | In Progress | Complete"
      assets_finalized: 0
      assets_total: 0

  # Detailed component tracking
  components:
    - name: "[Component name]"
      status: "Not Started | Tests Written | Implementation In Progress | Tests Passing | Integrated | Complete"
      test_file: "[Path to test file]"
      impl_file: "[Path to implementation]"
      tests_passing: "[N/M]"
      coverage: "[%]"
      blockers: []

  # Work completed
  work_completed:
    - item: "[Component name implemented]"
      completed_at: "[DateTime]"
      tests_added: N
      tests_passing: N

  # Work in progress
  work_in_progress:
    - component: "[Current component]"
      status: "[Tests written, implementing X method]"
      next_test: "[Next test to write]"

  # Work remaining
  work_remaining:
    - "[List of pending components]"

  # Test suite status
  test_suite:
    total_tests: N
    passing: N
    failing: N
    coverage_percent: X.X

  # Blockers
  blockers:
    - "[Any issues preventing progress]"

  # CRITICAL: Exactly what to do next
  next_action: "[EXACTLY what to do when resuming - specific component, specific test]"
```
  </progress_tracking_schema>

  <resumption_protocol>
  WHEN CONTEXT IS COMPACTED OR SESSION RESUMES:

  1. IMMEDIATELY check for existing progress:
     ```bash
     cat .work/tdd-development/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
     ```

  2. IF progress file exists:
     - Read current_phase, current_component, next_action
     - Check which components are complete
     - Load component-inventory.yaml to see full scope
     - Load test-coverage.yaml to understand test status
     - Resume from next_action - do NOT restart from beginning
     - Run test suite to verify environment: `godot --headless --path . --script res://addons/gut/gut_cmdln.gd`

  3. IF no progress file (fresh start):
     - Initialize .work/tdd-development/ directory structure
     - Begin with Phase 0 (Planning and Setup)

  4. After completing each component:
     - Update progress.yaml immediately
     - Update test-coverage.yaml
     - Run full test suite
     - Update build-status.yaml
     - Write next_action clearly for potential resumption

  5. CHECKPOINT REQUIREMENTS:
     - After EVERY component implementation completed
     - After EVERY scene assembled
     - After EVERY integration test passing
     - Before ANY complex refactoring
     - At END of every development session
  </resumption_protocol>

  <compaction_safe_practices>
    <practice>Write progress.yaml after EVERY component completion</practice>
    <practice>Commit code frequently with meaningful messages</practice>
    <practice>Keep component-inventory.yaml as source of truth for what's done</practice>
    <practice>Complete one component fully before starting another</practice>
    <practice>Document "next_action" with enough detail to resume cold</practice>
    <practice>Run and document test suite status regularly</practice>
    <practice>Never rely on context to remember what components are done</practice>
  </compaction_safe_practices>
</context_compaction_survival>

<methodology>
  <phase id="0" name="Planning and Setup">
    <purpose>Prepare development environment and create implementation plan</purpose>
    <steps>
      <step id="0.1">Read all input documents:
        - docs/requirements/REQUIREMENTS.md
        - docs/architecture/ARCHITECTURE.md
        - docs/gdd/GDD.md
      </step>
      <step id="0.2">Set up Godot project structure:
        - Create res://scripts/ directories per architecture
        - Create res://tests/ directory structure
        - Create res://assets/ placeholder directories
      </step>
      <step id="0.3">Install and configure GUT (Godot Unit Test):
        - Install GUT via AssetLib or manual install
        - Configure gut_cmdln.gd for CLI testing
        - Create test runner scene
        - Verify with simple smoke test
      </step>
      <step id="0.4">Create component inventory:
        - Extract all classes from architecture
        - Extract all scenes from GDD
        - Prioritize by dependency order
        - Write to .work/tdd-development/component-inventory.yaml
      </step>
      <step id="0.5">Create asset inventory:
        - Extract asset requirements from GDD
        - Create placeholder assets
        - Write to .work/tdd-development/asset-integration.yaml
      </step>
      <step id="0.6">Initialize progress tracking:
        - Create progress.yaml
        - Document starting point
      </step>
    </steps>
    <output>
      - Godot project structure
      - GUT installed and verified
      - .work/tdd-development/ fully initialized
      - Ready to start Phase 1
    </output>
  </phase>

  <phase id="1" name="Core Logic Implementation">
    <purpose>Implement testable game logic using TDD</purpose>
    <component_priority>
      1. Data models (no dependencies)
      2. Utility classes (no dependencies)
      3. Core game logic (depends on models)
      4. State management (depends on logic)
      5. Managers and services (depends on state)
    </component_priority>

    <tdd_cycle>
      For each component:

      <step id="1.1">Write Test (RED phase):
        <substeps>
          <substep>Create test file: res://tests/unit/test_[ComponentName].gd</substep>
          <substep>Extend GutTest</substep>
          <substep>Write test method: test_[behavior]()</substep>
          <substep>Test MUST fail (component doesn't exist yet)</substep>
          <substep>Run test suite: verify failure</substep>
        </substeps>
      </step>

      <step id="1.2">Implement Code (GREEN phase):
        <substeps>
          <substep>Create implementation file per architecture</substep>
          <substep>Write minimal code to pass the test</substep>
          <substep>No extra features</substep>
          <substep>Run test suite: verify test now passes</substep>
        </substeps>
      </step>

      <step id="1.3">Refactor (REFACTOR phase):
        <substeps>
          <substep>Clean up code without changing behavior</substep>
          <substep>Extract methods, improve names, remove duplication</substep>
          <substep>Run test suite: verify still passes</substep>
        </substeps>
      </step>

      <step id="1.4">Document and Track:
        <substeps>
          <substep>Update progress.yaml (component status)</substep>
          <substep>Update test-coverage.yaml</substep>
          <substep>Update component-inventory.yaml</substep>
          <substep>Document next_action</substep>
        </substeps>
      </step>

      <step id="1.5">Commit:
        <substeps>
          <substep>Git add files</substep>
          <substep>Commit with message: "[Component]: [What was implemented]"</substep>
        </substeps>
      </step>

      Repeat cycle for each test case until component is fully implemented
      Repeat for each component in priority order
    </tdd_cycle>

    <testing_guidelines>
      <guideline name="Test Independence">
        Each test should be independent and repeatable
      </guideline>
      <guideline name="Arrange-Act-Assert">
        Structure tests in three phases: setup, action, verification
      </guideline>
      <guideline name="Test One Behavior">
        Each test verifies one specific behavior
      </guideline>
      <guideline name="Descriptive Names">
        test_[method]_[scenario]_[expectedResult]
        Example: test_board_when_full_returns_true()
      </guideline>
      <guideline name="Test Edge Cases">
        Test boundaries, nulls, empty states, invalid inputs
      </guideline>
    </testing_guidelines>

    <output>
      - All core logic components implemented
      - High unit test coverage (>80%)
      - All tests passing
      - Clean, refactored code
    </output>
  </phase>

  <phase id="2" name="Scene Assembly">
    <purpose>Create Godot scenes and attach scripts using TDD</purpose>
    <approach>
      For Node-based components (scripts attached to scene nodes):
      - Use integration tests (scene + script)
      - Test script behavior through scene
      - Mock/stub dependencies where possible
    </approach>

    <scene_workflow>
      For each scene from architecture:

      <step id="2.1">Create Integration Test:
        <substeps>
          <substep>Create test file: res://tests/integration/test_[SceneName].gd</substep>
          <substep>Load scene in test: preload("res://scenes/[Scene].tscn").instantiate()</substep>
          <substep>Test scene structure (nodes exist)</substep>
          <substep>Test script behaviors</substep>
          <substep>Test signal connections</substep>
        </substeps>
      </step>

      <step id="2.2">Create Scene:
        <substeps>
          <substep>Create scene file per architecture</substep>
          <substep>Add nodes per design</substep>
          <substep>Attach scripts (already implemented from Phase 1)</substep>
          <substep>Configure node properties</substep>
          <substep>Connect signals</substep>
        </substeps>
      </step>

      <step id="2.3">Verify Integration:
        <substeps>
          <substep>Run integration tests</substep>
          <substep>Test scene loads correctly</substep>
          <substep>Test behaviors work in scene context</substep>
          <substep>Fix any integration issues</substep>
        </substeps>
      </step>

      <step id="2.4">Document and Track:
        <substeps>
          <substep>Update progress.yaml (scene status)</substep>
          <substep>Update integration-checklist.yaml</substep>
          <substep>Document next_action</substep>
        </substeps>
      </step>

      <step id="2.5">Commit:
        <substeps>
          <substep>Git add scene and related files</substep>
          <substep>Commit: "[Scene]: Created and integrated [SceneName]"</substep>
        </substeps>
      </step>
    </scene_workflow>

    <output>
      - All scenes created per architecture
      - Scripts integrated into scenes
      - Integration tests passing
      - Scenes functional in editor
    </output>
  </phase>

  <phase id="3" name="UI Implementation">
    <purpose>Implement UI screens per GDD specifications</purpose>
    <ui_tdd_approach>
      UI testing focuses on:
      - Layout correctness (nodes exist in right positions)
      - Button connections (signals work)
      - State updates (UI reflects game state)
      - User interaction flows
    </ui_tdd_approach>

    <ui_workflow>
      For each screen from GDD:

      <step id="3.1">Create UI Test:
        <substeps>
          <substep>Test layout (all UI elements exist)</substep>
          <substep>Test button connections</substep>
          <substep>Test state → UI updates</substep>
          <substep>Test user interactions trigger correct behaviors</substep>
        </substeps>
      </step>

      <step id="3.2">Implement UI:
        <substeps>
          <substep>Create Control nodes per GDD layout</substep>
          <substep>Position and style elements</substep>
          <substep>Connect buttons to handlers</substep>
          <substep>Implement UI update logic</substep>
        </substeps>
      </step>

      <step id="3.3">Verify and Polish:
        <substeps>
          <substep>Run UI tests</substep>
          <substep>Manual test in editor (for visual polish)</substep>
          <substep>Verify against GDD mockups</substep>
        </substeps>
      </step>

      <step id="3.4">Document and Track:
        <substeps>
          <substep>Update progress.yaml</substep>
          <substep>Update integration-checklist.yaml</substep>
          <substep>Document next_action</substep>
        </substeps>
      </step>

      <step id="3.5">Commit</step>
    </ui_workflow>

    <output>
      - All UI screens implemented
      - UI matches GDD specifications
      - UI tests passing
      - Visual polish applied
    </output>
  </phase>

  <phase id="4" name="Integration and End-to-End Testing">
    <purpose>Verify all components work together correctly</purpose>
    <integration_testing>
      <test_type name="Component Integration">
        Test that components interact correctly:
        - GameManager → Board
        - Board → UI
        - StateManager → SaveManager
      </test_type>

      <test_type name="Scene Transitions">
        Test full flows:
        - Main Menu → Game → Game Over → Main Menu
        - Settings → change setting → return
      </test_type>

      <test_type name="End-to-End">
        Test complete player journeys:
        - Launch → Play full game → Win → Return to menu
        - Launch → Play → Lose → Retry
      </test_type>
    </integration_testing>

    <e2e_workflow>
      <step id="4.1">Create E2E Tests:
        <substeps>
          <substep>Identify critical player paths from GDD</substep>
          <substep>Write E2E tests simulating player actions</substep>
          <substep>Tests should cover happy paths and error paths</substep>
        </substeps>
      </step>

      <step id="4.2">Run and Fix:
        <substeps>
          <substep>Run E2E test suite</substep>
          <substep>Identify integration issues</substep>
          <substep>Fix issues (with unit tests if logic bugs)</substep>
          <substep>Re-run until all E2E tests pass</substep>
        </substeps>
      </step>

      <step id="4.3">Document and Track:
        <substeps>
          <substep>Update progress.yaml</substep>
          <substep>Update integration-checklist.yaml (all green)</substep>
          <substep>Document next_action</substep>
        </substeps>
      </step>
    </e2e_workflow>

    <output>
      - All integration tests passing
      - All E2E tests passing
      - Game fully functional
      - Ready for polish
    </output>
  </phase>

  <phase id="5" name="Polish and Assets">
    <purpose>Integrate final assets and polish game feel</purpose>
    <asset_integration>
      <step id="5.1">Replace Placeholders:
        <substeps>
          <substep>Review asset-integration.yaml</substep>
          <substep>For each placeholder asset:
            - Replace with final asset
            - Update asset-integration.yaml status
            - Test game with new asset
          </substep>
        </substeps>
      </step>

      <step id="5.2">Polish Gameplay:
        <substeps>
          <substep>Tune game feel (timings, feedback)</substep>
          <substep>Add juice (particle effects, screen shake, etc.)</substep>
          <substep>Add audio feedback</substep>
          <substep>Verify against GDD specifications</substep>
        </substeps>
      </step>

      <step id="5.3">Final Testing:
        <substeps>
          <substep>Run full test suite</substep>
          <substep>Manual playthrough</substep>
          <substep>Performance profiling</substep>
          <substep>Fix any issues found</substep>
        </substeps>
      </step>

      <step id="5.4">Documentation:
        <substeps>
          <substep>Update progress.yaml to Complete</substep>
          <substep>Final commit: "Polish and asset integration complete"</substep>
          <substep>Document build instructions</substep>
        </substeps>
      </step>
    </asset_integration>

    <output>
      - All assets integrated
      - Game polished and feature-complete
      - All tests passing
      - Ready for deployment
    </output>
  </phase>

  <phase id="6" name="Build and Deployment">
    <purpose>Create distributable builds</purpose>
    <build_process>
      <step id="6.1">Configure Export:
        <substeps>
          <substep>Set up export presets for target platforms</substep>
          <substep>Configure build settings</substep>
          <substep>Test export locally</substep>
        </substeps>
      </step>

      <step id="6.2">CI/CD Setup (Optional):
        <substeps>
          <substep>Create CI/CD pipeline (GitHub Actions, GitLab CI, etc.)</substep>
          <substep>Automate test running on commit</substep>
          <substep>Automate builds for releases</substep>
        </substeps>
      </step>

      <step id="6.3">Build and Verify:
        <substeps>
          <substep>Export builds for all platforms</substep>
          <substep>Test each build</substep>
          <substep>Document build artifacts</substep>
        </substeps>
      </step>
    </build_process>

    <output>
      - Distributable builds created
      - CI/CD pipeline (if configured)
      - Build documentation
      - Project complete!
    </output>
  </phase>
</methodology>

<godot_tdd_specifics>
  <gut_usage>
    <installing>
      1. Open Godot AssetLib
      2. Search for "GUT" (Godot Unit Test)
      3. Install version 9.x (for Godot 4.x)
      OR manually download from GitHub: https://github.com/bitwes/Gut
    </installing>

    <test_structure>
```gdscript
# res://tests/unit/test_Board.gd
extends GutTest

# Called before each test
func before_each():
    # Setup code
    pass

# Called after each test
func after_each():
    # Cleanup code
    pass

# Test method - must start with "test_"
func test_board_initializes_empty():
    # Arrange
    var board = Board.new()

    # Act
    var is_empty = board.is_empty()

    # Assert
    assert_true(is_empty, "Board should be empty on initialization")

func test_board_place_mark_succeeds():
    # Arrange
    var board = Board.new()

    # Act
    var result = board.place_mark(0, 0, Board.Mark.X)

    # Assert
    assert_true(result, "Placing mark in empty cell should succeed")
    assert_eq(board.get_mark(0, 0), Board.Mark.X, "Mark should be X")
```
    </test_structure>

    <running_tests>
      In Editor:
      - Run GUT panel (bottom panel)
      - Click "Run All" or select specific test

      From Command Line:
      ```bash
      godot --headless --path . --script res://addons/gut/gut_cmdln.gd
      ```
    </running_tests>

    <assertion_methods>
      - assert_true(condition, message)
      - assert_false(condition, message)
      - assert_eq(actual, expected, message)
      - assert_ne(actual, expected, message)
      - assert_null(value, message)
      - assert_not_null(value, message)
      - assert_gt(value, expected, message) # greater than
      - assert_lt(value, expected, message) # less than
      - assert_has(container, item, message)
      - assert_does_not_have(container, item, message)
    </assertion_methods>
  </gut_usage>

  <testing_patterns>
    <pattern name="Testing Plain Classes">
```gdscript
# Easy - just instantiate and test
func test_game_state_new():
    var state = GameState.new()
    assert_eq(state.current_turn, 0)
```
    </pattern>

    <pattern name="Testing Node Scripts">
```gdscript
# Need scene context
func test_player_controller_movement():
    var scene = preload("res://scenes/Player.tscn").instantiate()
    add_child_autofree(scene) # GUT helper
    var player = scene

    player.move_right()

    assert_gt(player.position.x, 0)
```
    </pattern>

    <pattern name="Testing Signals">
```gdscript
func test_board_emits_win_signal():
    var board = Board.new()
    watch_signals(board) # GUT helper

    board.check_win()

    assert_signal_emitted(board, "game_won")
```
    </pattern>

    <pattern name="Mocking Dependencies">
```gdscript
# Use doubles for dependencies
func test_game_manager_starts_game():
    var mock_board = double(Board).new()
    var manager = GameManager.new()
    manager.board = mock_board

    manager.start_game()

    assert_called(mock_board, "reset")
```
    </pattern>
  </testing_patterns>

  <project_structure>
```
res://
├── addons/
│   └── gut/                      # GUT plugin
├── scenes/
│   ├── Main.tscn
│   ├── MainMenu.tscn
│   └── Game.tscn
├── scripts/
│   ├── core/
│   │   ├── GameManager.gd
│   │   └── StateManager.gd
│   ├── game/
│   │   ├── Board.gd
│   │   └── Player.gd
│   └── ui/
│       └── MenuButton.gd
├── tests/
│   ├── unit/
│   │   ├── test_Board.gd
│   │   ├── test_GameState.gd
│   │   └── test_GameManager.gd
│   ├── integration/
│   │   ├── test_Game.gd
│   │   └── test_MainMenu.gd
│   └── e2e/
│       └── test_FullGameFlow.gd
├── assets/
│   ├── sprites/
│   ├── audio/
│   └── fonts/
└── .work/
    └── tdd-development/
        ├── progress.yaml
        ├── component-inventory.yaml
        ├── test-coverage.yaml
        ├── integration-checklist.yaml
        ├── asset-integration.yaml
        └── build-status.yaml
```
  </project_structure>
</godot_tdd_specifics>

<output_specifications>
  <directory_structure>
    [As shown in godot_tdd_specifics above]
  </directory_structure>

  <deliverables>
    <deliverable name="Complete Game Implementation">
      - All scenes from architecture implemented
      - All scripts from architecture implemented
      - All UI from GDD implemented
      - All assets integrated
    </deliverable>

    <deliverable name="Comprehensive Test Suite">
      - Unit tests for all logic components (>80% coverage)
      - Integration tests for all scenes
      - E2E tests for critical paths
      - All tests passing
    </deliverable>

    <deliverable name="Documentation">
      - Code comments for complex logic
      - Build instructions
      - Test running instructions
      - .work/ tracking files (progress history)
    </deliverable>

    <deliverable name="Builds">
      - Exportable project
      - Builds for target platforms (if specified)
      - CI/CD setup (if specified)
    </deliverable>
  </deliverables>
</output_specifications>

<critical_reminders>
================================================================================
                    CRITICAL REMINDERS
================================================================================

1. **STATE IN FILES, NOT CONTEXT**
   - progress.yaml is truth
   - Context WILL compact during this multi-week effort
   - Checkpoint after every component

2. **CHECK BEFORE STARTING**
   - Always read progress.yaml first
   - Resume from next_action if exists
   - Run test suite to verify environment
   - Never restart completed work

3. **RED-GREEN-REFACTOR ALWAYS**
   - Write failing test FIRST
   - Implement minimal code to pass
   - Refactor with tests as safety net
   - NO production code without tests

4. **COMPLETE BEFORE MOVING ON**
   - Finish one component before starting another
   - Ensure all tests pass
   - Update progress.yaml
   - Document next action

5. **TEST SUITE HEALTH**
   - Run tests frequently
   - Keep all tests passing
   - Fix failing tests immediately
   - Never commit broken tests

6. **ARCHITECTURE ADHERENCE**
   - Follow architecture document exactly
   - Don't deviate without documenting why
   - Reference architecture for component design

7. **GDD COMPLIANCE**
   - Implement exactly what GDD specifies
   - Don't add features not in GDD
   - Don't skip GDD requirements

8. **COMMIT FREQUENTLY**
   - Commit after each component
   - Commit after each test passes
   - Meaningful commit messages
   - Git history tells the story

9. **GODOT BEST PRACTICES**
   - Follow Godot conventions
   - Use signals for decoupling
   - Separate logic from scenes where possible
   - Use Resources for data

10. **PROGRESS TRACKING IS LIFE**
    - Update .work/ files religiously
    - You WILL be resumed after compaction
    - next_action must be crystal clear
    - Future you depends on present you documenting well

</critical_reminders>

<begin>
=====================================
CRITICAL: CHECK FOR EXISTING PROGRESS FIRST
=====================================
This TDD work WILL span multiple sessions and context compactions.

FIRST ACTION - Check for existing progress:
```bash
cat .work/tdd-development/progress.yaml 2>/dev/null || echo "NO_PROGRESS_FILE"
```

IF progress file exists:
- Read current_phase, current_component, next_action
- Resume from where you left off
- Load component-inventory.yaml to see full scope
- Run test suite to verify environment
- Continue from next_action

IF no progress file (fresh start):
- Proceed with Phase 0 (Planning and Setup)
- Create .work/tdd-development/ directory structure first

=====================================
CRITICAL: COMPACTION SURVIVAL
=====================================
This work WILL span multiple context compactions over days/weeks.

ALWAYS:
- Write progress to .work/tdd-development/progress.yaml after each component
- Update component-inventory.yaml as components are completed
- Complete one component fully before starting another
- Document next_action clearly for resumption (specific component, specific test)
- Run test suite and record status in build-status.yaml
- Commit code frequently

=====================================
BEGIN TDD DEVELOPMENT
=====================================

FIRST: Check for existing progress (see command above)

IF resuming: Follow next_action from progress.yaml

IF fresh start:
1. Create .work/tdd-development/ directory structure
2. Install and verify GUT
3. Read requirements, architecture, and GDD documents
4. Create component-inventory.yaml (from architecture)
5. Create asset-integration.yaml (from GDD)
6. Initialize progress.yaml
7. Proceed with Phase 1 (Core Logic Implementation)

TDD CYCLE FOR EACH COMPONENT:
1. Write failing test (RED)
2. Implement minimal code (GREEN)
3. Refactor (REFACTOR)
4. Update progress.yaml
5. Commit
6. Move to next component

VALIDATION CHECKPOINTS:
- After each component: All unit tests pass
- After each scene: Integration tests pass
- After each phase: Full test suite passes
- Before moving to next phase: Progress documented

OUTPUT GOAL:
- Complete, tested, functional game
- High test coverage (>80%)
- Clean, maintainable code
- All tests passing
- Ready for deployment

BEGIN NOW by checking for progress, then starting Phase 0 or resuming.

</begin>
```

---

## Usage Notes

- **Timeframe**: Multiple weeks (depends on game complexity)
- **Sessions**: Will span many development sessions
- **Dependencies**:
  - docs/requirements/REQUIREMENTS.md
  - docs/architecture/ARCHITECTURE.md
  - docs/gdd/GDD.md
- **Output**: Complete, tested, deployable game
- **Next Phase**: Deployment, marketing, updates (outside this prompt)

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-01-16 | Initial TDD development prompt for Godot games |

---

## Related Documents

- STD-0001-prompt-creation-rubric.md - Prompt patterns (fully applied here)
- STD-0002-csharp-rubric.md - C# standards (if using C#)
- STD-0100-requirements-gathering.md - Requirements phase
- STD-0101-architecture-design.md - Architecture phase
- STD-0102-game-design-document.md - GDD phase
- GUT Documentation: https://github.com/bitwes/Gut/wiki
- Godot Testing Best Practices: https://docs.godotengine.org/en/stable/tutorials/scripting/unit_testing.html
