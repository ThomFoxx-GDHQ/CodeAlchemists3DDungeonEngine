public enum RaceType
{
    Human,
    Elf,
    Dwarf
}

public enum ClassType
{
    Warrior,
    Mage,
    Cleric,
    Ranger,
    Rogue
}

[System.Serializable]
public enum Stats
{
    Strength, 
    Agility,
    Constitution,
    Fortitude,
    Wisdom,
    Health,
    Magic
}

public enum MoveType
{
    SamePos,
    EmptySlot,
    AddingSlot,
    SwappingSlot,
    MoveToCharacter
}