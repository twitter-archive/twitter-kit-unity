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
	
	public class TwitterSession
	{
		public long id { get; private set; }

		public string userName { get; private set; }

		public AuthToken authToken { get; private set; }
	
		internal TwitterSession (long id, string userName, AuthToken authToken)
		{
			this.id = id;
			this.userName = userName;
			this.authToken = authToken;
		}
	
		internal Dictionary<string, object> ToDictionary ()
		{
			var sessionDictionary = new Dictionary<string, object> ();
			sessionDictionary.Add ("id", id);
			sessionDictionary.Add ("user_name", userName);
			sessionDictionary.Add ("auth_token", authToken.ToDictionary ());

			return sessionDictionary;
		}

		internal static string Serialize (TwitterSession session)
		{
			return Json.Serialize (session.ToDictionary ());
		}
	
		internal static TwitterSession Deserialize (string session)
		{
			if (session == null || session.Length == 0) {
				return null;
			}

			var sessionDictionary = Json.Deserialize (session) as Dictionary<string,object>;
			long id = Convert.ToInt64 (sessionDictionary ["id"]);
			string userName = sessionDictionary ["user_name"] as String;
		
			var tokenDictionary = sessionDictionary ["auth_token"] as Dictionary<string,object>;
			string token = tokenDictionary ["token"] as String;
			string secret = tokenDictionary ["secret"] as String;
		
			return new TwitterSession (id, userName, new AuthToken (token, secret));
		}
	}
}
