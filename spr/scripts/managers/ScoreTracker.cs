using Godot;

namespace SprGame.Managers;

/// <summary>
/// Tracks scores for a best-of-3 match.
/// First player to reach 2 wins is the match winner.
/// Draws do not count toward score.
/// Service Layer: Inherits RefCounted (minimal Godot dependency).
/// </summary>
public partial class ScoreTracker : RefCounted {
    /// <summary>
    /// Outcome of a single round
    /// </summary>
    public enum RoundResult {
        PlayerWins,
        AIWins,
        Draw
    }

    /// <summary>
    /// Winner of the overall match
    /// </summary>
    public enum MatchWinner {
        None,
        Player,
        AI
    }

    private int _playerScore;
    private int _aiScore;
    private const int WinningScore = 2;

    /// <summary>
    /// Gets the current player score
    /// </summary>
    public int GetPlayerScore() => _playerScore;

    /// <summary>
    /// Gets the current AI score
    /// </summary>
    public int GetAIScore() => _aiScore;

    /// <summary>
    /// Records the outcome of a round and updates scores.
    /// Draws do not increment any score.
    /// </summary>
    /// <param name="result">The outcome of the round</param>
    public void RecordRound(RoundResult result) {
        switch (result) {
            case RoundResult.PlayerWins:
                _playerScore++;
                break;
            case RoundResult.AIWins:
                _aiScore++;
                break;
            case RoundResult.Draw:
                // Draws don't count toward score
                break;
        }
    }

    /// <summary>
    /// Checks if the match is complete (someone reached winning score)
    /// </summary>
    /// <returns>True if match is complete, false otherwise</returns>
    public bool IsMatchComplete() {
        return _playerScore >= WinningScore || _aiScore >= WinningScore;
    }

    /// <summary>
    /// Determines the winner of the match.
    /// Returns None if match is not complete.
    /// </summary>
    /// <returns>The match winner</returns>
    public MatchWinner GetMatchWinner() {
        if (_playerScore >= WinningScore) {
            return MatchWinner.Player;
        }
        if (_aiScore >= WinningScore) {
            return MatchWinner.AI;
        }
        return MatchWinner.None;
    }
}
