using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class GenerateQRCode : MonoBehaviour
{
    public Text check;
    private DateTime time;
    
    private void Start()
    {
        time = DateTime.Now;
        UpdateTime();
    }

    private void UpdateTime()
    {
        StartCoroutine(WaitForMin());
        if ( time.TimeOfDay < new TimeSpan(11,0,0)
            || time.TimeOfDay >new TimeSpan(13,0,0) && time.TimeOfDay <  new TimeSpan(16,0,0))
        {
            check.text = "Check in";
        }
        else
        {
            check.text = "Check out";
        }
    }
    private IEnumerator WaitForMin()
    {
        yield return new WaitForSeconds(60);
        time = DateTime.Now;
        Debug.Log(time.ToString());
        UpdateTime();
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256,256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    void OnGUI()
    {
        Texture2D myQR = generateQR(PlayerPrefs.GetString("id") + " " + time.ToString());
        GUI.DrawTexture(new Rect(100, 200, 512, 512), myQR, ScaleMode.ScaleToFit);
    }
}


