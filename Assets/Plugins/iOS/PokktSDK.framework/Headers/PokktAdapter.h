
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "PokktModels.h"
#import "PokktAds.h"
//#import "PokktAdsDelegates.h"



@interface PokktNetworkModel : NSObject

@property (nonatomic, retain) NSString * name;

@property (nonatomic, retain) NSString * className;

@property (nonatomic, retain) NSDictionary  * customData;

@property (nonatomic, retain) NSString  * networkID;

@property (nonatomic, retain) NSString  * intgrationType;

@property (nonatomic, retain) NSString * allotedCacheTime;

@property (nonatomic, assign) int adFormat;

@property (nonatomic, assign) int responseFormat;

@property (nonatomic, retain) NSString * requestUrl;

@property (nonatomic) BOOL isSupportRewarded;

@property (nonatomic) BOOL isSupportNonRewarded;

@property (nonatomic, assign) int type;

@property (nonatomic, retain) NSString *comscorePartnerID;


- (void)initNetwork:(NSDictionary *)dictionary;


@end

@protocol PokktAdNetwork <NSObject>

@required

- (BOOL) checkAdFormatSupport:(PokktAdConfig *)adConfig;

- (void) initNetworkWithNetworkModel: (PokktNetworkModel *)networkModel;

- (PokktNetworkModel *)getNetworkModel;

- (void) cacheAd: (PokktAdConfig *)adConfig;

- (void) showAd: (PokktAdConfig *)adConfig viewController:(UIViewController *)viewController;

- (void) checkAdAvailability: (PokktAdConfig *)adConfig;

- (void) setCacheTimedOut: (PokktAdConfig *)adConfig;

- (BOOL) isAdCached: (PokktAdConfig *)adConfig;

- (void) notifyDataConsentChanged:(PokktConsentInfo *)consentInfo;

@optional

- (void) fetchAd:(PokktAdConfig *)adConfig
      withAdView:(UIView *)bannerView
withRootViewController:(UIViewController *)rootViewController
    withDelegate:(id<PokktBannerDelegate>)bannerDelegate
       onSuccess:(void(^)(id))successCallback
       onFailure:(void(^)(id))failureHandler;

@end


@interface PokktNetworkDelegate : NSObject

+ (void)didFinishedAdDownload: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig rewardValue: (float)reward
                 downLoadTime: (NSString *)downloadTime;

+ (void)didFailedAdDownload:(PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig errorMessage:(NSString *)errorMsg;

+ (void)onAdCompleted: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig;

+ (void)onAdDisplayed: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig;

+ (void)onAdGratified: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig rewardPoint: (float)reward;

+ (void)onAdSkipped: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig;

+ (void)onAdClosed: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig;

+ (void)didFailedToShowAd: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig;

+ (void)onAdAvailabilityStatus: (PokktNetworkModel *)networkModel adConfig: (PokktAdConfig *)adConfig isAdAvailable:(BOOL)isAdAvailable;
@end

