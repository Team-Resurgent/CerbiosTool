using CerbiosTool;

try
{
    var version = "V1.0.11";
    var application = new Application(version);
    application.Run();
}
catch (Exception ex)
{
    var now = DateTime.Now.ToString("MMddyyyyHHmmss");
    File.WriteAllText($"Crashlog-{now}.txt", ex.ToString());
}



