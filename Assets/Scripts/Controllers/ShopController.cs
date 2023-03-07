using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ShopType {orders, tableSets, tables };
public class ShopController : MonoBehaviour
{
    public static ShopController Instance;
    [SerializeField] GameObject[] contentPanels;
    [SerializeField] GameObject OrderShop;
    [SerializeField] GameObject TableUIPrefab;
    [SerializeField] GameObject BuyTPrefab;
    [SerializeField] Transform orderContent;
    [SerializeField] Transform tableContent;
    [SerializeField] Transform buytablecontent;
    [SerializeField] internal Order_Scriptable menu;
    [SerializeField] internal Table_Scriptable TMenu;
    [SerializeField] internal TableBuyUIElement Buymenu;

    [SerializeField] GameObject[] buyConfirmPanel;
    [SerializeField] GameObject successfulBuyPanel;
    [SerializeField] GameObject insufficentMoneyPanel;
    [SerializeField] GameObject successfulBuyBPanel;


    TextMeshProUGUI[] buyConfirmText;
    TextMeshProUGUI tableBSuccessfulText;
    Order selectedOrderData;
    Table selectedTableData;
    int selectedTableBuyPrice;

    int selectedOrderIndex;
    int selectedShopType;
    OrderUIElement[] orderUIElements;
    TableUIElement[] tableUIElements;
    BuyTableSetUpElement[] buytableUIElement;

