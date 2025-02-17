using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    private Vector2 moveDirection;
    private PlayerScript owner;  // Reference to the player

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = speed*(moveDirection * speed).normalized;
        Invoke("DestroyProjectile", lifetime);
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    public void SetOwner(PlayerScript player)
    {
        owner = player;
    }

    public void DestroyProjectile()
    {
        if (owner != null)
        {
            owner.OnProjectileDestroyed();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            DestroyProjectile();
        }
        if (collision.CompareTag("Ground"))
        {
            DestroyProjectile();
        }
        if (collision.CompareTag("Wall"))
        {
            DestroyProjectile();   
        }
    }
}
