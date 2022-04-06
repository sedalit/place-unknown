using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class SceneSerializer : MonoBehaviour
{
    [System.Serializable]
    public class SceneObjectState
    {
        public int SceneId;
        public long EntityId;
        public string State;
    }

    public const string FILE_NAME = "Save.dat";
    public static UnityAction GameSaved;
    [SerializeField] private PrefabsDataBase prefabsDataBase;
    public void SaveGame()
    {
        SaveToFile(FILE_NAME);
        GameSaved?.Invoke();
    }

    public void LoadGame()
    {
        LoadFromFile(FILE_NAME);
    }

    private void SaveToFile(string filePath)
    {
        List<SceneObjectState> savedObjects = new List<SceneObjectState>();
        GetAllSerializableEntities(savedObjects);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filePath);
        bf.Serialize(file, savedObjects);
        file.Close();
        Debug.Log("Сцена сохранена: " + Application.persistentDataPath + "/" + filePath);
    }

    private void LoadFromFile(string filePath)
    {
        Player.Instance = null;
        foreach (var entity in FindObjectsOfType<Entity>())
        {
            Destroy(entity.gameObject);
        }
        List<SceneObjectState> loadedObjects = new List<SceneObjectState>();
        if (File.Exists(Application.persistentDataPath + "/" + filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filePath, FileMode.Open);
            loadedObjects = (List<SceneObjectState>)bf.Deserialize(file);
            file.Close();
        }
        foreach (var entity in loadedObjects)
        {
            if (prefabsDataBase.IsPlayerId(entity.EntityId))
            {
                Destroy(Camera.main.gameObject);
                GameObject player = prefabsDataBase.CreatePlayer();
                player.GetComponent<ISerializableEntity>().DeserializeState(entity.State);
                loadedObjects.Remove(entity);
                break;
            }

        }
        foreach (var entity in loadedObjects)
        {
            GameObject g = prefabsDataBase.CreateEntityFromId(entity.EntityId);
            g.GetComponent<ISerializableEntity>().DeserializeState(entity.State);
        }
    }

    private void GetAllSerializableEntities(List<SceneObjectState> savedObjects)
    {
        foreach (var entity in FindObjectsOfType<Entity>())
        {
            ISerializableEntity serializableEntity = entity as ISerializableEntity;
            if (serializableEntity != null && serializableEntity.IsSerializable() != false)
            {
                SceneObjectState state = new SceneObjectState();
                state.EntityId = serializableEntity.EntityId;
                state.State = serializableEntity.SerializeState();
                savedObjects.Add(state);
            }
        }
    }
}
