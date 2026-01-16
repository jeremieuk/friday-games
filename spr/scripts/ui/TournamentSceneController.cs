using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.Managers;
using SPR;

namespace SprGame.UI;

/// <summary>
/// Controller for the Tournament game mode scene.
/// Manages bracket progression through single-elimination rounds.
/// </summary>
public partial class TournamentSceneController : Control {
    // Timing constants for dramatic reveal
    private const float PLAYER_REVEAL_DELAY = 0.5f;
    private const float AI_REVEAL_DELAY = 1.0f;
    private const float RESULT_DISPLAY_TIME = 2.0f;

    private GameManager? _gameManager;
    private TournamentController? _tournamentController;

    // UI References
    private Label? _titleLabel;
    private Label? _roundLabel;
    private Label? _opponentLabel;
    private Label? _scoreLabel;
    private Label? _resultLabel;
    private Button? _rockButton;
    private Button? _paperButton;
    private Button? _scissorsButton;
    private Button? _continueButton;
    private Button? _playAgainButton;

    // Choice display references
    private CanvasLayer? _revealOverlay;
    private ColorRect? _revealBackdrop;
    private VBoxContainer? _playerChoiceContainer;
    private Label? _playerChoiceEmoji;
    private VBoxContainer? _aiChoiceContainer;
    private Label? _aiChoiceEmoji;
    private Label? _aiChoiceLabel;

    // Bracket size (8 or 16) - can be set from mode selection
    private int _bracketSize = 8;

    public override void _Ready() {
        // Get UI node references
        _titleLabel = GetNodeOrNull<Label>("GameContainer/TopSection/TitleLabel");
        _roundLabel = GetNodeOrNull<Label>("GameContainer/TopSection/RoundLabel");
        _opponentLabel = GetNodeOrNull<Label>("GameContainer/TopSection/OpponentLabel");
        _scoreLabel = GetNodeOrNull<Label>("GameContainer/TopSection/ScoreLabel");
        _resultLabel = GetNodeOrNull<Label>("GameContainer/MiddleSection/ContentContainer/ResultLabel");
        _rockButton = GetNodeOrNull<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/RockButton");
        _paperButton = GetNodeOrNull<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/PaperButton");
        _scissorsButton = GetNodeOrNull<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/ScissorsButton");
        _continueButton = GetNodeOrNull<Button>("GameContainer/BottomSection/ContinueButton");
        _playAgainButton = GetNodeOrNull<Button>("GameContainer/BottomSection/PlayAgainButton");

        // Get choice display references
        _revealOverlay = GetNodeOrNull<CanvasLayer>("RevealOverlay");
        _revealBackdrop = GetNodeOrNull<ColorRect>("RevealOverlay/Backdrop");
        _playerChoiceContainer = GetNodeOrNull<VBoxContainer>("RevealOverlay/RevealCenter/RevealSection/PlayerChoiceContainer");
        _playerChoiceEmoji = GetNodeOrNull<Label>("RevealOverlay/RevealCenter/RevealSection/PlayerChoiceContainer/PlayerChoiceEmoji");
        _aiChoiceContainer = GetNodeOrNull<VBoxContainer>("RevealOverlay/RevealCenter/RevealSection/AIChoiceContainer");
        _aiChoiceEmoji = GetNodeOrNull<Label>("RevealOverlay/RevealCenter/RevealSection/AIChoiceContainer/AIChoiceEmoji");
        _aiChoiceLabel = GetNodeOrNull<Label>("RevealOverlay/RevealCenter/RevealSection/AIChoiceContainer/AIChoiceLabel");

        // Create and start game manager
        _gameManager = new GameManager();
        AddChild(_gameManager);
        _gameManager.StartTournament(_bracketSize);

        // Get the tournament controller
        _tournamentController = _gameManager.GetModeController() as TournamentController;

        // Connect button signals
        _rockButton?.Connect("pressed", new Callable(this, nameof(_OnRockButtonPressed)));
        _paperButton?.Connect("pressed", new Callable(this, nameof(_OnPaperButtonPressed)));
        _scissorsButton?.Connect("pressed", new Callable(this, nameof(_OnScissorsButtonPressed)));
        _continueButton?.Connect("pressed", new Callable(this, nameof(_OnContinueButtonPressed)));
        _playAgainButton?.Connect("pressed", new Callable(this, nameof(_OnPlayAgainButtonPressed)));

        // Hide continue/play again buttons initially
        if (_continueButton != null) _continueButton.Visible = false;
        if (_playAgainButton != null) _playAgainButton.Visible = false;

        UpdateTournamentDisplay();
        UpdateScoreDisplay();

        // Start gameplay music
        var audioManager = GetNodeOrNull<AudioManager>("/root/AudioManager");
        audioManager?.PlayMusic("music_gameplay");
    }

