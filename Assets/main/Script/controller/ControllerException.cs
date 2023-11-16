using System;
using System.Collections;
using System.Collections.Generic;
using main.Script.service;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerException : MonoBehaviour
{
    public GameObject camera;

    public void ClickCheck(bool checkin)
    {
        var find = camera.GetComponent<GenerateQRCode>();
        find.time = DateTime.Now;
        
        if (checkin)
        {
            find.check.text = "Check in\n(chỉ hiệu lực trong 1 phút)";
            find.checkQR = "Checkin";
        }
        else
        {
            find.check.text = "Check out\n(chỉ hiệu lực trong 1 phút)";
            find.checkQR = "Checkout";
        }
        
        find.enabled = true;
        StopCoroutine(WaitOneMin());
        StartCoroutine(WaitOneMin());
    }

    private IEnumerator WaitOneMin()
    {
        yield return new WaitForSeconds(60);
        SceneManager.LoadScene("CreateQR");
    }
}
