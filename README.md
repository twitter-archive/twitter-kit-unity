# Twitter Kit for Unity

Twitter Kit for Unity provides cross-platform support (iOS & Android) for authorizing users and composing Tweets, allowing you to share great moments from your game with the world.

## Building
1. Download Twitter Kit frameworks.
```
./scripts/download_ios_frameworks.sh [version #] (e.g.: ./scripts/download_ios_frameworks.sh 3.1.0)
```
2. Build the Android wrapper.
```
./scripts/build_android_wrapper.sh
```
3. Export the unitypackage.
```
./scripts/export_unity_package.sh
```

## Contributing

The master branch of this repository contains the latest stable release of Twitter Kit for Unity. See [CONTRIBUTING.md](https://github.com/twitter/twitter-kit-unity/blob/master/CONTRIBUTING.md) for more details about how to contribute.

## Contact

For usage questions post on [Twitter Community](https://twittercommunity.com/c/publisher/twitter).

Please report any bugs as [issues](https://github.com/twitter/twitter-kit-unity/issues).

Follow [@TwitterDev](http://twitter.com/twitterdev) on Twitter for updates.

## Code of Conduct

This, and all github.com/twitter projects, are under the [Twitter Open Source Code of Conduct](https://github.com/twitter/code-of-conduct/blob/master/code-of-conduct.md). Additionally, see the [Typelevel Code of Conduct](http://typelevel.org/conduct) for specific examples of harassing behavior that are not tolerated.

## License

Copyright 2017 Twitter, Inc.

Licensed under the Apache License, Version 2.0: http://www.apache.org/licenses/LICENSE-2.0
