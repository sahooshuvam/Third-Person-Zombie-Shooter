using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDestroyed : MonoBehaviour
{
    public float time = 0f;
    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        if (time >=5f)
        {
            Destroy(this.gameObject);
        }
    }
}
