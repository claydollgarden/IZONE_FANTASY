using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
using Newtonsoft.Json;

public class DataBaseManager : SingletonMonoBehaviour<DataBaseManager>
{
    //remote master data
    public CachedRecords<FieldSymbolDB> fieldSymbolDB;
    public CachedRecords<BattleCharacterDB> battleCharacterDB;
    public CachedRecords<BattleEnemySkillDB> battleEnemySkillDB;
    public CachedRecords<BattleEnemyCharStatusDB> battleEnemyStatusDB;
    public CachedRecords<VisualNovelDB> visualNovelDB;
    public CachedRecords<SingleBuffDB> singleBuffDB;

    //save data
    public SaveData saveData = new SaveData();
    override protected void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        fieldSymbolDB = new CachedRecords<FieldSymbolDB>();
        battleCharacterDB = new CachedRecords<BattleCharacterDB>();
        battleEnemySkillDB = new CachedRecords<BattleEnemySkillDB>();
        battleEnemyStatusDB = new CachedRecords<BattleEnemyCharStatusDB>();
        visualNovelDB = new CachedRecords<VisualNovelDB>();
        singleBuffDB = new CachedRecords<SingleBuffDB>();

        saveData.Load();
    }

    public void Init()
    {

    }

    private void OnApplicationQuit()
    {
        saveData.Save();
    }
}

[Serializable]
public class SaveData
{
    public UserData userData;

    public void Load()
    {
        //user local data
        if(PlayerPrefsUtil.Load<UserData>() != null)
        {
            Debug.Log("PlayerPrefsUtil true");
            userData.SetAsDefault();
        }
        else
        {
            Debug.Log("PlayerPrefsUtil false");
            userData.SetAsDefault();
        }
    }

    public void Save()
    {
        PlayerPrefsUtil.Commit(userData);
    }

    public void Delete()
    {
        PlayerPrefsUtil.DeleteAll();
    }
}

public static class PlayerPrefsUtil
{
    public static T Load<T>() where T : IUserLocalData, new()
    {
        T t = default;
        string serializedData = PlayerPrefs.GetString(typeof(T).Name);

        if (string.IsNullOrEmpty(serializedData) == false)
        {
            t = JsonConvert.DeserializeObject<T>(serializedData, new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace });
        }

        if (t == null)
        {
            t = new T();
            t.SetAsDefault();
        }

        return t;
    }

    public static void Commit<T>(T t) where T : IUserLocalData, new()
    {
        string serializedData = JsonConvert.SerializeObject(t, Formatting.None);
        PlayerPrefs.SetString(typeof(T).Name, serializedData);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}

public interface IUserLocalData
{
    void SetAsDefault();
}

[Serializable]
public class UserData : IUserLocalData
{
    public string name;
    public int userId;

    public int questStep;

    public List<int> myCharacters;
    public List<int> clearedQuests;

    public List<List<int>> myDeck;
    public List<List<int>> myDeckPosition;

    public void SetAsDefault()
    {
        name = "";
        userId = 0;
        questStep = 0;
        myCharacters = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}; //dummy data
        clearedQuests = new List<int>();
        myDeck = new List<List<int>> { new List<int>() { 4, 9 }, new List<int>() { }, new List<int>() { }, new List<int>() { }, new List<int>() { } };
        myDeckPosition = new List<List<int>> { new List<int>() { 2, 8 }, new List<int>() { }, new List<int>() { }, new List<int>() { }, new List<int>() { } };
    }
}

public class CachedRecords<T> where T : DBBase
{
    private readonly List<T> _list;
    private readonly Dictionary<int, T> _db;

    public CachedRecords()
    {
        string fileName = typeof(T).Name;
        TextAsset textAsset = Resources.Load<TextAsset>("JsonData/" + fileName);
        _list = JsonConvert.DeserializeObject<List<T>>(textAsset.text);
        _db = new Dictionary<int, T>();
        foreach (T record in _list)
        {
            _db.Add(record.id, record);
        }
    }

    public T Get(int id, bool ignoreNullLog = false)
    {
        if (_db.ContainsKey(id))
        {
            return _db[id];
        }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (ignoreNullLog == false)
        {

        }
#endif
        return null;
    }

    public List<T> GetAll()
    {
        return _list;
    }
}

public abstract class DBBase
{
    public int id;
}

[Serializable]
public class FieldSymbolDB : DBBase
{
    public string name;
    public int[] enemy;
    public int[] position;
}

[Serializable]
public class BattleCharacterDB : DBBase
{
    public string name;
    public int namenumber;
    public string title;
    public int hp;
    public int atk;
    public int def;
    public int speed;
    public int[] skill;
    public int[] position;
    public string description;
}

[Serializable]
public class BattleEnemyCharStatusDB : DBBase
{
    public string name;
    public int hp;
    public int atk;
    public int def;
    public int speed;
    public int[] enemyskill;
}

[Serializable]
public class BattleEnemySkillDB : DBBase
{
    public string name;
    public int atk;
    public int[] target;
    public int cooltime;
    public string effectpath;
    public string moviepath;
    public string iconpath;
    public string desc;
}

[Serializable]
public class VisualNovelDB : DBBase
{
    public string[] script;
    public int[] focuschar;
    public int[] leftchar;
    public int[] rightchar;
    public int[] screeneffect;
}

public class SingleBuffDB : DBBase
{
    public string name;
    public int effect;
    public int[] target;
    public int status;
    public int count;
    public int checkcount;
    public string iconpath;
    public string description;
}