    private void _OnRockButtonPressed() {
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("button_click");
        PlayRound(GameLogic.Choice.Rock);
    }

    private void _OnPaperButtonPressed() {
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("button_click");
        PlayRound(GameLogic.Choice.Paper);
    }

    private void _OnScissorsButtonPressed() {
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("button_click");
        PlayRound(GameLogic.Choice.Scissors);
    }

    private async void PlayRound(GameLogic.Choice playerChoice) {
        if (_gameManager == null || _tournamentController == null) return;

        DisableChoiceButtons();
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("choice_made");

        // Get AI choice and determine winner
        var outcome = _gameManager.PlayRoundWithAI(playerChoice);
        var result = (GameLogic.Result)(int)outcome["result"];
        var aiChoice = (GameLogic.Choice)(int)outcome["aiChoice"];

        // Process the round result in tournament controller
        _tournamentController.ProcessRoundResult(result);

        bool isMatchComplete = _tournamentController.IsMatchComplete();

        // Dramatic reveal sequence
        await DramaticRevealSequence(playerChoice, aiChoice, result, isMatchComplete);

        // Check if match is complete
        if (isMatchComplete) {
            bool playerWonMatch = _tournamentController.GetPlayerScore() > _tournamentController.GetOpponentScore();
            _tournamentController.OnMatchComplete(playerWonMatch);

            if (_tournamentController.IsModeComplete()) {
                // Tournament is over (champion or eliminated)
                ShowPlayAgainButton();
            } else {
                // Move to next round
                ShowContinueButton();
            }
        } else {
            EnableChoiceButtons();
            HideChoiceDisplays();
        }

        UpdateTournamentDisplay();
        UpdateScoreDisplay();
    }

    private void UpdateTournamentDisplay() {
        if (_tournamentController == null) return;

        var state = _tournamentController.GetStateSnapshot();

        if (_titleLabel != null) {
            if ((bool)state["isChampion"]) {
                _titleLabel.Text = "🏆 TOURNAMENT CHAMPION! 🏆";
            } else if ((bool)state["isEliminated"]) {
                _titleLabel.Text = "ELIMINATED";
            } else {
                _titleLabel.Text = "TOURNAMENT MODE";
            }
        }

        if (_roundLabel != null) {
            _roundLabel.Text = (string)state["roundName"];
        }

        if (_opponentLabel != null) {
            if (_tournamentController.IsModeComplete()) {
                _opponentLabel.Text = _tournamentController.GetStatusMessage();
            } else {
                _opponentLabel.Text = $"vs {state["opponentName"]}";
            }
        }

        // Update AI choice label with opponent name
        if (_aiChoiceLabel != null && !_tournamentController.IsModeComplete()) {
            _aiChoiceLabel.Text = $"{_tournamentController.GetCurrentOpponentName()} CHOSE:";
        }
    }

    private void UpdateScoreDisplay() {
        if (_tournamentController == null || _scoreLabel == null) return;

        _scoreLabel.Text = $"You: {_tournamentController.GetPlayerScore()} | Opponent: {_tournamentController.GetOpponentScore()}";
    }

    private void DisplayRoundResult(GameLogic.Result result, bool isMatchComplete) {
        if (_resultLabel == null || _tournamentController == null) return;

        var audioManager = GetNodeOrNull<AudioManager>("/root/AudioManager");

        if (isMatchComplete) {
            bool playerWon = _tournamentController.GetPlayerScore() > _tournamentController.GetOpponentScore();
            if (playerWon) {
                audioManager?.PlaySFX("match_victory");
                if (_tournamentController.IsChampion()) {
                    _resultLabel.Text = "🎉🏆 TOURNAMENT CHAMPION! 🏆🎉";
                } else {
                    _resultLabel.Text = $"✓ You advance to {_tournamentController.GetCurrentRoundName()}!";
                }
            } else {
                audioManager?.PlaySFX("match_defeat");
                _resultLabel.Text = "😞 ELIMINATED FROM TOURNAMENT";
            }
        } else {
            audioManager?.PlaySFX("choice_reveal");
            if (result == GameLogic.Result.PlayerWins) {
                audioManager?.PlaySFX("round_win");
            } else if (result == GameLogic.Result.AIWins) {
                audioManager?.PlaySFX("round_lose");
            }

            _resultLabel.Text = result switch {
                GameLogic.Result.PlayerWins => "✓ You win this round!",
                GameLogic.Result.AIWins => "✗ Opponent wins this round",
                GameLogic.Result.Draw => "⚖️ Draw - play again!",
                _ => ""
            };
        }
    }

