# IvarI.MEF.Plugins
The library will help you to load plugins in your app.

## How to use
1. Get it from [nuget.org](https://www.nuget.org/packages/IvarI.MEF.Plugins/).
2. Create loader configuration.
3. Create loader with configuration.
4. Load your plugins.

## Configuration
### Default
```csharp
using IvarI.Plugins.FileSystem;
var configuration = Configuration.GetDefault();
```
Include first-level subdirectories of {baseDirectory}/plugins/.
### Custom
```csharp
using IvarI.Plugins.FileSystem;
var configuration = new Configuration();
```
Configure it as you like.

## Loader
```csharp
using IvarI.Plugins.FileSystem;
var configuration = Configuration.GetDefault();
var loader = new Loader<INterface>(configuration); // INterface is your plugin type.
var plugins = loader.Load();
```
