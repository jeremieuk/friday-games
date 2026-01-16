using System.Threading.Tasks;
using Godot;
using SprGame.Core;
using SprGame.Managers;

namespace SprGame.UI;

/// <summary>
/// Controls the Single Match scene UI and gameplay flow.
/// Presentation Layer: Full Godot integration.
/// </summary>
public partial class SingleMatchController : Control {
    // Timing constants for dramatic reveal
    private const float PLAYER_REVEAL_DELAY = 0.5f;
    private const float AI_REVEAL_DELAY = 1.0f;
    private const float RESULT_DISPLAY_TIME = 2.0f;

    private GameManager? _gameManager;

    // UI References
    private Label? _scoreLabel;
    private Label? _resultLabel;
    private Button? _rockButton;
    private Button? _paperButton;
    private Button? _scissorsButton;
    private Button? _playAgainButton;

    // Choice display references
    private CanvasLayer? _revealOverlay;
    private ColorRect? _revealBackdrop;
    private VBoxContainer? _playerChoiceContainer;
    private Label? _playerChoiceEmoji;
    private VBoxContainer? _aiChoiceContainer;
    private Label? _aiChoiceEmoji;

    // Animation support
    private Tween? _currentTween;

    // Particle effects
    private CpuParticles2D? _rockDebris;
    private CpuParticles2D? _paperFlutter;
    private CpuParticles2D? _scissorsSparkle;
    private CpuParticles2D? _victoryConfetti;

    public override void _Ready() {
        // Get UI node references
        _scoreLabel = GetNode<Label>("GameContainer/TopSection/ScoreLabel");
        _resultLabel = GetNode<Label>("GameContainer/MiddleSection/ContentContainer/ResultLabel");
        _rockButton = GetNode<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/RockButton");
        _paperButton = GetNode<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/PaperButton");
        _scissorsButton = GetNode<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/ScissorsButton");
        _playAgainButton = GetNode<Button>("GameContainer/BottomSection/PlayAgainButton");

        // Get choice display references (overlay structure)
        _revealOverlay = GetNode<CanvasLayer>("RevealOverlay");
        _revealBackdrop = GetNode<ColorRect>("RevealOverlay/Backdrop");
        _playerChoiceContainer = GetNode<VBoxContainer>("RevealOverlay/RevealCenter/RevealSection/PlayerChoiceContainer");
        _playerChoiceEmoji = GetNode<Label>("RevealOverlay/RevealCenter/RevealSection/PlayerChoiceContainer/PlayerChoiceEmoji");
        _aiChoiceContainer = GetNode<VBoxContainer>("RevealOverlay/RevealCenter/RevealSection/AIChoiceContainer");
        _aiChoiceEmoji = GetNode<Label>("RevealOverlay/RevealCenter/RevealSection/AIChoiceContainer/AIChoiceEmoji");

        // Get particle effect references
        _rockDebris = GetNode<CpuParticles2D>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/RockButton/RockDebris");
        _paperFlutter = GetNode<CpuParticles2D>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/PaperButton/PaperFlutter");
        _scissorsSparkle = GetNode<CpuParticles2D>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/ScissorsButton/ScissorsSparkle");
        _victoryConfetti = GetNode<CpuParticles2D>("RevealOverlay/VictoryConfetti");

        // Create and start game manager
        _gameManager = new GameManager();
        AddChild(_gameManager);
        _gameManager.StartSingleMatch();

        UpdateScoreDisplay();

        // Connect button hover signals for animations and particles
        _rockButton.MouseEntered += () => {
            AnimateButtonHover(_rockButton);
            if (_rockDebris != null) _rockDebris.Emitting = true;
        };
        _rockButton.MouseExited += () => {
            AnimateButtonNormal(_rockButton);
            if (_rockDebris != null) _rockDebris.Emitting = false;
        };

        _paperButton.MouseEntered += () => {
            AnimateButtonHover(_paperButton);
            if (_paperFlutter != null) _paperFlutter.Emitting = true;
        };
        _paperButton.MouseExited += () => {
            AnimateButtonNormal(_paperButton);
            if (_paperFlutter != null) _paperFlutter.Emitting = false;
        };

        _scissorsButton.MouseEntered += () => {
            AnimateButtonHover(_scissorsButton);
            if (_scissorsSparkle != null) _scissorsSparkle.Emitting = true;
        };
        _scissorsButton.MouseExited += () => {
            AnimateButtonNormal(_scissorsButton);
            if (_scissorsSparkle != null) _scissorsSparkle.Emitting = false;
        };

        // Start gameplay music
        GetNode<AudioManager>("/root/AudioManager").PlayMusic("music_gameplay");
    }

