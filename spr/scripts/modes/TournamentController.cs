using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.Managers;

namespace SPR;

/// <summary>
/// Controller for Tournament mode.
/// Manages bracket progression through single-elimination rounds.
/// </summary>
public partial class TournamentController : RefCounted, IModeController {
    private ScoreTracker _scoreTracker = new();
    private int _tournamentSize;
    private int _currentRound;
    private int _totalRounds;
    private int _matchesWon;
    private string[] _opponentNames = System.Array.Empty<string>();
    private bool _isChampion;
    private bool _isEliminated;

    private static readonly string[] NamePrefixes = {
        "Iron", "Steel", "Thunder", "Shadow", "Flame",
        "Storm", "Frost", "Stone", "Night", "Swift",
        "Crimson", "Azure", "Golden", "Silver", "Dark"
    };

    private static readonly string[] NameSuffixes = {
        "Crusher", "Master", "Champion", "Warrior", "Legend",
        "Slayer", "Knight", "Hunter", "Striker", "Blade"
    };

    public void Initialize(GameConfig config) {
        _tournamentSize = config.TournamentSize;
        _totalRounds = _tournamentSize == 8 ? 3 : 4;
        _currentRound = 1;
        _matchesWon = 0;
        _isChampion = false;
        _isEliminated = false;
        _scoreTracker = new ScoreTracker();
        GenerateOpponentNames();
    }

    private void GenerateOpponentNames() {
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        _opponentNames = new string[_totalRounds];
        var usedNames = new System.Collections.Generic.HashSet<string>();

        for (int i = 0; i < _totalRounds; i++) {
            string name;
            do {
                int prefixIndex = rng.RandiRange(0, NamePrefixes.Length - 1);
                int suffixIndex = rng.RandiRange(0, NameSuffixes.Length - 1);
                name = $"{NamePrefixes[prefixIndex]} {NameSuffixes[suffixIndex]}";
            } while (usedNames.Contains(name));

            usedNames.Add(name);
            _opponentNames[i] = name;
        }
    }

    /// <summary>
    /// Gets the display name for the current round
    /// </summary>
    public string GetCurrentRoundName() {
        if (_tournamentSize == 8) {
            return _currentRound switch {
                1 => "Quarterfinals",
                2 => "Semifinals",
                3 => "FINALS",
                _ => $"Round {_currentRound}"
            };
        } else {
            return _currentRound switch {
                1 => "Round of 16",
                2 => "Quarterfinals",
                3 => "Semifinals",
                4 => "FINALS",
                _ => $"Round {_currentRound}"
            };
        }
    }

    /// <summary>
    /// Gets the name of the current opponent
    /// </summary>
    public string GetCurrentOpponentName() {
        if (_matchesWon < _opponentNames.Length) {
            return _opponentNames[_matchesWon];
        }
        return "Final Boss";
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

    public bool IsModeComplete() => _isChampion || _isEliminated;

    public void OnMatchComplete(bool playerWon) {
        if (playerWon) {
            _matchesWon++;
            if (_matchesWon >= _totalRounds) {
                _isChampion = true;
            } else {
                _currentRound++;
                ResetMatch();
            }
        } else {
            _isEliminated = true;
        }
    }

    public int GetPlayerScore() => _scoreTracker.GetPlayerScore();

    public int GetOpponentScore() => _scoreTracker.GetAIScore();

    /// <summary>
    /// Gets the current tournament round number
    /// </summary>
    public int GetCurrentRound() => _currentRound;

    /// <summary>
    /// Gets the total number of rounds in the tournament
    /// </summary>
    public int GetTotalRounds() => _totalRounds;

    /// <summary>
    /// Gets the number of matches won so far
    /// </summary>
    public int GetMatchesWon() => _matchesWon;

    /// <summary>
    /// Returns true if player won the tournament
    /// </summary>
    public bool IsChampion() => _isChampion;

    /// <summary>
    /// Returns true if player was eliminated
    /// </summary>
    public bool IsEliminated() => _isEliminated;

    public Dictionary GetStateSnapshot() {
        return new Dictionary {
            { "currentRound", _currentRound },
            { "totalRounds", _totalRounds },
            { "roundName", GetCurrentRoundName() },
            { "opponentName", GetCurrentOpponentName() },
            { "matchesWon", _matchesWon },
            { "isChampion", _isChampion },
            { "isEliminated", _isEliminated },
            { "playerScore", _scoreTracker.GetPlayerScore() },
            { "opponentScore", _scoreTracker.GetAIScore() },
            { "tournamentSize", _tournamentSize }
        };
    }

    public string GetStatusMessage() {
        if (_isChampion) return "TOURNAMENT CHAMPION!";
        if (_isEliminated) return $"Eliminated in {GetCurrentRoundName()}";
        return $"{GetCurrentRoundName()} vs {GetCurrentOpponentName()}";
    }
}
