# unity3d-localized-strings

A lightweight localization framework for Unity editor scripts.

```sh
$ yarn add rotorz/unity3d-localized-strings
```

This package is compatible with the [unity3d-package-syncer][tool] tool. Refer to the
tools' [README][tool] for information on syncing packages into a Unity project.

[tool]: https://github.com/rotorz/unity3d-package-syncer


## Setting up an environment with a single language domain

A custom language domain can be defined for a package in the following way:

```csharp
// Editor/MyPackageLang.cs
using Rotorz.Games.Localization;
using System.Globalization;

[DiscoverablePackageLanguage]
public sealed class MyPackageLang : PackageLanguage<MyPackageLang>
{
    public MyPackageLang()
        : base("@my-vendor-name/my-package", CultureInfo.GetCultureInfo("en-US"))
    {
    }


    // Additional custom helper functionality can be implemented here if required...
}
```

This can then be consumed in editor scripts:

```csharp
// Present label with localizable text to the user.
GUILayout.Label(MyPackageLang.Text("Hello, world!"));
```


## Contribution Agreement

This project is licensed under the MIT license (see LICENSE). To be in the best
position to enforce these licenses the copyright status of this project needs to
be as simple as possible. To achieve this the following terms and conditions
must be met:

- All contributed content (including but not limited to source code, text,
  image, videos, bug reports, suggestions, ideas, etc.) must be the
  contributors own work.

- The contributor disclaims all copyright and accepts that their contributed
  content will be released to the public domain.

- The act of submitting a contribution indicates that the contributor agrees
  with this agreement. This includes (but is not limited to) pull requests, issues,
  tickets, e-mails, newsgroups, blogs, forums, etc.
