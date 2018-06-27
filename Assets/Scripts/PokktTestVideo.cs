using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokkt;
using UnityEngine.UI;

public class PokktTestVideo : MonoBehaviour {
    string appId ="8bbeca57cc4c46b84fad1618cbb013b8";
    string securityKey ="348662285aa5b039a6ea2cd0e108a4dd";
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
            PokktAds.VideoAd.ShowNonRewarded("RewardedAd");
            }
    }

    public void VideCacheRewarded()
    {
        PokktAds.VideoAd.CacheNonRewarded("RewardedAd");
    }

}
