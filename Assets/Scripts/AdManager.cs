using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    string gameId = "3778963";
    bool testMode = false;
    private void Awake()
    {
        Advertisement.Initialize(gameId, testMode);
    }
    public void ShowStandartVideoAd()
    {
        Advertisement.Show("video");
    }
}