using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject levelMenu;
    [SerializeField] GameObject startMenu;
    public void StartLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GoToLevelChoose()
    {
        startMenu.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(true);
    }
}
