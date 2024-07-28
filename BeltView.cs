using Godot;
using System.Collections.Generic;

public partial class BeltView : Node2D
{
	private List<VialView> _slots = new();
	public VialView Selection;
	public BottleBelt Model { get; private set; }

	public void SetModel(BottleBelt model)
	{
		Model = model;
		for (int i = 0; i < _slots.Count; i++)
		{
			_slots[i].SetModel(model.Slots[i]);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_slots.Add(GetNode<VialView>("VialSlot1"));
		_slots.Add(GetNode<VialView>("VialSlot2"));
		_slots.Add(GetNode<VialView>("VialSlot3"));

		for (int i = 0; i < _slots.Count; i++)
		{
			_slots[i].Button.Pressed += () => SlotPressed(_slots[i]);
		}
	}

    private void SlotPressed(VialView vialView)
    {
		Selection = vialView;
        foreach (var slot in _slots)
		{
			slot.Highlight.Visible = slot == vialView;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