    private void Awake()
    {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
                Destroy(gameObject);
            tableBSuccessfulText = successfulBuyBPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ClickButton(int index)
    {
        for(int i=0; i < contentPanels.Length; i++)
        {
            if(index == i)
            {
                contentPanels[i].SetActive(true);
            }
            else
            {
                contentPanels[i].SetActive(false);
            }
        }
    }

    void Start()
    {
        buyConfirmText = new TextMeshProUGUI[3];
        GoOverOrder();
        GoOverTablesList();
        GoOverBuyTable();

        for(int i = 0; i < buyConfirmPanel.Length; i ++)
            buyConfirmText[i] = buyConfirmPanel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        selectedOrderIndex = 0;
    }

    public void GoOverOrder()
    {
        orderUIElements = new OrderUIElement[menu.orders.Length];

        for(int i = 0; i < menu.orders.Length; i++)
        {
            GameObject spawnedOrder = Instantiate(OrderShop, orderContent);
            int index = i;
            spawnedOrder.GetComponent<OrderUIElement>().SetUpElement(menu.orders[i]);
            spawnedOrder.GetComponent<OrderUIElement>().buyButton.onClick.AddListener(() => OnBuyButtonClick(ShopType.orders,index));
            orderUIElements[i] = spawnedOrder.GetComponent<OrderUIElement>();
        }
    }

    public void GoOverTablesList()
    {
        tableUIElements = new TableUIElement[TMenu.tables.Length];

        for (int i = 0; i < TMenu.tables.Length; i++)
        {
            GameObject spawnedTable = Instantiate(TableUIPrefab, tableContent);
            int index = i;
            spawnedTable.GetComponent<TableUIElement>().SetUpElement(TMenu.tables[i]);
            spawnedTable.GetComponent<TableUIElement>().buyButton.onClick.AddListener(() => OnBuyButtonClick(ShopType.tableSets, index));
            spawnedTable.GetComponent<TableUIElement>().selectButton.onClick.AddListener(() => OnSelectButtonClick(index));
            tableUIElements[i] = spawnedTable.GetComponent<TableUIElement>();
        }
    }

    public void GoOverBuyTable()
    {
        buytableUIElement = new BuyTableSetUpElement[Buymenu.unlockPrices.Length];

        for (int i = 0; i < Buymenu.unlockPrices.Length; i++)
        {
            GameObject spawnedOrder = Instantiate(BuyTPrefab, buytablecontent);
            int index = i;
            spawnedOrder.GetComponent<BuyTableSetUpElement>().SetUpElement(Buymenu.unlockPrices[i], i+1);
            spawnedOrder.GetComponent<BuyTableSetUpElement>().buyButton.onClick.AddListener(() => OnBuyButtonClick(ShopType.tables, index));
            buytableUIElement[i] = spawnedOrder.GetComponent<BuyTableSetUpElement>();
        }
    }

    void OnSelectButtonClick(int _index)
    {
        int previousTableIndex = TableController.Instance.currentSetIndex;
        TableController.Instance.currentSetIndex = _index;
        tableUIElements[previousTableIndex].SetUpElement(TMenu.tables[previousTableIndex]);
        tableUIElements[_index].SetUpElement(TMenu.tables[_index]);
        TableController.Instance.SetUpTableSet();
    }

    void OnBuyButtonClick(ShopType shopType,int _index)
    {
        selectedShopType = (int)shopType;
        selectedOrderIndex = _index;
        switch (shopType)
        {
            case ShopType.orders:
                selectedOrderData = menu.orders[_index];
                buyConfirmText[selectedShopType].text = $"Do you want to unlock {selectedOrderData.orderName} for {selectedOrderData.buyPrice}?";
                buyConfirmPanel[selectedShopType].SetActive(true);
                break;
            case ShopType.tableSets:
                selectedTableData = TMenu.tables[_index];
                buyConfirmText[selectedShopType].text = $"Do you want to unlock {selectedTableData.tableName} for {selectedTableData.buyPrice}?";
                buyConfirmPanel[selectedShopType].SetActive(true);
                break;
            case ShopType.tables:
                selectedTableBuyPrice = Buymenu.unlockPrices[_index];
                buyConfirmText[selectedShopType].text = $"Do you want to unlock {_index + 1} for {selectedTableBuyPrice}?";
                buyConfirmPanel[selectedShopType].SetActive(true);
                break;
        }
    }

    public void OnBuyConfirmCancel()
    {
        buyConfirmPanel[selectedShopType].SetActive(false);
    }

    public void OnBuyConfirm()
    {
        switch (selectedShopType)
        {
            case (int)ShopType.orders:
                if (PlayerController.Instance.currentMoney >= selectedOrderData.buyPrice)
                {
                    successfulBuyPanel.SetActive(true);
                    successfulBuyPanel.transform.GetChild(0).GetComponent<Image>().sprite = selectedOrderData.orderSprite;
                    PlayerController.Instance.currentMoney = PlayerController.Instance.currentMoney - selectedOrderData.buyPrice;
                    selectedOrderData.isUnlocked = true;
                    orderUIElements[selectedOrderIndex].SetUpElement(selectedOrderData);
                }
                else
                    insufficentMoneyPanel.SetActive(true);
                break;
            case (int)ShopType.tableSets:
                if (PlayerController.Instance.currentMoney >= selectedTableData.buyPrice)
                {
                    successfulBuyPanel.SetActive(true);
                    successfulBuyPanel.transform.GetChild(0).GetComponent<Image>().sprite = selectedTableData.tableSprite;
                    PlayerController.Instance.currentMoney = PlayerController.Instance.currentMoney - selectedTableData.buyPrice;
                    selectedTableData.isUnlocked = true;
                    tableUIElements[selectedOrderIndex].SetUpElement(selectedTableData);
                }
                else
                    insufficentMoneyPanel.SetActive(true);
                    break;
            case (int)ShopType.tables:
                if (PlayerController.Instance.currentMoney >= selectedTableBuyPrice)
                {
                    TableController.Instance.unlockedTableCount++;
                    successfulBuyBPanel.SetActive(true);
                    tableBSuccessfulText.text = $"You have Unlocked Table {selectedOrderIndex + 1}";
                    PlayerController.Instance.currentMoney = PlayerController.Instance.currentMoney - selectedTableBuyPrice;
                    buytableUIElement[selectedOrderIndex].SetUpElement(selectedTableBuyPrice, selectedOrderIndex  + 1);
                    if(selectedOrderIndex < Buymenu.unlockPrices.Length - 1)
                        buytableUIElement[selectedOrderIndex + 1].SetUpElement(Buymenu.unlockPrices[selectedOrderIndex + 1], selectedOrderIndex + 2);
                    TableController.Instance.SetUpTableSet();
                }
                else
                    insufficentMoneyPanel.SetActive(true);
                break;
        }
        buyConfirmPanel[selectedShopType].SetActive(false);
    }
}
