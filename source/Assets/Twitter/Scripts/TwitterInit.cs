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
	using TwitterKit.Unity;

	public class TwitterInit : MonoBehaviour
	{
		private static TwitterInit instance;

		void Awake()
		{
			// This singleton pattern ensures AwakeOnce() is only called once even when the scene
			// is reloaded (loading scenes destroy previous objects and wake up new ones)
			if (instance == null) {
				AwakeOnce ();

				instance = this;
				DontDestroyOnLoad(this);
			} else if (instance != this) {
				Destroy(this.gameObject);
			}
		}

		private void AwakeOnce ()
		{
			Twitter.AwakeInit();
		}
	}
}
