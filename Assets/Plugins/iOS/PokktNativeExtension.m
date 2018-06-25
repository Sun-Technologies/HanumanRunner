#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <PokktSDK/PokktSDK.h>


/***
 *
 *  CONSTS
 *
 ***/

// Operation Names

#define     SET_POKKT_CONFIG                @"setPokktConfig"
#define     SET_THIRDPARTY_ID               @"setThirdPartyUserId"
#define     SET_PLAYER_VIEW_CONFIG          @"setAdPlayerViewConfig"
#define     SET_USER_DETAILS                @"setUserDetails"
#define     SET_CALLBACK_EXTRA_PARAMS       @"setCallbackExtraParams"

#define     VIDEO_AD_CACHE_REWARDED                     @"VideoAd_cacheRewarded"
#define     VIDEO_AD_CACHE_NON_REWARDED                 @"VideoAd_cacheNonRewarded"
#define     VIDEO_AD_SHOW_REWARDED                      @"VideoAd_showRewarded"
#define     VIDEO_AD_SHOW_NON_REWARDED                  @"VideoAd_showNonRewarded"

#define     INTERSTITIAL_AD_CACHE_REWARDED                      @"Interstitial_cacheRewarded"
#define     INTERSTITIAL_AD_CACHE_NON_REWARDED                  @"Interstitial_cacheNonRewarded"
#define     INTERSTITIAL_AD_SHOW_REWARDED                       @"Interstitial_showRewarded"
#define     INTERSTITIAL_AD_SHOW_NON_REWARDED                   @"Interstitial_showNonRewarded"

#define     BANNER_LOAD                     @"Banner_loadBanner"
#define     BANNER_LOAD_WITH_RECT           @"Banner_loadBannerWithRect"
#define     BANNER_DESTROY                  @"Banner_destroyBanner"

#define     IN_GAME_ADS_FETCH_ASSETS        @"InGameAd_fetchAssets"
#define     IN_GAME_ADS_TRACK_FU_CLICK      @"InGameAd_trackFUClick"
#define     IN_GAME_ADS_UPDATE_IGA_DATA     @"InGameAd_updateIGAData"

#define     ANALYTICS_SET_DETAILS           @"Analytics_setAnalyticsDetails"
#define     ANALYTICS_TRACK_IAP             @"Analytics_trackIAP"

#define     DEBUGGING_SHOULD_DEBUG          @"Debugging_shouldDebug"
#define     DEBUGGING_EXPORT_LOG            @"Debugging_exportLog"
#define     DEBUGGING_EXPORT_LOG_TO_CLOUD   @"Debugging_exportLogToCloud"
#define     DEBUGGING_SHOW_TOAST            @"Debugging_showToast"
#define     DEBUGGING_SHOW_LOG              @"Debugging_showLog"

#define     SET_DATA_ACCESS_CONSENT         @"setDataAccessConsent"


/***
 *  Pokkt-to-Framework Events
 ***/

// Video Ad Related Events
#define EVENT_VIDEO_AD_CACHING_COMPLETED                      @"VideoAdCachingCompleted"
#define EVENT_VIDEO_AD_CACHING_FAILED                         @"VideoAdCachingFailed"
#define EVENT_VIDEO_AD_CLOSED                                 @"VideoAdClosed"
#define EVENT_VIDEO_AD_COMPLETED                              @"VideoAdCompleted"
#define EVENT_VIDEO_AD_FAILED_TO_SHOW                         @"VideoAdFailedToShow"
#define EVENT_VIDEO_AD_DISPLAYED                              @"VideoAdDisplayed"
#define EVENT_VIDEO_AD_SKIPPED                                @"VideoAdSkipped"
#define EVENT_VIDEO_AD_GRATIFIED                              @"VideoAdGratified"

