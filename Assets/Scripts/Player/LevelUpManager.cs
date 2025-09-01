using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : SingletonMonoBehaviour<LevelUpManager>
{
    [SerializeField] private List<UpgradeCard> allCards;
    [SerializeField] private LevelUpUI ui;
    public void ShowUpgradeChoices(int level)
    {
        if (level % 5 == 0)
        {

        }
        List<UpgradeCard> selectedCards = new List<UpgradeCard>();
        List<UpgradeCard> copy=new List<UpgradeCard>(allCards);
        for (int i=0; i<3&&copy.Count>0; i++)
        {
            int index=Random.Range(0, copy.Count);
            selectedCards.Add(copy[index]);
            copy.RemoveAt(index);
        }
        ui.ShowCards(selectedCards);
    }
}
