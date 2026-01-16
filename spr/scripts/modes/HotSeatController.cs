using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.Managers;

namespace SPR;

/// <summary>
/// Controller for Hot Seat (local multiplayer) mode.
/// Two players take turns on the same device with transition screens for privacy.
/// </summary>
public partial class HotSeatController : RefCounted, IModeController {
    /// <summary>
    /// The phases of a hot seat turn
    /// </summary>
    public enum TurnPhase {
        Player1Choosing,
        TransitionToPlayer2,
        Player2Choosing,
        TransitionToReveal,
        Revealing,
        ShowingResult
    }

    private ScoreTracker _scoreTracker = new();
    private int _winThreshold = 2;
    private string _player1Name = "Player 1";
    private string _player2Name = "Player 2";

    private TurnPhase _currentPhase = TurnPhase.Player1Choosing;
    private GameLogic.Choice? _player1Choice;
    private GameLogic.Choice? _player2Choice;
    private GameLogic.Result? _lastResult;

    public void Initialize(GameConfig config) {
        _winThreshold = config.WinThreshold;
        _player1Name = config.PlayerName;
        _player2Name = config.Player2Name;
        _scoreTracker = new ScoreTracker();
        ResetRound();
    }

    /// <summary>
    /// Resets the round state (choices and phase) for a new round
    /// </summary>
    public void ResetRound() {
        _currentPhase = TurnPhase.Player1Choosing;
        _player1Choice = null;
        _player2Choice = null;
        _lastResult = null;
    }

    public void ResetMatch() {
        _scoreTracker = new ScoreTracker();
        ResetRound();
    }

    /// <summary>
    /// Player 1 makes their choice
    /// </summary>
    public void Player1MakesChoice(GameLogic.Choice choice) {
        if (_currentPhase != TurnPhase.Player1Choosing) return;
        _player1Choice = choice;
        _currentPhase = TurnPhase.TransitionToPlayer2;
    }

    /// <summary>
    /// Called when Player 2 is ready (transition screen dismissed)
    /// </summary>
    public void Player2Ready() {
        if (_currentPhase != TurnPhase.TransitionToPlayer2) return;
        _currentPhase = TurnPhase.Player2Choosing;
    }

    /// <summary>
    /// Player 2 makes their choice
    /// </summary>
    public void Player2MakesChoice(GameLogic.Choice choice) {
        if (_currentPhase != TurnPhase.Player2Choosing) return;
        _player2Choice = choice;
        _currentPhase = TurnPhase.TransitionToReveal;
    }

    /// <summary>
    /// Called when both players are ready for the reveal
    /// </summary>
    public void ReadyForReveal() {
        if (_currentPhase != TurnPhase.TransitionToReveal) return;
        _currentPhase = TurnPhase.Revealing;
    }

    /// <summary>
    /// Reveals both choices and determines the winner.
    /// Call this after both players have made choices.
    /// </summary>
    public GameLogic.Result RevealAndDetermineWinner() {
        if (_player1Choice == null || _player2Choice == null) {
            GD.PushError("Cannot reveal: both players must make choices first");
            return GameLogic.Result.Draw;
        }

        var result = GameLogic.DetermineWinner(_player1Choice.Value, _player2Choice.Value);
        _lastResult = result;

        var roundResult = result switch {
            GameLogic.Result.PlayerWins => ScoreTracker.RoundResult.PlayerWins,
            GameLogic.Result.AIWins => ScoreTracker.RoundResult.AIWins,
            _ => ScoreTracker.RoundResult.Draw
        };
        _scoreTracker.RecordRound(roundResult);

        _currentPhase = TurnPhase.ShowingResult;
        return result;
    }

    /// <summary>
    /// Processes a round result (called after reveal)
    /// </summary>
    public void ProcessRoundResult(GameLogic.Result result) {
        // Already handled in RevealAndDetermineWinner
        // This is just for IModeController compatibility
    }

