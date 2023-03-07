using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUIElement : MonoBehaviour
{
    [SerializeField] Image orderImage;
    [SerializeField] GameObject lockedInfo;
    [SerializeField] GameObject unlockedInfo;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI buyPriceText;
    [SerializeField] TextMeshProUGUI orderPriceText;
    [SerializeField] internal Button buyButton;

    internal void SetUpElement(Order _orderData)
    {
        orderImage.sprite = _orderData.orderSprite;
        if (_orderData.isUnlocked)
        {
            unlockedInfo.SetActive(true);
            lockedInfo.SetActive(false);
            nameText.text = _orderData.orderName;
            orderPriceText.text = $"{_orderData.orderPrice}";
        }
        else
        {
            unlockedInfo.SetActive(false);
            buyPriceText.text = $"{_orderData.buyPrice}";
        }
    }
}
