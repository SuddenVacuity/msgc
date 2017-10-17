using System;
using System.Drawing;

public class EnvironmentData
{
    private string APPLICATION_VERSION = "v0.0.1.1";
    private string APPLICATION_OWNER = "Gerald Coggins";

    private string DIRECTORY_IMAGE = @"//images";
    private string DIRECTORY_BRUSH = @"//brushes";

    // handle on application directory
    private string APPLICATION_DIRECTORY = "";

    public EnvironmentData()
    {
        APPLICATION_DIRECTORY = Environment.CurrentDirectory;
    }

    public string getImageDirectory() { return APPLICATION_DIRECTORY + DIRECTORY_IMAGE; }
    public string getBrushDirectory() { return APPLICATION_DIRECTORY + DIRECTORY_BRUSH; }
    public string getApplicationVersion() { return APPLICATION_VERSION; }
    public string getApplicationOwner() { return APPLICATION_OWNER; }

}
