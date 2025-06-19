using UnityEngine;
using System.Collections.Generic;
using TMPro;
[System.Serializable]
public class Weapon
{
    public string name;
    public bool ranged;
    public float damage;
    public float fireRate;
    public GameObject bulletPrefab;
    public float bulletFireDuration; 
    public int bulletAmount; 
    public AudioClip fireSound; 
    public ParticleSystem fireParticle; 
    public Transform firePoint; 
}

public class OsmanAttack : MonoBehaviour
{
    [Header("Weapons")]
    public List<Weapon> weapons = new List<Weapon>();
    public Dictionary<string, Weapon> weaponDictionary = new Dictionary<string, Weapon>();
    
    [Header("Weapon Selection")]
    public int activeWeaponIndex = 0;
    
    [Header("Fire Point")]
    public Transform weaponPosition;

    private Weapon activeWeapon;
    private float lastFireTime;
    private Camera mainCamera; 
    public TextMeshProUGUI bulletText;

    void Start()
    {
        mainCamera = Camera.main;
        LoadWeapons();
        
        if (weapons.Count > 0)
            activeWeapon = weapons[activeWeaponIndex];
    }

    void Update()
    {
        SwitchWeapon();

        UpdateWeaponPosition(); 

        if (Input.GetMouseButton(0) && activeWeapon != null)
        {
            Fire();
        }

        if (bulletText != null && activeWeapon != null)
        {
            bulletText.text = activeWeapon.bulletAmount.ToString();
        }
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activeWeaponIndex = (activeWeaponIndex + 1) % weapons.Count;
            activeWeapon = weapons[activeWeaponIndex];
        }
    }

    void LoadWeapons()
    {
        weaponDictionary.Clear();
        foreach (Weapon weapon in weapons)
        {
            weaponDictionary.Add(weapon.name, weapon);
        }
    }

    void Fire()
    {
        if (activeWeapon == null) return;

        if (activeWeapon.bulletAmount <= 0) return;

        if (Time.time - lastFireTime < activeWeapon.bulletFireDuration) return;
        
        if (activeWeapon.ranged)
        {
            RangedFire();
        }
        else
        {
            MeleeFire();
        }

        if (activeWeapon.fireSound != null && activeWeapon.firePoint != null)
        {
            AudioSource.PlayClipAtPoint(activeWeapon.fireSound, activeWeapon.firePoint.position);
        }
        if (activeWeapon.fireParticle != null && activeWeapon.firePoint != null)
        {
            ParticleSystem ps = Instantiate(activeWeapon.fireParticle, activeWeapon.firePoint.position, activeWeapon.firePoint.rotation);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        activeWeapon.bulletAmount--;

        lastFireTime = Time.time;
    }

    void RangedFire()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 world = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));
        
        Vector2 direction = (world - weaponPosition.position).normalized;

        if (activeWeapon.firePoint == null) return; 
        
        GameObject bullet = Instantiate(activeWeapon.bulletPrefab, activeWeapon.firePoint.position, Quaternion.identity);
        OsmanBullet bulletScript = bullet.GetComponent<OsmanBullet>();
        
        if (bulletScript != null)
        {
            bulletScript.Fire(direction, activeWeapon.damage);
        }
    }

    void MeleeFire()
    {
        // Yakın saldırıyı da burada yapabiliriz yine isterseniz
    }

    void UpdateWeaponPosition()
    {
        if (weaponPosition == null || mainCamera == null) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = weaponPosition.position.z;

        Vector2 direction = (mouseWorldPos - weaponPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weaponPosition.rotation = Quaternion.Euler(0, 0, angle);

    }
}