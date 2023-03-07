using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentMoney;
    public int unlockedTablesCount;
    public bool[] ordersIsUnlocked;
    public bool[] tableSetsUnlocked;

    public GameData(Order_Scriptable _odata,Table_Scriptable _tdata)
    {
        currentMoney = 10;
        unlockedTablesCount = 1;
        ordersIsUnlocked = new bool[_odata.orders.Length];
        tableSetsUnlocked = new bool[_tdata.tables.Length];
        for(int i = 0; i < ordersIsUnlocked.Length; i ++)
        {
            ordersIsUnlocked[i] = _odata.orders[i].isUnlocked;
        }
        for (int i = 0; i < tableSetsUnlocked.Length; i++)
        {
            tableSetsUnlocked[i] = _tdata.tables[i].isUnlocked;
        }
    }
}

