using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TableUIElement : MonoBehaviour
{
    [SerializeField] Image tableImage;
    [SerializeField] GameObject lockedInfo;
    [SerializeField] GameObject unlockedInfo;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI buyPriceText;
    [SerializeField] internal Button buyButton;
    [SerializeField] internal Button selectButton;
    [SerializeField] GameObject equippedPanel;

    internal void SetUpElement(Table table)
    {
        tableImage.sprite = table.tableSprite;
        if (table.isUnlocked)
        {
            if (table.tableID == TableController.Instance.currentSetIndex)
            {
                equippedPanel.SetActive(true);
                selectButton.gameObject.SetActive(false);
            }
            else
            {
                equippedPanel.SetActive(false);
                selectButton.gameObject.SetActive(true);
            }

            unlockedInfo.SetActive(true);
            lockedInfo.SetActive(false);
            nameText.text = table.tableName;
        }
        else
        {
            unlockedInfo.SetActive(false);
            buyPriceText.text = $"{table.buyPrice}";
        }
    }
}
