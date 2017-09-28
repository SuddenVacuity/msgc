using System;
using System.Drawing;

public class EnvironmentData
{
    private string DIRECTORY_IMAGE = @"//images";
    private string DIRECTORY_BRUSH = @"//brushes";

    // handle on application directory
    private string APPLICATION_DIRECTORY = "";

    public void init()
    {
        APPLICATION_DIRECTORY = Environment.CurrentDirectory;
    }

    public string getImageDirectory() { return APPLICATION_DIRECTORY + DIRECTORY_IMAGE; }
    public string getBrushDirectory() { return APPLICATION_DIRECTORY + DIRECTORY_BRUSH; }

}
