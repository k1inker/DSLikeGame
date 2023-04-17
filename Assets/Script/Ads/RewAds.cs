using UnityEngine;
using GoogleMobileAds.Api;
namespace DS
{
    public class RewAds : MonoBehaviour
    {
        private string _rewardedUnitId = "***************************";
        private RewardedAd _rewardedAd;
        private SkinChanger _skinChanger;
        private void Awake()
        {
            _skinChanger = GetComponent<SkinChanger>();
        }
        private void OnEnable()
        {
            _rewardedAd = new RewardedAd(_rewardedUnitId);
            AdRequest adRequest = new AdRequest.Builder().Build();
            _rewardedAd.LoadAd(adRequest);
            _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        }

        private void HandleUserEarnedReward(object sender, Reward e)
        {
            _skinChanger.BuyButtonAction();
        }
        public void ShowAd()
        {
            if (_rewardedAd.IsLoaded())
                _rewardedAd.Show();
        }
    }
}
