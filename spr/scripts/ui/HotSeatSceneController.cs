using Godot;
using Godot.Collections;
using SprGame.Core;
using SprGame.Managers;
using SPR;

namespace SprGame.UI;

/// <summary>
/// Controller for the Hot Seat (local multiplayer) game mode scene.
/// Two players take turns on the same device with transition screens for privacy.
/// </summary>
public partial class HotSeatSceneController : Control {
    // Timing constants for dramatic reveal
    private const float CHOICE_REVEAL_DELAY = 0.5f;
    private const float RESULT_DISPLAY_TIME = 2.0f;

    private GameManager? _gameManager;
    private HotSeatController? _hotSeatController;

    // UI References
    private Label? _titleLabel;
    private Label? _currentPlayerLabel;
    private Label? _scoreLabel;
    private Label? _resultLabel;
    private Button? _rockButton;
    private Button? _paperButton;
    private Button? _scissorsButton;
    private Button? _playAgainButton;

    // Choice buttons container
    private Control? _choiceArea;

    // Transition screen
    private Control? _transitionScreen;
    private Label? _transitionMessage;
    private Button? _readyButton;

    // Choice display references
    private CanvasLayer? _revealOverlay;
    private ColorRect? _revealBackdrop;
    private VBoxContainer? _player1ChoiceContainer;
    private Label? _player1ChoiceEmoji;
    private Label? _player1ChoiceLabel;
    private VBoxContainer? _player2ChoiceContainer;
    private Label? _player2ChoiceEmoji;
    private Label? _player2ChoiceLabel;

    // Player names (can be set from config)
    private string _player1Name = "Player 1";
    private string _player2Name = "Player 2";

    public override void _Ready() {
        // Get UI node references
        _titleLabel = GetNodeOrNull<Label>("GameContainer/TopSection/TitleLabel");
        _currentPlayerLabel = GetNodeOrNull<Label>("GameContainer/TopSection/CurrentPlayerLabel");
        _scoreLabel = GetNodeOrNull<Label>("GameContainer/TopSection/ScoreLabel");
        _resultLabel = GetNodeOrNull<Label>("GameContainer/MiddleSection/ContentContainer/ResultLabel");
        _rockButton = GetNodeOrNull<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/RockButton");
        _paperButton = GetNodeOrNull<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/PaperButton");
        _scissorsButton = GetNodeOrNull<Button>("GameContainer/MiddleSection/ContentContainer/ChoiceButtons/ScissorsButton");
        _playAgainButton = GetNodeOrNull<Button>("GameContainer/BottomSection/PlayAgainButton");
        _choiceArea = GetNodeOrNull<Control>("GameContainer/MiddleSection/ContentContainer");

        // Get transition screen references
        _transitionScreen = GetNodeOrNull<Control>("TransitionScreen");
        _transitionMessage = GetNodeOrNull<Label>("TransitionScreen/CenterContainer/VBox/MessageLabel");
        _readyButton = GetNodeOrNull<Button>("TransitionScreen/CenterContainer/VBox/ReadyButton");

        // Get choice display references
        _revealOverlay = GetNodeOrNull<CanvasLayer>("RevealOverlay");
        _revealBackdrop = GetNodeOrNull<ColorRect>("RevealOverlay/Backdrop");
        _player1ChoiceContainer = GetNodeOrNull<VBoxContainer>("RevealOverlay/RevealCenter/RevealSection/Player1ChoiceContainer");
        _player1ChoiceEmoji = GetNodeOrNull<Label>("RevealOverlay/RevealCenter/RevealSection/Player1ChoiceContainer/Player1ChoiceEmoji");
        _player1ChoiceLabel = GetNodeOrNull<Label>("RevealOverlay/RevealCenter/RevealSection/Player1ChoiceContainer/Player1ChoiceLabel");
        _player2ChoiceContainer = GetNodeOrNull<VBoxContainer>("RevealOverlay/RevealCenter/RevealSection/Player2ChoiceContainer");
        _player2ChoiceEmoji = GetNodeOrNull<Label>("RevealOverlay/RevealCenter/RevealSection/Player2ChoiceContainer/Player2ChoiceEmoji");
        _player2ChoiceLabel = GetNodeOrNull<Label>("RevealOverlay/RevealCenter/RevealSection/Player2ChoiceContainer/Player2ChoiceLabel");

        // Create and start game manager
        _gameManager = new GameManager();
        AddChild(_gameManager);
        _gameManager.StartHotSeat(_player1Name, _player2Name);

        // Get the hot seat controller
        _hotSeatController = _gameManager.GetModeController() as HotSeatController;

        // Update player names from controller
        if (_hotSeatController != null) {
            _player1Name = _hotSeatController.GetPlayer1Name();
            _player2Name = _hotSeatController.GetPlayer2Name();
        }

        // Connect button signals
        _rockButton?.Connect("pressed", new Callable(this, nameof(_OnRockButtonPressed)));
        _paperButton?.Connect("pressed", new Callable(this, nameof(_OnPaperButtonPressed)));
        _scissorsButton?.Connect("pressed", new Callable(this, nameof(_OnScissorsButtonPressed)));
        _readyButton?.Connect("pressed", new Callable(this, nameof(_OnReadyButtonPressed)));
        _playAgainButton?.Connect("pressed", new Callable(this, nameof(_OnPlayAgainButtonPressed)));

        // Hide buttons/screens initially
        if (_playAgainButton != null) _playAgainButton.Visible = false;
        if (_transitionScreen != null) _transitionScreen.Visible = false;
        if (_revealOverlay != null) _revealOverlay.Visible = false;

        UpdateDisplay();
        UpdateScoreDisplay();

        // Start gameplay music
        var audioManager = GetNodeOrNull<AudioManager>("/root/AudioManager");
        audioManager?.PlayMusic("music_gameplay");
    }

