using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.AI;

namespace SprGame.Managers;

/// <summary>
/// Manages game flow and state transitions.
/// Orchestrates matches using ScoreTracker and AIOpponent.
/// Service Layer: Inherits Node for scene tree integration.
/// </summary>
public partial class GameManager : Node {
    /// <summary>
    /// Game state machine states
    /// </summary>
    public enum GameState {
        MainMenu,
        ModeSelection,
        Playing,
        GameOver
    }

    private GameState _currentState = GameState.MainMenu;
    private ScoreTracker? _scoreTracker;
    private AIOpponent? _aiOpponent;

    /// <summary>
    /// Gets the current game state
    /// </summary>
    public GameState GetCurrentState() => _currentState;

    /// <summary>
    /// Starts a new Single Match game.
    /// Resets scores and transitions to Playing state.
    /// </summary>
    public void StartSingleMatch() {
        _currentState = GameState.Playing;
        _scoreTracker = new ScoreTracker();
        _aiOpponent = new AIOpponent();
    }

    /// <summary>
    /// Gets the current scores as a dictionary for GDScript compatibility.
    /// </summary>
    /// <returns>Dictionary with "player" and "ai" keys</returns>
    public Dictionary GetCurrentScores() {
        if (_scoreTracker == null) {
            return new Dictionary {
                { "player", 0 },
                { "ai", 0 }
            };
        }

        return new Dictionary {
            { "player", _scoreTracker.GetPlayerScore() },
            { "ai", _scoreTracker.GetAIScore() }
        };
    }

    /// <summary>
    /// Plays a round with explicit player and AI choices.
    /// Used for testing and manual AI control.
    /// </summary>
    /// <param name="playerChoice">Player's choice</param>
    /// <param name="aiChoice">AI's choice</param>
    /// <returns>Dictionary with round outcome</returns>
    public Dictionary PlayRound(GameLogic.Choice playerChoice, GameLogic.Choice aiChoice) {
        if (_scoreTracker == null) {
            GD.PushError("Cannot play round: match not started");
            return CreateOutcomeDictionary(GameLogic.Result.Draw, 0, 0, false, playerChoice, aiChoice);
        }

        var result = GameLogic.DetermineWinner(playerChoice, aiChoice);

        var roundResult = result switch {
            GameLogic.Result.PlayerWins => ScoreTracker.RoundResult.PlayerWins,
            GameLogic.Result.AIWins => ScoreTracker.RoundResult.AIWins,
            _ => ScoreTracker.RoundResult.Draw
        };

        _scoreTracker.RecordRound(roundResult);

        bool isMatchComplete = _scoreTracker.IsMatchComplete();
        if (isMatchComplete) {
            _currentState = GameState.GameOver;
        }

        return CreateOutcomeDictionary(
            result,
            _scoreTracker.GetPlayerScore(),
            _scoreTracker.GetAIScore(),
            isMatchComplete,
            playerChoice,
            aiChoice
        );
    }

    /// <summary>
    /// Plays a round where the AI makes its own choice.
    /// This is the primary method for actual gameplay.
    /// </summary>
    /// <param name="playerChoice">Player's choice</param>
    /// <returns>Dictionary with round outcome</returns>
    public Dictionary PlayRoundWithAI(GameLogic.Choice playerChoice) {
        if (_aiOpponent == null) {
            GD.PushError("Cannot play round: AI not initialized");
            return CreateOutcomeDictionary(GameLogic.Result.Draw, 0, 0, false, playerChoice, GameLogic.Choice.Rock);
        }

        GameLogic.Choice aiChoice = _aiOpponent.MakeChoice();
        return PlayRound(playerChoice, aiChoice);
    }

    /// <summary>
    /// Creates an outcome dictionary for GDScript compatibility.
    /// </summary>
    private static Dictionary CreateOutcomeDictionary(
        GameLogic.Result result,
        int playerScore,
        int aiScore,
        bool isMatchComplete,
        GameLogic.Choice playerChoice,
        GameLogic.Choice aiChoice) {
        return new Dictionary {
            { "result", (int)result },
            { "playerScore", playerScore },
            { "aiScore", aiScore },
            { "isMatchComplete", isMatchComplete },
            { "playerChoice", (int)playerChoice },
            { "aiChoice", (int)aiChoice }
        };
    }
}
