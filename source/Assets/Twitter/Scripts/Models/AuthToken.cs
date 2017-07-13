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
	
	public class AuthToken
	{
		public string token { get; private set; }

		public string secret { get; private set; }
	
		internal AuthToken (string token, string secret)
		{
			this.token = token;
			this.secret = secret;
		}

		internal Dictionary<string, object> ToDictionary ()
		{
			var tokenDictionary = new Dictionary<string, object> ();
			tokenDictionary.Add ("token", token);
			tokenDictionary.Add ("secret", secret);

			return tokenDictionary;
		}
	}
}