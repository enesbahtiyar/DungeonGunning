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
    [SerializeField] private GameObject knifes;
    [SerializeField] private float knifesDeactiveTime = 10;
    [SerializeField] private Image knifesCooldownImg;
    [SerializeField] private float knifesCooldownTime;
    private bool knifesOnCooldown = false;
    [SerializeField] private CoolDownHandler coolDownHandler;


    [SerializeField] private GameObject bulletPrefab;
    private bool ricochetActive = false;
    [SerializeField] private float ricochetCooldownTime = 30f;
    [SerializeField] private float ricochetDuration = 10f;
    [SerializeField] private Image ricochetCooldownImg;

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
        if (Input.GetKeyDown(KeyCode.Alpha4)&&!knifesOnCooldown)
        {
            knifesOnCooldown = true;
            knifes.GetComponent<KnifesOsman>().deactiveTimer = knifesDeactiveTime;
            knifes.SetActive(true);
            knifesCooldownImg.gameObject.SetActive(true);
            coolDownHandler.StartCoolDown(knifesCooldownImg, knifesCooldownTime, () => knifesOnCooldown = false); ;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)&&!ricochetActive)
        {
            ricochetActive = true;
            OsmanBullet osmanBullet = bulletPrefab.GetComponent<OsmanBullet>();
            osmanBullet.canRicochet= true;
            osmanBullet.Invoke(nameof(osmanBullet.DeactiveRicochet), ricochetDuration);
            ricochetCooldownImg.gameObject.SetActive(true);
            coolDownHandler.StartCoolDown(ricochetCooldownImg, ricochetCooldownTime, () =>
            {
                ricochetCooldownImg.gameObject.SetActive(false);
            });
        }
    }
}
