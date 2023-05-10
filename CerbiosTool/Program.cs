using CerbiosTool;

try
{
    var version = "v1.0.3";
    var application = new Application(version);
    application.Run();
}
catch (Exception ex)
{
    var now = DateTime.Now.ToString("MMddyyyyHHmmss");
    File.WriteAllText($"Crashlog-{now}.txt", ex.ToString());
}



