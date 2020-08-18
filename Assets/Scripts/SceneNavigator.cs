using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneNavigator : MonoBehaviour
{
    [SerializeField] GameObject levelMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject settingsMenu;

    public void StartLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GoToLevelChoose()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(true);
        Time.timeScale = 1;
    }
    public void GoBackToMenu()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }
    public void GoToSettingsMenu()
    {
        startMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
