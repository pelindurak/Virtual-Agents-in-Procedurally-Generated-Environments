using UnityEngine;
using System.IO;

public class HandleTextFile: MonoBehaviour 
{

    public HandleTextFile()
    {

    }

    // https://discussions.unity.com/t/help-making-a-text-file-when-in-game/781044/6
    public void CreateText()
    {
        //Path of the file
        string path = Application.dataPath + "/Log.txt";
        //Create File if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Fitness values \n\n");
        }
        //Content of the file
        string content = "";
        //Add some to text to it
        File.AppendAllText(path, content);
    }

    public void WriteText(string content)
    {
        string path = Application.dataPath + "/Log.txt";
        File.AppendAllText(path, content);
    }
}
