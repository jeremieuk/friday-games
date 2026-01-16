using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.AI;
using SPR;

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
    private GameMode _currentMode = GameMode.SingleMatch;
    private ScoreTracker? _scoreTracker;
    private AIOpponent? _aiOpponent;
    private IModeController? _modeController;

    /// <summary>
    /// Gets the current game state
    /// </summary>
    public GameState GetCurrentState() => _currentState;

    /// <summary>
    /// Gets the current game mode
    /// </summary>
    public GameMode GetCurrentMode() => _currentMode;

    /// <summary>
    /// Gets the current mode controller (if any)
    /// </summary>
    public IModeController? GetModeController() => _modeController;

    /// <summary>
    /// Starts a new Single Match game.
    /// Resets scores and transitions to Playing state.
    /// </summary>
    public void StartSingleMatch() {
        _currentState = GameState.Playing;
        _currentMode = GameMode.SingleMatch;
        _scoreTracker = new ScoreTracker();
        _aiOpponent = new AIOpponent();
        _modeController = null;
    }

    /// <summary>
    /// Starts a Tournament game with the specified bracket size.
    /// </summary>
    /// <param name="bracketSize">Number of players in bracket (8 or 16)</param>
    public void StartTournament(int bracketSize = 8) {
        _currentState = GameState.Playing;
        _currentMode = GameMode.Tournament;
        _scoreTracker = new ScoreTracker();
        _aiOpponent = new AIOpponent();
        var config = GameConfig.Tournament(bracketSize);
        _modeController = new TournamentController();
        _modeController.Initialize(config);
    }

    /// <summary>
    /// Starts a Survival game with the specified starting HP.
    /// </summary>
    /// <param name="startingHp">Starting HP (default 10)</param>
    public void StartSurvival(int startingHp = 10) {
        _currentState = GameState.Playing;
        _currentMode = GameMode.Survival;
        _scoreTracker = new ScoreTracker();
        _aiOpponent = new AIOpponent();
        var config = GameConfig.Survival(startingHp);
        _modeController = new SurvivalController();
        _modeController.Initialize(config);
    }

    /// <summary>
    /// Starts a Hot Seat (local multiplayer) game.
    /// </summary>
    /// <param name="player1Name">First player's name</param>
    /// <param name="player2Name">Second player's name</param>
    public void StartHotSeat(string player1Name = "Player 1", string player2Name = "Player 2") {
        _currentState = GameState.Playing;
        _currentMode = GameMode.HotSeat;
        _scoreTracker = new ScoreTracker();
        _aiOpponent = null; // No AI in hot seat mode
        var config = GameConfig.HotSeat(player1Name, player2Name);
        _modeController = new HotSeatController();
        _modeController.Initialize(config);
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
