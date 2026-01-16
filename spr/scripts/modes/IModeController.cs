using Godot.Collections;
using SprGame.Core;

namespace SPR;

/// <summary>
/// Interface for all game mode controllers.
/// Defines the contract for mode-specific game logic.
/// </summary>
public interface IModeController {
    /// <summary>
    /// Initializes the mode with the given configuration
    /// </summary>
    void Initialize(GameConfig config);

    /// <summary>
    /// Resets the current match state (for starting a new match within the mode)
    /// </summary>
    void ResetMatch();

    /// <summary>
    /// Processes the result of a round
    /// </summary>
    void ProcessRoundResult(GameLogic.Result result);

    /// <summary>
    /// Checks if the current match is complete
    /// </summary>
    bool IsMatchComplete();

    /// <summary>
    /// Checks if the entire mode is complete (e.g., tournament finished, survival game over)
    /// </summary>
    bool IsModeComplete();

    /// <summary>
    /// Called when a match within the mode is complete.
    /// Returns true if player won the match, false if they lost.
    /// </summary>
    void OnMatchComplete(bool playerWon);

    /// <summary>
    /// Gets the current state as a dictionary for UI updates
    /// </summary>
    Dictionary GetStateSnapshot();

    /// <summary>
    /// Gets a status message describing the current state
    /// </summary>
    string GetStatusMessage();

    /// <summary>
    /// Gets the player's current score in the match
    /// </summary>
    int GetPlayerScore();

    /// <summary>
    /// Gets the opponent's current score in the match
    /// </summary>
    int GetOpponentScore();
}
