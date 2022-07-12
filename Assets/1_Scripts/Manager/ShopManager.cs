using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

[System.Serializable]
public class Item
{
    public string name;
    public int cost;
    public int amount;
}

public class ShopManager : MonoBehaviour
{
    public GameObject Shop;
    private Database data;

    [SerializeField] private Button[] buyButtons;
    [SerializeField] private TMP_Text[] itemsText;
    [SerializeField] private Item jumpPU;
    [SerializeField] private Item revivePU;
    [SerializeField] private Item bonusPU;

    private int money;
    

    private void Start()
    {
        data = FindObjectOfType<Database>();
        GetData();
    }

    private void GetData()
    {
        money = data.playerData.money;
        jumpPU.amount = data.playerData.powerupJump;
        revivePU.amount = data.playerData.powerupRevive;
        bonusPU.amount = data.playerData.powerupBonus;

        itemsText[0].text = money.ToString();
        itemsText[1].text = jumpPU.amount.ToString();
        itemsText[2].text = revivePU.amount.ToString();
        itemsText[3].text = bonusPU.amount.ToString();
    }

    private void UpdateData()
    {
        itemsText[0].text = money.ToString();
        itemsText[1].text = jumpPU.amount.ToString();
        itemsText[2].text = revivePU.amount.ToString();
        itemsText[3].text = bonusPU.amount.ToString();

        data.playerData.money = money;
        data.playerData.powerupJump = jumpPU.amount;
        data.playerData.powerupRevive = revivePU.amount;
        data.playerData.powerupBonus = bonusPU.amount;

        data.WriteToJson();
    }

    public void BuyJumpPU()
    {
        if (money >= jumpPU.cost)
        {
            jumpPU.amount++;
            UpdateData();
        }
    }

    public void BuyRevivePU()
    {
        if (money >= revivePU.cost)
        {
            revivePU.amount++;
            UpdateData();
        }
    }

    public void BuyBonusPU()
    {
        if (money >= bonusPU.cost)
        {
            bonusPU.amount++;
            UpdateData();
        }
    }

    [Button]
    public void ShowShop()
    {
        Shop.SetActive(true);
    }

    [Button]
    public void CloseShop()
    {
        Shop.SetActive(false);
    }
}