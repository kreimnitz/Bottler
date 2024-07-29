using System;
using System.Collections.Generic;
using System.Linq;

public class Player : HealthBarOwner
{
    public int Luck { get; set; } = 1;
    public BottleBelt BottleBelt { get; set; }
    public int CurrentEnergy { get; set; } = 3;
    public int MaxEnergy { get; set; } = 3;

    public Player()
    {
        BottleBelt = new(this);
        MaxHp = 20;
        CurrentHp = 20;
    }
}

public class BottleBelt
{
    private const int StartingSlotCount = 5;
    private static Random _rng = new Random();
    private Queue<Bottle> _bottleDeck = new();
    private  List<Bottle> _bottleDiscard = new();

    public List<BeltSlot> Slots { get; set; } = new();
    public Player Player { get; }
    public BottleBelt(Player player)
    {
        Player = player;
        for (int i = 0; i < StartingSlotCount; i++)
        {
            Slots.Add(new BeltSlot(this));
        }

        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Damage, 1));
        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Damage, 1));
        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Shield, 1));

        _bottleDiscard.Add(new Bottle(BottleType.Basic, PotionType.Damage, 2));
        _bottleDiscard.Add(new Bottle(BottleType.Basic, PotionType.Damage, 2));
        _bottleDiscard.Add(new Bottle(BottleType.Basic, PotionType.Damage, 2));
        _bottleDiscard.Add(new Bottle(BottleType.Basic, PotionType.Shield, 2));
        _bottleDiscard.Add(new Bottle(BottleType.Basic, PotionType.Shield, 2));
        _bottleDiscard.Add(new Bottle(BottleType.Basic, PotionType.Shield, 2));
        ShuffleDiscardUnderDeck();
    }

    public void ShuffleDiscardUnderDeck()
    {
        var shuffledDiscard = _bottleDiscard.OrderBy(_ => _rng.Next());
        foreach (Bottle bottle in shuffledDiscard)
        {
            _bottleDeck.Enqueue(bottle);
        }
        _bottleDiscard.Clear();
    }

    public void RefreshSlots()
    {
        foreach (var slot in Slots)
        {
            if (slot.Bottle != null)
            {
                _bottleDiscard.Add(slot.Bottle);
                slot.Bottle = null;
            }
        }
        if (Slots.Count > _bottleDeck.Count)
        {
            ShuffleDiscardUnderDeck();
        }
        foreach (var slot in Slots)
        {
            if (_bottleDeck.Count == 0)
            {
                continue;
            }
            slot.Bottle = _bottleDeck.Dequeue();
        }
    }

    public void AddToDiscard(Bottle bottle)
    {
        _bottleDiscard.Add(bottle);
    }
}

public class BeltSlot
{
    private BottleBelt _belt;
    public Bottle Bottle { get; set; }
    public BeltSlot(BottleBelt belt)
    {
        _belt = belt;
    }
    public Bottle UseBottle()
    {
        var bottle = Bottle;
        if (bottle.BottleType == BottleType.Endless)
        {
            _belt.AddToDiscard(bottle);
        }
        Bottle = null;
        return bottle;
    }
}

public class Bottle
{
    public BottleType BottleType { get; set; }
    public PotionType PotionType { get; set; }
    public int Power { get; set; }

    public Bottle(BottleType bottleType, PotionType potionType, int power)
    {
        BottleType = bottleType;
        PotionType = potionType;
        Power = power;
    }
}

public enum BottleType
{
    Basic,
    Endless,
    Area,
    Cursed,
    Syringe,
    Multi,
    Weapon
}

public enum PotionType
{
    Damage,
    Shield,
    Healing,
    Energy,
    ExtraVials,
    Luck,
}
