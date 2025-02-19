using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyScript : MonoBehaviour
{

    public Animator parentAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Animator found" + parentAnimator);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            parentAnimator.SetBool("TorchOn", true);

            gameObject.SetActive(false);
        }
    }
}
