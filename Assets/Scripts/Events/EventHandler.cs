using System;
using UnityEngine;

public static class EventHandler
{
    #region EXP EVENTS
    //EXP EVENTS
    public static event Action<int> OnLevelUp;

    public static void CallOnLevelUp(int level)
    {
        if (OnLevelUp != null)
        {
            OnLevelUp(level);
        }
    }
    
    public static event Action<int, int> OnXPChanged;
    
    public static void CallOnXPChanged(int currentXP, int maxXP)
    {
        if (OnXPChanged != null)
        {
            OnXPChanged(currentXP, maxXP);
        }
    }
    #endregion
    
}
