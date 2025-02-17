using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void startButton()
    {
        //load the tutorial scene
        SceneManager.LoadScene("Tutorial");
    }
    public void skipTutorial()
    {
        //load level 1 scene
        SceneManager.LoadScene("Level1");
    }
}
