using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
 
    public static bool NewGameBool = false;


    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistanceManager instance {get; private set;}

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one data persistance manager in the scene");
        }
        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void NewGame()
    {
        NewGameBool = true;
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //load any saved data from file using data handler
        this.gameData = dataHandler.Load();

        if(this.gameData == null)
        {
            Debug.Log("No data found starting new game");
            NewGame();
            
        }
 
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }

    public void  SaveGame()
    {
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }

        //save that data to file using data handler
        dataHandler.Save(gameData);
    }


    //testing

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();

    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //gets all instances where the IDataPersistance interface is used
    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
