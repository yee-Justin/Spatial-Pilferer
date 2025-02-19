using UnityEngine;

public class GateScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator torch1;
    public Animator torch2;
    public GameObject portal;
    void Start()
    {
        portal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (torch1.GetBool("TorchOn") && torch2.GetBool("TorchOn"))
        {
            portal.SetActive(true);
        }
        else
        {
            Debug.Log("Torch is OFF!");
        }

    }
}
