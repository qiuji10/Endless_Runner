using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Item
{
    public string name;
    public int cost;
    public int amount;
}

public class ShopManager : MonoBehaviour
{
    private Database data;

    [SerializeField] private Button[] buyButtons;
    [SerializeField] private TMP_Text[] itemsText;
    [SerializeField] private Item jumpPU;
    [SerializeField] private Item shieldPU;
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
        shieldPU.amount = data.playerData.powerupShield;
        bonusPU.amount = data.playerData.powerupBonus;

        itemsText[0].text = money.ToString();
        itemsText[1].text = jumpPU.amount.ToString();
        itemsText[2].text = shieldPU.amount.ToString();
        itemsText[3].text = bonusPU.amount.ToString();
    }

    private void UpdateData()
    {
        itemsText[0].text = money.ToString();
        itemsText[1].text = jumpPU.amount.ToString();
        itemsText[2].text = shieldPU.amount.ToString();
        itemsText[3].text = bonusPU.amount.ToString();

        data.playerData.money = money;
        data.playerData.powerupJump = jumpPU.amount;
        data.playerData.powerupShield = shieldPU.amount;
        data.playerData.powerupBonus = bonusPU.amount;

        data.SaveGame();
        data.Assigner();
    }

    public void BuyJumpPU()
    {
        if (money >= jumpPU.cost)
        {
            money -= jumpPU.cost;
            jumpPU.amount++;
            UpdateData();
        }
    }

    public void BuyShieldPU()
    {
        if (money >= shieldPU.cost)
        {
            money -= shieldPU.cost;
            shieldPU.amount++;
            UpdateData();
        }
    }

    public void BuyBonusPU()
    {
        if (money >= bonusPU.cost)
        {
            money -= bonusPU.cost;
            bonusPU.amount++;
            UpdateData();
        }
    }
}
