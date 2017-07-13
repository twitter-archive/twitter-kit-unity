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

UNITY_EXPORT_LOG="$PROJECT_ROOT/export_log.txt"
UNITY_EXPORT_PACKAGE="$PROJECT_ROOT/twitter-kit-unity.unitypackage"

UNITY_EXPORT_PATH="$UNITY_ANDROID_PLUGIN $UNITY_IOS_PLUGIN $UNITY_SOURCE"
UNITY_DEFAULT_BIN="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
UNITY_BIN="${UNITY_BIN:=$UNITY_DEFAULT_BIN}"

[ -e "$UNITY_BIN" ] || die "Unable to locate Unity application. Set location using UNITY_BIN.\n"

info "Exporting Twitter Kit Unity package\n"
$UNITY_BIN -batchmode -nographics -projectPath $UNITY_PACKAGE_ROOT -logFile $UNITY_EXPORT_LOG -exportPackage $UNITY_EXPORT_PATH $UNITY_EXPORT_PACKAGE -quit \
 || die "Failed to export Unity package. For more info see $UNITY_EXPORT_LOG\n"

 success "Unity package exported to $UNITY_EXPORT_PACKAGE\n"
 
