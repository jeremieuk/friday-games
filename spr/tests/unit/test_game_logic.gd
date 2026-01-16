extends GutTest
# Test suite for GameLogic core rules
# Tests the pure C# game logic with zero Godot dependencies

var GameLogicHelper = load("res://scripts/core/GameLogicHelper.cs")
var _logic  # Instance of helper

# C# enum values (GDScript can't access nested C# enums directly)
# Choice enum
const ROCK = 0
const PAPER = 1
const SCISSORS = 2
# Result enum
const PLAYER_WINS = 0
const AI_WINS = 1
const DRAW = 2

func before_each():
	_logic = GameLogicHelper.new()

func after_each():
	# GameLogicHelper extends RefCounted - just set to null, GC handles cleanup
	_logic = null

func test_rock_beats_scissors():
	var result = _logic.DetermineWinner(ROCK, SCISSORS)
	assert_eq(result, PLAYER_WINS, "Rock should beat Scissors")

func test_scissors_beats_paper():
	var result = _logic.DetermineWinner(
		SCISSORS,
		PAPER)
	assert_eq(result, PLAYER_WINS, "Scissors should beat Paper")

func test_paper_beats_rock():
	var result = _logic.DetermineWinner(
		PAPER,
		ROCK)
	assert_eq(result, PLAYER_WINS, "Paper should beat Rock")

func test_same_choice_is_draw():
	var result_rock = _logic.DetermineWinner(
		ROCK,
		ROCK)
	assert_eq(result_rock, DRAW, "Same choice should be Draw")

	var result_paper = _logic.DetermineWinner(
		PAPER,
		PAPER)
	assert_eq(result_paper, DRAW, "Same choice should be Draw")

	var result_scissors = _logic.DetermineWinner(
		SCISSORS,
		SCISSORS)
	assert_eq(result_scissors, DRAW, "Same choice should be Draw")

func test_rock_loses_to_paper():
	var result = _logic.DetermineWinner(
		ROCK,
		PAPER)
	assert_eq(result, AI_WINS, "Rock should lose to Paper")

func test_paper_loses_to_scissors():
	var result = _logic.DetermineWinner(
		PAPER,
		SCISSORS)
	assert_eq(result, AI_WINS, "Paper should lose to Scissors")

func test_scissors_loses_to_rock():
	var result = _logic.DetermineWinner(
		SCISSORS,
		ROCK)
	assert_eq(result, AI_WINS, "Scissors should lose to Rock")

func test_all_combinations_are_deterministic():
	# Verify that the same inputs always produce the same output
	for i in range(10):
		var result = _logic.DetermineWinner(
			ROCK,
			SCISSORS)
		assert_eq(result, PLAYER_WINS,
			"Deterministic: Rock vs Scissors should always return PlayerWins")
