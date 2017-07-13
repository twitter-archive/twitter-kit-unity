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

#if UNITY_ANDROID && !UNITY_EDITOR
namespace TwitterKit.Internal
{
	using UnityEngine;
	using System;
	using TwitterKit.Unity;
	
	internal class AndroidTwitterImpl : ITwitter
	{
		private AndroidJavaClass twitter = new AndroidJavaClass ("com.twitter.sdk.android.unity.TwitterKit");

		public void Init (string consumerKey, string consumerSecret)
		{
			twitter.CallStatic ("init", consumerKey, consumerSecret);
		}

		public void LogIn ()
		{
			twitter.CallStatic ("login");
		}

		public void LogOut ()
		{
			twitter.CallStatic ("logout");
		}

		public TwitterSession Session ()
		{
			string session = twitter.CallStatic<string> ("session");
			return TwitterSession.Deserialize (session);
		}

		public void RequestEmail (TwitterSession session)
		{
			twitter.CallStatic ("requestEmail", TwitterSession.Serialize (session));
		}

		public void Compose (TwitterSession session, String imageUri, String text, string[] hashtags)
		{
			string sessionStr = TwitterSession.Serialize (session);
			twitter.CallStatic ("compose", sessionStr, imageUri, text, hashtags);
		}
	}
}
#endif