// Interstitial Related Events
#define EVENT_INTERSTITIAL_CACHING_COMPLETED                      @"InterstitialCachingCompleted"
#define EVENT_INTERSTITIAL_CACHING_FAILED                         @"InterstitialCachingFailed"
#define EVENT_INTERSTITIAL_CLOSED                                 @"InterstitialClosed"
#define EVENT_INTERSTITIAL_COMPLETED                              @"InterstitialCompleted"
#define EVENT_INTERSTITIAL_FAILED_TO_SHOW                         @"InterstitialFailedToShow"
#define EVENT_INTERSTITIAL_DISPLAYED                              @"InterstitialDisplayed"
#define EVENT_INTERSTITIAL_SKIPPED                                @"InterstitialSkipped"
#define EVENT_INTERSTITIAL_GRATIFIED                              @"InterstitialGratified"

// Banner Ad Delegate
#define EVENT_BANNER_LOADED                   @"BannerLoaded"
#define EVENT_BANNER_LOAD_FAILED              @"BannerLoadFailed"

// IGA Delegate
#define EVENT_IGA_ASSETS_READY                @"IGAAssetsReady"
#define EVENT_IGA_ASSETS_FAILED               @"IGAAssetsFailed"



// banner related
#define TOP_LEFT        1
#define TOP_CENTER      2
#define TOP_RIGHT       3
#define MIDDLE_LEFT     4
#define MIDDLE_CENTER   5
#define MIDDLE_RIGHT    6
#define BOTTOM_LEFT     7
#define BOTTOM_CENTER   8
#define BOTTOM_RIGHT    9




/***
 *
 * DEFINITIONS
 *
 ***/


// utils
void notifyFramework(NSString *operation, NSString *param);
UIViewController* getRootViewController();
NSDictionary* extractJSON(NSString* jsonString);
PokktIAPDetails* parseIAPDetails(NSString* detailsString);
PokktAdPlayerViewConfig* getAdPlayerViewConfigFromJSONString(NSString* configString);
PokktUserInfo* getUserDetailsFromJSONString(NSString* stringData);
PokktAnalyticsDetails* getAnalyticsDetailsFromJSONString(NSString* stringData);
PokktConsentInfo* getConsentInfoFromJSONString(NSString* stringData);
NSString* stringifyJSONDictionary(NSDictionary *jsonObject);
NSString* getReturnParamsFromValues(NSString* _Nonnull screenName, bool isRewarded, NSMutableDictionary<NSString *, id> *paramList);


// banner-util methods
void topLeft(UIView * view);
void topRight(UIView * view);
void topCenter(UIView * view);
void middleLeft(UIView* view);
void middleRight(UIView * view);
void middleCenter(UIView * view);
void bottomLeft(UIView * view);
void bottomRight(UIView * view);
void bottomCenter(UIView * view);




/***
 *
 * ADAPTER
 *
 ***/

static PokktBannerView* bannerContainer;
static BOOL delegateAssigned = false;

@interface PokktAdsAdapter : NSObject<PokktVideoAdsDelegate, PokktInterstitialDelegate, PokktBannerDelegate, PokktIGADelegate>

+ (void) processForOperation:(NSString*)operation withParams:(NSString*)params;

@end




/***
 *
 *  NATIVE EXTENSION
 *  - these are exposed to the framework
 *  - frameworks will call these to communicate
 *
 ***/

void notifyNative(const char *operation, const char *args)
{
    [PokktDebugger printLog:[NSString stringWithFormat:@"notifyNative of operation: %s with params: %s", operation, args]];
    
    NSString *op = [NSString stringWithUTF8String:(operation)];
    NSString *params = [NSString stringWithUTF8String:(args)];
    
    [PokktAdsAdapter processForOperation:op withParams:params];
    
    if (!delegateAssigned)
    {
        PokktAdsAdapter *adapter = [[PokktAdsAdapter alloc] init];
        [PokktVideoAds setPokktVideoAdsDelegate:adapter];
        [PokktInterstitial setPokktInterstitialDelegate:adapter];
        [PokktBanner setPokktBannerDelegate:adapter];
        [PokktInGameAd setIGADelegate:adapter];
        
        delegateAssigned = true;
    }
}

