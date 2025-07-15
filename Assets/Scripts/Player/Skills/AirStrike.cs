using UnityEngine;

public class AirStrike : MonoBehaviour
{
    [SerializeField] private GameObject airStrikePlane;
    [SerializeField] private float spawnOffsetX = -10f; // kameran�n solundan ��ks�n
    [SerializeField] private float endOffsetX = 15f; // ekran�n sa��ndan ��ks�n
    [SerializeField] private float planeSpeed = 5f;
    [SerializeField] private int numberOfPlanes = 3;
    [SerializeField] private float yOffsetRange = 2f;

    public void StartAirStrike()
    {
        for (int i = 0; i < numberOfPlanes; i++)
        {
            float randomYOffset = Random.Range(-yOffsetRange, yOffsetRange);
            Vector3 spawnPos = new Vector3(transform.position.x + spawnOffsetX, transform.position.y + randomYOffset, 0);
            Vector3 targetPos = new Vector3(transform.position.x + endOffsetX, transform.position.y + randomYOffset, 0);
            GameObject plane = Instantiate(airStrikePlane, spawnPos, Quaternion.identity);
            plane.GetComponent<AirStrikePlane>().FlyTo(targetPos, planeSpeed,transform);
        }
        Invoke("CloseObject", 5f);
    }
    void CloseObject()
    {
        gameObject.SetActive(false);
    }
}
