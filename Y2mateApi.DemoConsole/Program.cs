namespace Y2mateApi.DemoConsole;

internal static class Program
{
    static async Task Main()
    {
        // Get the video ID
        Console.Write("Enter YouTube video ID or URL: ");
        var url = Console.ReadLine() ?? "";

        //var url = "https://www.youtube.com/watch?v=kRrUDyz6VJ8";
        //var url = "https://www.youtube.com/watch?v=Nc0sB1Bmf-A";

        var y2mate = new Y2mateClient();

        var links = await y2mate.AnalyzeAsync(url);

        Console.WriteLine();

        for (int i = 0; i < links.Count; i++)
            Console.WriteLine($"({i + 1}) Type: {links[i].FileType}, Quality: {links[i].Quality}");

        Console.WriteLine();
        Console.Write("Enter number: ");

        int which;

        // Read the selected index
        while (!int.TryParse(Console.ReadLine() ?? "", out which)
            || which < 1
            || which > links.Count)
        {
            Console.WriteLine("You entered an invalid number");
            Console.Write("Enter number: ");
        }

        var link = links[which - 1];
        var downloadUrl = await y2mate.ConvertAsync(link.Id, url);

        Console.WriteLine();
        Console.WriteLine($"File type: {link.FileType}");
        Console.WriteLine();
        Console.WriteLine($"File size: {link.Size}");
        Console.WriteLine();
        Console.WriteLine("Download url: " + downloadUrl);
        Console.ReadLine();
    }
}