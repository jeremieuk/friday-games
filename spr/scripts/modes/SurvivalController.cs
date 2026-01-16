using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.Managers;

namespace SPR;

/// <summary>
/// Controller for Survival mode.
/// Player has limited HP and tries to defeat as many opponents as possible.
/// HP decreases by 1 per match loss (not per round loss).
/// </summary>
public partial class SurvivalController : RefCounted, IModeController {
    private ScoreTracker _scoreTracker = new();
    private int _startingHp;
    private int _currentHp;
    private int _opponentsDefeated;
    private static int _allTimeHighScore;

    public void Initialize(GameConfig config) {
        _startingHp = config.SurvivalStartingHp;
        _currentHp = _startingHp;
        _opponentsDefeated = 0;
        _scoreTracker = new ScoreTracker();
    }

    public void ResetMatch() {
        _scoreTracker = new ScoreTracker();
    }

    public void ProcessRoundResult(GameLogic.Result result) {
        var roundResult = result switch {
            GameLogic.Result.PlayerWins => ScoreTracker.RoundResult.PlayerWins,
            GameLogic.Result.AIWins => ScoreTracker.RoundResult.AIWins,
            _ => ScoreTracker.RoundResult.Draw
        };
        _scoreTracker.RecordRound(roundResult);
    }

    public bool IsMatchComplete() => _scoreTracker.IsMatchComplete();

    public bool IsModeComplete() => _currentHp <= 0;

    public void OnMatchComplete(bool playerWon) {
        if (playerWon) {
            _opponentsDefeated++;
            if (_opponentsDefeated > _allTimeHighScore) {
                _allTimeHighScore = _opponentsDefeated;
            }
        } else {
            _currentHp--;
        }
        ResetMatch();
    }

    public int GetPlayerScore() => _scoreTracker.GetPlayerScore();

    public int GetOpponentScore() => _scoreTracker.GetAIScore();

    /// <summary>
    /// Gets the current HP
    /// </summary>
    public int GetCurrentHp() => _currentHp;

    /// <summary>
    /// Gets the starting HP
    /// </summary>
    public int GetStartingHp() => _startingHp;

    /// <summary>
    /// Gets the HP as a percentage (0.0 to 1.0)
    /// </summary>
    public float GetHpPercentage() => (float)_currentHp / _startingHp;

    /// <summary>
    /// Gets the number of opponents defeated this run
    /// </summary>
    public int GetOpponentsDefeated() => _opponentsDefeated;

    /// <summary>
    /// Gets the all-time high score (session only)
    /// </summary>
    public int GetHighScore() => _allTimeHighScore;

    /// <summary>
    /// Returns true if current score is a new high score
    /// </summary>
    public bool IsNewHighScore() => _opponentsDefeated > 0 && _opponentsDefeated >= _allTimeHighScore;

    /// <summary>
    /// Returns true if game is over (HP reached 0)
    /// </summary>
    public bool IsGameOver() => _currentHp <= 0;

    public Dictionary GetStateSnapshot() {
        return new Dictionary {
            { "currentHp", _currentHp },
            { "startingHp", _startingHp },
            { "hpPercentage", GetHpPercentage() },
            { "opponentsDefeated", _opponentsDefeated },
            { "highScore", _allTimeHighScore },
            { "isNewHighScore", IsNewHighScore() },
            { "isGameOver", IsGameOver() },
            { "playerScore", _scoreTracker.GetPlayerScore() },
            { "opponentScore", _scoreTracker.GetAIScore() }
        };
    }

    public string GetStatusMessage() {
        if (IsGameOver()) {
            if (IsNewHighScore()) {
                return $"NEW HIGH SCORE! Defeated: {_opponentsDefeated}";
            }
            return $"Game Over! Defeated: {_opponentsDefeated}";
        }
        return $"HP: {_currentHp}/{_startingHp} | Defeated: {_opponentsDefeated}";
    }
}
