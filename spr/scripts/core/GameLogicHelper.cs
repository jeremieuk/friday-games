using Godot;
using SprGame.Core;

/// <summary>
/// Helper class for GDScript to access GameLogic static methods.
/// GDScript cannot call static C# methods directly, so this wrapper
/// exposes them as instance methods.
/// </summary>
public partial class GameLogicHelper : RefCounted {
    public GameLogic.Result DetermineWinner(GameLogic.Choice playerChoice, GameLogic.Choice aiChoice) {
        return GameLogic.DetermineWinner(playerChoice, aiChoice);
    }

    // Expose enum values as properties for GDScript access
    public int Rock => (int)GameLogic.Choice.Rock;
    public int Paper => (int)GameLogic.Choice.Paper;
    public int Scissors => (int)GameLogic.Choice.Scissors;

    public int PlayerWins => (int)GameLogic.Result.PlayerWins;
    public int AIWins => (int)GameLogic.Result.AIWins;
    public int Draw => (int)GameLogic.Result.Draw;
}
