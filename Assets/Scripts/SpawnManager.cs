using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float time;
    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if (time > 3f)
        {
            GameObject zombieFromPool = ObjectPooling.Instance.GetObjectFromPool("Zombie");
            if (zombieFromPool != null)
            {
                zombieFromPool.SetActive(true);
            }
            time = 0f;
        }
    }
}
