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

set -eu

cd "$( dirname "${BASH_SOURCE[0]}" )/.."
PROJECT_ROOT=$(pwd)

UNITY_PACKAGE_ROOT="$PROJECT_ROOT/source"
UNITY_ANDROID_PLUGIN="Assets/Plugins/Android"
UNITY_IOS_PLUGIN="Assets/Plugins/iOS"
UNITY_ANDROID_WRAPPER="$PROJECT_ROOT/Native/TwitterKitAndroidWrapper/"
UNITY_IOS_WRAPPER="$PROJECT_ROOT/Native/TwitterKitIOSWrapper/"
UNITY_SOURCE="Assets/Twitter"

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
CYAN='\033[0;36m'
CLEAR_FORMATTING='\033[0m'

die () {
    printf >&2 "${RED}Error${CLEAR_FORMATTING} $@"
    exit 1
}

success () {
    printf >&2 "${GREEN}Success:${CLEAR_FORMATTING} $@"
}

info () {
    printf >&2 "${CYAN}Info:${CLEAR_FORMATTING} $@"
}

warning () {
    printf >&2 "${YELLOW}Warning:${CLEAR_FORMATTING} $@"
}

downloadArtifact() {
    local REMOTE_PATH=$1
    local OUTPUT_PATH=$2

    info "Downloading $REMOTE_PATH to $OUTPUT_PATH\n"
    curl --progress-bar -L -f "$REMOTE_PATH" > $OUTPUT_PATH
}

unzipArtifact() {
    local INPUT_PATH=$1

    info "Unzipping $INPUT_PATH\n"
    unzip -q $INPUT_PATH
}