    private void AnimateButtonHover(Button button) {
        var tween = CreateTween();
        tween.TweenProperty(button, "scale", new Vector2(1.05f, 1.05f), 0.1f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
    }

    private void AnimateButtonNormal(Button button) {
        var tween = CreateTween();
        tween.TweenProperty(button, "scale", Vector2.One, 0.1f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
    }

    private void _OnRockButtonPressed() {
        GetNode<AudioManager>("/root/AudioManager").PlaySFX("button_click");
        PlayRound(GameLogic.Choice.Rock);
    }

    private void _OnPaperButtonPressed() {
        GetNode<AudioManager>("/root/AudioManager").PlaySFX("button_click");
        PlayRound(GameLogic.Choice.Paper);
    }

    private void _OnScissorsButtonPressed() {
        GetNode<AudioManager>("/root/AudioManager").PlaySFX("button_click");
        PlayRound(GameLogic.Choice.Scissors);
    }

    private async void PlayRound(GameLogic.Choice playerChoice) {
        if (_gameManager == null) {
            return;
        }

        // Disable buttons immediately (prevent spam)
        DisableChoiceButtons();
        GetNode<AudioManager>("/root/AudioManager").PlaySFX("choice_made");

        // Get outcome (now includes aiChoice)
        var outcome = _gameManager.PlayRoundWithAI(playerChoice);
        var result = (GameLogic.Result)(int)outcome["result"];
        var aiChoice = (GameLogic.Choice)(int)outcome["aiChoice"];
        var isMatchComplete = (bool)outcome["isMatchComplete"];

        // Dramatic reveal sequence (3.5 seconds)
        await DramaticRevealSequence(playerChoice, aiChoice, result, isMatchComplete);

        // Re-enable or keep disabled based on match state
        if (isMatchComplete) {
            ShowPlayAgainButton();
        } else {
            EnableChoiceButtons();
            HideChoiceDisplays();
        }
    }

    private void UpdateScoreDisplay() {
        if (_gameManager == null || _scoreLabel == null) {
            return;
        }

        var scores = _gameManager.GetCurrentScores();
        _scoreLabel.Text = $"You: {scores["player"]} | AI: {scores["ai"]}";

        // Pulse animation
        _scoreLabel.Scale = new Vector2(1.2f, 1.2f);
        var tween = CreateTween();
        tween.TweenProperty(_scoreLabel, "scale", Vector2.One, 0.3f)
            .SetTrans(Tween.TransitionType.Elastic)
            .SetEase(Tween.EaseType.Out);
    }

    private void DisplayRoundResult(GameLogic.Result result, bool isMatchComplete) {
        if (_resultLabel == null) {
            return;
        }

        var audioManager = GetNode<AudioManager>("/root/AudioManager");

        if (isMatchComplete) {
            // Play match end sounds
            if (result == GameLogic.Result.PlayerWins) {
                audioManager.PlaySFX("match_victory");
                // Trigger victory confetti
                if (_victoryConfetti != null) {
                    _victoryConfetti.Emitting = true;
                }
            } else {
                audioManager.PlaySFX("match_defeat");
            }

            _resultLabel.Text = result switch {
                GameLogic.Result.PlayerWins => "🎉 YOU WIN THE MATCH! 🎉",
                GameLogic.Result.AIWins => "😞 AI WINS THE MATCH",
                _ => "MATCH COMPLETE"
            };
        } else {
            // Play round end sounds
            audioManager.PlaySFX("choice_reveal");

            if (result == GameLogic.Result.PlayerWins) {
                audioManager.PlaySFX("round_win");
            } else if (result == GameLogic.Result.AIWins) {
                audioManager.PlaySFX("round_lose");
            }

            _resultLabel.Text = result switch {
                GameLogic.Result.PlayerWins => "✓ You win this round!",
                GameLogic.Result.AIWins => "✗ AI wins this round",
                GameLogic.Result.Draw => "⚖️ Draw - play again!",
                _ => ""
            };
        }

        // Fade in animation with scale
        _resultLabel.Modulate = new Color(1, 1, 1, 0);
        _resultLabel.Scale = new Vector2(0.9f, 0.9f);

        var tween = CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(_resultLabel, "modulate:a", 1.0f, 0.4f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        tween.TweenProperty(_resultLabel, "scale", Vector2.One, 0.4f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }

    private async Task DramaticRevealSequence(
        GameLogic.Choice playerChoice,
        GameLogic.Choice aiChoice,
        GameLogic.Result result,
        bool isMatchComplete) {

        var audioManager = GetNode<AudioManager>("/root/AudioManager");

        // Show overlay with fade-in backdrop
        if (_revealOverlay != null) {
            _revealOverlay.Visible = true;
        }
        if (_revealBackdrop != null) {
            _revealBackdrop.Modulate = new Color(1, 1, 1, 0);
            var backdropTween = CreateTween();
            backdropTween.TweenProperty(_revealBackdrop, "modulate:a", 1.0f, 0.2f);
        }

        HideChoiceDisplays();
        if (_resultLabel != null) {
            _resultLabel.Text = " ";
        }

        // Stage 1: Show player choice
        ShowPlayerChoice(playerChoice);
        await ToSignal(GetTree().CreateTimer(PLAYER_REVEAL_DELAY),
                       SceneTreeTimer.SignalName.Timeout);

        // Stage 2: Show AI choice (with suspense!)
        audioManager.PlaySFX("choice_reveal");
        ShowAIChoice(aiChoice);
        await ToSignal(GetTree().CreateTimer(AI_REVEAL_DELAY),
                       SceneTreeTimer.SignalName.Timeout);

        // Stage 3: Show result
        UpdateScoreDisplay();
        DisplayRoundResult(result, isMatchComplete);
        await ToSignal(GetTree().CreateTimer(RESULT_DISPLAY_TIME),
                       SceneTreeTimer.SignalName.Timeout);

        // Hide overlay with fade-out
        if (_revealBackdrop != null) {
            var backdropTween = CreateTween();
            backdropTween.TweenProperty(_revealBackdrop, "modulate:a", 0.0f, 0.2f);
            await ToSignal(backdropTween, Tween.SignalName.Finished);
        }
        if (_revealOverlay != null) {
            _revealOverlay.Visible = false;
        }
    }

    private void ShowPlayerChoice(GameLogic.Choice choice) {
        if (_playerChoiceContainer == null || _playerChoiceEmoji == null) {
            return;
        }

        _playerChoiceEmoji.Text = GetChoiceEmoji(choice);
        _playerChoiceContainer.Modulate = new Color(1, 1, 1, 0);  // Start transparent
        _playerChoiceContainer.Scale = new Vector2(0.8f, 0.8f);   // Start smaller
        _playerChoiceContainer.Visible = true;

        // Animate fade in + scale up
        _currentTween = CreateTween();
        _currentTween.SetParallel(true);
        _currentTween.TweenProperty(_playerChoiceContainer, "modulate:a", 1.0f, 0.3f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        _currentTween.TweenProperty(_playerChoiceContainer, "scale", Vector2.One, 0.3f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }

    private void ShowAIChoice(GameLogic.Choice choice) {
        if (_aiChoiceContainer == null || _aiChoiceEmoji == null) {
            return;
        }

        _aiChoiceEmoji.Text = GetChoiceEmoji(choice);
        _aiChoiceContainer.Modulate = new Color(1, 1, 1, 0);  // Start transparent
        _aiChoiceContainer.Scale = new Vector2(0.8f, 0.8f);   // Start smaller
        _aiChoiceContainer.Visible = true;

        // Animate fade in + scale up
        _currentTween = CreateTween();
        _currentTween.SetParallel(true);
        _currentTween.TweenProperty(_aiChoiceContainer, "modulate:a", 1.0f, 0.3f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
        _currentTween.TweenProperty(_aiChoiceContainer, "scale", Vector2.One, 0.3f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }

    private void HideChoiceDisplays() {
        if (_playerChoiceContainer != null) {
            _playerChoiceContainer.Visible = false;
        }
        if (_aiChoiceContainer != null) {
            _aiChoiceContainer.Visible = false;
        }
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

    private void ShowPlayAgainButton() {
        if (_playAgainButton != null) {
            _playAgainButton.Visible = true;
        }
    }

    private void HidePlayAgainButton() {
        if (_playAgainButton != null) {
            _playAgainButton.Visible = false;
        }
    }

    private void _OnPlayAgainButtonPressed() {
        // Reset match
        if (_gameManager != null) {
            _gameManager.StartSingleMatch();
        }

        // Reset UI
        HideChoiceDisplays();
        EnableChoiceButtons();
        HidePlayAgainButton();
        UpdateScoreDisplay();

        if (_resultLabel != null) {
            _resultLabel.Text = " ";
        }
    }

    private void _OnBackButtonPressed() {
        GetTree().ChangeSceneToFile("res://scenes/main_menu/MainMenu.tscn");
    }
}
