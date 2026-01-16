extends GutTest
# Test suite for AIOpponent
# Tests random choice generation for AI player

var AIOpponent = load("res://scripts/ai/AIOpponent.cs")
var GameLogic = load("res://scripts/core/GameLogic.cs")

func test_ai_returns_valid_choice():
	var ai = AIOpponent.new()
	var choice = ai.MakeChoice()

	assert_true(
		choice == GameLogic.Choice.Rock or
		choice == GameLogic.Choice.Paper or
		choice == GameLogic.Choice.Scissors,
		"AI should return a valid choice (Rock, Paper, or Scissors)"
	)

func test_ai_returns_valid_choice_multiple_times():
	var ai = AIOpponent.new()

	for i in range(10):
		var choice = ai.MakeChoice()
		assert_true(
			choice == GameLogic.Choice.Rock or
			choice == GameLogic.Choice.Paper or
			choice == GameLogic.Choice.Scissors,
			"Each AI choice should be valid"
		)

func test_ai_randomness():
	# Over 30 choices, AI should pick all three options at least once
	# This is probabilistic but very unlikely to fail (probability ~0.0000046)
	var ai = AIOpponent.new()
	var choices = []

	for i in range(30):
		choices.append(ai.MakeChoice())

	var has_rock = false
	var has_paper = false
	var has_scissors = false

	for choice in choices:
		if choice == GameLogic.Choice.Rock:
			has_rock = true
		elif choice == GameLogic.Choice.Paper:
			has_paper = true
		elif choice == GameLogic.Choice.Scissors:
			has_scissors = true

	assert_true(has_rock, "AI should eventually pick Rock")
	assert_true(has_paper, "AI should eventually pick Paper")
	assert_true(has_scissors, "AI should eventually pick Scissors")

func test_ai_choices_are_distributed():
	# Over 100 choices, no single choice should dominate completely
	# Each choice should appear at least 15 times out of 100 (15% threshold)
	var ai = AIOpponent.new()
	var rock_count = 0
	var paper_count = 0
	var scissors_count = 0

	for i in range(100):
		var choice = ai.MakeChoice()
		match choice:
			GameLogic.Choice.Rock:
				rock_count += 1
			GameLogic.Choice.Paper:
				paper_count += 1
			GameLogic.Choice.Scissors:
				scissors_count += 1

	assert_gt(rock_count, 15, "Rock should appear at least 15 times out of 100")
	assert_gt(paper_count, 15, "Paper should appear at least 15 times out of 100")
	assert_gt(scissors_count, 15, "Scissors should appear at least 15 times out of 100")

func test_multiple_ai_instances_are_independent():
	# Two different AI instances should produce different sequences
	var ai1 = AIOpponent.new()
	var ai2 = AIOpponent.new()

	var choices1 = []
	var choices2 = []

	for i in range(10):
		choices1.append(ai1.MakeChoice())
		choices2.append(ai2.MakeChoice())

	var differences = 0
	for i in range(10):
		if choices1[i] != choices2[i]:
			differences += 1

	# With proper randomization, sequences should differ
	# (Probability of identical sequences is (1/3)^10 ≈ 0.0000169)
	assert_gt(differences, 0, "Different AI instances should produce different sequences")
