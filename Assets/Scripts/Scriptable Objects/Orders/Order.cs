using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Order", menuName = "Scriptables/Orders/Order")]
public class Order : ScriptableObject
{
    public int orderID;
    public bool isUnlocked;
    [Space(10)]
    public GameObject orderPrefab;
    public Sprite orderSprite;
    public string orderName;
    public float orderPrice;
    public float buyPrice;
}