char* getSDKVersionOnNative()
{
    return strdup([[NSString stringWithFormat:@"%@", [PokktAds getSDKVersion]] UTF8String]);
}

bool isVideoAdCachedOnNative(const char* screenName, bool isRewarded)
{
    [PokktDebugger printLog:(NSString *) @"isAdCachedOnNative called"];
    NSString *screenNameNS = [NSString stringWithUTF8String:screenName];
    
    return [PokktVideoAds isAdCached:(NSString *)screenNameNS isRewarded:(BOOL)isRewarded];
}

bool isInterstitialCachedOnNative(const char* screenName, bool isRewarded){
    [PokktDebugger printLog:(NSString *) @"isInterstitialCached called"];
    NSString *screenNameNS = [NSString stringWithUTF8String:screenName];
    
    return [PokktInterstitial isAdCached:(NSString *)screenNameNS isRewarded:(BOOL)isRewarded];
}


/***
 *
 *  ADATPTER: implementation
 *
 ***/

@implementation PokktAdsAdapter

+ (void) processForOperation:(NSString*)operation withParams:(NSString*)params
{
    // Common APIs
    if ([SET_POKKT_CONFIG isEqualToString:operation])
    {
        NSDictionary *jsonObject = extractJSON(params);
        NSString *appId = jsonObject[@"appId"];
        NSString *securityKey = jsonObject[@"securityKey"];
        [PokktAds setPokktConfigWithAppId:appId securityKey:securityKey];
    }
    else if ([SET_THIRDPARTY_ID isEqualToString:operation])
    {
        [PokktAds setThirdPartyUserId:params];
    }
    else if ([SET_PLAYER_VIEW_CONFIG isEqualToString:operation])
    {
        [PokktAds setPokktAdPlayerViewConfig:getAdPlayerViewConfigFromJSONString(params)];
    }
    else if ([SET_USER_DETAILS isEqualToString:operation])
    {
        [PokktAds setUserDetails:getUserDetailsFromJSONString(params)];
    }
    else if ([SET_CALLBACK_EXTRA_PARAMS isEqualToString:operation])
    {
        NSDictionary *paramDict = extractJSON(params);
        [PokktAds setCallbackExtraParam:paramDict];
    }
    
    
    // Video Ads APIs
    else if ([VIDEO_AD_CACHE_REWARDED isEqualToString:operation])
    {
        [PokktVideoAds cacheRewarded:params];
    }
    else if ([VIDEO_AD_CACHE_NON_REWARDED isEqualToString:operation])
    {
        [PokktVideoAds cacheNonRewarded:params];
    }
    else if ([VIDEO_AD_SHOW_REWARDED isEqualToString:operation])
    {
        [PokktVideoAds showRewarded:params withViewController:getRootViewController()];
    }
    else if ([VIDEO_AD_SHOW_NON_REWARDED isEqualToString:operation])
    {
        [PokktVideoAds showNonRewarded:params withViewController:getRootViewController()];
    }
    
    
    // Interstitial APIs
    else if ([INTERSTITIAL_AD_CACHE_REWARDED isEqualToString:operation])
    {
        [PokktInterstitial cacheRewarded:params];
    }
    else if ([INTERSTITIAL_AD_CACHE_NON_REWARDED isEqualToString:operation])
    {
        [PokktInterstitial cacheNonRewarded:params];
    }
    else if ([INTERSTITIAL_AD_SHOW_REWARDED isEqualToString:operation])
    {
        [PokktInterstitial showRewarded:params withViewController:getRootViewController()];
    }
    else if ([INTERSTITIAL_AD_SHOW_NON_REWARDED isEqualToString:operation])
    {
        [PokktInterstitial showNonRewarded:params withViewController:getRootViewController()];
    }
    
    
    // Banner Ads APIs
    else if ([BANNER_LOAD isEqualToString:operation])
    {
        NSDictionary *jsonObject = extractJSON(params);
        
        NSString* screenName = jsonObject[@"screenName"];
        int position = [jsonObject[@"bannerPosition"] intValue];
        
        if (bannerContainer != NULL)
        {
            [PokktBanner destroyBanner:bannerContainer];
            bannerContainer = NULL;
        }
        
        bannerContainer = [[PokktBannerView alloc] initWithFrame:CGRectMake(0, 0, 320, 50)];
        bannerContainer.translatesAutoresizingMaskIntoConstraints = NO;
        
        [getRootViewController().view addSubview:bannerContainer];
        
        switch (position)
        {
            case 1: topLeft(bannerContainer);       break;
            case 2: topCenter(bannerContainer);     break;
            case 3: topRight(bannerContainer);      break;
            case 4: middleLeft(bannerContainer);    break;
            case 5: middleCenter(bannerContainer);  break;
            case 6: middleRight(bannerContainer);   break;
            case 7: bottomLeft(bannerContainer);    break;
            case 8: bottomCenter(bannerContainer);  break;
            case 9: bottomRight(bannerContainer);   break;
            default: topCenter(bannerContainer);    break;
        }
        
        [PokktBanner loadBanner:bannerContainer withScreenName:screenName rootViewContorller:getRootViewController()];
    }
    else if ([BANNER_LOAD_WITH_RECT isEqualToString:operation])
    {
        NSDictionary *jsonObject = extractJSON(params);
        
        NSString *screenName = jsonObject[@"screenName"];
        int width = [jsonObject[@"width"] intValue];
        int height = [jsonObject[@"height"] intValue];
        float x = [jsonObject[@"x"] floatValue];
        float y = [jsonObject[@"y"]floatValue];
        
        if (bannerContainer != NULL)
        {
            [PokktBanner destroyBanner:bannerContainer];
            bannerContainer = NULL;
        }
        
        bannerContainer = [[PokktBannerView alloc] initWithFrame:CGRectMake(x, y, width, height)];
        [getRootViewController().view addSubview:bannerContainer];
        
        [PokktBanner loadBanner:bannerContainer withScreenName:screenName rootViewContorller:getRootViewController()];
    }
    else if ([BANNER_DESTROY isEqualToString:operation])
    {
        [PokktBanner destroyBanner:bannerContainer];
    }
    
    
    // IGA APIs
    else if ([IN_GAME_ADS_FETCH_ASSETS isEqualToString:operation])
    {
        [PokktInGameAd fetchIGAAssets:params];
    }
    else if ([IN_GAME_ADS_UPDATE_IGA_DATA isEqualToString:operation])
    {
        [PokktAds updateIGAData:params];
    }
    else if ([IN_GAME_ADS_TRACK_FU_CLICK isEqualToString:operation])
    {
        [PokktDebugger printLog:@"InGameAd_trackFUClick - TODO"];
        // TODO
    }
    
    
    // Analytics APIs
    else if ([ANALYTICS_SET_DETAILS isEqualToString:operation])
    {
        [PokktAds setPokktAnalyticsDetail:getAnalyticsDetailsFromJSONString(params)];
    }
    else if ([ANALYTICS_TRACK_IAP isEqualToString:operation])
    {
        [PokktAds trackIAP:parseIAPDetails(params)];
    }
    
    
    // Debugging APIs
    else if ([DEBUGGING_SHOULD_DEBUG isEqualToString:operation])
    {
        [PokktDebugger setDebug:[params boolValue]];
    }
    else if ([DEBUGGING_SHOW_LOG isEqualToString:operation])
    {
        [PokktDebugger printLog:params];
    }
    else if ([DEBUGGING_SHOW_TOAST isEqualToString:operation])
    {
        [PokktDebugger showToast:params viewController:getRootViewController()];
    }
    else if ([DEBUGGING_EXPORT_LOG isEqualToString:operation])
    {
        [PokktDebugger exportLog:getRootViewController()];
    }
    else if ([DEBUGGING_EXPORT_LOG_TO_CLOUD isEqualToString:operation])
    {
        [PokktDebugger printLog:@"Debugging_exportLogToCloud is not implemented in iOS!"];
    }
    else if ([DEBUGGING_SHOW_TOAST isEqualToString:operation])
    {
        [PokktDebugger showToast:params viewController:getRootViewController()];
    }
    
    
    // Data access consent
    else if ([SET_DATA_ACCESS_CONSENT isEqualToString:operation])
    {
        [PokktAds setPokktConsentInfo:getConsentInfoFromJSONString(params)];
    }
    
    
    else
    {
        NSLog(@"[POKKT-NATIVE] Unknown operation requested: %@", operation);
    }
}


