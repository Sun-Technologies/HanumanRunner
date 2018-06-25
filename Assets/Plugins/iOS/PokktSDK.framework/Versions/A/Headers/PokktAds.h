#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "PokktModels.h"


@protocol PokktVideoAdsDelegate <NSObject>

@optional

- (void)videoAdCachingCompleted: (NSString *)screenName isRewarded: (BOOL)isRewarded reward: (float)reward;

- (void)videoAdCachingFailed: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage;

- (void)videoAdCompleted: (NSString *)screenName isRewarded: (BOOL)isRewarded;

- (void)videoAdDisplayed: (NSString *)screenName isRewarded: (BOOL)isRewarded;

- (void)videoAdGratified: (NSString *)screenName reward:(float)reward;

- (void)videoAdSkipped: (NSString *)screenName isRewarded: (BOOL)isRewarded;

- (void)videoAdClosed:(NSString *)screenName isRewarded: (BOOL)isRewarded;

- (void)videoAdAvailabilityStatus: (NSString *)screenName isRewarded: (BOOL)isRewarded isAdAvailable: (BOOL)isAdAvailable;

- (void)videoAdFailedToShow: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage;

@end

@protocol PokktOutStreamsAdsDelegate <NSObject>

@optional


- (void)outStreamAdCompleted: (NSString *)screenName;

- (void)outStreamAdDisplayed: (NSString *)screenName;

- (void)outStreamAdReady:(NSString *)screenName;

- (void)outStreamAdFailedToShow: (NSString *)screenName errorMessage: (NSString *)errorMessage;

@end


@protocol PokktInterstitialDelegate <NSObject>

@optional

- (void)interstitialCachingCompleted: (NSString *)screenName isRewarded: (BOOL)isRewarded reward: (float)reward;

- (void)interstitialCachingFailed: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage;

- (void)interstitialCompleted: (NSString *)screenName isRewarded: (BOOL)isRewarded; //TODO: Requirement check

- (void)interstitialDisplayed: (NSString *)screenName isRewarded: (BOOL)isRewarded;

- (void)interstitialGratified: (NSString *)screenName reward:(float)reward;

- (void)interstitialSkipped: (NSString *)screenName isRewarded: (BOOL)isRewarded; //TODO: Requirement check

- (void)interstitialClosed:(NSString *)screenName isRewarded: (BOOL)isRewarded;

- (void)interstitialAvailabilityStatus: (NSString *)screenName isRewarded: (BOOL)isRewarded isAdAvailable: (BOOL)isAdAvailable;

- (void)interstitialFailedToShow: (NSString *)screenName isRewarded: (BOOL)isRewarded errorMessage: (NSString *)errorMessage;

@end


@protocol PokktBannerDelegate <NSObject>

@optional

- (void)bannerLoaded:(NSString *)screenName;

- (void)bannerLoadFailed:(NSString *)screenName errorMessage:(NSString *)errorMessage;

@end


@protocol PokktIGADelegate <NSObject>

@optional

- (void)onIGAAssetsReady: (NSString*)screenName igaAssets:(NSString*)igaAssets;

- (void)onIGAAssetsFailed: (NSString*)screenName errorMessage:(NSString *)errorMessage;

@end



@interface PokktBannerView : UIView <PokktBannerDelegate>

- (void)loadBanner:(NSString *)screenName rootViewController:(UIViewController *)rootViewController;
- (void)destroyBanner;

@end




@interface PokktOutStreamAds : NSObject

/*!
 @abstract
 Set the PokktOutStreamAds Delegate
 
 @param outStreamDelegate outStream Delegate
 
 @discussion Call this before show outStream ads. SDK uses this delegate to communicate to your app and notifies the status of ads.
 
 */
+ (void)setPokktOutStreamAdsDelegate: (id <PokktOutStreamsAdsDelegate>)outStreamDelegate;

/*!
 @abstract
 This method will be used to load outStream ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 
 @param container Container in which you will display the ad. Must not be `nil` and added to your scrollview.
 
 @param scrollView A scroll-view object on which container is added, pass same scrollview.
 */

