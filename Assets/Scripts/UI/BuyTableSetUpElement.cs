using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyTableSetUpElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PriceText;
    [SerializeField] TextMeshProUGUI UnlockedNumber;
    [SerializeField] TextMeshProUGUI LockedNumber;
    [SerializeField] internal Button buyButton;
    [SerializeField] GameObject lockedImage;
    internal void SetUpElement(int _price, int index)
    {
        if (IsUnlocked(index))
        {
            lockedImage.SetActive(false);
            buyButton.gameObject.SetActive(false);
            PriceText.gameObject.SetActive(false);
            LockedNumber.gameObject.SetActive(false);
            UnlockedNumber.gameObject.SetActive(true);
        }
        else if(TableController.Instance.unlockedTableCount == index)
        {
            lockedImage.SetActive(false);
        }
        PriceText.text = $"{_price}";
        UnlockedNumber.text = index.ToString();
        LockedNumber.text = index.ToString();
        LockedNumber.gameObject.SetActive(true);
    }

    private bool IsUnlocked(int index)
    {
        if (index > 3)
            return false;
        else
            return (TableController.Instance.unlockedTableCount > index);
    }
}