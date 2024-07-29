using Godot;
using System.Collections.Generic;

public partial class BeltView : Node2D
{
	public const int SlotPadding = 8;
	private List<VialView> _vialViews = new();
	private static PackedScene _vialSlotScene = GD.Load<PackedScene>("res://VialView.tscn");
	public VialView Selection { get; private set; }
	public BottleBelt Model { get; private set; }

	public void SetModel(BottleBelt model)
	{
		Model = model;
		for (int i = 0; i < model.Slots.Count; i++)
		{
			var vialView = _vialSlotScene.Instantiate() as VialView;
			AddChild(vialView);
			_vialViews.Add(vialView);
			_vialViews[i].SetModel(model.Slots[i]);
			vialView.Button.Pressed += () => VialViewPressed(vialView);
			var vialX = SlotPadding * (i + 1) + VialView.Width * i;
			vialView.Position = new(vialX, 0);
		}
	}

	public void ClearSelection()
	{
		Selection = null;
		foreach (var slot in _vialViews)
		{
			slot.Highlight.Visible = false;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    private void VialViewPressed(VialView vialView)
    {
		if (Model.Player.CurrentEnergy == 0 || vialView.Model.Bottle == null)
		{
			return;
		}

		if (Selection == vialView)
		{
			Selection = null;
			vialView.Highlight.Visible = false;
			return;
		}

		Selection = vialView;
        foreach (var slot in _vialViews)
		{
			slot.Highlight.Visible = slot == vialView;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
