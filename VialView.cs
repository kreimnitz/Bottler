using Godot;
using System;

public partial class VialView : Node2D
{
	private Label _label;
	public Node2D Highlight;
	public TextureButton Button;
	public BeltSlot Model { get; private set; }

	public void SetModel(BeltSlot slot)
	{
		Model = slot;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		Highlight = GetNode<Node2D>("Highlight");
		Button = GetNode<TextureButton>("TextureButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Model == null)
		{
			_label.Text = "Empty";
		}
		_label.Text = Model.Bottle.Label;
	}
}
