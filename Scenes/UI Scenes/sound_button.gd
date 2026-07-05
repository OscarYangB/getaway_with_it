extends Button

@onready var hover_sound := $ButtonHover
@onready var pressed_sound := $ButtonPressed

func _on_pressed():
	pressed_sound.play()

func _on_mouse_entered():
	hover_sound.play()
