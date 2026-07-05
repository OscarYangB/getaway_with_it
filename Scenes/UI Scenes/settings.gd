extends Node

@onready var settings_display := $CanvasLayer
var visible : bool = true:
	set(new):
		visible = new
		settings_display.visible = new

func _ready():
	hide()

func _on_master_volume_slider_value_changed(value):
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Master"), linear_to_db(value))


func _on_music_volume_slider_value_changed(value):
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("Music"), linear_to_db(value))


func _on_sfx_volume_slider_value_changed(value):
	AudioServer.set_bus_volume_db(AudioServer.get_bus_index("SFX"), linear_to_db(value))


func _on_back_button_pressed():
	hide()

func show():
	visible = true

func hide():
	visible = false
