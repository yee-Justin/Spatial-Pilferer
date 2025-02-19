using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float speed = 5f;  // Movement speed
    public float jumpForce = 7f;  // Jump force
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool teleportCooldown;

    public GameObject projectilePrefab;  // Assign in Inspector
    public GameObject spawnpoint;  // Assign in Inspector
    public Transform firePoint;  // Position where projectile spawns

    private GameObject activeProjectile;  // Stores the current projectile
    
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] hearts;  // Assign heart UI images in Inspector
    public Sprite fullHeart;  // Assign full heart sprite
    public Sprite emptyHeart; // Assign empty heart sprite
    public SpriteRenderer spriteRenderer;

    public Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHearts();
    }

    void Update()
    {
        UpdateHearts();
        if(currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
            Respawn();
            animator.SetBool("isDead", false);
            currentHealth = maxHealth;
        }
        spawnpoint = GameObject.FindGameObjectWithTag("Spawnpoint");

        Move();
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0) && activeProjectile == null)
        {
            Shoot();
        }

        if(activeProjectile != null)
        {
            if (Input.GetMouseButtonDown(1) && !teleportCooldown)
            {
                teleportCooldown = true;
                Teleport();
            }
        }

    }
    public void OnAttackAnimationEnd()
    {
        animator.SetBool("isAttacking", false); // Transition back to idle
    }


    private void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        if (moveInput != 0)
        {
            spriteRenderer.flipX = (moveInput == -1); // Flip if moving left
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }


        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }
    private void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        animator.SetBool("isAttacking", true);
        activeProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        activeProjectile.GetComponent<ProjectileScript>().SetDirection(direction);
        activeProjectile.GetComponent<ProjectileScript>().SetOwner(this);
    }


    private void Teleport()
    {
        transform.position = activeProjectile.transform.position; // moves to projectile location
        activeProjectile.GetComponent<ProjectileScript>().DestroyProjectile();
    }

    private void Respawn()
    {
        transform.position = spawnpoint.transform.position;
    }
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;  // Show full heart
            else
                hearts[i].sprite = emptyHeart; // Show empty heart
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHearts();
    }
    public void OnProjectileDestroyed()
    {
        Debug.Log("Projectile destroyed!");
        activeProjectile = null; // Allow shooting again
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detects if player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            teleportCooldown = false;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("KillZone"))
        {
            TakeDamage(1);
            Respawn();
        }

        if(collision.CompareTag("Portal"))
        {
            SceneManager.LoadScene("Level1");
        }

        if(collision.CompareTag("Gate"))
        {
            SceneManager.LoadScene("GameStart");
            Debug.Log("YOU WON!!!!!!!!!!!!!!!!!");
        }
        if (collision.CompareTag("HealthPotion"))
        {
            if(currentHealth < maxHealth)
            {
                currentHealth++;
                UpdateHearts();
                Destroy(collision.gameObject);
            }
        }
    }
}
