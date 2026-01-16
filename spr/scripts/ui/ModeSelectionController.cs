using Godot;

namespace SprGame.UI;

/// <summary>
/// Controller for the Mode Selection screen.
/// Handles navigation to different game modes.
/// </summary>
public partial class ModeSelectionController : Control {
    // UI Node references
    private Button? _singleMatchButton;
    private Button? _tournamentButton;
    private Button? _survivalButton;
    private Button? _hotSeatButton;
    private Button? _backButton;

    // Tournament config
    private OptionButton? _tournamentSizeOption;
    private Control? _tournamentConfig;

    // Hot Seat config
    private LineEdit? _player1NameInput;
    private LineEdit? _player2NameInput;
    private Control? _hotSeatConfig;
    private Button? _startHotSeatButton;

    public override void _Ready() {
        // Get button references
        _singleMatchButton = GetNodeOrNull<Button>("GameContainer/ModeButtons/SingleMatchButton");
        _tournamentButton = GetNodeOrNull<Button>("GameContainer/ModeButtons/TournamentButton");
        _survivalButton = GetNodeOrNull<Button>("GameContainer/ModeButtons/SurvivalButton");
        _hotSeatButton = GetNodeOrNull<Button>("GameContainer/ModeButtons/HotSeatButton");
        _backButton = GetNodeOrNull<Button>("GameContainer/BackButton");

        // Get tournament config references
        _tournamentSizeOption = GetNodeOrNull<OptionButton>("GameContainer/TournamentConfig/SizeOption");
        _tournamentConfig = GetNodeOrNull<Control>("GameContainer/TournamentConfig");

        // Get hot seat config references
        _player1NameInput = GetNodeOrNull<LineEdit>("GameContainer/HotSeatConfig/Player1Input");
        _player2NameInput = GetNodeOrNull<LineEdit>("GameContainer/HotSeatConfig/Player2Input");
        _hotSeatConfig = GetNodeOrNull<Control>("GameContainer/HotSeatConfig");
        _startHotSeatButton = GetNodeOrNull<Button>("GameContainer/HotSeatConfig/StartButton");

        // Connect button signals
        _singleMatchButton?.Connect("pressed", new Callable(this, nameof(_OnSingleMatchPressed)));
        _tournamentButton?.Connect("pressed", new Callable(this, nameof(_OnTournamentPressed)));
        _survivalButton?.Connect("pressed", new Callable(this, nameof(_OnSurvivalPressed)));
        _hotSeatButton?.Connect("pressed", new Callable(this, nameof(_OnHotSeatPressed)));
        _backButton?.Connect("pressed", new Callable(this, nameof(_OnBackPressed)));
        _startHotSeatButton?.Connect("pressed", new Callable(this, nameof(_OnStartHotSeatPressed)));

        // Hide config panels initially
        if (_tournamentConfig != null) _tournamentConfig.Visible = false;
        if (_hotSeatConfig != null) _hotSeatConfig.Visible = false;
    }

    private void _OnSingleMatchPressed() {
        GetTree().ChangeSceneToFile("res://scenes/single_match/SingleMatchScene.tscn");
    }

    private void _OnTournamentPressed() {
        // Get bracket size from option button if available, default to 8
        int bracketSize = 8;
        if (_tournamentSizeOption != null) {
            bracketSize = _tournamentSizeOption.Selected == 1 ? 16 : 8;
        }

        // Store config and navigate to tournament scene
        // The scene will read the config from a singleton or we pass it via the scene
        GetTree().ChangeSceneToFile("res://scenes/tournament/TournamentScene.tscn");
    }

    private void _OnSurvivalPressed() {
        GetTree().ChangeSceneToFile("res://scenes/survival/SurvivalScene.tscn");
    }

    private void _OnHotSeatPressed() {
        // Show hot seat config panel
        if (_hotSeatConfig != null) {
            _hotSeatConfig.Visible = true;
            // Set default names
            if (_player1NameInput != null) _player1NameInput.Text = "Player 1";
            if (_player2NameInput != null) _player2NameInput.Text = "Player 2";
        } else {
            // If no config panel, go directly with default names
            GetTree().ChangeSceneToFile("res://scenes/hotseat/HotSeatScene.tscn");
        }
    }

    private void _OnStartHotSeatPressed() {
        // Get player names
        string player1Name = _player1NameInput?.Text ?? "Player 1";
        string player2Name = _player2NameInput?.Text ?? "Player 2";

        // Ensure names aren't empty
        if (string.IsNullOrWhiteSpace(player1Name)) player1Name = "Player 1";
        if (string.IsNullOrWhiteSpace(player2Name)) player2Name = "Player 2";

        // Store names in autoload or pass to scene
        // For now, we'll let the scene use defaults - the scene can be enhanced later
        GetTree().ChangeSceneToFile("res://scenes/hotseat/HotSeatScene.tscn");
    }

    private void _OnBackPressed() {
        // Hide any open config panels
        if (_tournamentConfig != null) _tournamentConfig.Visible = false;
        if (_hotSeatConfig != null) _hotSeatConfig.Visible = false;

        GetTree().ChangeSceneToFile("res://scenes/main_menu/MainMenu.tscn");
    }
}
