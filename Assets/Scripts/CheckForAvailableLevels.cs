using UnityEngine;
using UnityEngine.UI;

public class CheckForAvailableLevels : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    private LevelManager levelManager;
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        for (int i=0; i<levelManager.GetLevelNum(); i++)
        {
            buttons[i].gameObject.SetActive(true);
        }
    }
}
