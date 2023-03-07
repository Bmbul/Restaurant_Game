using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    internal static DataManager Instance;
    internal GameData gameData;
    [SerializeField] Order_Scriptable oData;
    [SerializeField] Table_Scriptable tData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        if(PlayerPrefs.GetString("gamedata", string.Empty)  == string.Empty)
        {
            gameData = new GameData(oData, tData);
            string gameDataString = JsonUtility.ToJson(gameData);
            PlayerPrefs.SetString("gamedata", gameDataString);
        }
        else
        {
            gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("gamedata"));
        }
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    void LoadData()
    {
        TableController.Instance.unlockedTableCount = gameData.unlockedTablesCount;
        PlayerController.Instance.currentMoney = gameData.currentMoney;
        for(int i = 0; i < gameData.ordersIsUnlocked.Length;i++)
        {
            ShopController.Instance.menu.orders[i].isUnlocked = gameData.ordersIsUnlocked[i];
        }
        for (int i = 0; i < gameData.tableSetsUnlocked.Length; i++)
        {
            ShopController.Instance.TMenu.tables[i].isUnlocked = gameData.tableSetsUnlocked[i];
        }
    }

    public void SaveData()
    {
        gameData.unlockedTablesCount = TableController.Instance.unlockedTableCount;
        gameData.currentMoney = (int)PlayerController.Instance.currentMoney;
        for (int i = 0; i < gameData.ordersIsUnlocked.Length; i++)
        {
            gameData.ordersIsUnlocked[i] = ShopController.Instance.menu.orders[i].isUnlocked;
        }
        for (int i = 0; i < gameData.tableSetsUnlocked.Length; i++)
        {
            gameData.tableSetsUnlocked[i] = ShopController.Instance.TMenu.tables[i].isUnlocked;
        }
        string gameDataString = JsonUtility.ToJson(gameData);

        PlayerPrefs.SetString("gamedata", gameDataString);
    }
}
