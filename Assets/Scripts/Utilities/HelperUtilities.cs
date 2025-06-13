using UnityEngine;

public static class HelperUtilities
{
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fieldName + " is empty and must contain a value in object" + thisObject.name.ToString());
            return true;
        }

        return false;
    }
    
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fieldName + " is Null and must contain a value in object" + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueCheck,
        bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueCheck < 0)
            {
                Debug.Log(fieldName + " is negative and a positive value must contain a value in object" + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueCheck <= 0)
            {
                Debug.Log(fieldName + "must contain a positive value in object" + thisObject.name.ToString());
                error = true;
            }
        }
        return error;
    }
}
