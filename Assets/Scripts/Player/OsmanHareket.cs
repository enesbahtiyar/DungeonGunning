using UnityEngine;

public class OsmanHareket : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public float rotationSpeed = 10f;
    
    [Header("Camera Settings")]
    [SerializeField] public float cameraFollowSpeed = 5f;
    [SerializeField] public Vector3 cameraOffset = new Vector3(0, 0, -10f);

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private string lastPlayedAnimation;
    // [SerializeField]private BackgroundScroller backgroundScroller;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        PlayAnimation("IdleDown");
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }
    
    void LateUpdate()
    {
        FollowCamera();
    }

    private void MoveCharacter()
    {
        rb.linearVelocity = moveDirection * moveSpeed;

    }
    
    private void FollowCamera()
    {
        if (mainCamera == null)
            return;
            
        Vector3 targetPosition = transform.position + cameraOffset;
        
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position, 
            targetPosition, 
            cameraFollowSpeed * Time.deltaTime
        );
        // if (moveDirection != Vector2.zero)
        // {
        //     backgroundScroller.ScrollBackGround(transform);
        // }
    }
    
    private void UpdateAnimations()
    {
        if (animator == null)
            return;
            
        float speed = moveDirection.magnitude;
        
        Vector2 lookDirection = mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        
        if (angle < 0) angle += 360;
        
        if (speed < 0.1f)
        {
            if (angle >= 315 || angle < 45)
            {
                PlayAnimation("IdleRight");
                spriteRenderer.flipX = false;
            }
            else if (angle >= 45 && angle < 135)
            {
                PlayAnimation("IdleUp");
                spriteRenderer.flipX = false;
            }
            else if (angle >= 135 && angle < 225)
            {
                PlayAnimation("IdleRight");
                spriteRenderer.flipX = true;
            }
            else
            {
                PlayAnimation("IdleDown");
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            if (angle >= 315 || angle < 45)
            {
                PlayAnimation("WalkRight");
                spriteRenderer.flipX = false;
            }
            else if (angle >= 45 && angle < 135)
            {
                PlayAnimation("WalkUp");
                spriteRenderer.flipX = false;
            }
            else if (angle >= 135 && angle < 225)
            {
                PlayAnimation("WalkRight");
                spriteRenderer.flipX = true;
            }
            else
            {
                PlayAnimation("WalkDown");
                spriteRenderer.flipX = false;
            }
        }
    }
    
    private void PlayAnimation(string animationName)
    {
        if (lastPlayedAnimation == animationName)
            return;
            
        animator.Play(animationName);
        
        lastPlayedAnimation = animationName;
    }
    

}
