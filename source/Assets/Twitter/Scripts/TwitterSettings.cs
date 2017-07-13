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

using System;

namespace TwitterKit.Unity.Settings
{	
	using System.IO;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Twitter Kit Settings
	/// </summary>
	public class TwitterSettings : ScriptableObject
	{
		public const string TWITTER_KIT_SETTINGS_ASSET_NAME = "TwitterKitSettings";
		public const string TWITTER_KIT_SETTINGS_PATH = "Twitter/Resources";
		public const string API_KEY_NOT_SET = "Your Twitter App API Key has not been set. " + SET_SETTINGS_INFO;
		public const string API_SECRET_NOT_SET = "Your Twitter App API Secret has not been set. " + SET_SETTINGS_INFO;
		private const string SET_SETTINGS_INFO = "To Set: In the main Unity editor navigate to 'Twitter Kit -> Settings' (make sure the Inspector tab is open).";

		private static TwitterSettings instance;

		private static List<OnChangeCallback> onChangeCallbacks = new List<OnChangeCallback>();
		public delegate void OnChangeCallback();

		[SerializeField]
		private string consumerKey = string.Empty;
		[SerializeField]
		private string consumerSecret = string.Empty;

		public static TwitterSettings Instance
		{
			get
			{
				instance = NullableInstance;

				if (instance == null)
				{
					instance = ScriptableObject.CreateInstance<TwitterSettings>();
				}

				return instance;
			}
		}

		public static TwitterSettings NullableInstance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load(TWITTER_KIT_SETTINGS_ASSET_NAME) as TwitterSettings;
				}

				return instance;
			}
		}

		/// <summary>
		/// Gets or sets the Twitter App Consumer Key
		/// </summary>
		public static string ConsumerKey
		{
			get
			{
				return Instance.consumerKey;
			}

			set
			{
				if (Instance.consumerKey != value)
				{
					Instance.consumerKey = value;
					SettingsChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the Twitter App Consumer Secret
		/// </summary>
		public static string ConsumerSecret
		{
			get
			{
				return Instance.consumerSecret;
			}

			set
			{
				if (Instance.consumerSecret != value)
				{
					Instance.consumerSecret = value;
					SettingsChanged();
				}
			}
		}

		public static void RegisterChangeEventCallback(OnChangeCallback callback)
		{
			onChangeCallbacks.Add(callback);
		}

		public static void UnregisterChangeEventCallback(OnChangeCallback callback)
		{
			onChangeCallbacks.Remove(callback);
		}

		private static void SettingsChanged()
		{
			onChangeCallbacks.ForEach(callback => callback());
		}

	}

}