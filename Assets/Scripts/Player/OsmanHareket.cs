using UnityEngine;

public class OsmanHareket : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    [SerializeField] private float hareketHizi = 5f;
    [SerializeField] public float donusHizi = 10f;
    
    [Header("Kamera Ayarlari")]
    [SerializeField] public float kameraTakipHizi = 5f;
    [SerializeField] public Vector3 kameraOffset = new Vector3(0, 0, -10f);

    private Rigidbody2D fizik;
    private Camera anaKamera;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 hareketYonu;
    private Vector2 fareKonumu;
    private string sonOynatilanAnimasyon ;

    void Start()
    {
        fizik = GetComponent<Rigidbody2D>();
        anaKamera = Camera.main;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        OynatAnimasyon("BekleAsagi");
    }

    void Update()
    {
        float yatayGiris = Input.GetAxisRaw("Horizontal");
        float dikeyGiris = Input.GetAxisRaw("Vertical");
        hareketYonu = new Vector2(yatayGiris, dikeyGiris).normalized;

        fareKonumu = anaKamera.ScreenToWorldPoint(Input.mousePosition);
        
        AnimasyonlariGuncelle();
    }

    void FixedUpdate()
    {
        KarakteriHareketEttir();
    }
    
    void LateUpdate()
    {
        KamerayiTakipEttir();
    }

    private void KarakteriHareketEttir()
    {
        fizik.linearVelocity = hareketYonu * hareketHizi;
    }
    
    private void KamerayiTakipEttir()
    {
        if (anaKamera == null)
            return;
            
        Vector3 hedefPozisyon = transform.position + kameraOffset;
        
        anaKamera.transform.position = Vector3.Lerp(
            anaKamera.transform.position, 
            hedefPozisyon, 
            kameraTakipHizi * Time.deltaTime
        );
    }
    
    private void AnimasyonlariGuncelle()
    {
        if (animator == null)
            return;
            
        float hiz = hareketYonu.magnitude;
        
        Vector2 bakisYonu = fareKonumu - (Vector2)transform.position;
        float aci = Mathf.Atan2(bakisYonu.y, bakisYonu.x) * Mathf.Rad2Deg;
        
        if (aci < 0) aci += 360;
        
        if (hiz < 0.1f)
        {
            if (aci >= 315 || aci < 45)
            {
                OynatAnimasyon("BekleSag");
                spriteRenderer.flipX = false;
            }
            else if (aci >= 45 && aci < 135)
            {
                OynatAnimasyon("BekleYukari");
                spriteRenderer.flipX = false;
            }
            else if (aci >= 135 && aci < 225)
            {
                OynatAnimasyon("BekleSag");
                spriteRenderer.flipX = true;
            }
            else
            {
                OynatAnimasyon("BekleAsagi");
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            if (aci >= 315 || aci < 45)
            {
                OynatAnimasyon("YuruSag");
                spriteRenderer.flipX = false;
            }
            else if (aci >= 45 && aci < 135)
            {
                OynatAnimasyon("YuruYukari");
                spriteRenderer.flipX = false;
            }
            else if (aci >= 135 && aci < 225)
            {
                OynatAnimasyon("YuruSag");
                spriteRenderer.flipX = true;
            }
            else
            {
                OynatAnimasyon("YuruAsagi");
                spriteRenderer.flipX = false;
            }
        }
    }
    
    private void OynatAnimasyon(string animasyonAdi)
    {
        if (sonOynatilanAnimasyon == animasyonAdi)
            return;
            
        animator.Play(animasyonAdi);
        
        sonOynatilanAnimasyon = animasyonAdi;
    }
}