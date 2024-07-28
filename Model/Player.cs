using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    public int Luck { get; set; } = 1;
    public BottleBelt BottleBelt { get; set; }
    public int CurrentHp { get; set; } = 10;
    public int MaxHp { get; set; } = 10;
    public int CurrentEnergy { get; set; } = 2;
    public int MaxEnergy { get; set; } = 2;

    public Player()
    {
        BottleBelt = new();
    }
}

public class BottleBelt
{
    private static Random _rng = new Random();
    public List<BeltSlot> Slots { get; set; }
    private Queue<Bottle> _bottleDeck = new();
    private  List<Bottle> _bottleDiscard = new();
    public BottleBelt()
    {
        Slots = new()
        {
            new(this),
            new(this),
            new(this)
        };

        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Damage, 1));
        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Damage, 1));
        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Damage, 1));

        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Defence, 1));
        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Defence, 1));
        _bottleDiscard.Add(new Bottle(BottleType.Endless, PotionType.Defence, 1));
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
    public string Label => PotionType == PotionType.Damage ? "Damage" : "Shield";
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
    Defence,
    Healing,
    Energy,
    ExtraVials,
    Luck,
}
