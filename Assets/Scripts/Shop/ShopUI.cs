using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopCard[] shopCards;
    private List<ShopItem
        > currentItems;

    public void ShowShop(List<ShopItem> items)
    {
        gameObject.SetActive(true);
        currentItems = items;
        for(int i = 0; i < shopCards.Length; i++)
        {
            if (i < items.Count)
            {
                int index = i;
                shopCards[index].SetItem(items[index], () => BuyItem(items[index]));
            }
        }
    }
    private void BuyItem(ShopItem item)
    {
        item.UnlockWeapon();

    }
    public void CloseShop()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetState(GameState.Playing);
    }
}
