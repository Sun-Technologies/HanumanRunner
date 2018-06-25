
#import <Foundation/Foundation.h>

typedef enum : int
{
    VIDEO = 0,
    BANNER = 1,
    INTERSTITIAL = 3,
    INGAME_BRANDING = 4,
} AdFormatType;

typedef enum : int
{
    IN_NONE = 0,
    IN_STREAM = 1,
    IN_BANNER = 2,
    IN_ARTICLE = 3,
    IN_FEED = 4,
    INTERSTITIAL_SLIDER_FLOATING = 5
    
} OutStreamFormatType;

typedef enum : int {
    NONE,
    FLURRY_ANALYTICS,
    GOOGLE_ANALYTICS,
    MIXPANNEL_ANALYTICS,
    FABRIC_ANALYTICS
} AnalyticsType;
/**
 * This is ad request configuration. Developer should supply this in almost every ad related method.
 */

@interface PokktAdConfig : NSObject <NSCopying>

/*! @abstract screenName Screen name for which ad is requested. */

@property (nonatomic, retain) NSString *screenName;

/*! @abstract isRewarded rewarded or non rewarded ad request */

@property (nonatomic, assign) BOOL isRewarded;

/*! @abstract adFormat is a type for ad such as  0:video, 1: banner, 3: interstitial , default is 0 */

@property (nonatomic, assign) int adFormat;

@property (nonatomic, assign) int OutStreamAdFormat;

- (NSString *) getKey;

@end

@interface PokktAdPlayerViewConfig : NSObject

/*! @abstract defaultSkipTime duration to skip ad, default is 0 */

@property (nonatomic, assign) int defaultSkipTime;

/*! @abstract shouldAllowSkip set YES or NO to skip ad, if 'YES' it will allow user to skip ad , default is YES */

@property (nonatomic, assign) BOOL shouldAllowSkip;

/*! @abstract shouldAllowMute set YES or NO to skip ad,if 'YES' it will allow user to Mute ad , default is YES */

@property (nonatomic, assign) BOOL shouldAllowMute;

/*! @abstract shouldConfirmSkip set YES or NO to skip ad, default is YES */

@property (nonatomic, assign) BOOL shouldConfirmSkip;

/*! @abstract skipConfirmMessage custom message for skip alert*/

@property (nonatomic, strong) NSString * skipConfirmMessage;

/*! @abstract skipConfirmYesLabel custom text for skip alert 'YES' Label*/

@property (nonatomic, strong) NSString * skipConfirmYesLabel;

/*! @abstract skipConfirmNoLabel custom text for skip alert 'NO' Label*/

@property (nonatomic, strong) NSString * skipConfirmNoLabel;

/*! @abstract skipTimerMessage custom message for remianing time to skip ad*/

@property (nonatomic, strong) NSString * skipTimerMessage;

/*! @abstract incentiveMessage custom message for remianing time to get reward*/

@property (nonatomic, strong) NSString * incentiveMessage;

/*! @abstract shouldCollectFeedback set YES or NO , If Yes it will allow user to send feedback for ad.*/

@property (nonatomic, assign) BOOL shouldCollectFeedback;

/*! @abstract isAudioEnabled set YES or NO to mute ad,if 'YES' ad will be mute. , default is NO */

@property (nonatomic, assign) BOOL isAudioEnabled;

@end

@interface PokktIAPDetails : NSObject
{
    
}

@property (nonatomic, retain) NSString * productCurrency;
@property (nonatomic, retain) NSString * productDescription;
@property (nonatomic, retain) NSString * productIdentifier;
@property (nonatomic, retain) NSString * productPrice;
@property (nonatomic, retain) NSString * productTitle;

@end

@interface PokktUserInfo : NSObject

@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *age;
@property (nonatomic, retain) NSString *sex;
@property (nonatomic, retain) NSString *location;
@property (nonatomic, retain) NSString *maritalStatus;
@property (nonatomic, retain) NSString *birthday;
@property (nonatomic, retain) NSString *mobileNumber;
@property (nonatomic, retain) NSString *emailAddress;
@property (nonatomic, retain) NSString *facebookId;
@property (nonatomic, retain) NSString *twitterHandle;
@property (nonatomic, retain) NSString *educationInformation;
@property (nonatomic, retain) NSString *nationality;
@property (nonatomic, retain) NSString *employmentStatus;
@property (nonatomic, retain) NSString *maturityRating;

@end

@interface PokktAnalyticsDetails : NSObject

/*!
 @abstract
 Once you create application in google developer console, you will get analytics id.
 
 **/

@property (nonatomic, retain) NSString *googleTrackerID;

/*!
 @abstract
 Once you create application in Flurry dashboard, flurry application Id will get generated.
 
 **/

@property (nonatomic, retain) NSString *flurryTrackerID;

/*!
 @abstract
 Once you create application in mixpanel dashboard, project token will get generated.
 
 **/
@property (nonatomic, retain) NSString *mixPanelTrackerID;

/*!
 @abstract
 Once you create application in Fabric dashboard, project token will get generated.
 
 **/
@property (nonatomic, retain) NSString *fabricTrackerID;

/*!
 @abstract
 For sending any measurement the Analytics class needs to be configured. To do
 this we need to following steps:
 
 self.pokktConfig.eventType = FABRIC_ANALYTICS;
 
 self.pokktConfig.eventType = MIXPANNEL_ANALYTICS;
 
 self.pokktConfig.eventType = GOOGLE_ANALYTICS;
 
 self.pokktConfig.eventType = FLURRY_ANALYTICS;
 
 By Defaults, self.pokktConfig.eventType = NONE;
 
 **/

@property (nonatomic, assign) AnalyticsType eventType;


@end

