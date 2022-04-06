using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrefabsDataBase : ScriptableObject
{
    public Entity PlayerPrefab;
    public List<Entity> AllPrefabs;

    public GameObject CreateEntityFromId(long id)
    {
        foreach (var entity in AllPrefabs)
        {
            if ((entity is ISerializableEntity) == false) continue;
            if ((entity as ISerializableEntity).EntityId == id)
            {
                return Instantiate(entity.gameObject);
            }
        }
        return null;
    }

    public GameObject CreatePlayer()
    {
        return Instantiate(PlayerPrefab.gameObject);
    }

    public bool IsPlayerId(long id)
    {
        return id == (PlayerPrefab as ISerializableEntity).EntityId;
    }
}
