using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float time,timeStore;
    private Vector3 Direction;
    public float Speed;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision b)
    {
        
        MF_AutoPool.Despawn(gameObject);
            
    }
    void OnEnable()
    {
        time=timeStore;
    }
    void OnCollisionStay(Collision b)
    {
            MF_AutoPool.Despawn(gameObject);
    }
    void OnTriggerStay(Collider b)
    {
        if(b.gameObject.tag=="Wall" || b.gameObject.tag=="enemy")// && b.gameObject.tag!="BigBullet" && b.gameObject.tag!="BigBullet2")
        {
            MF_AutoPool.Despawn(gameObject);
            //Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider b)
    {
        if(b.gameObject.tag=="Wall" || b.gameObject.tag=="enemy" )//&& b.gameObject.tag!="BigBullet" && b.gameObject.tag!="BigBullet2")
        {
          //
          
            MF_AutoPool.Despawn(gameObject);
            //Destroy(gameObject);
        }
    //
    
    }
    void Start()
    {
       
        rigidBody=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Direction=transform.forward*Speed;
        //rigidBody.AddForce(Direction*1000000000);
        rigidBody.velocity=Direction;
        //Destroy(gameObject,time);
        if(time<=0)
        {
            MF_AutoPool.Despawn(gameObject,time);
        }
        if(time>=0)
        {
            time-=Time.deltaTime;
        }
    }
}
