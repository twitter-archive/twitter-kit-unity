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

namespace TwitterKit.Internal
{
	using UnityEngine;
	using System;
	using TwitterKit.Unity;
	
	internal class TwitterComponent : MonoBehaviour
	{
		public Action<TwitterSession> loginSuccessAction { set; get; }

		public Action<ApiError> loginFailureAction { set; get; }

		public Action<string> emailSuccessAction { set; get; }

		public Action<ApiError> emailFailureAction { set; get; }

		public Action<string> tweetSuccessAction { set; get; }

		public Action<ApiError> tweetFailureAction { set; get; }

		public Action tweetCancelAction { set; get; }
	
		public void Awake ()
		{
			MonoBehaviour.DontDestroyOnLoad (this);
		}

		public void LoginComplete (string session)
		{
			UnityEngine.Debug.Log ("Login request completed");
			if (loginSuccessAction != null) {
				UnityEngine.Debug.Log ("calling login success action");
				loginSuccessAction (TwitterSession.Deserialize (session));
			} else {
				UnityEngine.Debug.Log ("FAILED calling login success action");
			}
		}

		public void LoginFailed (string error)
		{
			UnityEngine.Debug.Log ("Login request failed");
			if (loginFailureAction != null) {
				loginFailureAction (ApiError.Deserialize (error));
			}
		}

		public void RequestEmailComplete (string email)
		{
			UnityEngine.Debug.Log ("Email request completed");
			if (emailSuccessAction != null) {
				emailSuccessAction (email);
			}
		}

		public void RequestEmailFailed (string error)
		{
			UnityEngine.Debug.Log ("Email request failed");
			if (emailFailureAction != null) {
				emailFailureAction (ApiError.Deserialize (error));
			}
		}

		public void TweetComplete (string tweetId)
		{
			UnityEngine.Debug.Log ("Tweet completed");
			if (tweetSuccessAction != null) {
				tweetSuccessAction (tweetId);
			}
		}

		public void TweetFailed (string error)
		{
			UnityEngine.Debug.Log ("Tweet failed");
			if (tweetFailureAction != null) {
				tweetFailureAction (ApiError.Deserialize (error));
			}
		}

		public void TweetCancelled ()
		{
			UnityEngine.Debug.Log ("Tweet cancelled");
			if (tweetCancelAction != null) {
				tweetCancelAction ();
			}
		}
	}
}