/***
 * Video Ad Delegate
 ***/

- (void) videoAdCachingCompleted:(NSString *) screenName isRewarded: (BOOL)isRewarded reward: (float)reward
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"REWARD"] = [[NSNumber numberWithFloat:reward] stringValue];
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, additionalParams);
    
    notifyFramework(EVENT_VIDEO_AD_CACHING_COMPLETED, params);
}

- (void) videoAdCachingFailed: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"ERROR_MESSAGE"] = errorMessage;
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, additionalParams);
    
    notifyFramework(EVENT_VIDEO_AD_CACHING_FAILED, params);
}

- (void) videoAdCompleted: (NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_VIDEO_AD_COMPLETED, params);
}

- (void) videoAdDisplayed: (NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_VIDEO_AD_DISPLAYED, params);
}

- (void) videoAdGratified: (NSString *)screenName reward:(float)reward
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"REWARD"] = [[NSNumber numberWithFloat:reward] stringValue];
    NSString *params = getReturnParamsFromValues(screenName, true, additionalParams);
    
    notifyFramework(EVENT_VIDEO_AD_GRATIFIED, params);
}

- (void) videoAdSkipped: (NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_VIDEO_AD_SKIPPED, params);
}

- (void) videoAdClosed:(NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_VIDEO_AD_CLOSED, params);
}

