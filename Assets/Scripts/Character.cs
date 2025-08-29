using UnityEngine;
using System;

using Random = UnityEngine.Random;

[System.Serializable]
public class Character
{
    public string name;
    [SerializeField] private int _portraitID;
    [SerializeField] private RaceType _race;
    [SerializeField] private ClassType _jobType;
    [SerializeField] private int _healthPoints;
    [SerializeField] private int _magicPoints;
    [SerializeField] private int _strength;
    [SerializeField] private int _agility;
    [SerializeField] private int _constitution;
    [SerializeField] private int _fortitude;
    [SerializeField] private int _wisdom;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _currentMagic;

    private ItemStruct[,] _inventory = new ItemStruct[8,4];

    public string Name => name;
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
    public int PortraitID => _portraitID;


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
    public Character(string name, RaceType race, ClassType jobType, int healthPoints, int magicPoints, int strength, int agility, int constitution, int fortitude, int wisdom,int portraitID)
    {
        this.name = name;
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
        this._portraitID = portraitID;
    }
   
    /// <summary>
    /// Creates a Generic Character with Random Stats
    /// </summary>
    public Character(string name, RaceType race, ClassType jobType)
    {
        this.name = name;
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

    public bool AddToInventory(Item item, int amount, Vector2Int position)
    {
        //check if item Matches
        if (_inventory[position.x, position.y].ID == item.ItemID)
            _inventory[position.x, position.y].Quantity += amount;

        //Fill in Empty
        else if (_inventory[position.x, position.y] == default)
            _inventory[position.x, position.y] = new ItemStruct(item.ItemID, amount);

        //Check if Empty
        else if (_inventory[position.x, position.y].ID != 0) return false;

        return true;
    }

    public void TestAdd()
    {
        ItemStruct testApple = new ItemStruct(1, 1);
        if (_inventory == null) Debug.Log("Inventory slot is Null");
        if (testApple == null) Debug.Log("Test Apple is Null");
        _inventory[0,0] = testApple;
    }
       
    public ItemStruct GetInventoryInfo(Vector2Int position)
    {
        return _inventory[position.x, position.y];
    }

    public ItemStruct[,] GetInventory()
    { return _inventory; }
}
