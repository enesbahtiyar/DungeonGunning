using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class Weapon
{
    public string name;
    public bool ranged;
    public bool isAutomatic = false;       // Basılı tutarak ateş edilsin mi?
    public float damage;
    public float fireRate;                 // Saniyede mermi sayısı
    public GameObject bulletPrefab;
    public float bulletFireDuration;
    public bool bought = false;            // Silah satın alındı mı?

    [Header("Shotgun Settings")]
    public int bulletPerShot = 1;          // Pompalı için bir atışta kaç mermi?
    public float spreadAngle = 5f;         // Mermilerin saçılma açısı

    [Header("Ammo Settings")]
    public int magazineSize = 10;
    public int currentAmmo = 10;
    public int totalAmmo = 100;
    public float reloadTime = 2f;
    public bool unlimitedAmmo = false;

    public AudioClip fireSound;
    public ParticleSystem fireParticle;
    public Transform firePoint;
    public GameObject activeweaponPrefab;  // Sahnedeki silah modeli
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

    private SpriteRenderer weaponSpriteRenderer;
    private bool isReloading = false;

    void Start()
    {
        mainCamera = Camera.main;
        LoadWeapons();

        // İlk satın alınmış silahı seç
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].bought)
            {
                SetActiveWeapon(i);
                break;
            }
        }
    }

    void Update()
    {
        HandleWeaponSwitchInput();
        UpdateWeaponPosition();

        if (activeWeapon != null && !isReloading)
        {
            if (activeWeapon.isAutomatic)
            {
                if (Input.GetMouseButton(0))
                    TryFire();
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    TryFire();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && activeWeapon != null && !isReloading)
            StartCoroutine(Reload());

        UpdateAmmoUI();
    }

    void HandleWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count > 0 && weapons[0].bought) SetActiveWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count > 1 && weapons[1].bought) SetActiveWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Count > 2 && weapons[2].bought) SetActiveWeapon(2);
    }

    void SetActiveWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count || !weapons[index].bought) return;

        // Tüm silah modellerini kapat
        foreach (Weapon w in weapons)
        {
            if (w.activeweaponPrefab != null)
                w.activeweaponPrefab.SetActive(false);
        }

        // Yeni aktif silahı ayarla
        activeWeaponIndex = index;
        activeWeapon = weapons[activeWeaponIndex];

        if (activeWeapon.activeweaponPrefab != null)
            activeWeapon.activeweaponPrefab.SetActive(true);

        // UI'yi güncelle
        UpdateAmmoUI();
    }

    void LoadWeapons()
    {
        weaponDictionary.Clear();
        foreach (Weapon weapon in weapons)
        {
            weaponDictionary.Add(weapon.name, weapon);
        }
    }

    void TryFire()
    {
        if (activeWeapon == null || isReloading) return;
        if (Time.time - lastFireTime < 1f / activeWeapon.fireRate) return;

        Fire();
        lastFireTime = Time.time;
    }

    void Fire()
    {
        if (!activeWeapon.unlimitedAmmo)
        {
            if (activeWeapon.currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }
        }

        if (activeWeapon.ranged)
            RangedFire();
        else
            MeleeFire();

        if (activeWeapon.fireSound != null && activeWeapon.firePoint != null)
            AudioSource.PlayClipAtPoint(activeWeapon.fireSound, activeWeapon.firePoint.position);

        if (activeWeapon.fireParticle != null && activeWeapon.firePoint != null)
        {
            ParticleSystem ps = Instantiate(activeWeapon.fireParticle, activeWeapon.firePoint.position, activeWeapon.firePoint.rotation);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        if (!activeWeapon.unlimitedAmmo)
            activeWeapon.currentAmmo--;

        UpdateAmmoUI();
    }

    IEnumerator Reload()
    {
        if (activeWeapon == null || activeWeapon.unlimitedAmmo || isReloading) yield break;

        isReloading = true;
        yield return new WaitForSeconds(activeWeapon.reloadTime);

        int neededAmmo = activeWeapon.magazineSize - activeWeapon.currentAmmo;
        if (activeWeapon.totalAmmo >= neededAmmo)
        {
            activeWeapon.totalAmmo -= neededAmmo;
            activeWeapon.currentAmmo = activeWeapon.magazineSize;
        }
        else
        {
            activeWeapon.currentAmmo += activeWeapon.totalAmmo;
            activeWeapon.totalAmmo = 0;
        }

        isReloading = false;
        UpdateAmmoUI();
    }

    void RangedFire()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 world = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));
        Vector2 baseDirection = (world - weaponPosition.position).normalized;

        if (activeWeapon.firePoint == null) return;

        // Pompalı tüfek gibi birden fazla mermi at
        for (int i = 0; i < activeWeapon.bulletPerShot; i++)
        {
            float angleOffset = Random.Range(-activeWeapon.spreadAngle, activeWeapon.spreadAngle);
            Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
            Vector2 finalDirection = rotation * baseDirection;

            GameObject bullet = Instantiate(activeWeapon.bulletPrefab, activeWeapon.firePoint.position, Quaternion.identity);
            OsmanBullet bulletScript = bullet.GetComponent<OsmanBullet>();
            if (bulletScript != null)
                bulletScript.Fire(finalDirection, activeWeapon.damage);
        }
    }

    void MeleeFire()
    {
        // Yakın dövüş için buraya ekleme yapılabilir
    }

    void UpdateWeaponPosition()
    {
        if (weaponPosition == null || mainCamera == null) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = weaponPosition.position.z;

        Vector2 direction = (mouseWorldPos - weaponPosition.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        weaponPosition.rotation = Quaternion.Euler(0, 0, angle);
        UpdateWeaponFlip(angle);
    }

    void UpdateWeaponFlip(float angle)
    {
        if (activeWeapon != null && activeWeapon.activeweaponPrefab != null)
        {
            weaponSpriteRenderer = activeWeapon.activeweaponPrefab.GetComponent<SpriteRenderer>();
            if (weaponSpriteRenderer == null) return;

            if (angle < 0) angle += 360;

            if (angle >= 135 && angle < 225)
                weaponSpriteRenderer.flipY = true;
            else
                weaponSpriteRenderer.flipY = false;
        }
    }

    void UpdateAmmoUI()
    {
        if (bulletText == null || activeWeapon == null) return;

        if (activeWeapon.unlimitedAmmo)
            bulletText.text = "∞";
        else
            bulletText.text = activeWeapon.currentAmmo + "/" + activeWeapon.totalAmmo;
    }
}
