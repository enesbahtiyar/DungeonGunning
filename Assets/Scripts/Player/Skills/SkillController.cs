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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            isTargeting = true;
            
        }
        if (isTargeting)
        {

            if (currentMarker == null)
            {
                currentMarker = Instantiate(airStrikeMarker, Vector3.zero, Quaternion.identity);
            }
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMarker.transform.position = mousePos;
            if (Input.GetMouseButtonDown(0))
            {
                currentMarker.GetComponent<AirStrike>().StartAirStrike();
                isTargeting = false;
            }
        }
    }
}
