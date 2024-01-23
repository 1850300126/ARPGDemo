using EasyUpdateDemoSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 普通类 对象 对象池数据
/// </summary>
public class ObjectPoolData
{
    public ObjectPoolData(object obj)
    {
        PushObj(obj);
    }
    // 对象容器
    public Queue<object> poolQueue = new Queue<object>();
    /// <summary>
    /// 将对象放进对象池
    /// </summary>
    public void PushObj(object obj)
    {
        poolQueue.Enqueue(obj);
    }

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <returns></returns>
    public object GetObj()
    {
        return poolQueue.Dequeue();
    }
}
/// <summary>
/// GameObject对象池数据
/// </summary>
public class GameObjectPoolData
{
    // 对象池中 父节点
    public GameObject fatherObj;
    // 对象容器
    public Queue<GameObject> poolQueue;

    public GameObjectPoolData(GameObject obj, GameObject poolRootObj)
    {
        // 创建父节点 并设置到对象池根节点下方
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.SetParent(poolRootObj.transform);
        poolQueue = new Queue<GameObject>();
        // 把首次创建时候 需要放入的对象 放进容器
        PushObj(obj);
    }


    /// <summary>
    /// 将对象放进对象池
    /// </summary>
    public void PushObj(GameObject obj)
    {
        // 对象进容器
        poolQueue.Enqueue(obj);
        // 设置父物体
        obj.transform.SetParent(fatherObj.transform);
        // 设置隐藏
        obj.SetActive(false);
    }

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj(Transform parent = null)
    {
        GameObject obj = poolQueue.Dequeue();

        // 显示对象
        obj.SetActive(true);
        // 设置父物体
        obj.transform.SetParent(parent);
        if (parent == null)
        {
            // 回归默认场景
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(obj, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        return obj;
    }
}
public class PoolSystem : MonoBehaviour
{   
    public static PoolSystem instance;
    // 根节点
    [SerializeField]
    private GameObject poolRootObj;
    /// <summary>
    /// GameObject对象容器
    /// </summary>
    public Dictionary<string, GameObjectPoolData> gameObjectPoolDic = new Dictionary<string, GameObjectPoolData>();
    /// <summary>
    /// 普通类 对象容器
    /// </summary>
    public Dictionary<string, ObjectPoolData> objectPoolDic = new Dictionary<string, ObjectPoolData>();

    public void OnLoaded()
    {
        instance = this;


        poolRootObj = this.gameObject;
    }

    #region GameObject对象相关操作

    /// <summary>
    /// 获取GameObject,但是如果没有则返回Null
    /// </summary>
    public GameObject GetGameObject(string assetName, Transform parent = null)
    {
        GameObject obj = null;
        // 检查有没有这一层
        if (gameObjectPoolDic.TryGetValue(assetName, out GameObjectPoolData poolData) && poolData.poolQueue.Count > 0)
        {
            obj = poolData.GetObj(parent);
        }
        return obj;
    }

    /// <summary>
    /// GameObject放进对象池
    /// </summary>
    public void PushGameObject(GameObject obj)
    {
        string name = obj.name;
        // 现在有没有这一层
        if (gameObjectPoolDic.TryGetValue(name, out GameObjectPoolData poolData))
        {
            poolData.PushObj(obj);
        }
        else
        {
            gameObjectPoolDic.Add(name, new GameObjectPoolData(obj, poolRootObj));
        }
    }

    #endregion

    #region 普通对象相关操作
    /// <summary>
    /// 获取普通对象
    /// </summary>
    public T GetObject<T>() where T : class, new()
    {
        T obj;
        if (CheckObjectCache<T>())
        {
            string name = typeof(T).FullName;
            obj = (T)objectPoolDic[name].GetObj();
            return obj;
        }
        else
        {
            return new T();
        }
    }

    /// <summary>
    /// GameObject放进对象池
    /// </summary>
    /// <param name="obj"></param>
    public void PushObject(object obj)
    {
        string name = obj.GetType().FullName;
        // 现在有没有这一层
        if (objectPoolDic.ContainsKey(name))
        {
            objectPoolDic[name].PushObj(obj);
        }
        else
        {
            objectPoolDic.Add(name, new ObjectPoolData(obj));
        }
    }

    private bool CheckObjectCache<T>()
    {
        string name = typeof(T).FullName;
        return objectPoolDic.ContainsKey(name) && objectPoolDic[name].poolQueue.Count > 0;
    }
    #endregion


    #region 删除
    /// <summary>
    /// 删除全部
    /// </summary>
    /// <param name="clearGameObject">是否删除游戏物体</param>
    /// <param name="clearCObject">是否删除普通C#对象</param>
    public void Clear(bool clearGameObject = true, bool clearCObject = true)
    {
        if (clearGameObject)
        {
            for (int i = 0; i < poolRootObj.transform.childCount; i++)
            {
                Destroy(poolRootObj.transform.GetChild(i).gameObject);
            }
            gameObjectPoolDic.Clear();
        }

        if (clearCObject)
        {
            objectPoolDic.Clear();
        }
    }

    public void ClearAllGameObject()
    {
        Clear(true, false);
    }
    public void ClearGameObject(string prefabName)
    {
        GameObject go = poolRootObj.transform.Find(prefabName).gameObject;
        if (ReferenceEquals(go, null) == false)
        {
            Destroy(go);
            gameObjectPoolDic.Remove(prefabName);

        }

    }
    public void ClearGameObject(GameObject prefab)
    {
        ClearGameObject(prefab.name);
    }

    public void ClearAllObject()
    {
        Clear(false, true);
    }
    public void ClearObject<T>()
    {
        objectPoolDic.Remove(typeof(T).FullName);
    }
    public void ClearObject(Type type)
    {
        objectPoolDic.Remove(type.FullName);
    }
    #endregion

    // public Dictionary<string, APISystem.APICallFunction> api_functions = new Dictionary<string, APISystem.APICallFunction>();

    // [System.Serializable]
    // public class Pool
    // {
    //     public string tag;
    //     public GameObject prefab;
    //     public int size;
    //     public Pool()
    //     {

    //     }
    //     public Pool(string tag, GameObject prefab, int size)
    //     {
    //         this.tag = tag;
    //         this.prefab = prefab;
    //         this.size = size;
    //     }
    // }

    // [SerializeField]
    // public List<Pool> pools = new List<Pool>();

    // Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    // Dictionary<string, GameObject> pools_dictionary = new Dictionary<string, GameObject>();

    // public static PoolSystem instance;    //单例模式，便于访问对象池

    // private void Awake()
    // {
    //     instance = this;
    // }

    // public void OnLoaded()
    // {
    //     api_functions.Add("add_pool", AddPool);
    //     api_functions.Add("push_from_pool", PushFromPool);
    //     api_functions.Add("push_from_pool_distory_by_time", PushFromPoolAndDistoryByTime);

    //     APISystem.instance.RegistAPI("pool_system", OnPoolSystemAPIFunction);

    //     pools.Clear();
    // }

    // public object OnPoolSystemAPIFunction(string function, object[] param)
    // {
    //     if (api_functions.ContainsKey(function) == true)
    //         return api_functions[function](param);

    //     return null;
    // }

    // public GameObject PushFromPool(string tag, Vector3 positon, Quaternion rotation)     //从对象池中获取对象的方法
    // {
    //     if (!poolDictionary.ContainsKey(tag))  //如果对象池字典中不包含所需的对象池
    //     {
    //         Debug.Log("Pool: " + tag + " does not exist");
    //         return null;
    //     }
    //     GameObject objectToSpawn;
    //     if (poolDictionary[tag].Count <= 0)
    //     {
    //         objectToSpawn = Instantiate(pools_dictionary[tag], transform);
    //     }
    //     else
    //     {
    //         objectToSpawn = poolDictionary[tag].Dequeue();//出队，从对象池中获取所需的对象
    //     }


    //     objectToSpawn.transform.position = positon;  //设置获取到的对象的位置
    //     objectToSpawn.transform.rotation = rotation; //设置对象的旋转
    //     objectToSpawn.SetActive(true);                //将对象从隐藏设为激活

    //     //poolDictionary[tag].Enqueue(objectToSpawn);     //再次入队，可以重复使用，如果需要的对象数量超过对象池内对象的数量，在考虑扩大对象池
    //     //这样重复使用就不必一直生成和消耗对象，节约了大量性能
    //     return objectToSpawn;  //返回对象
    // }

    // /// <summary>
    // /// 从对象池中获取对象
    // /// </summary>
    // /// <param name="tag">param[0]</param>
    // /// <param name="positon">param[1]</param>
    // /// <param name="rotation">param[2]</param>
    // /// <returns>返回物品</returns>
    // public GameObject PushFromPool(object[] param)     //从对象池中获取对象的方法
    // {
    //     if (!poolDictionary.ContainsKey((string)param[0]))  //如果对象池字典中不包含所需的对象池
    //     {
    //         // tag does not exist
    //         Debug.Log("Pool: " + param[0] + " does not exist");
    //         return null;
    //     }
    //     GameObject objectToSpawn;
    //     // poolDictionary[tag]
    //     if (poolDictionary[(string)param[0]].Count <= 0)
    //     {
    //         // pools_dictionary[tag]
    //         objectToSpawn = Instantiate(pools_dictionary[(string)param[0]], transform);
    //     }
    //     else
    //     {
    //         // poolDictionary[tag]
    //         objectToSpawn = poolDictionary[(string)param[0]].Dequeue();//出队，从对象池中获取所需的对象
    //     }


    //     objectToSpawn.transform.position = (Vector3)param[1];  //设置获取到的对象的位置
    //     objectToSpawn.transform.rotation = (Quaternion)param[2]; //设置对象的旋转
    //     objectToSpawn.SetActive(true);                //将对象从隐藏设为激活

    //     //poolDictionary[(string)param[0]].Enqueue(objectToSpawn);     //再次入队，可以重复使用，如果需要的对象数量超过对象池内对象的数量，在考虑扩大对象池
    //     //这样重复使用就不必一直生成和消耗对象，节约了大量性能
    //     return objectToSpawn;  //返回对象
    // }

    // /// <summary>
    // /// 从对象池中获取对象,然后过time秒回收
    // /// </summary>
    // /// <param name="tag">param[0]</param>
    // /// <param name="positon">param[1]</param>
    // /// <param name="rotation">param[2]</param>
    // /// <param name="time">param[3]</param>
    // /// <returns>返回物品</returns>
    // public GameObject PushFromPoolAndDistoryByTime(object[] param)
    // {
    //     GameObject go = PushFromPool((string)param[0], (Vector3)param[1], (Quaternion)param[2]);
    //     StartCoroutine(PushFromPoolDistroy((string)param[0], go, (float)param[3]));
    //     return go;
    // }

    // private IEnumerator PushFromPoolDistroy(string tag, GameObject objectToSpawn, float time)
    // {
    //     yield return new WaitForSeconds(time);
    //     PullFromPool(tag, objectToSpawn);
    // }

    // /// <summary>
    // /// 卸载对象的方法
    // /// </summary>
    // /// <param name="tag"></param>
    // /// <param name="objectToSpawn"></param>
    // public void PullFromPool(string tag, GameObject objectToSpawn)
    // {
    //     objectToSpawn.SetActive(false);
    //     if (!poolDictionary[tag].Contains(objectToSpawn))
    //         poolDictionary[tag].Enqueue(objectToSpawn);

    // }

    // /// <summary>
    // /// 添加对象池(标签，GameObject，数量)
    // /// </summary>
    // public object AddPool(object[] param)
    // {
    //     string tag = (string)param[0];

    //     // 判断是否已有
    //     foreach(Pool _pool in pools)
    //     {
    //         if(_pool.tag.Contains(tag))
    //         {
    //             return null;
    //         }
    //     }

    //     GameObject go = (GameObject)param[1];
    //     int size = (int)param[2];
    //     Pool pool = new Pool(tag, go, size);

    //     pools.Add(pool);

    //     UpdateDictionary(pool);

    //     return null;
    // }

    // /// <summary>
    // /// 更新对象池字典用于索引，即 poolDictionary & pools_dictionary
    // /// </summary>
    // public void UpdateDictionary(Pool pool)
    // {
    //     Queue<GameObject> objectPool = new Queue<GameObject>();     //为每个对象池创建队列
    //     for (int i = 0; i < pool.size; i++)
    //     {
    //         GameObject obj = Instantiate(pool.prefab, transform);
    //         obj.SetActive(false);   //隐藏对象池中的对象
    //         objectPool.Enqueue(obj);//将对象入队
    //     }
    //     // 通过tag索引Queue
    //     poolDictionary.Add(pool.tag, objectPool);   //添加到字典后可以通过tag来快速访问对象池
    //                                                 // 通过tag索引prefab
    //     pools_dictionary.Add(pool.tag, pool.prefab);
    // }
}