- (void) videoAdFailedToShow: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"ERROR_MESSAGE"] = errorMessage;
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, additionalParams);
    
    notifyFramework(EVENT_VIDEO_AD_FAILED_TO_SHOW, params);
}




/***
 * Interstitial Delegate
 ***/

- (void) interstitialCachingCompleted:(NSString *) screenName isRewarded: (BOOL)isRewarded reward: (float)reward
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"REWARD"] = [[NSNumber numberWithFloat:reward] stringValue];
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, additionalParams);
    
    notifyFramework(EVENT_INTERSTITIAL_CACHING_COMPLETED, params);
}

- (void) interstitialCachingFailed: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"ERROR_MESSAGE"] = errorMessage;
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, additionalParams);
    
    notifyFramework(EVENT_INTERSTITIAL_CACHING_FAILED, params);
}

- (void) interstitialCompleted: (NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_INTERSTITIAL_COMPLETED, params);
}

- (void) interstitialDisplayed: (NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_INTERSTITIAL_DISPLAYED, params);
}

- (void) interstitialGratified: (NSString *)screenName reward:(float)reward
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"REWARD"] = [[NSNumber numberWithFloat:reward] stringValue];
    NSString *params = getReturnParamsFromValues(screenName, true, additionalParams);
    
    notifyFramework(EVENT_INTERSTITIAL_GRATIFIED, params);
}

- (void) interstitialSkipped: (NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_INTERSTITIAL_SKIPPED, params);
}

- (void) interstitialClosed:(NSString *)screenName isRewarded: (BOOL)isRewarded
{
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, nil);
    
    notifyFramework(EVENT_INTERSTITIAL_CLOSED, params);
}

- (void) interstitialFailedToShow: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"ERROR_MESSAGE"] = errorMessage;
    NSString *params = getReturnParamsFromValues(screenName, isRewarded, additionalParams);
    
    notifyFramework(EVENT_INTERSTITIAL_FAILED_TO_SHOW, params);
}




