using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyScript : MonoBehaviour
{
    public Animator animator;
    public PlayerScript player;
    public float speed = 0.0f;
    public int health = 2;
    private float deathTime = 1f;
    private float timeSinceLastHit = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(player.transform.position, transform.position);
        Debug.Log(distance);
        if (health == 0) //if health is 0 play death animation and then destroy object
        {
            returnToIdle();
            animator.SetBool("Dead", true);
            Destroy(gameObject, deathTime); // destroy after death animation finishes
        }else if(timeSinceLastHit < 1.5f) //if we are currently being hit play the hit animation
        {
            animator.SetBool("Hit", true);
        }
        else if (distance < 1.5f)//if player is close enough attack
        {
            speed = 0;
            animator.SetFloat("Speed", speed);
            animator.SetBool("Attacking", true);
        }
        else if (distance < 3)
        { //if player is close move towards them
            speed = 1.0f;
            //make enemy move towards player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            //make enemy face player
            if(player.transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            animator.SetFloat("Speed", speed);
            animator.SetBool("Attacking", false);

        }
        else
        {
            returnToIdle();
        }
        timeSinceLastHit += Time.deltaTime;
    }
    //function for taking damage
    public void takeDamage()
    {
        if(timeSinceLastHit >= 1.5f)
        {
            timeSinceLastHit = 0;
            health--;
            returnToIdle();
            animator.SetBool("Hit", true);
        }
       
    }
    public void returnToIdle()
    {
        speed = 0;
        animator.SetFloat("Speed", speed);
        animator.SetBool("Attacking", false);
        animator.SetBool("Hit", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //take damage if hit by projectile
        if (collision.CompareTag("Projectile"))
        {
            takeDamage();
        }
    }
}