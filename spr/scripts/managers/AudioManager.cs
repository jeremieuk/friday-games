using Godot;

namespace SprGame.Managers;

/// <summary>
/// Manages audio playback for SFX and music.
/// Singleton autoload for global access.
/// Service Layer: Minimal Godot dependencies for audio.
///
/// TODO: Add actual audio files to assets/audio/ directory
/// Required files (MVP subset):
/// - sfx_button_click.wav
/// - sfx_choice_reveal.wav
/// - sfx_round_win.wav
/// - sfx_round_lose.wav
/// - sfx_match_victory.wav
/// - sfx_match_defeat.wav
/// - music_menu.ogg
/// - music_gameplay.ogg
/// </summary>
public partial class AudioManager : Node {
    // Audio players will be added as child nodes
    private AudioStreamPlayer? _sfxPlayer;
    private AudioStreamPlayer? _musicPlayer;

    // Volume levels (in dB) per game design doc
    private const float SfxVolume = -10.0f;
    private const float MusicVolume = -15.0f;

    public override void _Ready() {
        // Create SFX player
        _sfxPlayer = new AudioStreamPlayer {
            Name = "SFXPlayer",
            VolumeDb = SfxVolume
        };
        AddChild(_sfxPlayer);

        // Create Music player
        _musicPlayer = new AudioStreamPlayer {
            Name = "MusicPlayer",
            VolumeDb = MusicVolume
        };
        AddChild(_musicPlayer);

        GD.Print("AudioManager initialized (audio files not yet loaded)");
    }

    /// <summary>
    /// Plays a sound effect.
    /// TODO: Load actual audio files when available.
    /// </summary>
    public void PlaySFX(string sfxName) {
        if (_sfxPlayer == null) {
            return;
        }

        // TODO: Load and play actual audio file
        // string path = $"res://assets/audio/{sfxName}.wav";
        // var stream = GD.Load<AudioStream>(path);
        // if (stream != null) {
        //     _sfxPlayer.Stream = stream;
        //     _sfxPlayer.Play();
        // }

        GD.Print($"[AudioManager] Would play SFX: {sfxName}");
    }

    /// <summary>
    /// Plays background music in a loop.
    /// TODO: Load actual music files when available.
    /// </summary>
    public void PlayMusic(string musicName) {
        if (_musicPlayer == null) {
            return;
        }

        // TODO: Load and play actual music file
        // string path = $"res://assets/audio/{musicName}.ogg";
        // var stream = GD.Load<AudioStream>(path);
        // if (stream != null) {
        //     _musicPlayer.Stream = stream;
        //     _musicPlayer.Play();
        // }

        GD.Print($"[AudioManager] Would play music: {musicName}");
    }

    /// <summary>
    /// Stops currently playing music
    /// </summary>
    public void StopMusic() {
        _musicPlayer?.Stop();
    }

    /// <summary>
    /// Sets SFX volume
    /// </summary>
    public void SetSFXVolume(float volumeDb) {
        if (_sfxPlayer != null) {
            _sfxPlayer.VolumeDb = volumeDb;
        }
    }

    /// <summary>
    /// Sets music volume
    /// </summary>
    public void SetMusicVolume(float volumeDb) {
        if (_musicPlayer != null) {
            _musicPlayer.VolumeDb = volumeDb;
        }
    }
}
