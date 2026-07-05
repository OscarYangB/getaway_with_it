extends Node

func _on_start_button_pressed():
	get_tree().change_scene_to_file("res://Scenes/main.tscn")


func _on_settings_button_pressed():
	Settings.show()


func _on_quit_button_pressed():
	get_tree().quit()
