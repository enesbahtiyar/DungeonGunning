using System.Collections.Generic;
using UnityEngine;

public class VendorTrigger : MonoBehaviour
{
    [SerializeField]private ShopUI shopUI;
    [SerializeField] private List<ShopItem> possibleItems;
    [SerializeField] private int itemsToShow = 3;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            List<ShopItem> chosenItems=new List<ShopItem>(possibleItems);
            List<ShopItem>shopItems=new List<ShopItem>();
            for (int i=0; i<itemsToShow&&chosenItems.Count>0; i++)
            {
                int index=Random.Range(0, chosenItems.Count);
                shopItems.Add(chosenItems[index]);
                chosenItems.RemoveAt(index);
            }
            shopUI.ShowShop(shopItems);
            GameManager.Instance.SetState(GameState.Paused);
        }
    }
}
