extends GutTest
# Test suite for GameManager
# Tests the state machine and game orchestration

var GameManager = load("res://scripts/managers/GameManager.cs")
var GameLogic = load("res://scripts/core/GameLogic.cs")

func test_initial_state_is_main_menu():
	var manager = GameManager.new()
	assert_eq(manager.GetCurrentState(), GameManager.GameState.MainMenu,
		"Initial state should be MainMenu")

func test_transition_to_playing():
	var manager = GameManager.new()
	manager.StartSingleMatch()
	assert_eq(manager.GetCurrentState(), GameManager.GameState.Playing,
		"State should transition to Playing after StartSingleMatch")

func test_scores_start_at_zero():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	var initial_scores = manager.GetCurrentScores()
	assert_eq(initial_scores["player"], 0, "Player score should start at 0")
	assert_eq(initial_scores["ai"], 0, "AI score should start at 0")

func test_play_round_with_player_win():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	var outcome = manager.PlayRound(
		GameLogic.Choice.Rock,
		GameLogic.Choice.Scissors
	)

	assert_eq(outcome["result"], GameLogic.Result.PlayerWins,
		"Player should win Rock vs Scissors")
	assert_eq(outcome["playerScore"], 1, "Player score should be 1")
	assert_eq(outcome["aiScore"], 0, "AI score should be 0")
	assert_eq(outcome["playerChoice"], GameLogic.Choice.Rock, "Player choice should be Rock")
	assert_eq(outcome["aiChoice"], GameLogic.Choice.Scissors, "AI choice should be Scissors")
	assert_false(outcome["isMatchComplete"], "Match should not be complete after 1 round")

func test_play_round_with_ai_win():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	var outcome = manager.PlayRound(
		GameLogic.Choice.Rock,
		GameLogic.Choice.Paper
	)

	assert_eq(outcome["result"], GameLogic.Result.AIWins,
		"AI should win Paper vs Rock")
	assert_eq(outcome["playerScore"], 0, "Player score should be 0")
	assert_eq(outcome["aiScore"], 1, "AI score should be 1")
	assert_eq(outcome["playerChoice"], GameLogic.Choice.Rock, "Player choice should be Rock")
	assert_eq(outcome["aiChoice"], GameLogic.Choice.Paper, "AI choice should be Paper")

func test_play_round_with_draw():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	var outcome = manager.PlayRound(
		GameLogic.Choice.Rock,
		GameLogic.Choice.Rock
	)

	assert_eq(outcome["result"], GameLogic.Result.Draw, "Should be a draw")
	assert_eq(outcome["playerScore"], 0, "Scores should not change on draw")
	assert_eq(outcome["aiScore"], 0, "Scores should not change on draw")
	assert_eq(outcome["playerChoice"], GameLogic.Choice.Rock, "Player choice should be Rock")
	assert_eq(outcome["aiChoice"], GameLogic.Choice.Rock, "AI choice should be Rock")

func test_match_completes_after_player_wins_two():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	# Player wins round 1
	var outcome1 = manager.PlayRound(
		GameLogic.Choice.Rock,
		GameLogic.Choice.Scissors
	)
	assert_false(outcome1["isMatchComplete"], "Match continues after 1 win")
	assert_eq(manager.GetCurrentState(), GameManager.GameState.Playing,
		"State should remain Playing")

	# Player wins round 2
	var outcome2 = manager.PlayRound(
		GameLogic.Choice.Paper,
		GameLogic.Choice.Rock
	)
	assert_eq(outcome2["playerChoice"], GameLogic.Choice.Paper, "Player choice should be Paper")
	assert_eq(outcome2["aiChoice"], GameLogic.Choice.Rock, "AI choice should be Rock")
	assert_true(outcome2["isMatchComplete"], "Match should be complete after 2 wins")
	assert_eq(manager.GetCurrentState(), GameManager.GameState.GameOver,
		"State should transition to GameOver")

