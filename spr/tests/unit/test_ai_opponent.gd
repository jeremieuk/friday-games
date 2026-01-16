extends GutTest
# Test suite for AIOpponent
# Tests random choice generation for AI player

var AIOpponent = load("res://scripts/ai/AIOpponent.cs")
var _ai  # Instance for each test

# C# enum values (GDScript can't access nested C# enums directly)
const ROCK = 0
const PAPER = 1
const SCISSORS = 2

func before_each():
	_ai = AIOpponent.new()

func after_each():
	# AIOpponent extends RefCounted - just set to null, GC handles cleanup
	_ai = null

func test_ai_returns_valid_choice():
	var choice = _ai.MakeChoice()

	assert_true(
		choice == ROCK or
		choice == PAPER or
		choice == SCISSORS,
		"AI should return a valid choice (Rock, Paper, or Scissors)"
	)

func test_ai_returns_valid_choice_multiple_times():
	for i in range(10):
		var choice = _ai.MakeChoice()
		assert_true(
			choice == ROCK or
			choice == PAPER or
			choice == SCISSORS,
			"Each AI choice should be valid"
		)

func test_ai_randomness():
	# Over 30 choices, AI should pick all three options at least once
	# This is probabilistic but very unlikely to fail (probability ~0.0000046)
	var choices = []

	for i in range(30):
		choices.append(_ai.MakeChoice())

	var has_rock = false
	var has_paper = false
	var has_scissors = false

	for choice in choices:
		if choice == ROCK:
			has_rock = true
		elif choice == PAPER:
			has_paper = true
		elif choice == SCISSORS:
			has_scissors = true

	assert_true(has_rock, "AI should eventually pick Rock")
	assert_true(has_paper, "AI should eventually pick Paper")
	assert_true(has_scissors, "AI should eventually pick Scissors")

func test_ai_choices_are_distributed():
	# Over 100 choices, no single choice should dominate completely
	# Each choice should appear at least 15 times out of 100 (15% threshold)
	var rock_count = 0
	var paper_count = 0
	var scissors_count = 0

	for i in range(100):
		var choice = _ai.MakeChoice()
		match choice:
			ROCK:
				rock_count += 1
			PAPER:
				paper_count += 1
			SCISSORS:
				scissors_count += 1

	assert_gt(rock_count, 15, "Rock should appear at least 15 times out of 100")
	assert_gt(paper_count, 15, "Paper should appear at least 15 times out of 100")
	assert_gt(scissors_count, 15, "Scissors should appear at least 15 times out of 100")

func test_multiple_ai_instances_are_independent():
	# Two different AI instances should produce different sequences
	var ai2 = AIOpponent.new()

	var choices1 = []
	var choices2 = []

	for i in range(10):
		choices1.append(_ai.MakeChoice())
		choices2.append(ai2.MakeChoice())

	# ai2 is RefCounted - GC handles cleanup when it goes out of scope

	var differences = 0
	for i in range(10):
		if choices1[i] != choices2[i]:
			differences += 1

	# With proper randomization, sequences should differ
	# (Probability of identical sequences is (1/3)^10 ≈ 0.0000169)
	assert_gt(differences, 0, "Different AI instances should produce different sequences")
