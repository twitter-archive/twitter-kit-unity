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
using UnityEditor;
using UnityEngine;
using TwitterKit.Unity;
using TwitterKit.Unity.Settings;
using System.IO;

[InitializeOnLoad]
[CustomEditor(typeof(TwitterSettings))]
public class TwitterSettingsEditor : Editor
{
	public TwitterSettingsEditor()
	{
		TwitterSettings.RegisterChangeEventCallback(this.SettingsChanged);
	}

	[MenuItem("Tools/Twitter Kit %t")]
	public static void Init()
	{
		var instance = TwitterSettings.NullableInstance;

		if (instance == null)
		{
			instance = ScriptableObject.CreateInstance<TwitterSettings>();
			string properPath = Path.Combine(Application.dataPath, TwitterSettings.TWITTER_KIT_SETTINGS_PATH);
			if (!Directory.Exists(properPath))
			{
				Directory.CreateDirectory(properPath);
			}

			string fullPath = Path.Combine(Path.Combine("Assets", TwitterSettings.TWITTER_KIT_SETTINGS_PATH), TwitterSettings.TWITTER_KIT_SETTINGS_ASSET_NAME + ".asset");
			AssetDatabase.CreateAsset(instance, fullPath);
		}

		Selection.activeObject = TwitterSettings.Instance;
	}

	private void SettingsChanged()
	{
		EditorUtility.SetDirty((TwitterSettings)target);
	}

	public override void OnInspectorGUI()
	{
		GUI.changed = false;
		EditorGUILayout.LabelField("Twitter Kit Settings", EditorStyles.boldLabel);
		EditorGUILayout.Separator ();
		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("API Key:");
		TwitterSettings.ConsumerKey = EditorGUILayout.TextField(TwitterSettings.ConsumerKey);
		EditorGUILayout.Separator ();
		EditorGUILayout.LabelField("API Secret:");
		TwitterSettings.ConsumerSecret = EditorGUILayout.TextField(TwitterSettings.ConsumerSecret);

		if (GUI.changed) {
			this.SettingsChanged ();
		}
	}

}

