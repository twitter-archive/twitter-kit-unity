// Copyright (C) 2017 Twitter, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#import <TwitterKit/TwitterKit.h>
#import "TwitterKitIOSWrapper.h"

/**
 *  Constants for posting responses via messages to Unity.
 */
static const char * TWTRInternalGameObject = "TwitterGameObject";
static const char * TWTRUnityAPIMethodLogInComplete = "LoginComplete";
static const char * TWTRUnityAPIMethodLogInFailed = "LoginFailed";
static const char * TWTRUnityAPIMethodRequestEmailComplete = "RequestEmailComplete";
static const char * TWTRUnityAPIMethodRequestEmailFailed = "RequestEmailFailed";
static const char * TWTRUnityAPIMethodTweetComplete = "TweetComplete";
static const char * TWTRUnityAPIMethodTweetFailed = "TweetFailed";
static const char * TWTRUnityAPIMethodTweetCancelled = "TweetCancelled";

#pragma mark - String Helpers

static char * cStringCopy(const char *string)
{
    if (string == NULL)
        return NULL;
    
    char *res = (char *)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}

static NSString * const NSStringFromCString(const char *string)
{
    if (string != NULL) {
        return [NSString stringWithUTF8String:string];
    } else {
        return nil;
    }
}

static char * serializedJSONFromNSDictionary(NSDictionary *dictionary)
{
    if (!dictionary) {
        return NULL;
    }
    
    NSData *serializedData = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:nil];
    NSString *serializedJSONString = [[NSString alloc] initWithData:serializedData encoding:NSUTF8StringEncoding];
    return cStringCopy([serializedJSONString UTF8String]);
}

#pragma mark - Helpers

// Serialization format is set to match Android so we can share serializer and deserializers in Unity
static NSDictionary * NSDictionaryFromTWTRSession(TWTRSession *session)
{
    if (!session) {
        return nil;
    }
    
    return @{ @"id": @(session.userID.longLongValue), @"auth_token": @{ @"token": session.authToken, @"secret": session.authTokenSecret }, @"user_name": session.userName };
}

static NSDictionary * NSDictionaryFromError(NSError *error)
{
    return @{ @"code": @(error.code), @"message": error.localizedDescription };
}

static TwitterUnityWrapper *_instance = [TwitterUnityWrapper sharedInstance];

@implementation TwitterUnityWrapper

+ (TwitterUnityWrapper *)sharedInstance
{
    if(_instance == nil) {
        _instance = [[TwitterUnityWrapper alloc] init];
    }

    return _instance;
}

- (id)init
{
    if(_instance != nil) {
        return _instance;
    }
    
    if ((self = [super init])) {
        _instance = self;
        UnityRegisterAppDelegateListener(self);
    }
    
    return self;
}

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString *,id> *)options {
    return [[Twitter sharedInstance] application:app openURL:url options:options];
}

- (void)onOpenURL:(NSNotification *)notification
{
    [[Twitter sharedInstance] application:[UIApplication sharedApplication]
                                  openURL:notification.userInfo[@"url"]
                                  options:notification.userInfo[@"annotation"]];
}

- (void)composerDidCancel:(TWTRComposerViewController *)controller
{
    UnitySendMessage(TWTRInternalGameObject, TWTRUnityAPIMethodTweetCancelled, "");
}

- (void)composerDidFail:(TWTRComposerViewController *)controller withError:(NSError *)error
{
    NSDictionary *errorDictionary = NSDictionaryFromError(error);
    char *serializedError = serializedJSONFromNSDictionary(errorDictionary);
    UnitySendMessage(TWTRInternalGameObject, TWTRUnityAPIMethodTweetFailed, serializedError);
}

- (void)composerDidSucceed:(TWTRComposerViewController *)controller withTweet:(TWTRTweet *)tweet
{
    char *cStringTweetId = cStringCopy([tweet.tweetID UTF8String]);
    UnitySendMessage(TWTRInternalGameObject, TWTRUnityAPIMethodTweetComplete, cStringTweetId);
}

@end

__BEGIN_DECLS

#pragma mark - TwitterKit Unity Wrapper

/**
 *  Starts TwitterKit shared instance and loads the required API keys.
 *
 *  @param consumerKey      (Required) Twitter App Consumer Key (API Key).
 *  @param consumerSecret   (Required) Twitter App Consumer Secret (API Secret).
 */
void TwitterInit(const char *consumerKey, const char *consumerSecret)
{
    [[Twitter sharedInstance] startWithConsumerKey:NSStringFromCString(consumerKey) consumerSecret:NSStringFromCString(consumerSecret)];
}