    private void _OnRockButtonPressed() {
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("button_click");
        MakeChoice(GameLogic.Choice.Rock);
    }

    private void _OnPaperButtonPressed() {
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("button_click");
        MakeChoice(GameLogic.Choice.Paper);
    }

    private void _OnScissorsButtonPressed() {
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("button_click");
        MakeChoice(GameLogic.Choice.Scissors);
    }

    private void MakeChoice(GameLogic.Choice choice) {
        if (_hotSeatController == null) return;

        var phase = _hotSeatController.GetCurrentPhase();
        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("choice_made");

        if (phase == HotSeatController.TurnPhase.Player1Choosing) {
            _hotSeatController.Player1MakesChoice(choice);
            ShowTransitionScreen();
        } else if (phase == HotSeatController.TurnPhase.Player2Choosing) {
            _hotSeatController.Player2MakesChoice(choice);
            ShowTransitionScreen();
        }
    }

    private void ShowTransitionScreen() {
        if (_transitionScreen == null || _transitionMessage == null || _hotSeatController == null) return;

        // Hide choice area
        if (_choiceArea != null) _choiceArea.Visible = false;

        // Show transition screen with appropriate message
        _transitionMessage.Text = _hotSeatController.GetTransitionMessage();
        _transitionScreen.Visible = true;

        // Update ready button text based on phase
        if (_readyButton != null) {
            var phase = _hotSeatController.GetCurrentPhase();
            if (phase == HotSeatController.TurnPhase.TransitionToPlayer2) {
                _readyButton.Text = "I'M READY";
            } else if (phase == HotSeatController.TurnPhase.TransitionToReveal) {
                _readyButton.Text = "REVEAL!";
            }
        }
    }

    private async void _OnReadyButtonPressed() {
        if (_hotSeatController == null) return;

        var phase = _hotSeatController.GetCurrentPhase();

        if (phase == HotSeatController.TurnPhase.TransitionToPlayer2) {
            // Player 2 is ready to make their choice
            _hotSeatController.Player2Ready();
            if (_transitionScreen != null) _transitionScreen.Visible = false;
            if (_choiceArea != null) _choiceArea.Visible = true;
            UpdateDisplay();
        } else if (phase == HotSeatController.TurnPhase.TransitionToReveal) {
            // Both players ready for reveal
            _hotSeatController.ReadyForReveal();
            if (_transitionScreen != null) _transitionScreen.Visible = false;
            await PlayRevealSequence();
        }
    }

