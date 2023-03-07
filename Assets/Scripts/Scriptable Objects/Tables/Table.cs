using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Table", menuName = "Scriptables/Tables/Table")]
public class Table : ScriptableObject
{
    public int tableID;
    public bool isUnlocked;
    [Space(10)]
    public GameObject tablePrefab;
    public Sprite tableSprite;
    public string tableName;
    public float buyPrice;
}
