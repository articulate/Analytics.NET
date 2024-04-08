﻿Analytics.NET
=============

This is our fork of the Analytics.NET library for Segment.

Analytics.NET is a .NET / C# client for [Segment](https://segment.com).

## Documentation

Documentation is available at [https://segment.com/libraries/.net](https://segment.com/libraries/.net).

## Releasing

This package is not currently built and deployed by AppVeyor like most of our others. To release a new version:

- Update the version in `Analytics\Analytics.cs`
- Update the version in `Analytics\Analytics.csproj`
- Run `dotnet pack`
- Deploy the resulting `.nupkg` file to GitHub packages.

## License

```
WWWWWW||WWWWWW
 W W W||W W W
      ||
    ( OO )__________
     /  |           \
    /o o|    MIT     \
    \___/||_||__||_|| *
         || ||  || ||
        _||_|| _||_||
       (__|__|(__|__|
```