/**
 *  Convenience method for launching Twitter login.
 */
void TwitterLogIn()
{
    UIViewController *rootViewController = [[[UIApplication sharedApplication] keyWindow] rootViewController];
    [[Twitter sharedInstance] logInWithViewController:rootViewController completion:^(TWTRSession *session, NSError *error) {
        if (session) {
            NSDictionary *sessionDictionary = NSDictionaryFromTWTRSession(session);
            char *serializedSession = serializedJSONFromNSDictionary(sessionDictionary);
            UnitySendMessage(TWTRInternalGameObject, TWTRUnityAPIMethodLogInComplete, serializedSession);
            free(serializedSession);
        } else {
            NSDictionary *errorDictionary = NSDictionaryFromError(error);
            char *serializedError = serializedJSONFromNSDictionary(errorDictionary);
            UnitySendMessage(TWTRInternalGameObject, TWTRUnityAPIMethodLogInFailed, serializedError);
            free(serializedError);
        }
    }];
}

/**
 *  Convenience method for logging out the current active user session.
 */
void TwitterLogOut()
{
    TWTRSession *lastSession = [[Twitter.sharedInstance sessionStore] session];
    if (lastSession) {
        [[Twitter.sharedInstance sessionStore] logOutUserID:lastSession.userID];
    }
}

/**
 *  Convenience method for retrieving the active user session.
 *
 *  @warning Returns a newly allocated const char * instance.
 */
const char * TwitterSession() {
    TWTRSession *userSession = [[Twitter.sharedInstance sessionStore] session];
    return serializedJSONFromNSDictionary(NSDictionaryFromTWTRSession(userSession));
}

/**
 *  Convenience method for showing the Tweet composer with an app card preview.
 *
 *  @param userID          (Required) ID of the logged-in user to be composing the Tweet.
 *  @param imageURI        (Optional) URI to promo image stored on disk.
 *  @param text            (Optional) Text to pre-populate the Tweet with.
 *  @param hashtags        (Optional) An array of hashtags to prepopulate the composer with.
 *  @param hashtagCount    (Optional) The number of hashtags contained in the hashtags array.
 */
void TwitterCompose(const char *userID, const char *imageURI, const char *text, const char *hashtags[], int hashtagCount)
{
    if (!userID) {
        NSLog(@"Missing required parameter userID to compose a Tweet");
        return;
    }

    UIImage *image = [[UIImage alloc] initWithContentsOfFile:NSStringFromCString(imageURI)];
    if (!image) {
        NSLog(@"Cannot find image specified at %@", NSStringFromCString(imageURI));
    }
    
    NSString *hashtagsString = [[NSMutableString alloc] init];
    for (NSInteger idx = 0; idx < hashtagCount; idx++) {
        hashtagsString = [hashtagsString stringByAppendingString:[NSString stringWithFormat:@" %@",NSStringFromCString(hashtags[idx])]];
    }

    TWTRComposerViewController *composerVC = [[TWTRComposerViewController alloc] initWithInitialText:[NSString stringWithFormat:@"%@%@", NSStringFromCString(text), hashtagsString] image:image videoURL:nil];
    composerVC.delegate = TwitterUnityWrapper.sharedInstance;
    
    [[[[UIApplication sharedApplication] keyWindow] rootViewController] presentViewController:composerVC animated:YES completion:nil];
}

/**
 *  Convenience method for bringing up a view to the user to request their email address.
 *
 *  @param userID (Required) ID of the logged-in user to request email address from.
 */
void TwitterRequestEmail(const char *userID)
{
    if (!userID) {
        NSLog(@"Missing required userID.");
        return;
    }
    
    TWTRAPIClient *client = [[TWTRAPIClient alloc] initWithUserID:NSStringFromCString(userID)];
    [client requestEmailForCurrentUser:^(NSString *email, NSError *error) {
        if (email) {
            char *cStringEmail = cStringCopy([email UTF8String]);
            UnitySendMessage(TWTRInternalGameObject, TWTRUnityAPIMethodRequestEmailComplete, cStringEmail);
            free(cStringEmail);
        } else {
            NSDictionary *errorDictionary = NSDictionaryFromError(error);
            char *serializedError = serializedJSONFromNSDictionary(errorDictionary);
            UnitySendMessage(TWTRInternalGameObject, TWTRUnityAPIMethodRequestEmailFailed, serializedError);
            free(serializedError);
        }
    }];
}

__END_DECLS
