using Godot;
using SprGame.Core;

namespace SprGame.AI;

/// <summary>
/// AI opponent that makes random choices for MVP.
/// Future enhancements could add difficulty levels or patterns.
/// Service Layer: Uses Godot's RandomNumberGenerator.
/// </summary>
public partial class AIOpponent : RefCounted {
    private readonly RandomNumberGenerator _rng = new();

    /// <summary>
    /// Initializes AI opponent with randomized seed
    /// </summary>
    public AIOpponent() {
        _rng.Randomize();
    }

    /// <summary>
    /// Makes a random choice for the AI player.
    /// For MVP, this is completely random with no pattern or difficulty.
    /// </summary>
    /// <returns>A random choice (Rock, Paper, or Scissors)</returns>
    public GameLogic.Choice MakeChoice() {
        int randomValue = _rng.RandiRange(0, 2);
        return (GameLogic.Choice)randomValue;
    }
}
