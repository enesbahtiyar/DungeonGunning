using System;
using UnityEngine;

public class WhipWeapon: MonoBehaviour
{
    [SerializeField] float timeToAttack = 1.5f;
    [SerializeField] private GameObject rightWhipObject;
    [SerializeField] private GameObject leftWhipObject;
    
    private float timer;
    private OsmanHareket playerMovement;

    [SerializeField] private Vector2 whipAttackSize;
    [SerializeField] private float whipDamage = 40f;

    private void Start()
    {
        playerMovement = GetComponentInParent<OsmanHareket>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Attack();
        }
    }

    private void Attack()
    {
        timer = timeToAttack;

        if (playerMovement.lastHorizontalVector > 0)
        {
            rightWhipObject.SetActive(true);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(rightWhipObject.transform.position, whipAttackSize, 0f);
            ApplyDamage(colliders);
        }
        else
        {
            leftWhipObject.SetActive(true);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(leftWhipObject.transform.position, whipAttackSize, 0f);
            ApplyDamage(colliders);
        }
    }

    private void ApplyDamage(Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            OsmanHealth health = colliders[i].GetComponent<OsmanHealth>();

            if (health != null)
            {
                health.DecreaseHealth(whipDamage);
            }
        }
    }
}
