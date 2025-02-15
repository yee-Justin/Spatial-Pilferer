using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Animator animator;
    public PlayerScript player;
    public float speed = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        //if player is close enough attack
        if (distance < 0.5){
            animator.SetBool("Attacking", true);
        }
        else if (distance < 3){ //if player is close move towards them
            speed = 1.0f;
            Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            animator.SetFloat("Speed", speed);
            animator.SetBool("Attacking", false);

        }
        else
        {
            speed = 0;
            animator.SetFloat("Speed", speed);
            animator.SetBool("Attacking", false);
        }
    }
}
