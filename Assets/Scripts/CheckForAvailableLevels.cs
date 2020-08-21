using UnityEngine;
using UnityEngine.UI;

public class CheckForAvailableLevels : MonoBehaviour
{
    [SerializeField] Image[] images;
    private LevelManager levelManager;
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        for (int i=0; i<levelManager.GetLevelNum(); i++)
        {
            images[i].gameObject.SetActive(false);
        }
    }
}
