﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(player.transform.position.x,20,player.transform.position.z);
    }
}