func test_match_completes_after_ai_wins_two():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	# AI wins round 1
	manager.PlayRound(GameLogic.Choice.Rock, GameLogic.Choice.Paper)
	# AI wins round 2
	var outcome = manager.PlayRound(GameLogic.Choice.Scissors, GameLogic.Choice.Rock)

	assert_eq(outcome["playerChoice"], GameLogic.Choice.Scissors, "Player choice should be Scissors")
	assert_eq(outcome["aiChoice"], GameLogic.Choice.Rock, "AI choice should be Rock")
	assert_true(outcome["isMatchComplete"], "Match should be complete")
	assert_eq(manager.GetCurrentState(), GameManager.GameState.GameOver,
		"State should be GameOver")

func test_best_of_three_scenario():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	# Round 1: Player wins
	var outcome1 = manager.PlayRound(GameLogic.Choice.Rock, GameLogic.Choice.Scissors)
	assert_eq(outcome1["playerChoice"], GameLogic.Choice.Rock)
	assert_eq(outcome1["aiChoice"], GameLogic.Choice.Scissors)
	assert_eq(outcome1["playerScore"], 1)
	assert_eq(outcome1["aiScore"], 0)
	assert_false(outcome1["isMatchComplete"])

	# Round 2: AI wins
	var outcome2 = manager.PlayRound(GameLogic.Choice.Rock, GameLogic.Choice.Paper)
	assert_eq(outcome2["playerChoice"], GameLogic.Choice.Rock)
	assert_eq(outcome2["aiChoice"], GameLogic.Choice.Paper)
	assert_eq(outcome2["playerScore"], 1)
	assert_eq(outcome2["aiScore"], 1)
	assert_false(outcome2["isMatchComplete"])

	# Round 3: Player wins (match complete)
	var outcome3 = manager.PlayRound(GameLogic.Choice.Paper, GameLogic.Choice.Rock)
	assert_eq(outcome3["playerChoice"], GameLogic.Choice.Paper)
	assert_eq(outcome3["aiChoice"], GameLogic.Choice.Rock)
	assert_eq(outcome3["playerScore"], 2)
	assert_eq(outcome3["aiScore"], 1)
	assert_true(outcome3["isMatchComplete"])
	assert_eq(manager.GetCurrentState(), GameManager.GameState.GameOver)

func test_draws_dont_advance_match():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	# Multiple draws
	manager.PlayRound(GameLogic.Choice.Rock, GameLogic.Choice.Rock)
	manager.PlayRound(GameLogic.Choice.Paper, GameLogic.Choice.Paper)
	var outcome = manager.PlayRound(GameLogic.Choice.Scissors, GameLogic.Choice.Scissors)

	assert_eq(outcome["playerChoice"], GameLogic.Choice.Scissors)
	assert_eq(outcome["aiChoice"], GameLogic.Choice.Scissors)
	assert_eq(outcome["playerScore"], 0, "Draws should not increase scores")
	assert_eq(outcome["aiScore"], 0, "Draws should not increase scores")
	assert_false(outcome["isMatchComplete"], "Draws should not complete match")

func test_play_round_with_ai_choice():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	# PlayRound should accept player choice only and let AI make its own choice
	var outcome = manager.PlayRoundWithAI(GameLogic.Choice.Rock)

	assert_eq(outcome["playerChoice"], GameLogic.Choice.Rock, "Player choice should be Rock")
	assert_true(
		outcome["aiChoice"] == GameLogic.Choice.Rock or
		outcome["aiChoice"] == GameLogic.Choice.Paper or
		outcome["aiChoice"] == GameLogic.Choice.Scissors,
		"AI choice should be valid"
	)
	assert_true(
		outcome["result"] == GameLogic.Result.PlayerWins or
		outcome["result"] == GameLogic.Result.AIWins or
		outcome["result"] == GameLogic.Result.Draw,
		"Result should be valid"
	)

func test_reset_match():
	var manager = GameManager.new()
	manager.StartSingleMatch()

	# Play some rounds
	manager.PlayRound(GameLogic.Choice.Rock, GameLogic.Choice.Scissors)
	manager.PlayRound(GameLogic.Choice.Paper, GameLogic.Choice.Rock)

	# Reset
	manager.StartSingleMatch()

	var scores = manager.GetCurrentScores()
	assert_eq(scores["player"], 0, "Scores should reset")
	assert_eq(scores["ai"], 0, "Scores should reset")
	assert_eq(manager.GetCurrentState(), GameManager.GameState.Playing,
		"State should be Playing after reset")
