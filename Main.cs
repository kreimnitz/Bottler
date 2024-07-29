using Godot;
using System;

public partial class Main : Node2D
{
	private Player _player = new();
	private Enemy _enemy = new();
	private BeltView _beltView;
	private EnemyView _enemyView;
	private PlayerView _playerView;
	private Label _throwsLabel;
	private Button _endTurnButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_throwsLabel = GetNode<Label>("ThrowsLabel");
		_endTurnButton = GetNode<Button>("EndTurnButton");
		_endTurnButton.Pressed += EndTurn;

		_player.BottleBelt.RefreshSlots();
		_playerView = GetNode<PlayerView>("PlayerView");
		_playerView.SetModel(_player);
		_playerView.Button.Pressed += OnPlayerClicked;

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
		_throwsLabel.Text = $"Throws Remaining: {_player.CurrentEnergy}/{_player.MaxEnergy}";
	}

	private void EndTurn()
	{
		_player.TakeDamage(2);
		_player.BottleBelt.RefreshSlots();
		_player.CurrentEnergy = _player.MaxEnergy;
	}

	private void OnEnemyClicked(EnemyView enemyView)
	{
		if (_beltView.Selection.Model.Bottle != null && _player.CurrentEnergy > 0)
		{
			_player.CurrentEnergy--;
			var bottle = _beltView.Selection.Model.UseBottle();
			ApplyBottle(enemyView.Model, bottle);
			_beltView.ClearSelection();
		}
	}

	private void OnPlayerClicked()
	{
		if (_beltView.Selection.Model.Bottle != null && _player.CurrentEnergy > 0)
		{
			_player.CurrentEnergy--;
			var bottle = _beltView.Selection.Model.UseBottle();
			ApplyBottle(_player, bottle);
			_beltView.ClearSelection();
		}
	}

	private void ApplyBottle(HealthBarOwner target, Bottle bottle)
	{
		if (bottle.PotionType == PotionType.Damage)
		{
			target.CurrentHp -= bottle.Power;
		}
		if (bottle.PotionType == PotionType.Shield)
		{
			target.CurrentShield += bottle.Power;
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