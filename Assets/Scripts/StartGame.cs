using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartGame : MonoBehaviour
{
    public void StartFirstLevel()
    {
        SceneManager.LoadScene("FirstLevel");
    }
}
