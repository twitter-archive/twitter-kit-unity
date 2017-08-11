#!/bin/bash
#
# Copyright (C) 2017 Twitter, Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
if [ "$#" -ne 1 ]; then
  echo "Usage: ./scripts/download_ios_frameworks.sh [version #] (e.g.: ./scripts/download_ios_frameworks.sh 3.1.0)" >&2
  exit 1
fi

. $(dirname $0)/common.sh

TWITTER_KIT_IOS_URL="https://ton.twimg.com/syndication/twitterkit/ios/$1/Twitter-Kit-iOS.zip"
UNITY_IOS_PLUGIN_PATH=$UNITY_PACKAGE_ROOT/$UNITY_IOS_PLUGIN
TEMP_PATH="output.zip"
mkdir -p $UNITY_IOS_PLUGIN_PATH

pushd $PROJECT_ROOT > /dev/null

downloadArtifact $TWITTER_KIT_IOS_URL $TEMP_PATH || die "Failed to download $REMOTE_PATH\n"

unzipArtifact $TEMP_PATH || die "Failed to unzip $TEMP_PATH\n"

info "Moving Twitter-Kit-iOS/* into $UNITY_IOS_PLUGIN_PATH\n"
mv Twitter-Kit-iOS/* $UNITY_IOS_PLUGIN_PATH || die "Failed to move Twitter-Kit-iOS/* to $UNITY_IOS_PLUGIN_PATH\n"

info "Cleaning up temporary files\n"
rmdir Twitter-Kit-iOS
rm -f $TEMP_PATH
popd > /dev/null
