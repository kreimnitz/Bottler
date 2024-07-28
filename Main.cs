using Godot;
using System;

public partial class Main : Node2D
{
	private Player _player = new();
	private Enemy _enemy = new();
	private BeltView _beltView;
	private EnemyView _enemyView;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player.BottleBelt.RefreshSlots();
		_beltView = GetNode<BeltView>("BeltView");
		_beltView.SetModel(_player.BottleBelt);

		_enemyView = GetNode<EnemyView>("Enemy");
		_enemyView.SetModel(_enemy);
		_enemyView.Button.Pressed += () => OnEnemyClicked(_enemyView);

		_player.BottleBelt.RefreshSlots();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnEnemyClicked(EnemyView enemyView)
	{
		if (_beltView.Selection != null)
		{
			var bottle = _beltView.Selection.Model.UseBottle();
			enemyView.Model.CurrentHp -= bottle.Power;
		}
	}
}


/*
MAP STUFF
Locations
	Battle, Basic, Elite Boss
	Safe point
	    Rest
		Scavenge
	Leatherworker
		Upgrade belt
	Woodworker
		Upgrade wagon
	Random Event

Vial Belt:
	Determines how many vials per turn you start with
	Modify Belt/Slots?
		Reloading slot
		Extra size slot
		Seeking slot

Vial Sizes:
	Determines how much harvestible stuff can fit in your bottle, determines strengh of maxxed out potion

Vial Types:
	Endless Flask
		Infinite uses
		Start with one full of a weak attack and one with weak defend
	Area Flask
		Hits all enemies with contents
	Cursed Flask
		Hurts yourself for some bonus
	Syringe Dart
		Bypasses armor/defenses
	Multi-Compartment bottle
		Combines different harvest types into one attack
	Weapon Flask
		Deals flat damage regardless of contents

Essence Types:
	Damage
	Defense
	Healing
	Extra Throws
	Extra Vials
	Buffs/Debuffs
	Luck
	Summoning

Harvest Example:
	Damage x 20
	Armor x 5
	Charging Bull (Gives some buff) x 2
*/