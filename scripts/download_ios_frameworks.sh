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

. $(dirname $0)/common.sh

TWITTER_KIT_IOS_URL="https://ton.twimg.com/syndication/twitterkit/ios/3.0.3-update/TwitterKitManual.zip"
UNITY_IOS_PLUGIN_PATH=$UNITY_PACKAGE_ROOT/$UNITY_IOS_PLUGIN
TEMP_PATH="output.zip"
mkdir -p $UNITY_IOS_PLUGIN_PATH

pushd $PROJECT_ROOT > /dev/null

downloadArtifact $TWITTER_KIT_IOS_URL $TEMP_PATH || die "Failed to download $REMOTE_PATH\n"

unzipArtifact $TEMP_PATH || die "Failed to unzip $TEMP_PATH\n"

info "Moving TwitterKitManual/* into $UNITY_IOS_PLUGIN_PATH\n"
mv TwitterKitManual/* $UNITY_IOS_PLUGIN_PATH || die "Failed to move TwitterKitManual/* to $UNITY_IOS_PLUGIN_PATH\n"

info "Cleaning up temporary files\n"
rmdir TwitterKitManual
rm -f $TEMP_PATH
popd > /dev/null
