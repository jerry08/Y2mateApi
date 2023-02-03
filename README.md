
<h1 align="center">
    Y2mateApi
</h1>

<p align="center">
   <a href="https://discord.gg/mhxsSMy2Nf"><img src="https://img.shields.io/badge/Discord-7289DA?style=for-the-badge&logo=discord&logoColor=white"></a>
   <a href="https://nuget.org/packages/Y2mateApi"><img src="https://img.shields.io/nuget/dt/Y2mateApi.svg?label=Downloads&color=%233DDC84&logo=nuget&logoColor=%23fff&style=for-the-badge"></a>
</p>

**Y2mateApi** is an unofficial api for y2mate to analyze, convert and download videos/audio from youtube.

### 🌟STAR THIS REPOSITORY TO SUPPORT THE DEVELOPER AND ENCOURAGE THE DEVELOPMENT OF THIS PROJECT!


## Install

- 📦 [NuGet](https://nuget.org/packages/Y2mateApi): `dotnet add package Y2mateApi`

## Usage

**Y2mateApi** exposes its functionality through a single entry point — the `Y2mateClient` class.
Create an instance of this class and use the provided operations to send requests.

```csharp
using Y2mateApi;

var y2mate = new Y2mateClient();

// Your youtube url (or id only)
var url = "https://www.youtube.com/watch?v=kRrUDyz6VJ8";

var links = await y2mate.AnalyzeAsync(url);
var selectedLink = links[0];
var downloadUrl = await y2mate.ConvertAsync(selectedLink.Id, url);

// Download from the url...
```