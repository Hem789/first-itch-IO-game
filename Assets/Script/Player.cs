using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private RaycastHit ground,ground1;
    private BoxCollider colli;
    //private Collider[] ragdoll;
    public Rigidbody rb;
    private Vector3 Direction;
    public float moveSpeed,swim,swimf,height,ctime;
    private float time=8,reloadTime;
    //public Joystick joystick,firestick;
    public Animator anime;
    public Transform pivot,piv,sphere,cam;
    //public GameObject body;
    //public FixedButton jumpbool,firebool;//croachbool;
    private bool ragdollcontrol=false,croach=false,falling=false;
    public AudioSource Swimming,reloader;
    public AudioClip reloading;
    private GameManager manager;
    private IK shoot;
    public Collision col;
    void OnCollisionEnter(Collision a)
    {
        if(a.gameObject.tag=="Bullet0")
        {
            manager.hit(1);
        }
        if(a.gameObject.tag=="Sniper" || a.gameObject.tag=="BigBullet2")
        {
            manager.hit(20);
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        manager=FindObjectOfType<GameManager>();
       // joystick=FindObjectOfType<Joystick>();
        colli=GetComponent<BoxCollider>();
        rb=GetComponent<Rigidbody>();
        //ragdoll=GetComponentsInChildren<rb>(true);
        shoot=GetComponent<IK>();
        doll(ragdollcontrol);
        Swimming.enabled=false;
        anime.Play("idle");
    }
    void LateUpdate()
    {
        if(Physics.Raycast(transform.position+new Vector3(0,1F,0),-transform.up,out ground,50))
        {
            if(transform.position.y>swim)
            {
        rb.AddForce(transform.up*(-5000));
            }
            if(ground.transform.gameObject.tag!="Water")
            {
           if(ground.distance>height )
           {
            falling=true;
           }
           
            }
            if(ground.distance<=1.5F && falling==true)
            {
            ragdollcontrol=true;
            doll(true);
            //}
            //if(transform.position.y<=ground.point.y+1 && falling==true)
            //{
                //transform.position=new Vector3(transform.position.x,ground.point.y,transform.position.z);
                rb.constraints=RigidbodyConstraints.FreezePositionY;
            }
        }
       
    }
    void FixedUpdate()
    {
        //if(ragdollcontrol==true)
        //{
        //    piv.transform.position=new Vector3(body.transform.position.x,4.5F,body.transform.position.z);
        //}
        if(croach==true)
        {
            if(Input.GetAxis("Fire1")>0)
            {
            if(Input.GetAxis("Vertical")>=0)
            {
            anime.SetFloat("croach",1.5F);
            }
            else
            {
                anime.SetFloat("croach",-1);
                rb.AddForce(transform.forward*-30000);
            }
            }
            else
            {
                anime.SetFloat("croach",1.5F);
            }
        }
        if(transform.position.y<=swim+.2F && transform.position.y>=swimf-0.2F && ragdollcontrol==false)
        {
            anime.SetBool("Swim",true);
            //if(Input.GetAxis("Vertical")==0||Input.GetAxis("Horizontal")==0)
            if(Input.GetAxis("Vertical")==0 || Input.GetAxis("Horizontal")==0)
            {
                anime.Play("Swimming Idle");
            transform.position=new Vector3(transform.position.x,swimf,transform.position.z);
            //transform.rotation=Quaternion.Euler(0,cam.transform.rotation.y,0);
            //transform.LookAt(new Vector3(cam.transform.position.x,transform.position.y,cam.transform.position.z));
            Swimming.enabled=false;
            }
            //if(Input.GetAxis("Vertical")!=0||Input.GetAxis("Horizontal")!=0)
            if(Input.GetAxis("Vertical")!=0 || Input.GetAxis("Horizontal")!=0)
            {
                anime.Play("Swim Forward");
            transform.position=new Vector3(transform.position.x,swim,transform.position.z);
            transform.position+=transform.forward*0.1F;
            //transform.LookAt(null);
            Swimming.enabled=true;
            }
            rb.useGravity=false;
            
        }
        if(transform.position.y<swimf-0.2 && ragdollcontrol==false)
        {
            anime.SetBool("Swim",true);
            transform.position+=new Vector3(0,0.03F,0);
            rb.useGravity=false;
        }
        if(transform.position.y>swim+0.2 && ragdollcontrol==false)
        {
            rb.useGravity=true;
            Swimming.enabled=false;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.Pause==true || manager.follow==false )
        {
            Swimming.enabled=false;
            reloader.enabled=false;
        }
        if(manager.Pause==false)
        {
        reloader.enabled=true;
        pivot.transform.parent=null;
        pivot.transform.position=transform.position;
        if(ragdollcontrol==true)
        {
            manager.over();
            Swimming.enabled=false;
        if(time<=0)
            {
                manager.gameOver();
            }
            else
            {
                time-=Time.deltaTime;
            } 
        }
        if(ragdollcontrol==false)
        {
        if( manager.healthCount==0)
        {
            
            ragdollcontrol=true;
            doll(true);
            
            rb.constraints=RigidbodyConstraints.FreezePositionY;
        }
        if(Input.GetKey(KeyCode.R))
        {
            if(manager.canChange==true)
        {
            if((manager.zoom==true &&manager.sniper>0)||(manager.zoom==false &&manager.gun>0))
            {
        reloader.PlayOneShot(reloading,1);
        anime.SetBool("reload",true);
        manager.reloadPressed();
            }
        }
        }
        if(Input.GetKey(KeyCode.C) && ctime<=0)
        {
        croach=!croach;
        //anime.applyRootMotion=!croach;
        sitting(croach);
        ctime=1;
        }
        if(ctime>0)
        {
            ctime-=Time.deltaTime;
        }
        float yStr=Direction.y;
        Direction=piv.transform.forward*Input.GetAxis("Vertical")+piv.transform.right*Input.GetAxis("Horizontal");
        Direction=Direction.normalized * moveSpeed;
        Direction.y=yStr;
        anime.SetBool("run",false);
        anime.SetBool("ufri",false);
        anime.SetBool("Fire",false);
        anime.SetBool("Swim",false);
        if(manager.canChange==true)
        {
        anime.SetBool("reload",false);
        }
        
        if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0)
        {
            anime.SetBool("run",true);
            rb.AddForce(transform.forward*90000);
            Quaternion turn=Quaternion.LookRotation(new Vector3(Direction.x,0,Direction.z));
            if(Input.GetAxis("Fire1")==0)//firebool.Pressed==false //&& firestick.Horizontal==0 && firestick.Vertical==0 && Input.GetKey(KeyCode.M)==false)
            {

            transform.rotation=Quaternion.Slerp(transform.rotation,turn,.5F);
            }
            else
            {
                if((manager.zoom==true && (manager.sniper1>0 || manager.sniper>5)) || (manager.zoom==false && (manager.gun1>0 || manager.gun>30)))
                {
                if(Input.GetAxis("Vertical")<0)
                transform.position+=transform.right*Input.GetAxis("Horizontal")*.035F;
                if(Input.GetAxis("Vertical")>=0)
                transform.position+=transform.right*Input.GetAxis("Horizontal")*.07F;
                
                }
                else
                {
                    transform.rotation=Quaternion.Slerp(transform.rotation,turn,.5F);
                }
            }

        }
       
        if(Input.GetButtonDown("Jump") )//|| jumpbool.Pressed)
        {
           jump();
        
        }
        if(Input.GetAxis("Fire1")>0)//|| firebool.Pressed==true || firestick.Horizontal!=0 || firestick.Vertical!=0))// && (Input.GetAxis("Vertical")>=0 ||Input.GetAxis("Vertical")>0) && Input.GetAxis("Horizontal")>=-.6F &&Input.GetAxis("Horizontal")<=.6F)
        {
           fire();
            
        }
        else
        {
            anime.SetBool("back",false);
        }
    
        
        }
        }
    }
    void jump()
    {
        anime.SetBool("ufri",true);
        rb.AddForce(transform.up*300000);
    }
    void fire()
    {
        if(manager.gun1!=0 && manager.sniper1!=0)
        {
            if(Input.GetAxis("Vertical")>=0 )
            {
                anime.SetBool("Fire",true);
                reloadTime=0;
            }
            else
            {
                rb.AddForce(transform.forward*-90000);
            }
        }
        else
        {
            if((manager.sniper1<=0 && manager.zoom==true && manager.sniper>0) || (manager.gun1<=0 && manager.zoom==false&& manager.gun>0))
            { 
            anime.SetBool("reload",true);
            if(reloadTime==0)
            {
            reloader.PlayOneShot(reloading,1);
            reloadTime++;
            }
            }
        }
    }
    

    
    void doll(bool ragdollcontrol)
    {
        shoot.Gun.SetActive(false);
        shoot.rifle.enabled=false;
        rb.useGravity=!ragdollcontrol;
        colli.enabled=!ragdollcontrol;
        anime.enabled=!ragdollcontrol;
    }    
    void sitting(bool sit)
    {
        if(sit==true)
        {
            colli.size=new Vector3(0.5F,1.2F,1F);
            colli.center=new Vector3(0,0.6F,0.2F);
            anime.SetBool("sit",true);
            manager.sit();
        }
        if(sit==false)
        {
            colli.size=new Vector3(0.7F,1.843569F,0.7F);
            colli.center=new Vector3(0,0.9F,0);
            anime.SetBool("sit",false);
            manager.stand();
        }

    }
   
}