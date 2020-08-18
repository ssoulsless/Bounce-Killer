using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private string key = "levelNum";
    private int levelNum;
    void Awake()
    {
        if (PlayerPrefs.HasKey(key))
        {
            levelNum = PlayerPrefs.GetInt(key);
        }
        else
        {
            levelNum = 1;
            PlayerPrefs.SetInt(key, levelNum);
            PlayerPrefs.Save();
        }
    }
    public void CompleteLevel()
    {
        levelNum++;
        PlayerPrefs.SetInt(key, levelNum);
        PlayerPrefs.Save();
    }
    public int GetLevelNum()
    {
        return levelNum;
    }
}
