using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.Managers;
using SPR;

namespace SprGame.UI;

/// <summary>
/// Controller for the Survival game mode scene.
/// Player has limited HP and tries to defeat as many opponents as possible.
/// </summary>
public partial class SurvivalSceneController : Control {
    // Timing constants for dramatic reveal
    private const float PLAYER_REVEAL_DELAY = 0.5f;
    private const float AI_REVEAL_DELAY = 1.0f;
    private const float RESULT_DISPLAY_TIME = 2.0f;

    private GameManager? _gameManager;
    private SurvivalController? _survivalController;

    // UI References
    private Label? _titleLabel;
    private Label? _hpLabel;
    private ProgressBar? _hpBar;
    private Label? _defeatedLabel;
    private Label? _highScoreLabel;
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

    public override void _Ready() {
        // Get UI node references
        _titleLabel = GetNodeOrNull<Label>("GameContainer/TopSection/TitleLabel");
        _hpLabel = GetNodeOrNull<Label>("GameContainer/TopSection/HPLabel");
        _hpBar = GetNodeOrNull<ProgressBar>("GameContainer/TopSection/HPBar");
        _defeatedLabel = GetNodeOrNull<Label>("GameContainer/TopSection/DefeatedLabel");
        _highScoreLabel = GetNodeOrNull<Label>("GameContainer/TopSection/HighScoreLabel");
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

        // Create and start game manager
        _gameManager = new GameManager();
        AddChild(_gameManager);
        _gameManager.StartSurvival(10);

        // Get the survival controller
        _survivalController = _gameManager.GetModeController() as SurvivalController;

        // Connect button signals
        _rockButton?.Connect("pressed", new Callable(this, nameof(_OnRockButtonPressed)));
        _paperButton?.Connect("pressed", new Callable(this, nameof(_OnPaperButtonPressed)));
        _scissorsButton?.Connect("pressed", new Callable(this, nameof(_OnScissorsButtonPressed)));
        _continueButton?.Connect("pressed", new Callable(this, nameof(_OnContinueButtonPressed)));
        _playAgainButton?.Connect("pressed", new Callable(this, nameof(_OnPlayAgainButtonPressed)));

        // Hide continue/play again buttons initially
        if (_continueButton != null) _continueButton.Visible = false;
        if (_playAgainButton != null) _playAgainButton.Visible = false;

        UpdateSurvivalDisplay();
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
        if (_gameManager == null || _survivalController == null) return;

        DisableChoiceButtons();
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("choice_made");

        // Get AI choice and determine winner
        var outcome = _gameManager.PlayRoundWithAI(playerChoice);
        var result = (GameLogic.Result)(int)outcome["result"];
        var aiChoice = (GameLogic.Choice)(int)outcome["aiChoice"];

        // Process the round result in survival controller
        _survivalController.ProcessRoundResult(result);

        bool isMatchComplete = _survivalController.IsMatchComplete();

        // Dramatic reveal sequence
        await DramaticRevealSequence(playerChoice, aiChoice, result, isMatchComplete);

        // Check if match is complete
        if (isMatchComplete) {
            bool playerWonMatch = _survivalController.GetPlayerScore() > _survivalController.GetOpponentScore();
            _survivalController.OnMatchComplete(playerWonMatch);

            UpdateSurvivalDisplay();

            if (_survivalController.IsModeComplete()) {
                // Game over (HP reached 0)
                ShowPlayAgainButton();
            } else {
                // Continue to next opponent
                ShowContinueButton();
            }
        } else {
            EnableChoiceButtons();
            HideChoiceDisplays();
        }

        UpdateScoreDisplay();
    }