/***
 * Banner Ads Delegate
 ***/

- (void)bannerLoaded:(NSString *)screenName
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"SCREEN_NAME"] = screenName;
    NSString *params = stringifyJSONDictionary(additionalParams);
    
    notifyFramework(EVENT_BANNER_LOADED, params);
}

- (void)bannerLoadFailed:(NSString *)screenName errorMessage:(NSString *)errorMessage
{
    NSMutableDictionary *additionalParams = [[NSMutableDictionary alloc] init];
    additionalParams[@"SCREEN_NAME"] = screenName;
    additionalParams[@"ERROR_MESSAGE"] = errorMessage;
    NSString *params = stringifyJSONDictionary(additionalParams);
    
    notifyFramework(EVENT_BANNER_LOAD_FAILED, params);
}




/***
 * IGA Delegate
 ***/

- (void)onIGAAssetsReady:(NSString *)screenName igaAssets:(NSString *)igaAssets
{
    notifyFramework(EVENT_IGA_ASSETS_READY, igaAssets);
}

- (void)onIGAAssetsFailed:(NSString *)screenName errorMessage:(NSString *)errorMessage
{
    notifyFramework(EVENT_IGA_ASSETS_FAILED, errorMessage);
}

@end




/***
 *
 * UTILITIES
 *
 ***/

UIViewController* getRootViewController()
{
    UIViewController *viewController = [[[UIApplication sharedApplication] keyWindow] rootViewController];
    return viewController;
}

NSDictionary* extractJSON(NSString* jsonString)
{
    NSError* jsonError;
    NSData* jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary* jsonObject = [NSJSONSerialization JSONObjectWithData:jsonData
                                                               options:NSJSONReadingMutableContainers
                                                                 error:&jsonError];
    return jsonObject;
}

PokktIAPDetails* parseIAPDetails(NSString* detailsString)
{
    NSDictionary* jsonObject = extractJSON(detailsString);
    
    PokktIAPDetails* details = [[PokktIAPDetails alloc] init];
    
    details.productIdentifier = jsonObject[@"productId"];
    details.productPrice = [NSString stringWithFormat:@"%d", [jsonObject[@"price"] intValue]];
    details.productCurrency = jsonObject[@"currencyCode"];
    details.productTitle = jsonObject[@"title"];
    details.productDescription = jsonObject[@"description"];
    
    // TODO
    //detail.setPurchaseData(jsonObject.optString("purchaseData"));
    //detail.setPurchaseSignature(jsonObject.optString("purchaseSignature"));
    //detail.setPurchaseStore(IAPStoreType.valueOf(jsonObject.optString("purchaseStore")));
    
    return details;
}

PokktAdPlayerViewConfig* getAdPlayerViewConfigFromJSONString(NSString* configString)
{
    NSDictionary* jsonObject = extractJSON(configString);
    
    PokktAdPlayerViewConfig* config = [[PokktAdPlayerViewConfig alloc] init];
    
    config.shouldAllowSkip = [jsonObject[@"shouldAllowSkip"] boolValue];
    config.defaultSkipTime = [jsonObject[@"defaultSkipTime"] floatValue];
    config.skipConfirmMessage = jsonObject[@"skipConfirmMessage"];
    config.shouldAllowMute = [jsonObject[@"shouldAllowMute"] boolValue];
    config.shouldConfirmSkip = [jsonObject[@"shouldSkipConfirm"] boolValue];
    config.shouldCollectFeedback = [jsonObject[@"shouldCollectFeedback"] boolValue];
    config.isAudioEnabled = [jsonObject[@"isAudioEnabled"] boolValue];
    config.skipConfirmYesLabel = jsonObject[@"skipConfirmYesLabel"];
    config.skipConfirmNoLabel = jsonObject[@"skipConfirmNoLabel"];
    config.skipTimerMessage = jsonObject[@"skipTimerMessage"];
    config.incentiveMessage = jsonObject[@"incentiveMessage"];
    
    return config;
}

