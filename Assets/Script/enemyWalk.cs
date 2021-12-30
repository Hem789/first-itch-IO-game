using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyWalk : MonoBehaviour
{
    public Transform t1,t2,t3,t4,t5,t6,target,gunpoint;
    public GameObject gun,handgun,backgun,coin;
    private GameObject player;
    private RaycastHit hit,nevRay;
    private float destroy,health=5;
    private Vector3 moveDirection,distance;
    public Animator anime;
    public bool follow,shoot,dead=false,doNevMesh=true;
    public bool seen=false;
    private Collider mainCollider;
    private Rigidbody[] rag;
    private Rigidbody Rb;
    public float swimf,swim;
    private NavMeshAgent agent;
    private GameManager manager;
    public LayerMask hataa;
    private int lvl;
    void Awake()
    {
        transform.gameObject.tag="enemy";
        manager=FindObjectOfType<GameManager>();
        lvl=manager.level;
    }
    void OnCollisionStay(Collision a)
    {
       
        if(a.gameObject.tag=="Water")
        {
            doNevMesh=false;
            agent.enabled=false;
            transform.LookAt(player.transform.position);
        }
    }
    // Start is called before the first frame update
    void OnCollisionEnter(Collision a)
    {
        
        /*if(a.gameObject.tag=="Bullet0")
        {
            health--;
        }
        if(a.gameObject.tag=="BigBullet")
        {
            health=0;
        }
        if(a.gameObject.tag=="BigBullet2")
        {
            health=0;
        }*/
        if(a.gameObject.tag=="Sniper")
        {
            health=0;
        }
        
    }
    void OnTriggerEnter(Collider a)
    {
        if(a.gameObject.tag=="Bullet")
        {
            health--;
            seen=true;
        }

    }
    void OnCollisionExit(Collision a)
    {
        if(a.gameObject.tag=="Water")
        {
            agent.enabled=true;
            doNevMesh=true;
        }
    }
    void Start()
    {
        anime.applyRootMotion=true;
        agent=GetComponent<NavMeshAgent>();
        agent.enabled=false;
        Rb=GetComponent<Rigidbody>();
        mainCollider=GetComponent<Collider>();
        rag=GetComponentsInChildren<Rigidbody>(true);
        ragdoll(false);
        target.position=t1.position;
        t1.transform.parent=null;
        t2.transform.parent=null;
        t3.transform.parent=null;
        t4.transform.parent=null;
        t5.transform.parent=null;
        t6.transform.parent=null;
        target.transform.parent=null;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if(health<=0 && dead==false)
        {
            manager.enemy();
            MF_AutoPool.Spawn(coin,transform.position+new Vector3(0,1,0),transform.rotation);
            dead=true;
            ragdoll(true);
        }
        if(transform.position.y<-13)
        {
            Destroy(gameObject);
        }      
    }
    void FixedUpdate()
    {
        player=GameObject.FindWithTag("Player");
        
        if(dead==false)
        {
        distance=transform.position-player.transform.position;
      
        }
    //}
    //void Update()
    //{
        if(!agent.isOnNavMesh)
        {
            agent.enabled=false;
            anime.applyRootMotion=true;
        }
        transform.rotation=Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
        player=GameObject.FindWithTag("Player");
        //anime.SetBool("Swim",false);
        if(dead==true)
        {
            t1.transform.parent=transform;
            t2.transform.parent=transform;
            t3.transform.parent=transform;
            t4.transform.parent=transform;
            t5.transform.parent=transform;
            t6.transform.parent=transform;
            agent.enabled=false;
            Destroy(gameObject,10);
        }
        if(dead==false )
        {
            if(manager.Deaths>2 && manager.follow==true)
            {
                seen=true;
                agent.enabled=true;
                anime.applyRootMotion=false;
                shoot=true;
                t1.gameObject.SetActive(false);
                t2.gameObject.SetActive(false);
                t3.gameObject.SetActive(false);
                t4.gameObject.SetActive(false);
                t5.gameObject.SetActive(false);
                t6.gameObject.SetActive(false);
            }
            if(transform.position.y<=swim+.2F)
            {
                anime.SetBool("reload",false);
                moveDirection=player.transform.position-transform.position;
                transform.LookAt(player.transform);
                transform.rotation=Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
                //anime.SetBool("Swim",true);
                agent.enabled=false;
                anime.applyRootMotion=true;
            
            if(moveDirection.magnitude>15)
            {
            transform.position=new Vector3(transform.position.x,swim,transform.position.z);
            transform.position+=transform.forward*0.1F;
            anime.SetBool("run",true);
            }
            if(moveDirection.magnitude<15)
            {
            anime.SetBool("run",false);
            transform.position=new Vector3(transform.position.x,swimf,transform.position.z);
            }
            foreach(Rigidbody rig in rag)
            {
                rig.useGravity=false;
            }
            Rb.useGravity=false;
            }

            if(transform.position.y>swim+0.2)
            {
                foreach(Rigidbody rig in rag)
            {
                rig.useGravity=true;
            }
            Rb.useGravity=true;
            }
            follow=false;
        anime.SetBool("move",false);
        gunpoint.LookAt(player.transform.position+new Vector3(0,1F,0));
       
        if(Physics.Raycast(gunpoint.position,gunpoint.transform.forward,out hit,50,~hataa))
        {
            if(hit.transform.gameObject.tag=="Player" )
            {
                seen=true;
            
                shoot=true;
                //}
            }
           
                if( manager.follow==false)
                {
                    seen=false;
                    shoot=false;
                    anime.applyRootMotion=true;
                    anime.SetBool("run",false);
                    anime.SetBool("move",true);
                    anime.SetBool("reload",false);
                }
            }
           
        if(seen==false && manager.Deaths<=2)
        {
        if(target.position==t6.position && moveDirection.magnitude<=2F) 
        {
            target.position=t1.position;
        }
         if(target.position==t5.position && moveDirection.magnitude<=2F)
        {
            target.position=t6.position;
        }
         if(target.position==t4.position && moveDirection.magnitude<=2F)
        {
            target.position=t5.position;
        }
         if(target.position==t3.position && moveDirection.magnitude<=2F)
        {
            target.position=t4.position;
        }
         if(target.position==t2.position && moveDirection.magnitude<=2F)
        {
            target.position=t3.position;
        }
         if(target.position==t1.position && moveDirection.magnitude<=2F)
        {
            target.position=t2.position;
        }
        moveDirection=target.position-transform.position;
        Quaternion lookway=Quaternion.LookRotation(new Vector3(moveDirection.x,0,moveDirection.z));
        transform.rotation=Quaternion.Slerp(transform.rotation,lookway,0.5F);
        anime.SetBool("move",true);
        }
        if(seen==true && doNevMesh==true && transform.position.y>swim+.5F)
        {
        agent.enabled=true;
        if(manager.follow==true)
        {
            agent.enabled=true;
        anime.applyRootMotion=false;
        //if( player.transform.position.y<3 )
        //{
            if(agent.isOnNavMesh)
        agent.SetDestination(player.transform.position);  
        //}

        moveDirection=player.transform.position-transform.position;
        if(agent.isOnNavMesh)
        {
        if(agent.remainingDistance>agent.stoppingDistance)
        {
            agent.updateRotation=true;
            agent.updatePosition=true;
            anime.SetBool("run",true);
        }
        if(agent.remainingDistance<=agent.stoppingDistance)
        {
            anime.SetBool("run",false);
            agent.updateRotation=false;
            agent.updatePosition=false;
            transform.LookAt(player.transform);
            transform.rotation=Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
        }
        }
        }
        /*if(moveDirection.magnitude>25)
        {
        shoot=false;
        anime.SetBool("shoot",false);
        } */
        } 
    }
    }
    void ragdoll(bool a)
    {
    gun.SetActive(false);
    handgun.SetActive(false);
    backgun.SetActive(false);
    foreach(Rigidbody rig in rag)
    {
        rig.useGravity=true;
    }
    Rb.useGravity=true;
    mainCollider.enabled=!a;
    anime.enabled=!a;       
    }
}