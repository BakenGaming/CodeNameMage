using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    #region Setup
    [SerializeField] private bool dontDestroyOnLoad = false;

    private GameObject emptyHolder;

    private static GameObject particleSystemsEmpty;
    private static GameObject gameObjectsEmpty;
    private static GameObject projectilesEmpty;
    private static GameObject enemiesEmpty;
    private static GameObject collectablesEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> objectPools;
    private static Dictionary<GameObject, GameObject> cloneToPrefabMap;

    public enum PoolType
    {
        particles, gameObjects, projectiles, enemies, collectables
    }
    private static PoolType poolingType;

    void Awake()
    {
        objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        emptyHolder = new GameObject("Object Pools");

        particleSystemsEmpty = new GameObject("Particles");
        particleSystemsEmpty.transform.SetParent(emptyHolder.transform);

        gameObjectsEmpty = new GameObject("GameObjects");
        gameObjectsEmpty.transform.SetParent(emptyHolder.transform);

        projectilesEmpty = new GameObject("Projectiles");
        projectilesEmpty.transform.SetParent(emptyHolder.transform);

        enemiesEmpty = new GameObject("Enemies");
        enemiesEmpty.transform.SetParent(emptyHolder.transform);

        collectablesEmpty = new GameObject("Collectables");
        collectablesEmpty.transform.SetParent(emptyHolder.transform);

        if(dontDestroyOnLoad)
            DontDestroyOnLoad(particleSystemsEmpty.transform.root);
    }
    #endregion
    #region Create Pools
    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.gameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );
        objectPools.Add(prefab, pool);
    }
    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.gameObjects)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, pos, rot);
        obj.SetActive(true);
        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);
        return obj;
    }
    private static void CreatePool(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.gameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, parent, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );
        objectPools.Add(prefab, pool);
    }
    private static GameObject CreateObject(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.gameObjects)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = rot;
        obj.transform.localScale = Vector3.one;
        obj.SetActive(true);

        return obj;
    }
    #endregion
    #region Handle Objects
    private static void OnGetObject(GameObject obj)
    {
        //optional logic
    }
    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    private static void OnDestroyObject(GameObject obj)
    {
        if(cloneToPrefabMap.ContainsKey(obj))
            cloneToPrefabMap.Remove(obj);
    }
    private static GameObject SetParentObject(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.particles:
                return particleSystemsEmpty;
            case PoolType.gameObjects:
                return gameObjectsEmpty;
            case PoolType.projectiles:
                return projectilesEmpty;
            case PoolType.enemies:
                return enemiesEmpty;
            case PoolType.collectables:
                return collectablesEmpty;
            default: return null;
        }
    }
    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.gameObjects)
    {
        if(cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if(obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }
            if(objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else Debug.LogWarning($"Trying to return an object that is not pooled: {obj.name}");
    }
    #endregion
    #region Spawn Objects
    private static T SpawnObject<T>(GameObject objToSpawn, Vector3 spawnPos, Quaternion spawnRot, PoolType poolType = PoolType.gameObjects, bool setActive = false) where T : Object
    {
        if(!objectPools.ContainsKey(objToSpawn))
        {
            CreatePool(objToSpawn, spawnPos, spawnRot, poolType);
        }
        GameObject obj = objectPools[objToSpawn].Get();
        if(obj != null)
        {
            if(!cloneToPrefabMap.ContainsKey(obj))
            {
                cloneToPrefabMap.Add(obj, objToSpawn);
            }
            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRot;
            obj.SetActive(setActive);

            if(typeof(T) == typeof(GameObject)) return obj as T;

            T component = obj.GetComponent<T>();
            if(component == null)
            {
                Debug.LogError($"Object {objToSpawn.name} doesn't have a comonent of type {typeof(T)}");
                return null;
            }
            return component;
        }
        return null;
    }
    private static T SpawnObject<T>(GameObject objToSpawn, Transform parent, Quaternion spawnRot, PoolType poolType = PoolType.gameObjects, bool setActive = false) where T : Object
    {
        if(!objectPools.ContainsKey(objToSpawn))
        {
            CreatePool(objToSpawn, parent, spawnRot, poolType);
        }
        GameObject obj = objectPools[objToSpawn].Get();
        if(obj != null)
        {
            if(!cloneToPrefabMap.ContainsKey(obj))
            {
                cloneToPrefabMap.Add(obj, objToSpawn);
            }
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = spawnRot;
            obj.SetActive(setActive);

            if(typeof(T) == typeof(GameObject)) return obj as T;

            T component = obj.GetComponent<T>();
            if(component == null)
            {
                Debug.LogError($"Object {objToSpawn.name} doesn't have a comonent of type {typeof(T)}");
                return null;
            }
            return component;
        }
        return null;
    }
    //Normal Spawning
    public static T SpawnObject<T>(T typeOfPrefab, Vector3 spawnPos, Quaternion spawnRot, PoolType poolType = PoolType.gameObjects, bool isActive = false) where T : Component
    {
        return SpawnObject<T>(typeOfPrefab, spawnPos, spawnRot, poolType, isActive);
    }
    public static GameObject SpawnObject(GameObject objToSpawn, Vector3 spawnPos, Quaternion spawnRot, PoolType poolType = PoolType.gameObjects, bool isActive = false)
    {
        return SpawnObject<GameObject>(objToSpawn, spawnPos, spawnRot, poolType, isActive);
    }
    //Automatically Set the parent
    public static T SpawnObject<T>(T typeOfPrefab, Transform parent, Quaternion spawnRot, PoolType poolType = PoolType.gameObjects) where T : Component
    {
        return SpawnObject<T>(typeOfPrefab, parent, spawnRot, poolType);
    }
    public static GameObject SpawnObject(GameObject objToSpawn, Transform parent, Quaternion spawnRot, PoolType poolType = PoolType.gameObjects)
    {
        return SpawnObject<GameObject>(objToSpawn, parent, spawnRot, poolType);
    }
    #endregion
}
