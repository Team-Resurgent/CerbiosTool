using CerbiosTool;

try
{
    var version = "V2.0.1";
    var application = new ApplicationUI(version);
    application.Run();
}
catch (Exception ex)
{
    var now = DateTime.Now.ToString("MMddyyyyHHmmss");
    File.WriteAllText($"Crashlog-{now}.txt", ex.ToString());
    Console.WriteLine($"Error: {ex}");
}