    private async System.Threading.Tasks.Task PlayRevealSequence() {
        if (_hotSeatController == null) return;

        var p1Choice = _hotSeatController.GetPlayer1Choice();
        var p2Choice = _hotSeatController.GetPlayer2Choice();

        if (p1Choice == null || p2Choice == null) return;

        var result = _hotSeatController.RevealAndDetermineWinner();
        bool isMatchComplete = _hotSeatController.IsMatchComplete();

        // Dramatic reveal sequence
        await DramaticRevealSequence(p1Choice.Value, p2Choice.Value, result, isMatchComplete);

        // Update display
        UpdateScoreDisplay();

        if (isMatchComplete) {
            ShowPlayAgainButton();
        } else {
            // Reset for next round
            _hotSeatController.ResetRound();
            if (_choiceArea != null) _choiceArea.Visible = true;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay() {
        if (_hotSeatController == null) return;

        if (_titleLabel != null) {
            if (_hotSeatController.IsMatchComplete()) {
                _titleLabel.Text = $"🎉 {_hotSeatController.GetWinnerName()} WINS! 🎉";
            } else {
                _titleLabel.Text = "HOT SEAT MODE";
            }
        }

        if (_currentPlayerLabel != null) {
            string currentPlayer = _hotSeatController.GetCurrentPlayerName();
            if (!string.IsNullOrEmpty(currentPlayer)) {
                _currentPlayerLabel.Text = $"{currentPlayer}'s Turn";
                _currentPlayerLabel.Visible = true;
            } else {
                _currentPlayerLabel.Visible = false;
            }
        }

        // Update choice labels for reveal
        if (_player1ChoiceLabel != null) {
            _player1ChoiceLabel.Text = $"{_player1Name} CHOSE:";
        }
        if (_player2ChoiceLabel != null) {
            _player2ChoiceLabel.Text = $"{_player2Name} CHOSE:";
        }
    }

    private void UpdateScoreDisplay() {
        if (_hotSeatController == null || _scoreLabel == null) return;

        _scoreLabel.Text = $"{_player1Name}: {_hotSeatController.GetPlayerScore()} | {_player2Name}: {_hotSeatController.GetOpponentScore()}";
    }

    private void DisplayRoundResult(GameLogic.Result result, bool isMatchComplete) {
        if (_resultLabel == null || _hotSeatController == null) return;

        var audioManager = GetNodeOrNull<AudioManager>("/root/AudioManager");

        if (isMatchComplete) {
            string winner = _hotSeatController.GetWinnerName();
            audioManager?.PlaySFX("match_victory");
            _resultLabel.Text = $"🎉 {winner} WINS THE MATCH! 🎉";
        } else {
            audioManager?.PlaySFX("choice_reveal");
            string roundWinner = _hotSeatController.GetRoundWinnerName();

            _resultLabel.Text = result switch {
                GameLogic.Result.PlayerWins => $"✓ {_player1Name} wins this round!",
                GameLogic.Result.AIWins => $"✓ {_player2Name} wins this round!",
                GameLogic.Result.Draw => "⚖️ Draw - play again!",
                _ => ""
            };
        }
    }

    private async System.Threading.Tasks.Task DramaticRevealSequence(
        GameLogic.Choice player1Choice,
        GameLogic.Choice player2Choice,
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

        // Show both choices simultaneously
        ShowPlayer1Choice(player1Choice);
        await ToSignal(GetTree().CreateTimer(CHOICE_REVEAL_DELAY), SceneTreeTimer.SignalName.Timeout);

        GetNodeOrNull<AudioManager>("/root/AudioManager")?.PlaySFX("choice_reveal");
        ShowPlayer2Choice(player2Choice);
        await ToSignal(GetTree().CreateTimer(CHOICE_REVEAL_DELAY), SceneTreeTimer.SignalName.Timeout);

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

    private void ShowPlayer1Choice(GameLogic.Choice choice) {
        if (_player1ChoiceContainer == null || _player1ChoiceEmoji == null) return;

        _player1ChoiceEmoji.Text = GetChoiceEmoji(choice);
        _player1ChoiceContainer.Modulate = new Color(1, 1, 1, 0);
        _player1ChoiceContainer.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(_player1ChoiceContainer, "modulate:a", 1.0f, 0.3f);
    }

    private void ShowPlayer2Choice(GameLogic.Choice choice) {
        if (_player2ChoiceContainer == null || _player2ChoiceEmoji == null) return;

        _player2ChoiceEmoji.Text = GetChoiceEmoji(choice);
        _player2ChoiceContainer.Modulate = new Color(1, 1, 1, 0);
        _player2ChoiceContainer.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(_player2ChoiceContainer, "modulate:a", 1.0f, 0.3f);
    }

    private void HideChoiceDisplays() {
        if (_player1ChoiceContainer != null) _player1ChoiceContainer.Visible = false;
        if (_player2ChoiceContainer != null) _player2ChoiceContainer.Visible = false;
    }

    private static string GetChoiceEmoji(GameLogic.Choice choice) {
        return choice switch {
            GameLogic.Choice.Rock => "🪨",
            GameLogic.Choice.Paper => "📄",
            GameLogic.Choice.Scissors => "✂️",
            _ => "?"
        };
    }

    private void ShowPlayAgainButton() {
        if (_playAgainButton != null) _playAgainButton.Visible = true;
        if (_currentPlayerLabel != null) _currentPlayerLabel.Visible = false;
    }

    private void _OnPlayAgainButtonPressed() {
        // Restart hot seat
        _gameManager?.StartHotSeat(_player1Name, _player2Name);
        _hotSeatController = _gameManager?.GetModeController() as HotSeatController;

        if (_playAgainButton != null) _playAgainButton.Visible = false;
        if (_choiceArea != null) _choiceArea.Visible = true;

        HideChoiceDisplays();
        UpdateDisplay();
        UpdateScoreDisplay();
        if (_resultLabel != null) _resultLabel.Text = " ";
    }

    private void _OnBackButtonPressed() {
        GetTree().ChangeSceneToFile("res://scenes/mode_selection/ModeSelection.tscn");
    }
}
