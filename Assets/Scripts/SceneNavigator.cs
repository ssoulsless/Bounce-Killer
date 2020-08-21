using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneNavigator : MonoBehaviour
{
    [SerializeField] GameObject levelMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject settingsMenu;

    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    public void StartLevel(int sceneId)
    {
        if (levelManager.GetLevelNum() >= sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }
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
