using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokkt;
using UnityEngine.UI;

public class PokktTestVideo : MonoBehaviour {
    string appId ="14a3f7b59b47db5d8ab89404cf21c3fd";
    string securityKey ="8c6afcb30b8ac14ef14d64f4c5002db0";
  //  public Button btn;
    
	// Use this for initialization
	void Start () {
        PokktAds.SetPokktConfig(
            appId,
            securityKey, 
            Pokkt.Extensions.PokktExtensionProvider.GetExtension(), 
            true);
        //PokktAds.Debugging.ShouldDebug( true );

        // setup consent
        PokktAds.ConsentInfo consentInfo = new PokktAds.ConsentInfo();
        consentInfo.GDPRApplicable = true;
        //true if GDPR is applicable.
        consentInfo.GDPRConsentAvailable = true;
        //true if user has given consent to use personal details for ad targeting.
        PokktAds.SetDataAccessConsent(consentInfo);
        VideCacheRewarded();
    }

    // Update is called once per frame
    void Update () {
    }
    
    public void VideocacheAdd()
    {
      // PokktAds.VideoAd.CacheRewarded("RewardAd");
    }
    public void VideoAd()
    {
        if (PokktAds.VideoAd.IsAdCached("RewardedAd", true))
            {
            PokktAds.VideoAd.ShowNonRewarded("RewardAd");
            }
    }

    public void VideCacheRewarded()
    {
        PokktAds.VideoAd.CacheNonRewarded("RewardedAd");
    }

}
