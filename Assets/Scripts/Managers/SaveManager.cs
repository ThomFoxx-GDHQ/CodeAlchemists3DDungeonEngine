using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoSingleton<SaveManager>
{
    string _jsonString = string.Empty;
    string _path;
    string _masterListPath;
    [SerializeField] string _masterListFilename;
    [SerializeField] string _encryptKey;

    public override void Init()
    {
        _path = Application.persistentDataPath;
        _masterListPath = Path.Combine(_path, _masterListFilename + ".json");

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
        serializedList.masterList = list;

        _jsonString = JsonUtility.ToJson(serializedList);
        _jsonString = Cipher.EncryptDecrypt(_jsonString, _encryptKey);
        SaveData();
    }

    public void CharacterFileToList(string file)
    {
        MasterCharacterList list = new MasterCharacterList();
        file = Cipher.EncryptDecrypt(file, _encryptKey);

        list = JsonUtility.FromJson<MasterCharacterList>(file);
        CharacterManager.Instance.ReloadMasterList(list.masterList);
    }

    public void SaveData()
    {
        Debug.Log($"Path: {_masterListPath}");

        File.WriteAllText(_masterListPath, _jsonString);
    }

   /* private void OnApplicationQuit()
    {
        SaveData();
    }*/
}
