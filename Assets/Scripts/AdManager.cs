using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] Button button;
    private GameManager gameManager;
    string gameId = "3778963";
    string myPlacementId = "rewardedVideo";
    private void Awake()
    {
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            Advertisement.Initialize(gameId, true);
            if (button) button.onClick.AddListener(ShowRewardedVideo);
    }
    public void ShowStandartVideoAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Video");
        }
    }
    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
   }
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            button.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            gameManager.RewardForWatchingAd();
        }
        else if (showResult == ShowResult.Skipped)
        {
            gameManager.RewardForWatchingAd();
        }
        else if (showResult == ShowResult.Failed)
        {
            gameManager.RewardForWatchingAd();
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
