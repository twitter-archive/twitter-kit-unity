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

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using TwitterKit.Unity;
using TwitterKit.Unity.Settings;

public class TwitterPostProcessBuild {

	private const string URL_TYPES = "CFBundleURLTypes";
	private const string URL_SCHEMES = "CFBundleURLSchemes";
	private const string APPLICATION_QUERIES_SCHEMES = "LSApplicationQueriesSchemes";

	[PostProcessBuild]
	public static void UpdateXCodePlist(BuildTarget buildTarget, string pathToBuiltProject) {
		#if UNITY_IOS
		if (buildTarget == BuildTarget.iOS) {

			// Check Consumer Key
			if (string.IsNullOrEmpty(TwitterSettings.ConsumerKey)) {
				TwitterKit.Internal.Utils.LogError(TwitterSettings.API_KEY_NOT_SET);
				return;
			}

			// Get plist
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));

			// Get root
			PlistElementDict rootDict = plist.root;

			// Modify Info.Plist for Twitter Kit (https://dev.twitter.com/twitterkit/ios/installation)
			PlistElementArray bundleURLTypesArray = rootDict[URL_TYPES] as PlistElementArray;
			if (bundleURLTypesArray == null) {
			    bundleURLTypesArray = rootDict.CreateArray (URL_TYPES);
			}
			PlistElementDict dict = bundleURLTypesArray.AddDict ();
			PlistElementArray bundleURLSchemesArray = dict.CreateArray (URL_SCHEMES);
			bundleURLSchemesArray.AddString ("twitterkit-" + TwitterSettings.ConsumerKey);
			PlistElementArray queriesSchemesArray = rootDict[APPLICATION_QUERIES_SCHEMES] as PlistElementArray;
			if (queriesSchemesArray == null) {
				queriesSchemesArray = rootDict.CreateArray (APPLICATION_QUERIES_SCHEMES);
			}
			queriesSchemesArray.AddString ("twitter");
			queriesSchemesArray.AddString ("twitterauth");

			// Write to file
			File.WriteAllText(plistPath, plist.WriteToString());
		}
		#endif
	}

}
