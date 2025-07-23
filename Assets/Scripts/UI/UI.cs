using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Image xp;

    private void OnEnable()
    {
        PlayerStats.Instance.OnLevelUp += PlayerStats_OnLevelUp;
        PlayerStats.Instance.OnXPChanged += PlayerStats_OnXPChanged;
        PlayerStats.Instance.OnCoinAdded += PlayerStats_onCoinAdded;
    }

    private void OnDisable()
    {
        PlayerStats.Instance.OnLevelUp -= PlayerStats_OnLevelUp;
        PlayerStats.Instance.OnXPChanged -= PlayerStats_OnXPChanged;
        PlayerStats.Instance.OnCoinAdded -= PlayerStats_onCoinAdded;

    }

    private void PlayerStats_OnLevelUp(int obj)
    {
        levelText.text = obj.ToString();
    }

    private void PlayerStats_OnXPChanged(int arg1, int arg2)
    {
        xp.fillAmount = (float)arg1 / arg2;
    }
      private void PlayerStats_onCoinAdded(int arg1)
    {
        coinText.text=arg1.ToString();
    }

}
