namespace SPR;

/// <summary>
/// Game mode types available in the game
/// </summary>
public enum GameMode {
    SingleMatch,
    Tournament,
    Survival,
    HotSeat
}

/// <summary>
/// Configuration for game modes.
/// Immutable record that holds all settings needed to start a game.
/// </summary>
public record GameConfig(
    GameMode Mode,
    string PlayerName = "Player",
    string Player2Name = "Player 2",
    int WinThreshold = 2,
    int TournamentSize = 8,
    int SurvivalStartingHp = 10
) {
    /// <summary>
    /// Creates a default Single Match configuration
    /// </summary>
    public static GameConfig SingleMatch() => new(GameMode.SingleMatch);

    /// <summary>
    /// Creates a Tournament configuration with specified bracket size
    /// </summary>
    public static GameConfig Tournament(int bracketSize = 8) =>
        new(GameMode.Tournament, TournamentSize: bracketSize);

    /// <summary>
    /// Creates a Survival configuration with specified starting HP
    /// </summary>
    public static GameConfig Survival(int startingHp = 10) =>
        new(GameMode.Survival, SurvivalStartingHp: startingHp);

    /// <summary>
    /// Creates a Hot Seat configuration with player names
    /// </summary>
    public static GameConfig HotSeat(string player1Name = "Player 1", string player2Name = "Player 2") =>
        new(GameMode.HotSeat, PlayerName: player1Name, Player2Name: player2Name);
}
