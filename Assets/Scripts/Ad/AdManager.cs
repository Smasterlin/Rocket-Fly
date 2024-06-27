using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("广告的初始化")]
    [SerializeField] string _androidGameId = "5627678";
    [SerializeField] string _iOSGameId = "5627679";
    [SerializeField] bool _testMode = true;
    private string _gameId;

    [Header("激励广告")]
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidRewardAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSRewardAdUnitId = "Rewarded_iOS";
    string _adRewardUnitId = null;// This will remain null for unsupported platforms

    //[Header("插屏广告")]
    //[SerializeField] string _androidAdUnitId = "Interstitial_Android";
    //[SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    //string _adUnitId;

    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
        //初始化广告
        InitializeAds();

        //激励广告
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adRewardUnitId = _iOSRewardAdUnitId;
#elif UNITY_ANDROID
        _adRewardUnitId = _androidRewardAdUnitId;
#endif
        // Disable the button until the ad is ready to show:
        _showAdButton.interactable = false;


        ////插屏广告
        //// Get the Ad Unit ID for the current platform:
        //_adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
        //    ? _iOsAdUnitId
        //    : _androidAdUnitId;
    }
    #region 初始化广告
    private void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        //加载激励广告
        LoadAd();
        OnUnityAdsAdLoaded(_adRewardUnitId);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    #endregion

    #region 激励广告
    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adRewardUnitId);
        Advertisement.Load(_adRewardUnitId, this);
    }
    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adRewardUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        //_showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adRewardUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adRewardUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            UIManager.instance.GetReward();
            _showAdButton.gameObject.SetActive(false);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
    #endregion
}
