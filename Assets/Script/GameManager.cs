using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public FieldSymbolDB currentBattleMapData;
    public UserData userData;

    public SceneStatus currentSceneState = 0;

    public Sprite[] charSheet;
    public Sprite[] charIconSheet;

    public enum SceneStatus
    {
        title = 0,
        main,
        map,
        battle,
        deck,
        active,
        gacha,
        reinforce,
        setting
    }

    override protected void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        userData = DataBaseManager.Instance.saveData.userData;

        charSheet = Resources.LoadAll<Sprite>("character");
        charIconSheet = Resources.LoadAll<Sprite>("charicon");
        //DataBaseManager.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
