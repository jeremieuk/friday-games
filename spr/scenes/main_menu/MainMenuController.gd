extends Control
# Main Menu Controller
# Handles navigation from main menu to game modes

func _on_play_button_pressed():
	# For MVP, skip mode selection and go directly to Single Match
	get_tree().change_scene_to_file("res://scenes/single_match/SingleMatchScene.tscn")

func _on_exit_button_pressed():
	get_tree().quit()
