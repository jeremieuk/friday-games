extends GutTest
# Test suite for GameManager
# Tests the state machine and game orchestration

var GameManager = load("res://scripts/managers/GameManager.cs")
var _manager  # Instance for each test

# C# enum values (GDScript can't access nested C# enums directly)
# Choice enum
const ROCK = 0
const PAPER = 1
const SCISSORS = 2
# Result enum
const PLAYER_WINS = 0
const AI_WINS = 1
const DRAW = 2
# GameState enum
const STATE_MAIN_MENU = 0
const STATE_MODE_SELECTION = 1
const STATE_PLAYING = 2
const STATE_GAME_OVER = 3

func before_each():
	_manager = GameManager.new()

func after_each():
	if _manager:
		_manager.free()
		_manager = null

func test_initial_state_is_main_menu():
	assert_eq(_manager.GetCurrentState(), STATE_MAIN_MENU,
		"Initial state should be MainMenu")

func test_transition_to_playing():
	_manager.StartSingleMatch()
	assert_eq(_manager.GetCurrentState(), STATE_PLAYING,
		"State should transition to Playing after StartSingleMatch")

func test_scores_start_at_zero():
	_manager.StartSingleMatch()

	var initial_scores = _manager.GetCurrentScores()
	assert_eq(initial_scores["player"], 0, "Player score should start at 0")
	assert_eq(initial_scores["ai"], 0, "AI score should start at 0")

func test_play_round_with_player_win():
	_manager.StartSingleMatch()

	var outcome = _manager.PlayRound(ROCK, SCISSORS)

	assert_eq(outcome["result"], PLAYER_WINS,
		"Player should win Rock vs Scissors")
	assert_eq(outcome["playerScore"], 1, "Player score should be 1")
	assert_eq(outcome["aiScore"], 0, "AI score should be 0")
	assert_eq(outcome["playerChoice"], ROCK, "Player choice should be Rock")
	assert_eq(outcome["aiChoice"], SCISSORS, "AI choice should be Scissors")
	assert_false(outcome["isMatchComplete"], "Match should not be complete after 1 round")

func test_play_round_with_ai_win():
	_manager.StartSingleMatch()

	var outcome = _manager.PlayRound(ROCK, PAPER)

	assert_eq(outcome["result"], AI_WINS,
		"AI should win Paper vs Rock")
	assert_eq(outcome["playerScore"], 0, "Player score should be 0")
	assert_eq(outcome["aiScore"], 1, "AI score should be 1")
	assert_eq(outcome["playerChoice"], ROCK, "Player choice should be Rock")
	assert_eq(outcome["aiChoice"], PAPER, "AI choice should be Paper")

func test_play_round_with_draw():
	_manager.StartSingleMatch()

	var outcome = _manager.PlayRound(ROCK, ROCK)

	assert_eq(outcome["result"], DRAW, "Should be a draw")
	assert_eq(outcome["playerScore"], 0, "Scores should not change on draw")
	assert_eq(outcome["aiScore"], 0, "Scores should not change on draw")
	assert_eq(outcome["playerChoice"], ROCK, "Player choice should be Rock")
	assert_eq(outcome["aiChoice"], ROCK, "AI choice should be Rock")

func test_match_completes_after_player_wins_two():
	_manager.StartSingleMatch()

	# Player wins round 1
	var outcome1 = _manager.PlayRound(ROCK, SCISSORS)
	assert_false(outcome1["isMatchComplete"], "Match continues after 1 win")
	assert_eq(_manager.GetCurrentState(), STATE_PLAYING,
		"State should remain Playing")

	# Player wins round 2
	var outcome2 = _manager.PlayRound(PAPER, ROCK)
	assert_eq(outcome2["playerChoice"], PAPER, "Player choice should be Paper")
	assert_eq(outcome2["aiChoice"], ROCK, "AI choice should be Rock")
	assert_true(outcome2["isMatchComplete"], "Match should be complete after 2 wins")
	assert_eq(_manager.GetCurrentState(), STATE_GAME_OVER,
		"State should transition to GameOver")

