using UnityEngine;
using GoogleMobileAds.Api;
public class InterAds : MonoBehaviour
{
    private InterstitialAd _interstitialAd;
    private string _interstitialUnitId = "*************************";
    private void OnEnable()
    {
        _interstitialAd = new InterstitialAd(_interstitialUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _interstitialAd.LoadAd(adRequest);
    }
    public void ShowAd()
    {
        if (_interstitialAd.IsLoaded())
            _interstitialAd.Show();
    }
}
