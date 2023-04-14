using UnityEngine;
using GoogleMobileAds.Api;
public class AdsInitialize : MonoBehaviour
{
    private void Awake()
    {
        MobileAds.Initialize(initStatus => { });
        //DontDestroyOnLoad(this);
    }
}
