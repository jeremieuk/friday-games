extends Control
# Main Menu Controller
# Handles navigation from main menu to game modes

func _on_play_button_pressed():
	# Navigate to mode selection screen
	get_tree().change_scene_to_file("res://scenes/mode_selection/ModeSelection.tscn")

func _on_exit_button_pressed():
	get_tree().quit()
