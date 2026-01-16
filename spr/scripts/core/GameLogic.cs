namespace SprGame.Core;

/// <summary>
/// Pure C# game logic for Scissors-Paper-Rock with zero Godot dependencies.
/// Contains the deterministic rules for winner determination.
/// 100% testable in isolation.
/// </summary>
public static class GameLogic {
    /// <summary>
    /// Player's choice options
    /// </summary>
    public enum Choice {
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }

    /// <summary>
    /// Round outcome from player's perspective
    /// </summary>
    public enum Result {
        PlayerWins,
        AIWins,
        Draw
    }

    /// <summary>
    /// Determines the winner of a round based on classic SPR rules.
    /// Rock beats Scissors, Scissors beats Paper, Paper beats Rock.
    /// Same choice results in a Draw.
    /// </summary>
    /// <param name="player">Player's choice</param>
    /// <param name="ai">AI's choice</param>
    /// <returns>Result indicating winner from player's perspective</returns>
    public static Result DetermineWinner(Choice player, Choice ai) {
        // Same choice is always a draw
        if (player == ai) {
            return Result.Draw;
        }

        // Pattern matching for all winning conditions
        return (player, ai) switch {
            (Choice.Rock, Choice.Scissors) => Result.PlayerWins,
            (Choice.Scissors, Choice.Paper) => Result.PlayerWins,
            (Choice.Paper, Choice.Rock) => Result.PlayerWins,
            _ => Result.AIWins
        };
    }
}
