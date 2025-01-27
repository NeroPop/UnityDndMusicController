using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MusicMixer;

public static class SaveSystem
{
    public static void SaveSettings(SettingsManager manager)
    {
        // Gets the scene name from the settings manager
        string sceneName = manager.SceneName;

        // Constructs the full path
        string directoryPath = Application.persistentDataPath + $"/{sceneName}";
        string filePath = directoryPath + "/settings.banana";

        // Ensure the directory exists
        Directory.CreateDirectory(directoryPath);

        // Creates a new binary file to store the data
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);

        // Calls the SettingsData class to set up itself and collect the data
        SettingsData data = new SettingsData(manager);

        // Saves the data into the file
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsData LoadSettings(string sceneName)
    {
        string filePath = Application.persistentDataPath + $"/{sceneName}/settings.banana";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + filePath);
            return null;
        }
    }
}
