using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Y2mateApi.Exceptions;

namespace Y2mateApi;

/// <summary>
/// CLient for interacting with y2mate
/// </summary>
public class Y2mateClient
{
    private readonly HttpClient _http;

    /// <summary>
    /// Initializes an instance of <see cref="Y2mateClient" />.
    /// </summary>
    public Y2mateClient(HttpClient httpClient)
    {
        _http = httpClient;
    }

    /// <summary>
    /// Initializes an instance of <see cref="Y2mateClient" />.
    /// </summary>
    public Y2mateClient() : this(Http.Client)
    {
    }

    /// <summary>
    /// Analyzes a youtube video.
    /// </summary>
    public async ValueTask<List<Link>> AnalyzeAsync(
        VideoId videoId,
        CancellationToken cancellationToken = default)
    {
        var links = new List<Link>();

        //var hostUrl = "https://www.y2mate.com/mates/analyze/ajax";
        var hostUrl = "https://corsproxy.io/?https://www.y2mate.com/mates/analyzeV2/ajax";

        var formContent = new FormUrlEncodedContent(new KeyValuePair<string?, string?>[]
        {
            //new KeyValuePair<string, string>("_id", y2id),
            //new KeyValuePair<string, string>("ajax", "1"),
            //new KeyValuePair<string, string>("fquality", quality),
            //new KeyValuePair<string, string>("ftype", type),
            //new KeyValuePair<string, string>("token", ""),
            //new KeyValuePair<string, string>("type", "youtube"),
            //new KeyValuePair<string, string>("v_id", video_id)

            new("k_query", $"https://www.youtube.com/watch?v={videoId}"),
            //new("k_auto", "0"),
            //new("hl", "en"),
        });

        var response = await _http.PostAsync(
            hostUrl,
            formContent,
            cancellationToken
        );

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (content.Contains("Try again in"))
            throw new Y2mateException("y2mate.com returned Try again in 5 seconds. Might be cause by wrong video id");

        if (content.Contains("Press f5 to try again."))
            throw new Y2mateException("y2mate.com returned Press f5 to try again. Might be caused by wrong quality or type");

        var data = JObject.Parse(content);
        if (data["status"]?.ToString() != "ok")
            throw new Y2mateException(data["mess"]!.ToString());

        var mp4Links = data["links"]!["mp4"]?.Values();
        if (mp4Links is not null)
        {
            foreach (var mp4Link in mp4Links)
            {
                // Add only .mp4 links. Skip others that we don't really use like .gp
                if (mp4Link["f"]?.ToString() != "mp4")
                    continue;

                // Skip auto quality
                if (mp4Link["q"]?.ToString() == "auto")
                    continue;

                var link = new Link()
                {
                    Id = mp4Link["k"]!.ToString(),
                    Size = mp4Link["size"]!.ToString(),
                    Quality = mp4Link["q"]!.ToString(),
                    FileType = FileType.Mp4
                };

                links.Add(link);
            }
        }

        var mp3Links = data["links"]!["mp3"]?.Values();
        if (mp3Links is not null)
        {
            foreach (var mp3Link in mp3Links)
            {
                if (mp3Link["f"]?.ToString().StartsWith("mp3") == false
                    && mp3Link["f"]?.ToString().StartsWith("m4a") == false)
                {
                    continue;
                }

                var link = new Link()
                {
                    Id = mp3Link["k"]!.ToString(),
                    Size = mp3Link["size"]!.ToString(),
                    Quality = mp3Link["q"]!.ToString(),
                    FileType = mp3Link["f"]!.ToString().Contains("m4a")
                        ? FileType.M4a
                        : FileType.Mp3
                };

                links.Add(link);
            }
        }

        return links;
    }

    /// <summary>
    /// Converts the video using and returns the download url.
    /// </summary>
    public async ValueTask<string?> ConvertAsync(
        string id,
        VideoId videoId,
        CancellationToken cancellationToken = default)
    {
        var formContent = new FormUrlEncodedContent(new KeyValuePair<string?, string?>[]
        {
            new("vid", videoId),
            new("k", id),
        });

        var response = await _http.PostAsync(
            "https://corsproxy.io/?https://www.y2mate.com/mates/convertV2/index",
            formContent,
            cancellationToken
        );

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrEmpty(content))
            throw new Y2mateException("Nothing found.");

        var data = JObject.Parse(content);

        var error = data["mess"]!.ToString();
        if (!string.IsNullOrEmpty(error))
            throw new Y2mateException(error);

        return data["dlink"]!.ToString();
    }
}