    private async System.Threading.Tasks.Task DramaticRevealSequence(
        GameLogic.Choice playerChoice,
        GameLogic.Choice aiChoice,
        GameLogic.Result result,
        bool isMatchComplete) {

        // Show overlay
        if (_revealOverlay != null) _revealOverlay.Visible = true;
        if (_revealBackdrop != null) {
            _revealBackdrop.Modulate = new Color(1, 1, 1, 0);
            var backdropTween = CreateTween();
            backdropTween.TweenProperty(_revealBackdrop, "modulate:a", 1.0f, 0.2f);
        }

        HideChoiceDisplays();
        if (_resultLabel != null) _resultLabel.Text = " ";

        // Show player choice
        ShowPlayerChoice(playerChoice);
        await ToSignal(GetTree().CreateTimer(PLAYER_REVEAL_DELAY), SceneTreeTimer.SignalName.Timeout);

        // Show AI choice
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("choice_reveal");
        ShowAIChoice(aiChoice);
        await ToSignal(GetTree().CreateTimer(AI_REVEAL_DELAY), SceneTreeTimer.SignalName.Timeout);

        // Show result
        UpdateScoreDisplay();
        DisplayRoundResult(result, isMatchComplete);
        await ToSignal(GetTree().CreateTimer(RESULT_DISPLAY_TIME), SceneTreeTimer.SignalName.Timeout);

        // Hide overlay
        if (_revealBackdrop != null) {
            var backdropTween = CreateTween();
            backdropTween.TweenProperty(_revealBackdrop, "modulate:a", 0.0f, 0.2f);
            await ToSignal(backdropTween, Tween.SignalName.Finished);
        }
        if (_revealOverlay != null) _revealOverlay.Visible = false;
    }

    private void ShowPlayerChoice(GameLogic.Choice choice) {
        if (_playerChoiceContainer == null || _playerChoiceEmoji == null) return;

        _playerChoiceEmoji.Text = GetChoiceEmoji(choice);
        _playerChoiceContainer.Modulate = new Color(1, 1, 1, 0);
        _playerChoiceContainer.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(_playerChoiceContainer, "modulate:a", 1.0f, 0.3f);
    }

    private void ShowAIChoice(GameLogic.Choice choice) {
        if (_aiChoiceContainer == null || _aiChoiceEmoji == null) return;

        _aiChoiceEmoji.Text = GetChoiceEmoji(choice);
        _aiChoiceContainer.Modulate = new Color(1, 1, 1, 0);
        _aiChoiceContainer.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(_aiChoiceContainer, "modulate:a", 1.0f, 0.3f);
    }

    private void HideChoiceDisplays() {
        if (_playerChoiceContainer != null) _playerChoiceContainer.Visible = false;
        if (_aiChoiceContainer != null) _aiChoiceContainer.Visible = false;
    }

    private static string GetChoiceEmoji(GameLogic.Choice choice) {
        return choice switch {
            GameLogic.Choice.Rock => "🪨",
            GameLogic.Choice.Paper => "📄",
            GameLogic.Choice.Scissors => "✂️",
            _ => "?"
        };
    }

    private void DisableChoiceButtons() {
        if (_rockButton != null) _rockButton.Disabled = true;
        if (_paperButton != null) _paperButton.Disabled = true;
        if (_scissorsButton != null) _scissorsButton.Disabled = true;
    }

    private void EnableChoiceButtons() {
        if (_rockButton != null) _rockButton.Disabled = false;
        if (_paperButton != null) _paperButton.Disabled = false;
        if (_scissorsButton != null) _scissorsButton.Disabled = false;
    }

    private void ShowContinueButton() {
        if (_continueButton != null) _continueButton.Visible = true;
        DisableChoiceButtons();
    }

    private void ShowPlayAgainButton() {
        if (_playAgainButton != null) _playAgainButton.Visible = true;
        DisableChoiceButtons();
    }

    private void _OnContinueButtonPressed() {
        // Hide continue button and enable choice buttons for next match
        if (_continueButton != null) _continueButton.Visible = false;
        EnableChoiceButtons();
        HideChoiceDisplays();
        UpdateTournamentDisplay();
        UpdateScoreDisplay();
        if (_resultLabel != null) _resultLabel.Text = " ";
    }

    private void _OnPlayAgainButtonPressed() {
        // Restart tournament
        _gameManager?.StartTournament(_bracketSize);
        _tournamentController = _gameManager?.GetModeController() as TournamentController;

        if (_playAgainButton != null) _playAgainButton.Visible = false;
        if (_continueButton != null) _continueButton.Visible = false;

        EnableChoiceButtons();
        HideChoiceDisplays();
        UpdateTournamentDisplay();
        UpdateScoreDisplay();
        if (_resultLabel != null) _resultLabel.Text = " ";
    }

    private void _OnBackButtonPressed() {
        GetTree().ChangeSceneToFile("res://scenes/mode_selection/ModeSelection.tscn");
    }
}
