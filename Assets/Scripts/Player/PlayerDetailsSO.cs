using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header BASE DETAILS
    [Space(10)]
    #endregion

    #region Tooltip
    [Tooltip("Player Character Name")]
    #endregion
    public string playercharacterName;
    #region Tooltip

    [Tooltip("Prefab Gameobject for the player")]

    #endregion
    public GameObject playerPrefab;
    #region Tooltip

    [Tooltip("Player Runtime Animator Controller")]

    #endregion
    public RuntimeAnimatorController RuntimeAnimatorController;


    #region Header Health

    [Space(10)]
    [Header("Health")]

    #endregion

    #region Tooltip

    [Tooltip("Player starting health amount")]

    #endregion
    public int playerHealthAmount;

    #region Header Other

    [Space(10)] [Header("Other")]

    #endregion

    //minimapIcon
    public Sprite playerMiniMapIcon;
    
    //playerHandSprite
    public Sprite playerHandSprite;


    #region Validation
    #if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playercharacterName), playercharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMiniMapIcon), playerMiniMapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(RuntimeAnimatorController), RuntimeAnimatorController);
    }
    #endif
    #endregion
}
