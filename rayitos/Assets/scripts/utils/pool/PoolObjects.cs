using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YaguarLib.Pool
{
    public class PoolObjects : MonoBehaviour
    {
        [SerializeField] Transform container;

        [SerializeField] List<GameObject> objectsToPool;
        Dictionary<string, List<GameObject>> all;

        private void Awake()
        {
            all = new Dictionary<string, List<GameObject>>();
            foreach (GameObject go in objectsToPool)
            {
                all.Add(go.name, new List<GameObject>());
                AddNeObject(go.name);
                Debug.Log(go.name);
                go.SetActive(false);
                Debug.Log(go);
            }
        }
        public GameObject Get(string key)
        {
            foreach (KeyValuePair<string, List<GameObject>> d in all)
            {
                if (d.Key == key)
                {
                    GameObject go = GetObjectInDic(d.Value);
                    if (go == null)
                        go = AddNeObject(key);
                    go.gameObject.SetActive(true);
                    return go;
                }
            }
            Debug.LogError("No obj on pool: " + key);
            return null;
        }
        GameObject GetObjectInDic(List<GameObject> allInDic)
        {
            print(allInDic.Count);
            foreach (GameObject go in allInDic)
            {
                print(go);
                print(go.activeSelf);
                if (!go.activeSelf)
                    return go;
            }
            return null;
        }
        public GameObject AddNeObject(string key)
        {
            print("AddNeObject " + key);
            foreach (GameObject go in objectsToPool)
            {
                if (key == go.name)
                {
                    print("key " + key);
                    foreach (KeyValuePair<string, List<GameObject>> d in all)
                    {
                        print(d.Key);
                        if (d.Key == key)
                        {
                            print("key " + key + " - " + d.Key);
                            GameObject newGO = Instantiate(go, container);
                            newGO.name = key;
                            newGO.SetActive(false);
                            d.Value.Add(newGO);
                            return newGO;
                        }
                    }
                }
            }
            return null;
        }
        public void Pool(GameObject go)
        {
            print("Pool "  + go  + " " + all.Count);
            go.SetActive(false);
            go.transform.SetParent(container);
        }
    }
}