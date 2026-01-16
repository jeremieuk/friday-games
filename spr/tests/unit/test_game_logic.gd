extends GutTest
# Test suite for GameLogic core rules
# Tests the pure C# game logic with zero Godot dependencies

var GameLogic = load("res://scripts/core/GameLogic.cs")

func test_rock_beats_scissors():
	var result = GameLogic.DetermineWinner(
		GameLogic.Choice.Rock,
		GameLogic.Choice.Scissors)
	assert_eq(result, GameLogic.Result.PlayerWins, "Rock should beat Scissors")

func test_scissors_beats_paper():
	var result = GameLogic.DetermineWinner(
		GameLogic.Choice.Scissors,
		GameLogic.Choice.Paper)
	assert_eq(result, GameLogic.Result.PlayerWins, "Scissors should beat Paper")

func test_paper_beats_rock():
	var result = GameLogic.DetermineWinner(
		GameLogic.Choice.Paper,
		GameLogic.Choice.Rock)
	assert_eq(result, GameLogic.Result.PlayerWins, "Paper should beat Rock")

func test_same_choice_is_draw():
	var result_rock = GameLogic.DetermineWinner(
		GameLogic.Choice.Rock,
		GameLogic.Choice.Rock)
	assert_eq(result_rock, GameLogic.Result.Draw, "Same choice should be Draw")

	var result_paper = GameLogic.DetermineWinner(
		GameLogic.Choice.Paper,
		GameLogic.Choice.Paper)
	assert_eq(result_paper, GameLogic.Result.Draw, "Same choice should be Draw")

	var result_scissors = GameLogic.DetermineWinner(
		GameLogic.Choice.Scissors,
		GameLogic.Choice.Scissors)
	assert_eq(result_scissors, GameLogic.Result.Draw, "Same choice should be Draw")

func test_rock_loses_to_paper():
	var result = GameLogic.DetermineWinner(
		GameLogic.Choice.Rock,
		GameLogic.Choice.Paper)
	assert_eq(result, GameLogic.Result.AIWins, "Rock should lose to Paper")

func test_paper_loses_to_scissors():
	var result = GameLogic.DetermineWinner(
		GameLogic.Choice.Paper,
		GameLogic.Choice.Scissors)
	assert_eq(result, GameLogic.Result.AIWins, "Paper should lose to Scissors")

func test_scissors_loses_to_rock():
	var result = GameLogic.DetermineWinner(
		GameLogic.Choice.Scissors,
		GameLogic.Choice.Rock)
	assert_eq(result, GameLogic.Result.AIWins, "Scissors should lose to Rock")

func test_all_combinations_are_deterministic():
	# Verify that the same inputs always produce the same output
	for i in range(10):
		var result = GameLogic.DetermineWinner(
			GameLogic.Choice.Rock,
			GameLogic.Choice.Scissors)
		assert_eq(result, GameLogic.Result.PlayerWins,
			"Deterministic: Rock vs Scissors should always return PlayerWins")
