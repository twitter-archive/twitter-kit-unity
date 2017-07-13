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
	using System.Collections;

	public static class Utils {
		private const string TWITTER_KIT = "TwitterKit";

		public static void Log (string message)
		{
			Log (message, "d");
		}

		public static void LogError (string message)
		{
			Log (message, "e");
		}

		private static void Log (string message, string logType)
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			var logClass = new AndroidJavaClass("android.util.Log");
			logClass.CallStatic<int> (logType, TWITTER_KIT, message);
			#else
			if (logType == "e") {
				Debug.LogError (TWITTER_KIT + ": " + message);
			} else {
				Debug.Log (TWITTER_KIT + ": " + message);
			}
			#endif
		}

	}
}