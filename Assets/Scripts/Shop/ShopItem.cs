using UnityEngine;
[CreateAssetMenu(fileName = "NewShopItem", menuName = "Scriptable Objects/ShopItem")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int cost;
    public int damageUpgradeCost = 100;
    public int fireRateUpgradeCost = 100;
    public string description;

    [Header("Weapon Unlock")]
    public string weaponName;
    private bool isBought = false;

    public void UnlockWeapon()
    {
        if (!string.IsNullOrEmpty(weaponName))
        {
            OsmanAttack attack = FindFirstObjectByType<OsmanAttack>();

            if (attack != null && attack.weaponDictionary.ContainsKey(weaponName))
            {
                attack.weaponDictionary[weaponName].bought = true;
                isBought = true;
                Debug.Log("Weapon bought: " + weaponName);
            }
        }
    }
    public bool BuyStatus()
    {
        if (!string.IsNullOrEmpty(weaponName))
        {
            OsmanAttack attack = FindFirstObjectByType<OsmanAttack>();

            if (attack != null && attack.weaponDictionary.ContainsKey(weaponName))
            {
                return attack.weaponDictionary[weaponName].bought;
            }
        }
        return false;
    }
    public void UpgradeDamage()
    {
        if (!string.IsNullOrEmpty(weaponName))
        {
            OsmanAttack attack = FindFirstObjectByType<OsmanAttack>();
            if (attack != null && attack.weaponDictionary.ContainsKey(weaponName))
            {
                attack.weaponDictionary[weaponName].damage *= 1.4f;
            }
        }
    }
    public void UpgradeFireRate()
    {
        if (!string.IsNullOrEmpty(weaponName))
        {
            OsmanAttack attack = FindFirstObjectByType<OsmanAttack>();

            if (attack != null && attack.weaponDictionary.ContainsKey(weaponName))
            {
                attack.weaponDictionary[weaponName].fireRate *= 1.4f;
            }
        }
    }
}
