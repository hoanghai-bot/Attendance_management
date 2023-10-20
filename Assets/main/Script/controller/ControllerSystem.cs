using System.Collections;
using System.Collections.Generic;
using main.Script.controller;
using UnityEngine;

public class ControllerSystem : FireBaseCommon
{
    public GameObject button;
    // Start is called before the first frame update
    async void Start()
    {
        base.Start();
        if (await CheckPermission("system"))
        {
            button.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