PokktUserInfo* getUserDetailsFromJSONString(NSString* stringData)
{
    NSDictionary *jsonObject = extractJSON(stringData);
    
    PokktUserInfo *info = [[PokktUserInfo alloc] init];
    
    info.name = jsonObject[@"name"];
    info.age = jsonObject[@"age"];
    info.sex = jsonObject[@"sex"];
    info.mobileNumber = jsonObject[@"mobileNo"];
    info.emailAddress = jsonObject[@"emailAddress"];
    info.location = jsonObject[@"location"];
    info.birthday = jsonObject[@"birthday"];
    info.maritalStatus = jsonObject[@"maritalStatus"];
    info.facebookId = jsonObject[@"facebookId"];
    info.twitterHandle = jsonObject[@"twitterHandle"];
    info.educationInformation = jsonObject[@"education"];
    info.nationality = jsonObject[@"nationality"];
    info.employmentStatus = jsonObject[@"employment"];
    info.maturityRating = jsonObject[@"maturityRating"];
    
    return info;
}

PokktAnalyticsDetails* getAnalyticsDetailsFromJSONString(NSString* stringData)
{
    NSDictionary *jsonObject = extractJSON(stringData);
    
    PokktAnalyticsDetails *details = [[PokktAnalyticsDetails alloc] init];
    
    // analytics
    details.googleTrackerID = jsonObject[@"googleAnalyticsID"];
    details.mixPanelTrackerID = jsonObject[@"mixPanelProjectToken"];
    details.flurryTrackerID = jsonObject[@"flurryApplicationKey"];
    
    if (jsonObject[@"selectedAnalyticsType"] != nil && ![jsonObject[@"selectedAnalyticsType"] isEqual:@""])
    {
        if ([jsonObject[@"selectedAnalyticsType"] isEqual:@"FLURRY"])
            details.eventType = FLURRY_ANALYTICS;
        
        else if ([jsonObject[@"selectedAnalyticsType"] isEqual:@"GOOGLE_ANALYTICS"])
            details.eventType = GOOGLE_ANALYTICS;
        
        else if ([jsonObject[@"selectedAnalyticsType"] isEqual:@"MIXPANNEL"])
            details.eventType = MIXPANNEL_ANALYTICS;
    }
    
    return details;
}

PokktConsentInfo* getConsentInfoFromJSONString(NSString* stringData)
{
    NSDictionary *jsonObject = extractJSON(stringData);
    
    PokktConsentInfo* info = [[PokktConsentInfo alloc] init];
    [info setIsGDPRApplicable:[jsonObject[@"GDPRApplicable"] boolValue]];
    [info setIsGDPRConsentAvailable:[jsonObject[@"GDPRConsentAvailable"] boolValue]];
    
    [PokktDebugger printLog:[NSString stringWithFormat:@"GDPRApplicable: %@, GDPRConsentAvailable: %@",
                             info.isGDPRApplicable ? @"true" : @"false", info.isGDPRConsentAvailable ? @"true" : @"false"]];
    
    return info;
}

NSString* getReturnParamsFromValues(NSString* _Nonnull screenName, bool isRewarded, NSMutableDictionary<NSString *, id> *paramList)
{
    if (!paramList)
    {
        paramList = [[NSMutableDictionary alloc] init];
    }
    
    paramList[@"SCREEN_NAME"] = screenName;
    paramList[@"IS_REWARDED"] = isRewarded ? @"true" : @"false";;
    
    NSString *params = stringifyJSONDictionary(paramList);
    return params;
}


NSString* stringifyJSONDictionary(NSDictionary *jsonObject)
{
    NSError *jsonError;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:jsonObject options:0 error:&jsonError];
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return jsonString;
}

