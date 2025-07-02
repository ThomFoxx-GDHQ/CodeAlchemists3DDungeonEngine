using UnityEngine;

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

public class Character
{
    private string _name;
    private RaceType _race;
    private ClassType _jobType;
    private int _healthPoints;
    private int _magicPoints;
    private int _strength;
    private int _agility;
    private int _constitution;
    private int _fortitude;
    private int _wisdom;
    private int _currentHealth;
    private int _currentMagic;

    public string Name => _name;
    public RaceType Race => _race;
    public ClassType JobType => _jobType;
    public int HealthPoints => _healthPoints;
    public int MagicPoints => _magicPoints;
    public int Strength => _strength;
    public int Agility => _agility;
    public int Constitution => _constitution;
    public int Fortitude => _fortitude;
    public int Wisdom => _wisdom;
    public int CurrentHealth => _currentHealth;
    public int CurrentMagic => _currentMagic;


    /// <summary>
    /// Constructor for creating a new Character from existed or created data
    /// </summary>
    /// <param name="name"> First and Last name of Character</param>
    /// <param name="race"> Races: Human, Elf, Dwarf</param>
    /// <param name="jobType"> Classes: Warrior, Mage, Cleric, Ranger, Rogue</param>
    /// <param name="healthPoints">Max Health Points</param>
    /// <param name="magicPoints">Max Mana Points</param>
    /// <param name="strength"></param>
    /// <param name="agility"></param>
    /// <param name="constitution"></param>
    /// <param name="fortitude"></param>
    /// <param name="wisdom"></param>
    public Character(string name, RaceType race, ClassType jobType, int healthPoints, int magicPoints, int strength, int agility, int constitution, int fortitude, int wisdom)
    {
        this._name = name;
        this._race = race;
        this._jobType = jobType;
        this._healthPoints = healthPoints;
        _currentHealth = healthPoints;
        this._magicPoints = magicPoints;
        _currentMagic = magicPoints;
        this._strength = strength;
        this._agility = agility;
        this._constitution = constitution;
        this._fortitude = fortitude;
        this._wisdom = wisdom;
    }

    /// <summary>
    /// Creates a Generic Character with Random Stats
    /// </summary>
    public Character(string name, RaceType race, ClassType jobType)
    {
        this._name = name;
        this._race = race;
        this._jobType = jobType;
        this._healthPoints = 100;
        _currentHealth = _healthPoints;
        this._magicPoints = 100;
        _currentMagic = _magicPoints;
        this._strength = Random.Range(12,20);
        this._agility = Random.Range(12, 20);
        this._constitution = Random.Range(12, 20);
        this._fortitude = Random.Range(12, 20);
        this._wisdom = Random.Range(12, 20);
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount >0)
            _currentHealth -= damageAmount;
    }
}
