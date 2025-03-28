using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using YamlDotNet;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private YamlDotNet.Serialization.Serializer yamlSerializer = new YamlDotNet.Serialization.Serializer();
    private YamlDotNet.Serialization.Deserializer yamlDeserializer = new YamlDotNet.Serialization.Deserializer();

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                // deserialize the JSON string to a C# object
                // loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                loadedData = yamlDeserializer.Deserialize<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load data from " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // create the file if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // use yaml to serialize the data
            string dataToStore = yamlSerializer.Serialize(data);

            // write the serialized data to the file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save data to " + fullPath + "\n" + e);
        }
    }
}
