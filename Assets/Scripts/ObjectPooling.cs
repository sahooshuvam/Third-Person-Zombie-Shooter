using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPooling : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int number;
    public float spawnRadius;
    public bool spawnOnStart = true;
    Vector3 result;
    public static ObjectPooling Instance;
    public List<GameObject> pool = new List<GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        return;
    }
    // Start is called before the first frame update
    void Start()
    {
        AddToPool(number);
    }
    public void AddToPool(int numbers)
    {
        for (int i = 0; i < numbers; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
            {
                result = hit.position;
                GameObject temp = Instantiate(zombiePrefab, result, Quaternion.identity);
                temp.SetActive(false);
                pool.Add(temp);
            }
            else
                i--;           
        }
    }

    public GameObject GetObjectFromPool(string tagName)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].gameObject.tag == tagName)
            {
                if (!pool[i].activeInHierarchy)
                {
                    return pool[i];
                }
            }
        }
        return null;
    }
}
