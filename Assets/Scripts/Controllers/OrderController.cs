using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderController : MonoBehaviour
{
    public static OrderController Instance;
    [SerializeField] Order_Scriptable orderScriptable;
    List<int> unlockedOrderIDs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        unlockedOrderIDs = new List<int>();
        foreach(var element in orderScriptable.orders)
        {
            if (element.isUnlocked)
                unlockedOrderIDs.Add(element.orderID);
        }
    }

    public Order GetRandomOrder()
    {
        int randomNum = Random.Range(0, unlockedOrderIDs.Count);
        return orderScriptable.orders[unlockedOrderIDs[randomNum]];
    }

    public void Cook(Order _currentOrder)
    {
        Instantiate(_currentOrder.orderPrefab);
    }

    public Order GetOrder(int _orderID)
    {
        return orderScriptable.orders[_orderID];
    }

    public void AddNewUnlockedOrder(int _newID)
    {
        unlockedOrderIDs.Add(_newID);
    }
}