NSLayoutConstraint* getLayoutConstraint(UIView* view, NSLayoutAttribute layoutAttribute1, NSLayoutRelation layoutRelation, UIView* toItem, NSLayoutAttribute layoutAttribute2, CGFloat multiplier, CGFloat constant)
{
    return [NSLayoutConstraint constraintWithItem:view
                                        attribute:layoutAttribute1
                                        relatedBy:layoutRelation
                                           toItem:toItem
                                        attribute:layoutAttribute2
                                       multiplier:multiplier
                                         constant:constant];
}

void updateBannerLocation(UIView* view, NSLayoutConstraint* leading, NSLayoutConstraint* bottom, NSLayoutConstraint* trailing, NSLayoutConstraint* height)
{
    [view addConstraint:trailing];
    [view.superview addConstraint:bottom];
    [view.superview addConstraint:leading];
    [view addConstraint:height];
}

//Top
void topLeft(UIView* view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeLeft, NSLayoutRelationEqual, view.superview, NSLayoutAttributeLeft, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeTop, NSLayoutRelationEqual, view.superview, NSLayoutAttributeTop, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

void topRight(UIView * view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeRight, NSLayoutRelationEqual, view.superview, NSLayoutAttributeRight, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeTop, NSLayoutRelationEqual, view.superview, NSLayoutAttributeTop, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

void topCenter(UIView * view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeCenterX, NSLayoutRelationEqual, view.superview, NSLayoutAttributeCenterX, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeTop, NSLayoutRelationEqual, view.superview, NSLayoutAttributeTop, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

// Middle
void middleLeft(UIView* view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeLeft, NSLayoutRelationEqual, view.superview, NSLayoutAttributeLeft, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeCenterY, NSLayoutRelationEqual, view.superview, NSLayoutAttributeCenterY, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

void middleRight(UIView * view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeRight, NSLayoutRelationEqual, view.superview, NSLayoutAttributeRight, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeCenterY, NSLayoutRelationEqual, view.superview, NSLayoutAttributeCenterY, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

void middleCenter(UIView * view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeCenterX, NSLayoutRelationEqual, view.superview, NSLayoutAttributeCenterX, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeCenterY, NSLayoutRelationEqual, view.superview, NSLayoutAttributeCenterY, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

//Bottom
void bottomLeft(UIView * view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeLeading, NSLayoutRelationEqual, view.superview, NSLayoutAttributeLeading, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeBottom, NSLayoutRelationEqual, view.superview, NSLayoutAttributeBottom, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

void bottomRight(UIView * view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeRight, NSLayoutRelationEqual, view.superview, NSLayoutAttributeRight, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeBottom, NSLayoutRelationEqual, view.superview, NSLayoutAttributeBottom, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}

void bottomCenter(UIView * view)
{
    NSLayoutConstraint* leading = getLayoutConstraint(view, NSLayoutAttributeCenterX, NSLayoutRelationEqual, view.superview, NSLayoutAttributeCenterX, 1.0f, 0.f);
    NSLayoutConstraint* bottom = getLayoutConstraint(view, NSLayoutAttributeBottom, NSLayoutRelationEqual, view.superview, NSLayoutAttributeBottom, 1.0f, 0.f);
    NSLayoutConstraint* trailing = getLayoutConstraint(view, NSLayoutAttributeWidth, NSLayoutRelationEqual, nil, NSLayoutAttributeWidth, 1.0f, 320);
    NSLayoutConstraint* height = getLayoutConstraint(view, NSLayoutAttributeHeight, NSLayoutRelationEqual, nil, NSLayoutAttributeNotAnAttribute, 0, 50);
    
    updateBannerLocation(view, leading, bottom, trailing, height);
}




/***
 *
 *  POKKT-TO-FRAMEWORK
 *
 ***/

void notifyFramework(NSString *operation, NSString *param)
{
    // unity receiver game-object
    NSString *toPokktAdsGO = @"PokktAdsGO";
    
    UnitySendMessage([toPokktAdsGO UTF8String],
                     [operation UTF8String],
                     [param UTF8String]);
}


/***
 *  Framework Specifics
 ***/

