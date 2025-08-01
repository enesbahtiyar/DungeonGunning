using UnityEngine;
[CreateAssetMenu(fileName = "NewShopItem", menuName = "Scriptable Objects/ShopItem")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int cost;
    public string description;

    [Header("Weapon Unlock")]
    public string weaponName;
    public void UnlockWeapon()
    {
        if (!string.IsNullOrEmpty(weaponName))
        {
            OsmanAttack attack = FindFirstObjectByType<OsmanAttack>();

            if (attack != null && attack.weaponDictionary.ContainsKey(weaponName))
            {
                attack.weaponDictionary[weaponName].bought = true;
                Debug.Log("Weapon bought: " + weaponName);
            }
        }
    }
}
