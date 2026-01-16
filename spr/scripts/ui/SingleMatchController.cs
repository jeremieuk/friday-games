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

    // Clash particle effects
    private CpuParticles2D? _scissorsShatter;
    private CpuParticles2D? _paperWrap;
    private CpuParticles2D? _paperShred;
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

        // Get clash particle effect references
        _scissorsShatter = GetNode<CpuParticles2D>("RevealOverlay/ClashParticles/ScissorsShatter");
        _paperWrap = GetNode<CpuParticles2D>("RevealOverlay/ClashParticles/PaperWrap");
        _paperShred = GetNode<CpuParticles2D>("RevealOverlay/ClashParticles/PaperShred");
        _victoryConfetti = GetNode<CpuParticles2D>("RevealOverlay/VictoryConfetti");

        // Create and start game manager
        _gameManager = new GameManager();
        AddChild(_gameManager);
        _gameManager.StartSingleMatch();

        UpdateScoreDisplay();

        // Connect button hover signals for animations only
        _rockButton.MouseEntered += () => AnimateButtonHover(_rockButton);
        _rockButton.MouseExited += () => AnimateButtonNormal(_rockButton);
        _paperButton.MouseEntered += () => AnimateButtonHover(_paperButton);
        _paperButton.MouseExited += () => AnimateButtonNormal(_paperButton);
        _scissorsButton.MouseEntered += () => AnimateButtonHover(_scissorsButton);
        _scissorsButton.MouseExited += () => AnimateButtonNormal(_scissorsButton);

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

        // Stage 2.5: Clash Animation (skip for draws)
        if (result != GameLogic.Result.Draw) {
            await TriggerClashAnimation(playerChoice, aiChoice, result);
        }

        // Stage 3: Show result
        UpdateScoreDisplay();
        DisplayRoundResult(result, isMatchComplete);
        await ToSignal(GetTree().CreateTimer(RESULT_DISPLAY_TIME),
                       SceneTreeTimer.SignalName.Timeout);

        // Reset emoji positions and visibility for next round
        if (_playerChoiceEmoji != null) {
            _playerChoiceEmoji.Position = Vector2.Zero;
            _playerChoiceEmoji.Scale = Vector2.One;
            _playerChoiceEmoji.Modulate = new Color(1, 1, 1, 1);
        }
        if (_aiChoiceEmoji != null) {
            _aiChoiceEmoji.Position = Vector2.Zero;
            _aiChoiceEmoji.Scale = Vector2.One;
            _aiChoiceEmoji.Modulate = new Color(1, 1, 1, 1);
        }

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

    /// <summary>
    /// Triggers the clash animation showing the cards moving together, colliding, and the loser breaking apart.
    /// </summary>
    private async Task TriggerClashAnimation(
        GameLogic.Choice playerChoice,
        GameLogic.Choice aiChoice,
        GameLogic.Result result) {

        // Calculate collision point
        Vector2 collisionPoint = GetCollisionMidpoint();

        // Determine which emoji loses and which wins
        Label? losingEmoji = result == GameLogic.Result.PlayerWins ? _aiChoiceEmoji : _playerChoiceEmoji;
        Label? winningEmoji = result == GameLogic.Result.PlayerWins ? _playerChoiceEmoji : _aiChoiceEmoji;
        GameLogic.Choice losingChoice = result == GameLogic.Result.PlayerWins ? aiChoice : playerChoice;
        GameLogic.Choice winningChoice = result == GameLogic.Result.PlayerWins ? playerChoice : aiChoice;

        // Phase 1: Move both emojis toward center (0.4s)
        var playerTween = CreateTween();
        var aiTween = CreateTween();

        if (_playerChoiceEmoji != null) {
            Vector2 playerTarget = collisionPoint - _playerChoiceEmoji.Size / 2;
            playerTween.TweenProperty(_playerChoiceEmoji, "global_position", playerTarget, 0.4f)
                .SetTrans(Tween.TransitionType.Cubic)
                .SetEase(Tween.EaseType.In);
        }

        if (_aiChoiceEmoji != null) {
            Vector2 aiTarget = collisionPoint - _aiChoiceEmoji.Size / 2;
            aiTween.TweenProperty(_aiChoiceEmoji, "global_position", aiTarget, 0.4f)
                .SetTrans(Tween.TransitionType.Cubic)
                .SetEase(Tween.EaseType.In);
        }

        // Wait for movement to complete
        await ToSignal(GetTree().CreateTimer(0.4f), SceneTreeTimer.SignalName.Timeout);

        // Phase 2: Emit particles at collision point (0.1s)
        CpuParticles2D? particleToEmit = GetDefeatParticle(losingChoice, winningChoice);

        if (particleToEmit != null) {
            particleToEmit.Position = collisionPoint;
            particleToEmit.Emitting = true;
        }

        // Optional impact sound (uncomment when audio file is available)
        // GetNode<AudioManager>("/root/AudioManager").PlaySFX("impact");

        // Phase 3: Break/fade losing emoji or special animation for paper-wraps-rock (0.3s)
        if (losingEmoji != null) {
            bool isPaperWrap = losingChoice == GameLogic.Choice.Rock && winningChoice == GameLogic.Choice.Paper;

            if (isPaperWrap) {
                // Rock stays visible but shakes (paper wraps around it)
                var shakeTween = CreateTween();
                shakeTween.TweenProperty(losingEmoji, "rotation", 0.1f, 0.1f);
                shakeTween.TweenProperty(losingEmoji, "rotation", -0.1f, 0.1f);
                shakeTween.TweenProperty(losingEmoji, "rotation", 0.0f, 0.1f);
            } else {
                // Normal break animation (scale to 0, fade out)
                var breakTween = CreateTween();
                breakTween.SetParallel(true);
                breakTween.TweenProperty(losingEmoji, "scale", Vector2.Zero, 0.3f)
                    .SetTrans(Tween.TransitionType.Back)
                    .SetEase(Tween.EaseType.In);
                breakTween.TweenProperty(losingEmoji, "modulate:a", 0.0f, 0.3f)
                    .SetTrans(Tween.TransitionType.Cubic);
            }
        }

        // Winning emoji bounces back slightly
        if (winningEmoji != null) {
            var bounceTween = CreateTween();
            bounceTween.TweenProperty(winningEmoji, "scale", new Vector2(1.2f, 1.2f), 0.15f)
                .SetTrans(Tween.TransitionType.Elastic)
                .SetEase(Tween.EaseType.Out);
            bounceTween.TweenProperty(winningEmoji, "scale", Vector2.One, 0.15f);
        }

        // Total time: 0.4 (move) + 0.1 (particles) + 0.3 (break) = 0.8s
        await ToSignal(GetTree().CreateTimer(0.4f), SceneTreeTimer.SignalName.Timeout);
    }

    /// <summary>
    /// Gets the appropriate defeat particle effect based on what choice lost and what defeated it.
    /// </summary>
    private CpuParticles2D? GetDefeatParticle(
        GameLogic.Choice losingChoice,
        GameLogic.Choice winningChoice) {

        return (losingChoice, winningChoice) switch {
            (GameLogic.Choice.Scissors, GameLogic.Choice.Rock) => _scissorsShatter,
            (GameLogic.Choice.Rock, GameLogic.Choice.Paper) => _paperWrap,
            (GameLogic.Choice.Paper, GameLogic.Choice.Scissors) => _paperShred,
            _ => null
        };
    }

    /// <summary>
    /// Gets the global position of the player's choice emoji for particle positioning.
    /// </summary>
    private Vector2 GetPlayerChoicePosition() {
        if (_playerChoiceEmoji != null) {
            return _playerChoiceEmoji.GlobalPosition + _playerChoiceEmoji.Size / 2;
        }
        return Vector2.Zero;
    }

    /// <summary>
    /// Gets the global position of the AI's choice emoji for particle positioning.
    /// </summary>
    private Vector2 GetAIChoicePosition() {
        if (_aiChoiceEmoji != null) {
            return _aiChoiceEmoji.GlobalPosition + _aiChoiceEmoji.Size / 2;
        }
        return Vector2.Zero;
    }

    /// <summary>
    /// Calculates the midpoint between player and AI emoji positions for collision.
    /// </summary>
    private Vector2 GetCollisionMidpoint() {
        Vector2 playerPos = GetPlayerChoicePosition();
        Vector2 aiPos = GetAIChoicePosition();
        return new Vector2(
            (playerPos.X + aiPos.X) / 2,
            (playerPos.Y + aiPos.Y) / 2
        );
    }
}