+(void)loadOutstreamAd:(NSString*) screenName placeHolder:(UIView *)container scrollView:(UIScrollView *)scrollView;

/*!
 @abstract
 This method will be used to load outStream ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 
 @param indexPath An index path locating a row in tableView where to insert Ad. Must not be `nil`.
 
 @param tableview A UITableView object on which container is added..
 */

+(void)loadOutstreamAd:(NSString*) screenName atIndexPath:(NSIndexPath *)indexPath tableView:(UITableView *)tableView;

/*!
 @abstract
 This method will be used to load outStream ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 
 @param webView A UIWebView object on which container is added.
 */
+(void)loadOutstreamAd:(NSString*) screenName inWebView:(UIWebView *)webView;

/*!
 @abstract
 This method will will inform that its parent view controller did disappear.
 
 @param screenName Screen name for which ad is requested.
 
 @param container Container in which your ad is displaying. Must not be `nil`.
 
 @discussion This method has to be called in your view controller -viewDidDisappear: method and if you are requesting for full screen ad.
 */


+(void)initAgent:(NSString *)screenName;
+(BOOL)checkAndHandleRequest:(NSURLRequest *)request forScreenName:(NSString *)screenName;

+(void)disappearScrollViewOutStreamAd:(NSString *)screenName inContainer:(UIView *)container;

/*!
 @abstract
 This method will will inform that its parent view controller did disappear.
 
 @param screenName Screen name for which ad is requested.
 
 @param tableView tableView in which your ad is displaying. Must not be `nil`.
 
 @discussion This method has to be called in your view controller -viewDidDisappear: method and if you are requesting for full screen ad.
 */

+(void)disappearTableViewOutStreamAd:(NSString *)screenName inContainer:(UITableView *)tableView;

/*!
 @abstract
 This method will will inform that its parent view controller did disappear.
 
 @param screenName Screen name for which ad is requested.
 
 @param webView UIWebView in which your ad is displaying. Must not be `nil`.
 
 @discussion This method has to be called in your view controller -viewDidDisappear: method and if you are requesting for full screen ad.
 */

+(void)disappearWebViewOutStreamAd:(NSString *)screenName inContainer:(UIWebView *)webView;

@end

@interface PokktVideoAds : NSObject

/*!
 @abstract
 Set the PokktVideoAds Delegate
 
 @param videoDelegate video Delegate
 
 @discussion Call this before cache and show video ads. SDK uses this delegate to communicate to your app and notifies the status of ads, such as an ad has been cached or failed, ad watched
 completely or skipped and ad availability status.
 
 */

+ (void)setPokktVideoAdsDelegate: (id <PokktVideoAdsDelegate>)videoDelegate;

/*!
 @abstract
 This method will be used to cache rewarded video ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 */

+(void) cacheRewarded:(NSString*) screenName;

/*!
 @abstract
 This method will be used to show rewarded video ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 
 @param viewController The UIViewController in which you will display the ad. Must not be `nil`. This is required so that the ad can present additional view controllers if the user interacts with it.
 */

+(void) showRewarded:(NSString*) screenName withViewController:(UIViewController *) viewController;

/*!
 @abstract
 This method will be used to cache non rewarded video ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 */

+(void) cacheNonRewarded:(NSString*) screenName;

/*!
 @abstract
 This method will be used to show non rewarded video ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 
 @param viewController The UIViewController in which you will display the ad. Must not be `nil`. This is required so that the ad can present additional view controllers if the user interacts with it.
 */

+(void) showNonRewarded:(NSString*) screenName withViewController:(UIViewController *) viewController;

/*!
 @abstract
 This method can be used by developer to check if there is ad cached for a particular screenName and is available to show.
 
 @param screenName Screen name for which ad is requested.
 
 @param isRewarded Ad is rewarded or not.
 */

+(BOOL) isAdCached:(NSString *)screenName isRewarded:(BOOL)isRewarded;

@end


@interface PokktInterstitial : NSObject

