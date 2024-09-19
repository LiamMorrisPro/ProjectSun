using UnityEngine;
using System;
using System.IO;


public class FileDataHandler
{
    private string dataDirpath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirpath, string dataFileName)
    {
        this.dataDirpath = dataDirpath;
        this.dataFileName = dataFileName;
    }
 
    public GameData Load()
    {
        string fullpath = Path.Combine(dataDirpath, dataFileName); 
        GameData loadedData = null;
        if(File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //deserialize the data from Json back into the c# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load from file: " + fullpath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullpath = Path.Combine(dataDirpath, dataFileName); 
        try
        {
            //create directory file will be saved in if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            //serialize the c# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            //write the serialized data to file
            using(FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file" + fullpath + "\n" + e);
        }
    }
}
