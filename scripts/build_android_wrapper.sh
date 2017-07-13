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

UNITY_ANDROID_PLUGIN_PATH=$UNITY_PACKAGE_ROOT/$UNITY_ANDROID_PLUGIN
mkdir -p $UNITY_ANDROID_PLUGIN_PATH

pushd $UNITY_ANDROID_WRAPPER > /dev/null

BUILD_DIR=${PWD}"/app/build/outputs/unity_plugin/"

info "Building Android wrapper\n"
./gradlew -q releaseCreateUnityPackage || die "Android Build failed\n"

info "Copying Android wrapper to $UNITY_ANDROID_PLUGIN_PATH\n"
cp $BUILD_DIR/* $UNITY_ANDROID_PLUGIN_PATH \
  || die "Failed to copy $BUILD_DIR/* to $UNITY_ANDROID_PLUGIN_PATH\n"

popd > /dev/null