    private void UpdateSurvivalDisplay() {
        if (_survivalController == null) return;

        var state = _survivalController.GetStateSnapshot();
        int currentHp = (int)state["currentHp"];
        int startingHp = (int)state["startingHp"];
        int defeated = (int)state["opponentsDefeated"];
        int highScore = (int)state["highScore"];
        bool isGameOver = (bool)state["isGameOver"];
        bool isNewHighScore = (bool)state["isNewHighScore"];

        if (_titleLabel != null) {
            if (isGameOver) {
                _titleLabel.Text = isNewHighScore ? "🎉 NEW HIGH SCORE! 🎉" : "GAME OVER";
            } else {
                _titleLabel.Text = "SURVIVAL MODE";
            }
        }

        if (_hpLabel != null) {
            _hpLabel.Text = $"HP: {currentHp}/{startingHp}";
            // Change color based on HP
            if (currentHp <= 3) {
                _hpLabel.Modulate = new Color(1, 0.3f, 0.3f, 1); // Red when low
            } else if (currentHp <= 5) {
                _hpLabel.Modulate = new Color(1, 0.8f, 0.3f, 1); // Yellow when medium
            } else {
                _hpLabel.Modulate = new Color(0.3f, 1, 0.3f, 1); // Green when high
            }
        }

        if (_hpBar != null) {
            _hpBar.Value = (float)state["hpPercentage"] * 100;
        }

        if (_defeatedLabel != null) {
            _defeatedLabel.Text = $"Opponents Defeated: {defeated}";
        }

        if (_highScoreLabel != null) {
            if (isNewHighScore && defeated > 0) {
                _highScoreLabel.Text = $"🏆 NEW BEST: {defeated}";
                _highScoreLabel.Modulate = new Color(1, 0.84f, 0, 1); // Gold
            } else {
                _highScoreLabel.Text = $"Best: {highScore}";
                _highScoreLabel.Modulate = new Color(1, 1, 1, 1);
            }
        }
    }

    private void UpdateScoreDisplay() {
        if (_survivalController == null || _scoreLabel == null) return;

        _scoreLabel.Text = $"You: {_survivalController.GetPlayerScore()} | AI: {_survivalController.GetOpponentScore()}";
    }

    private void DisplayRoundResult(GameLogic.Result result, bool isMatchComplete) {
        if (_resultLabel == null || _survivalController == null) return;

        var audioManager = GetNodeOrNull<AudioManager>("/root/AudioManager");

        if (isMatchComplete) {
            bool playerWon = _survivalController.GetPlayerScore() > _survivalController.GetOpponentScore();
            if (playerWon) {
                audioManager?.PlaySFX("match_victory");
                _resultLabel.Text = "✓ Opponent defeated!";
            } else {
                audioManager?.PlaySFX("match_defeat");
                int hpAfterLoss = _survivalController.GetCurrentHp() - 1; // HP will be decremented
                if (hpAfterLoss <= 0) {
                    _resultLabel.Text = $"💀 GAME OVER - Defeated {_survivalController.GetOpponentsDefeated()} opponents!";
                } else {
                    _resultLabel.Text = $"✗ You lost! HP -1 ({hpAfterLoss} remaining)";
                }
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
                GameLogic.Result.AIWins => "✗ AI wins this round",
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
        if (_continueButton != null) _continueButton.Visible = false;
        EnableChoiceButtons();
        HideChoiceDisplays();
        UpdateSurvivalDisplay();
        UpdateScoreDisplay();
        if (_resultLabel != null) _resultLabel.Text = " ";
    }

    private void _OnPlayAgainButtonPressed() {
        // Restart survival
        _gameManager?.StartSurvival(10);
        _survivalController = _gameManager?.GetModeController() as SurvivalController;

        if (_playAgainButton != null) _playAgainButton.Visible = false;
        if (_continueButton != null) _continueButton.Visible = false;

        EnableChoiceButtons();
        HideChoiceDisplays();
        UpdateSurvivalDisplay();
        UpdateScoreDisplay();
        if (_resultLabel != null) _resultLabel.Text = " ";
    }

    private void _OnBackButtonPressed() {
        GetTree().ChangeSceneToFile("res://scenes/mode_selection/ModeSelection.tscn");
    }
}
