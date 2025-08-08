using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{


    [SerializeField] AirStrikePooler airStrikePooler;
    [SerializeField] private GameObject airStrikeMarker;
    [SerializeField] private Image airstrikeCooldownImg;
    [SerializeField] private float airstrikeCooldownTime;
    private bool airstrikeOnCooldown = false;
    private GameObject currentMarker;
    private bool isTargeting = false;

    [SerializeField] private CoolDownHandler coolDownHandler;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha5) && !airstrikeOnCooldown)
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
                airstrikeOnCooldown = true;
                airstrikeCooldownImg.gameObject.SetActive(true);
                coolDownHandler.StartCoolDown(airstrikeCooldownImg, airstrikeCooldownTime, () =>
                { airstrikeOnCooldown = false; airstrikeCooldownImg.gameObject.SetActive(false); });
                isTargeting = false;
                currentMarker = null;
            }
        }
    }
}
