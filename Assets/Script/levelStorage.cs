using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelStorage : MonoBehaviour
{
    public static int currentLevel,yourScore;
    public int don;
    public Animator anime;
    private Vector3 distance;
    private GameObject player;
    public Text level,score;
    void Start()
    {
        if(FindObjectOfType<GameManager>())
        {
        currentLevel=FindObjectOfType<GameManager>().level;
        }
        if(level)
        {
                int h=currentLevel-1;
            level.text="Level "+h;    
            //}
            score.text="Your Score="+yourScore+" kills,";
        }
        
    }
    void Update()
    {
         if(FindObjectOfType<GameManager>())
        {
        yourScore=FindObjectOfType<GameManager>().Deaths;
        }
        don=currentLevel;
        if(anime!=null)
        {
        player=GameObject.FindWithTag("Player");
        distance=transform.position-player.transform.position;
        if(distance.magnitude>30)
        {
            anime.enabled=false;
        }
        if(distance.magnitude<=30)
        {
            anime.enabled=true;
        }
        }
    }


    // Update is called once per frame
    public void Yes()
    {
        SceneManager.LoadScene(currentLevel);
    }

    // Update is called once per frame
    public void No()
    {
        SceneManager.LoadScene(0);
    }
    void OnTriggerEnter(Collider a)
    {
        if(a.gameObject.tag=="Player")
        {
        SaveManager.Save(this);
        if(FindObjectOfType<GameManager>())
        {
            FindObjectOfType<GameManager>().levelUP();
        }
        }
    }
    public void Reset()
    {
        currentLevel=0;
        don=1;
        SaveManager.Save(this);
    }

}
