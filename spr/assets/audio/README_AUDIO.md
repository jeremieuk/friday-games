# Audio Assets for SPR Card Battler (MVP)

## Required Audio Files

AudioManager is configured and integrated, but audio files are not yet sourced.
When audio files are available, place them in this directory.

### Sound Effects (SFX)
Place in `assets/audio/`:

1. **sfx_button_click.wav** - Button hover/click feedback
2. **sfx_choice_reveal.wav** - When both choices are revealed
3. **sfx_round_win.wav** - Player wins a round
4. **sfx_round_lose.wav** - AI wins a round
5. **sfx_match_victory.wav** - Player wins the match (2 rounds)
6. **sfx_match_defeat.wav** - AI wins the match (2 rounds)

### Music (Background)
Place in `assets/audio/`:

1. **music_menu.ogg** - Main menu background music (looping)
2. **music_gameplay.ogg** - Single Match gameplay music (looping)

## Audio Specifications

### SFX Requirements
- Format: WAV (16-bit, 44.1kHz recommended)
- Duration: 0.1-2 seconds (short, punchy)
- Volume: Normalized to -3dB peak
- Style: Cartoon/playful (matches visual design)

### Music Requirements
- Format: OGG Vorbis (for looping)
- Duration: 1-3 minutes per track
- Volume: Normalized to -6dB peak
- Style: Upbeat, playful, non-intrusive
- Loop: Seamless loop points

## Volume Levels (Configured in AudioManager)
- **SFX**: -10dB
- **Music**: -15dB

These can be adjusted in `scripts/managers/AudioManager.cs`

## Audio Sourcing Options

### Free Resources
- **Freesound.org** - Community sound library
- **OpenGameArt.org** - Game audio assets
- **Incompetech.com** - Royalty-free music (Kevin MacLeod)
- **Zapsplat.com** - Free SFX library

### Paid Resources
- **AudioJungle** - Professional game audio
- **Pond5** - Stock audio marketplace
- **Soundsnap** - Sound effects library

## Implementation Status

✅ AudioManager.cs created and configured
✅ Autoload singleton registered in project.godot
✅ Audio triggers integrated in SingleMatchController.cs
❌ Audio files not yet sourced (placeholders only)

When audio files are added, uncomment the loading code in `AudioManager.cs` (lines marked with TODO).
