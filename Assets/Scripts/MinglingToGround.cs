using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinglingToGround : MonoBehaviour
{
    float destroyHeight;
    public float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.tag == "Ragdoll_Zombie")
        {
            Invoke("ReadyToSink", delayTime);
        }
    }

    public void SinkIntoGround()
    {
        this.transform.Translate(0f, -0.001f, 0f);
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }
    }

    public void ReadyToSink()
    {
        destroyHeight = Terrain.activeTerrain.SampleHeight(this.transform.position) - 5f;
        Collider[] colliderList = this.transform.GetComponentsInChildren<Collider>();
        foreach (Collider item in colliderList)
        {
            Destroy(item);
        }
        InvokeRepeating("SinkIntoGround", 5f, 0.1f);
    }
}
