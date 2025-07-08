using UnityEngine;

public class SkillController : MonoBehaviour
{
    
    [SerializeField] private GameObject airStrikeMarker;
    private GameObject currentMarker;
    private bool isTargeting = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            isTargeting = true;
        }
        if (isTargeting)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (currentMarker == null)
            {
                currentMarker = airStrikePooler.spawnFromPool("AirstrikeMarker", mousePos, Quaternion.identity);
            }
            
            currentMarker.transform.position = mousePos;
            
            if (Input.GetMouseButtonDown(0))
            {
                currentMarker.GetComponent<AirStrike>().StartAirStrike();
                isTargeting = false;
                currentMarker = null;
            }
        }
    }
}
