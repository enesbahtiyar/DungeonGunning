using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Player Details")]
public class EnemySO : ScriptableObject
{
    #region Header BASE DETAILS
    [Space(10)]
    #endregion

    #region Tooltip
    [Tooltip("Enemy Name")]
    #endregion
    public string enemyName;
    #region Tooltip

    [Tooltip("Prefab Gameobject for the enemy")]

    #endregion
    public GameObject enemyPrefab;
    #region Tooltip

    [Tooltip("Enemy Runtime Animator Controller")]

    #endregion
    public RuntimeAnimatorController enemyRuntimeAnimatorController;


    #region Header Health

    [Space(10)]
    [Header("Health")]

    #endregion

    #region Tooltip

    [Tooltip("Enemy starting health amount")]

    #endregion
    public int enemyHealthAmount;

    #region Header Other

    [Space(10)]
    [Header("Other")]

    #endregion

    //minimapIcon
    public Sprite enemyMiniMapIcon;

    //playerHandSprite
    public Sprite enemyHandSprite;


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(enemyHealthAmount), enemyHealthAmount, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyMiniMapIcon), enemyMiniMapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyHandSprite), enemyHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyRuntimeAnimatorController), enemyRuntimeAnimatorController);
    }
#endif
    #endregion
}
