extends GutTest
# Test suite for ScoreTracker
# Tests best-of-3 match scoring logic

var ScoreTracker = load("res://scripts/managers/ScoreTracker.cs")

func test_initial_scores_are_zero():
	var tracker = ScoreTracker.new()
	assert_eq(tracker.GetPlayerScore(), 0, "Player score should start at 0")
	assert_eq(tracker.GetAIScore(), 0, "AI score should start at 0")

func test_record_player_win():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	assert_eq(tracker.GetPlayerScore(), 1, "Player score should be 1 after win")
	assert_eq(tracker.GetAIScore(), 0, "AI score should still be 0")

func test_record_ai_win():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.AIWins)
	assert_eq(tracker.GetPlayerScore(), 0, "Player score should still be 0")
	assert_eq(tracker.GetAIScore(), 1, "AI score should be 1 after win")

func test_record_draw():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.Draw)
	assert_eq(tracker.GetPlayerScore(), 0, "Player score should stay 0 after draw")
	assert_eq(tracker.GetAIScore(), 0, "AI score should stay 0 after draw")

func test_multiple_rounds():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	tracker.RecordRound(ScoreTracker.RoundResult.AIWins)
	tracker.RecordRound(ScoreTracker.RoundResult.Draw)
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)

	assert_eq(tracker.GetPlayerScore(), 2, "Player should have 2 wins")
	assert_eq(tracker.GetAIScore(), 1, "AI should have 1 win")

func test_match_not_complete_initially():
	var tracker = ScoreTracker.new()
	assert_false(tracker.IsMatchComplete(), "Match should not be complete at start")

func test_match_not_complete_with_one_win():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	assert_false(tracker.IsMatchComplete(), "Match should not be complete with 1 win")

func test_match_complete_when_player_wins_two():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	assert_true(tracker.IsMatchComplete(), "Match should be complete when player has 2 wins")

func test_match_complete_when_ai_wins_two():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.AIWins)
	tracker.RecordRound(ScoreTracker.RoundResult.AIWins)
	assert_true(tracker.IsMatchComplete(), "Match should be complete when AI has 2 wins")

func test_winner_is_none_initially():
	var tracker = ScoreTracker.new()
	assert_eq(tracker.GetMatchWinner(), ScoreTracker.MatchWinner.None,
		"Winner should be None at start")

func test_winner_is_player_after_two_wins():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	assert_eq(tracker.GetMatchWinner(), ScoreTracker.MatchWinner.Player,
		"Winner should be Player after 2 wins")

func test_winner_is_ai_after_two_wins():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.AIWins)
	tracker.RecordRound(ScoreTracker.RoundResult.AIWins)
	assert_eq(tracker.GetMatchWinner(), ScoreTracker.MatchWinner.AI,
		"Winner should be AI after 2 wins")

func test_draws_dont_count_toward_score():
	var tracker = ScoreTracker.new()
	tracker.RecordRound(ScoreTracker.RoundResult.Draw)
	tracker.RecordRound(ScoreTracker.RoundResult.Draw)
	tracker.RecordRound(ScoreTracker.RoundResult.Draw)
	assert_eq(tracker.GetPlayerScore(), 0, "Draws should not increase player score")
	assert_eq(tracker.GetAIScore(), 0, "Draws should not increase AI score")
	assert_false(tracker.IsMatchComplete(), "Draws should not complete match")

func test_best_of_three_scenario():
	# Simulate: Player wins, AI wins, Player wins
	var tracker = ScoreTracker.new()

	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	assert_false(tracker.IsMatchComplete(), "Match continues after first round")

	tracker.RecordRound(ScoreTracker.RoundResult.AIWins)
	assert_false(tracker.IsMatchComplete(), "Match continues with tie score")

	tracker.RecordRound(ScoreTracker.RoundResult.PlayerWins)
	assert_true(tracker.IsMatchComplete(), "Match completes when player reaches 2")
	assert_eq(tracker.GetMatchWinner(), ScoreTracker.MatchWinner.Player,
		"Player should be winner")
