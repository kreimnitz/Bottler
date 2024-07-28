using System;
using Godot;

public partial class EnemyView : Node2D
{
	private ProgressBar _healthBar;
	public TextureButton Button { get; set; }

	public Enemy Model { get; private set; }
	public void SetModel(Enemy model)
	{
		Model = model;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		Button = GetNode<TextureButton>("Button");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Model == null)
		{
			return;
		}

		_healthBar.Value = 100 * Model.CurrentHp / Model.MaxHp;
	}
}
