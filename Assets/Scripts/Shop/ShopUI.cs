using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopCard[] shopCards;
    private List<ShopItem> currentItems;

    public void ShowShop(List<ShopItem> items)
    {
        gameObject.SetActive(true);
        currentItems = items;
        for (int i = 0; i < shopCards.Length; i++)
        {
            if (i < items.Count)
            {
                int index = i;
                shopCards[index].SetItem(items[index], () => BuyItem(items[index], index, items[index].cost), 
                    items[index].BuyStatus(), () => UpgradeDmg(index, items[index]), () => UpgradeFireRate(index, items[index]),
                    items[index].damageUpgradeCost, items[index].fireRateUpgradeCost);
            }
        }
    }
    private void BuyItem(ShopItem item, int _index, int cost)
    {
        if (PlayerStats.Instance.coinCount >= cost)
        {
            item.UnlockWeapon();
            shopCards[_index].BuyStatus(true);
            PlayerStats.Instance.AddCoins(-cost);
        }
        else
        {
            Debug.LogWarning("Not Enough Coins");
        }
    }
    public void CloseShop()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetState(GameState.Playing);
    }
    private void UpgradeDmg(int _index,ShopItem item)
    {
        shopCards[_index].UpgradeDamage();
        item.UpgradeDamage();
    }
    private void UpgradeFireRate(int _index, ShopItem item)
    {
       shopCards[_index].UpgradeFireRate();
        item.UpgradeFireRate();
    }
}