/*!
 @abstract
 Set the PokktInterstitialAds Delegate
 
 @param interstitialDelegate Interstitial Delegate
 
 @discussion Call this before cache and show Interstitial ads. SDK uses this delegate to communicate to your app and notifies the status of ads, such as an ad has been cached or failed, ad watched
 completely or skipped and ad availability status.
 
 */

+ (void)setPokktInterstitialDelegate: (id <PokktInterstitialDelegate>)interstitialDelegate;

/*!
 @abstract
 This method will be used to cache rewarded interstitial ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 */

+(void) cacheRewarded:(NSString*) screenName;

/*!
 @abstract
 This method will be used to show rewarded interstitial ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 
 @param viewController The UIViewController in which you will display the ad. Must not be `nil`. This is required so that the ad can present additional view controllers if the user interacts with it.
 */

+(void) showRewarded:(NSString*) screenName withViewController:(UIViewController *) viewController;

/*!
 @abstract
 This method will be used to cache non rewarded interstitial ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 */

+(void) cacheNonRewarded:(NSString*) screenName;

/*!
 @abstract
 This method will be used to show non rewarded interstitial ad of a particular screenname.
 
 @param screenName Screen name for which ad is requested.
 
 @param viewController The UIViewController in which you will display the ad. Must not be `nil`. This is required so that the ad can present additional view controllers if the user interacts with it.
 */

+(void) showNonRewarded:(NSString*) screenName withViewController:(UIViewController *) viewController;

/*!
 @abstract
 This method can be used by developer to check if there is ad cached for a particular screenName and is available to show.
 
 @param screenName Screen name for which ad is requested.
 
 @param isRewarded Ad is rewarded or not.
 */
+(BOOL) isAdCached:(NSString *)screenName isRewarded:(BOOL)isRewarded;

@end

@interface PokktBanner : NSObject

/*!
 @abstract
 Set the PokktBannerAd Delegate
 
 @param bannerDelegate Banner Delegate
 
 @discussion Call this before load Banner ads. SDK uses this delegate to communicate to your app and notifies the status of ads, such as an ad has been cached or failed.
 */

+ (void)setPokktBannerDelegate: (id <PokktBannerDelegate>)bannerDelegate;

/*!
 @abstract
 initWithBannerAdSize will create a banner view and will display on screen.
 
 @param adSize size of banner.
 */

+ (PokktBannerView *)initWithBannerAdSize:(CGRect)adSize;

/*!
 @abstract
 Load banner will load the banner and will display on screen.
 
 @param bannerView is a view on that banner will display. It must be added on your viewController before loading banner.
 
 @param viewController The UIViewController in which you will display the banner. Must not be `nil`. This is required so that the ad can present additional view controllers if the user interacts with it.
 
 @param screenName  screenName Screen name for which ad is requested.
 
 */

+ (void)loadBanner:(PokktBannerView *)bannerView withScreenName:(NSString *)screenName rootViewContorller:(UIViewController *)viewController;

/*!
 @abstract
 Use destroyBanner method use to remove your bannerContainer.
 
 @param bannerContainer your view which is used to show banner ad
 */

+ (void)destroyBanner:(PokktBannerView *)bannerContainer;

@end

@interface PokktInGameAd : NSObject

/*!
 @abstract
 Set the PokktIGADelegate Delegate
 
 @param igaDelegate IGA Delegate
 
 */

+ (void) setIGADelegate:(id<PokktIGADelegate>)igaDelegate;

+ (void) fetchIGAAssets:(NSString*)screenName;

@end

@interface PokktDebugger : NSObject

/*!
 @abstract
 Returns the debug enable status of the POKKT SDK.
 */

+ (BOOL)isDebugEnabled;

/*!
 @abstract
 This method enables/disables the debug logging done by SDK.
 
 by default, logging is off.
 
 @param isDebug true if debug on else false
 */

+ (void)setDebug:(BOOL)isDebug;

/*!
 @abstract
 This exposed method let developers export POKKT SDK log.
 
 @param viewController The UIViewController in which you will see all installed app on your device to export log file . Must not be `nil`.
 */

