using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField]private CardSlotUI[]  cardSlotUI;
    private List<UpgradeCard> currentChoices;
    public void ShowCards(List<UpgradeCard> cards)
    {
        gameObject.SetActive(true);
        currentChoices = cards;
        for (int i = 0; i < cardSlotUI.Length; i++)
        {
            if(i<cards.Count)
            {
                int index = i;
                cardSlotUI[index].SetCard(cards[index], () => OnCardSelected(cards[index]));
            }
            else
            {
                cardSlotUI[i].Hide();
            }
            Time.timeScale = 0;
        }
    }
    public void OnCardSelected(UpgradeCard card)
    {
        card.Apply(PlayerStats.Instance);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