func test_match_completes_after_ai_wins_two():
	_manager.StartSingleMatch()

	# AI wins round 1
	_manager.PlayRound(ROCK, PAPER)
	# AI wins round 2
	var outcome = _manager.PlayRound(SCISSORS, ROCK)

	assert_eq(outcome["playerChoice"], SCISSORS, "Player choice should be Scissors")
	assert_eq(outcome["aiChoice"], ROCK, "AI choice should be Rock")
	assert_true(outcome["isMatchComplete"], "Match should be complete")
	assert_eq(_manager.GetCurrentState(), STATE_GAME_OVER,
		"State should be GameOver")

func test_best_of_three_scenario():
	_manager.StartSingleMatch()

	# Round 1: Player wins
	var outcome1 = _manager.PlayRound(ROCK, SCISSORS)
	assert_eq(outcome1["playerChoice"], ROCK)
	assert_eq(outcome1["aiChoice"], SCISSORS)
	assert_eq(outcome1["playerScore"], 1)
	assert_eq(outcome1["aiScore"], 0)
	assert_false(outcome1["isMatchComplete"])

	# Round 2: AI wins
	var outcome2 = _manager.PlayRound(ROCK, PAPER)
	assert_eq(outcome2["playerChoice"], ROCK)
	assert_eq(outcome2["aiChoice"], PAPER)
	assert_eq(outcome2["playerScore"], 1)
	assert_eq(outcome2["aiScore"], 1)
	assert_false(outcome2["isMatchComplete"])

	# Round 3: Player wins (match complete)
	var outcome3 = _manager.PlayRound(PAPER, ROCK)
	assert_eq(outcome3["playerChoice"], PAPER)
	assert_eq(outcome3["aiChoice"], ROCK)
	assert_eq(outcome3["playerScore"], 2)
	assert_eq(outcome3["aiScore"], 1)
	assert_true(outcome3["isMatchComplete"])
	assert_eq(_manager.GetCurrentState(), STATE_GAME_OVER)

func test_draws_dont_advance_match():
	_manager.StartSingleMatch()

	# Multiple draws
	_manager.PlayRound(ROCK, ROCK)
	_manager.PlayRound(PAPER, PAPER)
	var outcome = _manager.PlayRound(SCISSORS, SCISSORS)

	assert_eq(outcome["playerChoice"], SCISSORS)
	assert_eq(outcome["aiChoice"], SCISSORS)
	assert_eq(outcome["playerScore"], 0, "Draws should not increase scores")
	assert_eq(outcome["aiScore"], 0, "Draws should not increase scores")
	assert_false(outcome["isMatchComplete"], "Draws should not complete match")

func test_play_round_with_ai_choice():
	_manager.StartSingleMatch()

	# PlayRound should accept player choice only and let AI make its own choice
	var outcome = _manager.PlayRoundWithAI(ROCK)

	assert_eq(outcome["playerChoice"], ROCK, "Player choice should be Rock")
	assert_true(
		outcome["aiChoice"] == ROCK or
		outcome["aiChoice"] == PAPER or
		outcome["aiChoice"] == SCISSORS,
		"AI choice should be valid"
	)
	assert_true(
		outcome["result"] == PLAYER_WINS or
		outcome["result"] == AI_WINS or
		outcome["result"] == DRAW,
		"Result should be valid"
	)

func test_reset_match():
	_manager.StartSingleMatch()

	# Play some rounds
	_manager.PlayRound(ROCK, SCISSORS)
	_manager.PlayRound(PAPER, ROCK)

	# Reset
	_manager.StartSingleMatch()

	var scores = _manager.GetCurrentScores()
	assert_eq(scores["player"], 0, "Scores should reset")
	assert_eq(scores["ai"], 0, "Scores should reset")
	assert_eq(_manager.GetCurrentState(), STATE_PLAYING,
		"State should be Playing after reset")
