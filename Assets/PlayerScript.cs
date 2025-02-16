using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 5f;  // Movement speed
    public float jumpForce = 7f;  // Jump force
    private Rigidbody2D rb;
    private bool isGrounded;


    public GameObject projectilePrefab;  // Assign in Inspector
    public Transform firePoint;  // Position where projectile spawns

    private GameObject activeProjectile;  // Stores the current projectile

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
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
            if (Input.GetMouseButtonDown(1))
            {
                Teleport();
            }
        }

    }
    private void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
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
        Debug.Log("Mouse Position: " + mousePosition);
        Debug.Log("Direction: " + direction);

        Debug.Log("Projectile instantiated!");
        activeProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Debug.Log("Setting direction!");
        activeProjectile.GetComponent<ProjectileScript>().SetDirection(direction);
        activeProjectile.GetComponent<ProjectileScript>().SetOwner(this);
    }

    private void Teleport()
    {
        transform.position = activeProjectile.transform.position; // moves to projectile location
        activeProjectile.GetComponent<ProjectileScript>().DestroyProjectile();
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
        }
    }
}
