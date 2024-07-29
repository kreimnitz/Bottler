using Godot;
using System;

public abstract class HealthBarOwner
{
	public int CurrentHp { get; set; } = 0;
	public int MaxHp { get; set; } = 1;
	public int CurrentShield { get; set; } = 0;

	public virtual void TakeDamage(int damage)
	{
		if (damage <= CurrentShield)
		{
			CurrentShield -= damage;
		}
		else
		{
			CurrentHp -= damage - CurrentShield;
			CurrentShield = 0;
		}
	}
}

public partial class HealthBar : ProgressBar
{
	private ProgressBar _shieldBar;
	public HealthBarOwner Model { get; private set; }
	public void SetModel(HealthBarOwner model)
	{
		Model = model;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shieldBar = GetNode<ProgressBar>("ShieldBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Model == null)
		{
			return;
		}
		Value = 100 * Model.CurrentHp / Model.MaxHp;
		_shieldBar.Value = 100 * Model.CurrentShield / Model.MaxHp;
	}
}
