using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birth : MonoBehaviour
{
    public float time,Xmin,Xmax,Zmin,Zmax;
    private float timeStore,x,z;
    public GameObject enemy;
    void Start()
    {
        timeStore=time;
    }
       void FixedUpdate()
    {
        if(time>0)
        {
            time-=Time.deltaTime;
        }
        else
        {
            x=Random.Range(Xmin,Xmax);
            z=Random.Range(Zmin,Zmax);
            Instantiate(enemy,new Vector3(x,3,z),Quaternion.identity);
            time=timeStore;
        }
    }
}
