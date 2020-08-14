using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneNavigator : MonoBehaviour
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
        Time.timeScale = 1;
    }
    public void GoBackToMenu()
    {
        startMenu.gameObject.SetActive(true);
        levelMenu.gameObject.SetActive(false);
    }
}