    public bool IsMatchComplete() => _scoreTracker.IsMatchComplete();

    public bool IsModeComplete() => IsMatchComplete();

    public void OnMatchComplete(bool playerWon) {
        // In hot seat mode, we just reset for a new match
        // The "playerWon" indicates Player 1 won
    }

    public int GetPlayerScore() => _scoreTracker.GetPlayerScore();

    public int GetOpponentScore() => _scoreTracker.GetAIScore();

    /// <summary>
    /// Gets the current turn phase
    /// </summary>
    public TurnPhase GetCurrentPhase() => _currentPhase;

    /// <summary>
    /// Gets Player 1's choice (if made)
    /// </summary>
    public GameLogic.Choice? GetPlayer1Choice() => _player1Choice;

    /// <summary>
    /// Gets Player 2's choice (if made)
    /// </summary>
    public GameLogic.Choice? GetPlayer2Choice() => _player2Choice;

    /// <summary>
    /// Gets Player 1's name
    /// </summary>
    public string GetPlayer1Name() => _player1Name;

    /// <summary>
    /// Gets Player 2's name
    /// </summary>
    public string GetPlayer2Name() => _player2Name;

    /// <summary>
    /// Gets the name of the player whose turn it currently is
    /// </summary>
    public string GetCurrentPlayerName() {
        return _currentPhase switch {
            TurnPhase.Player1Choosing => _player1Name,
            TurnPhase.Player2Choosing => _player2Name,
            _ => ""
        };
    }

    /// <summary>
    /// Gets the transition message to display
    /// </summary>
    public string GetTransitionMessage() {
        return _currentPhase switch {
            TurnPhase.TransitionToPlayer2 => $"{_player2Name}'s Turn",
            TurnPhase.TransitionToReveal => "Ready for Reveal?",
            _ => ""
        };
    }

    /// <summary>
    /// Gets the match winner (from Player 1's perspective)
    /// </summary>
    public ScoreTracker.MatchWinner GetMatchWinner() => _scoreTracker.GetMatchWinner();

    /// <summary>
    /// Gets the name of the match winner
    /// </summary>
    public string GetWinnerName() {
        return GetMatchWinner() switch {
            ScoreTracker.MatchWinner.Player => _player1Name,
            ScoreTracker.MatchWinner.AI => _player2Name, // In HotSeat, "AI" slot is Player 2
            _ => "Nobody"
        };
    }

    /// <summary>
    /// Gets the name of the round winner (after reveal)
    /// </summary>
    public string GetRoundWinnerName() {
        if (_lastResult == null) return "";
        return _lastResult.Value switch {
            GameLogic.Result.PlayerWins => _player1Name,
            GameLogic.Result.AIWins => _player2Name,
            _ => "Nobody"
        };
    }

    public Dictionary GetStateSnapshot() {
        return new Dictionary {
            { "phase", (int)_currentPhase },
            { "phaseName", _currentPhase.ToString() },
            { "player1Name", _player1Name },
            { "player2Name", _player2Name },
            { "player1Score", _scoreTracker.GetPlayerScore() },
            { "player2Score", _scoreTracker.GetAIScore() },
            { "transitionMessage", GetTransitionMessage() },
            { "currentPlayerName", GetCurrentPlayerName() },
            { "isMatchComplete", IsMatchComplete() },
            { "winnerName", IsMatchComplete() ? GetWinnerName() : "" },
            { "player1Choice", _player1Choice.HasValue ? (int)_player1Choice.Value : -1 },
            { "player2Choice", _player2Choice.HasValue ? (int)_player2Choice.Value : -1 }
        };
    }

    public string GetStatusMessage() {
        if (IsMatchComplete()) {
            return $"{GetWinnerName()} Wins!";
        }
        return $"{_player1Name}: {_scoreTracker.GetPlayerScore()} | {_player2Name}: {_scoreTracker.GetAIScore()}";
    }
}
