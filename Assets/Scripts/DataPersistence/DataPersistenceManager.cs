using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
    [SerializeField] private bool IDIN = false;
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public static DataPersistenceManager instance { get; private set; }
    private List<IDataPersistence> dataPersistenceObjects;
    private GameData gameData;
    private FileDataHandler dataHandler;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("that is odd, more than one dpm, PUNISHMENT TIME!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

    }

    private void Start()
    {
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null && IDIN)
        {
            NewGame();
        }
        this.dataPersistenceObjects = GetAllDataPersistenceObjects();
        if (this.gameData == null)
        {
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

    }

    public void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        LoadGame();

    }

    public void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
    {
        if (scene.name != "Map")
        {
            SaveGame();
        }
    }
    public void SaveGame()
    {
        if (this.gameData == null)
        {
            return;
        }
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> GetAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistences);
    }

    public bool hasGameData()
    {
        return gameData != null;
    }
}
