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
	using System;
	using System.Collections.Generic;
	using ThirdParty.MiniJSON;
	
	public class ApiError
	{
		public int code { get; private set; }

		public string message { get; private set; }

		internal ApiError (int code, string message)
		{
			this.code = code;
			this.message = message;
		}

		internal static ApiError Deserialize (string error)
		{
			if (error == null || error.Length == 0) {
				return null;
			}

			var errorDictionary = Json.Deserialize (error) as Dictionary<string,object>;
			int code = Convert.ToInt32 (errorDictionary ["code"]);
			string message = errorDictionary ["message"] as String;

			return new ApiError (code, message);
		}
	}
}