+ (void)exportLog:(UIViewController *)viewController;

/*!
 @abstract
 This is an utility method to show toast.
 
 @param viewController The UIViewController in which you will display the toast message. Must not be `nil`.
 @param message Message to show
 */

+ (void)showToast:(NSString *)message viewController:(UIViewController *)viewController;

/*!
 @abstract
 This is an method to print log.
 
 @param message Message to show
 */

+(void)printLog:(NSString *)message;

@end

@interface PokktConsentInfo : NSObject

/*!
 @abstract
 Set GDPRApplicable true or false. Default is false.
 **/

@property (nonatomic) BOOL isGDPRApplicable;

/*!
 @abstract
 Set GDPRConsentAvailable true or false. Default is false.
 
 **/

@property (nonatomic) BOOL isGDPRConsentAvailable;

/*!
 @abstract
 This is unique id which will be given to user and generated by developer/publisher.
 Developer/publisher can used this Id to identify their user for the purposes like S2S integration to gratify user
 **/

@end

@interface PokktAds : NSObject

/*!
 @abstract
 This method is used to set ConsentInfo for GDPR
 
 @param consentObject.
 
 */

+(void) setPokktConsentInfo:(PokktConsentInfo *)consentObject;

/*!
 @abstract
 This method is used to get ConsentInfo
 
 */


+(PokktConsentInfo *)getPokktConsentInfo;

/*!
 @abstract
 This method is used to set applicationID and security key
 
 @param appId Application Id which you will get from Pokkt dashboard when you create application their.
 
 @param securityKey Security Key which you will get from Pokkt dashboard when you create application their.
 */

+ (void) setPokktConfigWithAppId:(NSString*) appId securityKey:(NSString*) securityKey;

/*!
 @abstract
 This method is used to set extra parameters to POKKT SDK
 
 @param extraParam You can set extra parameters to POKKT  SDK, to be passed back to your server via POKKT server callback. These Extra parameters will be in key-value pair.The key must be alphanumeric value
 
 */

+(void)setCallbackExtraParam:(NSDictionary *)extraParam;

/*!
 @abstract
 This is unique id which will be given to user and generated by developer/publisher.
 Developer/publisher can used this Id to identify their user for the purposes like S2S integration to gratify user
 
 **/

+ (void) setThirdPartyUserId:(NSString*) userId;

// optional parameters
/*!
 @abstract
 This is optional, set user detail like name , age , sex etc.
 
 **/

+ (void)setUserDetails: (PokktUserInfo *)userInfo;

/*!
 @abstract
 Returns the version of the POKKT SDK.
 */

+ (NSString *)getSDKVersion;

/*!
 @abstract
 This function fetch all notification from Pokkt server. All fetched notification will fire on date which you set on POKKT Dashboard.
 
 */

+ (void)callBackgroundTaskCompletedHandler:(void (^)(UIBackgroundFetchResult result))completionHandler;

/*!
 @abstract
 This function will send recieved notification data to POKKT server.
 
 @param localNotification received notification in AppDelegate.m class. You need to pass    same.
 
 */

+ (void)inAppNotificationEvent:(UILocalNotification *)localNotification;

/*!
 @abstract
 This method is called by developer to register notification.
 
 @param aSelector Selector method to register notification.
 
 @param application {@link UIApplication}
 */

+ (void)setupNotification:(SEL)aSelector application:(UIApplication*)application;

// optional parameters
/*!
 @abstract
 This is optional, set player detail like skip time, skip message, shouldAllowSkip , skipConfirmMessage etc.
 
 **/

+ (void)setPokktAdPlayerViewConfig: (PokktAdPlayerViewConfig *)adPlayerViewConfig;

+ (void)setPokktAnalyticsDetail:(PokktAnalyticsDetails *)analyticsDetail;

+ (void)trackIAP:(PokktIAPDetails *)inAppPurchaseDetails;

+ (void) updateIGAData:(NSString*)igaAnalyticsData;

@end
