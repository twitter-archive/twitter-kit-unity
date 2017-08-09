/*
 * Copyright (C) 2015 Twitter, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
namespace TwitterKit.Unity
{	
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	using Internal;
	using TwitterKit.Unity.Settings;

	public sealed class Twitter : ScriptableObject
	{
		private static ITwitter twitter;
		private static GameObject twitterGameObject;

		public static void AwakeInit ()
		{
			twitterGameObject = new GameObject ("TwitterGameObject");
			twitterGameObject.AddComponent<TwitterComponent> ();

#if UNITY_IOS && !UNITY_EDITOR
			twitter = new IOSTwitterImpl();
#elif UNITY_ANDROID && !UNITY_EDITOR
			twitter = new AndroidTwitterImpl();
#else
			twitter = new EditorTwitterImpl();
#endif
		}

		/// <summary>
		/// Init Twitter Kit
		/// </summary>
		public static void Init ()
		{
			if (string.IsNullOrEmpty(TwitterSettings.ConsumerKey)) {
				TwitterKit.Internal.Utils.LogError(TwitterSettings.API_KEY_NOT_SET);
				return;
			}
			if (string.IsNullOrEmpty(TwitterSettings.ConsumerSecret)) {
				TwitterKit.Internal.Utils.LogError(TwitterSettings.API_SECRET_NOT_SET);
				return;
			}
			twitter.Init (TwitterSettings.ConsumerKey, TwitterSettings.ConsumerSecret);
		}

		/// <summary>
		/// Show login with Twitter
		/// <param name="successCallback">Callback to call on success</param>
		/// <param name="failureCallback">Callback to call on failure</param>
		/// </summary>
		public static void LogIn (Action<TwitterSession> successCallback = null, Action<ApiError> failureCallback = null)
		{
			twitterGameObject.GetComponent<TwitterComponent> ().loginSuccessAction = successCallback;
			twitterGameObject.GetComponent<TwitterComponent> ().loginFailureAction = failureCallback;
			twitter.LogIn ();
		}

		/// <summary>
		/// Logout of current session
		/// </summary>
		public static void LogOut ()
		{
			twitter.LogOut ();
		}

		/// <summary>
		/// Returns the active session.
		/// </summary>
		public static TwitterSession Session { get { return twitter.Session (); } }

		/// <summary>
		/// Request Twitter users email address
		/// <param name="session">User's session from Login</param>
		/// <param name="successCallback">Callback to call on success</param>
		/// <param name="failureCallback">Callback to call on failure</param>
		/// </summary>
		public static void RequestEmail (TwitterSession session, Action<string> successCallback = null, Action<ApiError> failureCallback = null)
		{
			twitterGameObject.GetComponent<TwitterComponent> ().emailSuccessAction = successCallback;
			twitterGameObject.GetComponent<TwitterComponent> ().emailFailureAction = failureCallback;
			twitter.RequestEmail (session);
		}

		/// <summary>
		/// Show Twitter composer
		/// <param name="session">User’s session from Login</param>
		/// <param name="imageUri">uri of image to include in tweet</param>
		/// <param name="text">text to tweet to pre-fill</param>
		/// <param name="hashtags">hashtags to pre-fill</param>
		/// <param name="successCallback">Callback to call on success</param>
		/// <param name="failureCallback">Callback to call on failure</param>
		/// <param name="cancelCallback">Callback to call on cancel</param>
		/// </summary>
		public static void Compose (TwitterSession session, String imageUri, String text, string[] hashtags = null, Action<string> successCallback = null, Action<ApiError> failureCallback = null, Action cancelCallback = null)
		{
			twitterGameObject.GetComponent<TwitterComponent> ().tweetSuccessAction = successCallback;
			twitterGameObject.GetComponent<TwitterComponent> ().tweetFailureAction = failureCallback;
			twitterGameObject.GetComponent<TwitterComponent> ().tweetCancelAction = cancelCallback;
			twitter.Compose (session, imageUri, text, hashtags);
		}
	}
}
