extends GutTest
# Test suite for ScoreTracker
# Tests best-of-3 match scoring logic

var ScoreTracker = load("res://scripts/managers/ScoreTracker.cs")
var _tracker  # Instance for each test

# C# enum values (GDScript can't access nested C# enums directly)
# RoundResult enum
const ROUND_PLAYER_WINS = 0
const ROUND_AI_WINS = 1
const ROUND_DRAW = 2
# MatchWinner enum
const WINNER_NONE = 0
const WINNER_PLAYER = 1
const WINNER_AI = 2

func before_each():
	_tracker = ScoreTracker.new()

func after_each():
	# ScoreTracker extends RefCounted - just set to null, GC handles cleanup
	_tracker = null

func test_initial_scores_are_zero():
	assert_eq(_tracker.GetPlayerScore(), 0, "Player score should start at 0")
	assert_eq(_tracker.GetAIScore(), 0, "AI score should start at 0")

func test_record_player_win():
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	assert_eq(_tracker.GetPlayerScore(), 1, "Player score should be 1 after win")
	assert_eq(_tracker.GetAIScore(), 0, "AI score should still be 0")

func test_record_ai_win():
	_tracker.RecordRound(ROUND_AI_WINS)
	assert_eq(_tracker.GetPlayerScore(), 0, "Player score should still be 0")
	assert_eq(_tracker.GetAIScore(), 1, "AI score should be 1 after win")

func test_record_draw():
	_tracker.RecordRound(ROUND_DRAW)
	assert_eq(_tracker.GetPlayerScore(), 0, "Player score should stay 0 after draw")
	assert_eq(_tracker.GetAIScore(), 0, "AI score should stay 0 after draw")

func test_multiple_rounds():
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	_tracker.RecordRound(ROUND_AI_WINS)
	_tracker.RecordRound(ROUND_DRAW)
	_tracker.RecordRound(ROUND_PLAYER_WINS)

	assert_eq(_tracker.GetPlayerScore(), 2, "Player should have 2 wins")
	assert_eq(_tracker.GetAIScore(), 1, "AI should have 1 win")

func test_match_not_complete_initially():
	assert_false(_tracker.IsMatchComplete(), "Match should not be complete at start")

func test_match_not_complete_with_one_win():
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	assert_false(_tracker.IsMatchComplete(), "Match should not be complete with 1 win")

func test_match_complete_when_player_wins_two():
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	assert_true(_tracker.IsMatchComplete(), "Match should be complete when player has 2 wins")

func test_match_complete_when_ai_wins_two():
	_tracker.RecordRound(ROUND_AI_WINS)
	_tracker.RecordRound(ROUND_AI_WINS)
	assert_true(_tracker.IsMatchComplete(), "Match should be complete when AI has 2 wins")

func test_winner_is_none_initially():
	assert_eq(_tracker.GetMatchWinner(), WINNER_NONE,
		"Winner should be None at start")

func test_winner_is_player_after_two_wins():
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	assert_eq(_tracker.GetMatchWinner(), WINNER_PLAYER,
		"Winner should be Player after 2 wins")

func test_winner_is_ai_after_two_wins():
	_tracker.RecordRound(ROUND_AI_WINS)
	_tracker.RecordRound(ROUND_AI_WINS)
	assert_eq(_tracker.GetMatchWinner(), WINNER_AI,
		"Winner should be AI after 2 wins")

func test_draws_dont_count_toward_score():
	_tracker.RecordRound(ROUND_DRAW)
	_tracker.RecordRound(ROUND_DRAW)
	_tracker.RecordRound(ROUND_DRAW)
	assert_eq(_tracker.GetPlayerScore(), 0, "Draws should not increase player score")
	assert_eq(_tracker.GetAIScore(), 0, "Draws should not increase AI score")
	assert_false(_tracker.IsMatchComplete(), "Draws should not complete match")

func test_best_of_three_scenario():
	# Simulate: Player wins, AI wins, Player wins
	_tracker.RecordRound(ROUND_PLAYER_WINS)
	assert_false(_tracker.IsMatchComplete(), "Match continues after first round")

	_tracker.RecordRound(ROUND_AI_WINS)
	assert_false(_tracker.IsMatchComplete(), "Match continues with tie score")

	_tracker.RecordRound(ROUND_PLAYER_WINS)
	assert_true(_tracker.IsMatchComplete(), "Match completes when player reaches 2")
	assert_eq(_tracker.GetMatchWinner(), WINNER_PLAYER,
		"Player should be winner")
