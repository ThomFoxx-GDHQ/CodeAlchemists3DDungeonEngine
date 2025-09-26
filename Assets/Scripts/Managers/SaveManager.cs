using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    string _jsonString = string.Empty;
    string _path;
    string _masterListPath;
    string _partyPath;
    [SerializeField] string _masterListFilename;
    [SerializeField] string _partyFilename;
    [SerializeField] string _encryptKey;
    string[] _fileSlots = new string[3];

    public override void Init()
    {
        _path = Application.persistentDataPath;
        _masterListPath = Path.Combine(_path, _masterListFilename + ".json");
        _partyPath = Path.Combine(_path, _partyFilename + ".json");

        if (File.Exists(_masterListPath))
        {
            Debug.Log("File Exists");
            _jsonString = File.ReadAllText(_masterListPath);
            CharacterFileToList(_jsonString);
        }
        else Debug.Log("File not Found");
    }

    public void CharacterListToJson(List<Character> list)
    {
        MasterCharacterList serializedList = new MasterCharacterList();
        /*foreach (var character in list)
        {
            character.SaveInventory();
        }*/
        serializedList.masterList = list;

        _jsonString = string.Empty;
        _jsonString = JsonUtility.ToJson(serializedList);
        _jsonString = Cipher.EncryptDecrypt(_jsonString, _encryptKey);
        SaveMasterListData();
    }

    public void CharacterFileToList(string file)
    {
        MasterCharacterList list = new MasterCharacterList();
        file = Cipher.EncryptDecrypt(file, _encryptKey);

        list = JsonUtility.FromJson<MasterCharacterList>(file);
        /*foreach (var character in list.masterList)
        {
            character.LoadInventory();
        }*/
        CharacterManager.Instance.ReloadMasterList(list.masterList);
    }

    public void PartyListToJson(List<Character> list)
    {
        PartyMasterList serializedList = new PartyMasterList();

        foreach (Character character in list)
        {
            character.SaveInventory();
        }
        serializedList.partyList = list;

        _jsonString = string.Empty;
        _jsonString = JsonUtility.ToJson(serializedList);
        _jsonString = Cipher.EncryptDecrypt (_jsonString, _encryptKey);
        SavePartyListData();
    }

    public void PartyFileToList(string file)
    {
        PartyMasterList list = new PartyMasterList();
        file = Cipher.EncryptDecrypt (file, _encryptKey);

        list = JsonUtility.FromJson<PartyMasterList>(file);
        foreach (var character in list.partyList)
        {
            InventoryManager.Instance.SetCharacter(character);
            character.LoadInventory();
            PartyManager.Instance.AddPartyMember(character);
        }
        //PartyManager.Instance.LoadListToParty(list.partyList);
    }

    [ContextMenu("Force Save")]
    public void SaveMasterListData()
    {
        Debug.Log($"Path: {_masterListPath}");

        File.WriteAllText(_masterListPath, _jsonString);
    }

    public void SavePartyListData()
    {
        Debug.Log($"Path: {_partyPath}");

        File.WriteAllText(_partyPath, _jsonString);
    }

    public void LoadParty()
    {
        if (File.Exists(_partyPath))
        {
            _jsonString = File.ReadAllText(_partyPath);
            PartyFileToList(_jsonString);
        }
        else Debug.LogWarning("Party File Does not exist");
    }

    public void SaveParty(int index, string partyFileName)
    {
        if (index >= _fileSlots.Length) return;

        _partyFilename = partyFileName;
        _partyPath = Path.Combine(_path, _partyFilename + ".json");
        _fileSlots[index] = partyFileName;

        PartyListToJson(PartyManager.Instance.PartyList);
    }

   /* private void OnApplicationQuit()
    {
        SaveData();
    }*/
}
