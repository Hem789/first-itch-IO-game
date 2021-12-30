using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool canChange=true,snireload=false;
    private Camera camu;
    public bool Pause=false,scoped=false,follow=true;
    public int gun1,sniper1,healthKit=0,level;
    private float overTime=1,Rclick;
    public float zoomFOV=5F, normalFOV,healthCount=100F;
    public int gun,sniper,Deaths;
    public bool zoom=false;
    public GameObject M16,M17,minimap,sspng,sphere,Pmenu,loading;
    public bool croach=false,sniperShoot=true;
    public Text bullet, health, kills;
    public new Text name;
   // public GameObject Retry;
    
    public void levelUP()
    {
        loading.SetActive(true);
    if(level>=2 && level<=5)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    else
    {
         Cursor.lockState = CursorLockMode.None;
         Cursor.visible=true;
        SceneManager.LoadScene(0);
        
    }

    }
       
  
    public void play()
    {
        Time.timeScale=1;
        Pause=false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible=false;
        minimap.SetActive(true);
    }
    public void hit(float a)
    {
        healthCount-=a;
        if(healthCount<0)
        {
            healthCount=0;
        }
        health.text=": "+healthCount+"/"+healthKit;
    }
    public void plus()
    {
        healthKit++;
        health.text=": "+healthCount+"/"+healthKit;
    }
    public void browncoin()
    {
        if(sniper<=10)
        {
        sniper+=5;
        }
        if(sniper>10 && sniper<15)
        {
            sniper=15;
        }
        if(zoom==true)
        {
        bullet.text=": "+sniper1+"/"+sniper;
        }
    }
    public void goldcoin()
    {
        if(gun<=960)
        {
            gun+=30;
        }
        if(gun>960 && gun<990)
        {
            gun=990;
        }
        if(zoom==false)
        {
        bullet.text=": "+gun1+"/"+gun;
        }
    }
    void Awake()
    {
        level=SceneManager.GetActiveScene().buildIndex;
        Time.timeScale=1;
        Pause=false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible=false;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        loading.SetActive(false);
        gun1=30;
        gun=990;
        sniper=10;
        sniper1=5;
        camu=Camera.main;
    }
    //public void FOV()
    //{
       
    //}
    public void sit()
    {
        croach=true;
    }
    public void stand()
    {
        croach=false;
    }
    public void shoot()
    {
        if(zoom==false)
        {
        if(gun1>0)
        {
            gun1--;
        }
        if(gun1==0)
        {
            canChange=false;
            StartCoroutine(reload());
            
        }
        bullet.text=": "+gun1+"/"+gun;
        }
        else
        {
        if(sniper1>0)
        {
            sniper1--;
            sniperShoot=false;
            canChange=false;
            StartCoroutine(reload());
        }
        if(sniper1==0 )
        {
            canChange=false;
            sniperShoot=false;
            StartCoroutine(reload());
            
        }
        bullet.text=": "+sniper1+"/"+sniper; 
        
    }
    }
    IEnumerator reload()
    {
        yield return new WaitForSeconds(2F);
        sniperShoot=true;
        if(zoom==false)
        {
        
        if(gun>30)
        {
            gun1=30;
            gun-=30;
        }
        if(gun<=30)
        {
            gun1=gun;
            gun=0;
        }
        bullet.text=": "+gun1+"/"+gun;
        }
        if(zoom==true)
        {
        
        if(sniper>5 && (sniper1==0|| snireload==true))
        {
            sniper1=5;
            sniper-=5;
            snireload=false;
        }
        if(sniper<=5 &&(sniper1==0|| snireload==true))
        {
            sniper1=sniper;
            sniper=0;
            snireload=false;
        }  
        bullet.text=": "+sniper1+"/"+sniper; 
        }
        canChange=true;
    }
    public void reloadPressed()
    {
        canChange=false;
        snireload=true;
        StartCoroutine(reload());
        
    }
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale=0;
            Pause=true;
            Pmenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible=true;
            minimap.SetActive(false);
        }
        if(Input.GetAxis("Mouse ScrollWheel")!=0)
        {
        if(scoped==false && Pause==false && canChange==true)
        {
        zoom=!zoom;
        //}
        if(zoom==true)
        {
            name.text="Sniper";
            bullet.text=": "+sniper1+"/"+sniper;
            M16.SetActive(false);
            M17.SetActive(true);
            //scope.SetActive(true);
        }
        if(zoom==false)
        {
            name.text="M16";
            bullet.text=": "+gun1+"/"+gun;
            M16.SetActive(true);
            M17.SetActive(true);
            //scope.SetActive(false);
        }
            }
        }

        if(zoom==true)
        {
            if(Rclick>0)
            {
                Rclick-=Time.deltaTime;
            }
            if(Input.GetAxis("Fire2")>0 && Rclick<=0)
            {
                Rclick=.2F;
                 scoped=!scoped;
        if(scoped==true)
        {
            //enter.SetActive(false);
            sspng.SetActive(true);
            normalFOV=camu.fieldOfView;
            camu.fieldOfView=zoomFOV;
            sphere.SetActive(false);
        }
        else
        {
            sphere.SetActive(true);
            sspng.SetActive(false);
            camu.fieldOfView=normalFOV;
        }
            }
        }

        if(Input.GetKey(KeyCode.X))
        {
             if(healthKit>0 && follow==true)
       {
           healthCount=100;
           healthKit--;
           health.text=": "+healthCount+"/"+healthKit;
       }
        }
    }
    
    // Update is called once per frame
    public void gameOver()
    {
        
        //Time.timeScale=0;
        //Pause=true;
        //Retry.SetActive(true);
        health.text=": 0/"+healthKit;
        if(overTime<=0)
        {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible=true;
        SceneManager.LoadScene(1);
        }
        else
        {
            overTime-=Time.deltaTime;
        }
    }
    public void enemy()
    {
        Deaths++;
        kills.text=": "+ Deaths;
    }
    public void over()
    {
        follow=false;
        healthCount--;
        if(healthCount<=0)
        {
            healthCount=0;
        }
        

    } 
}
