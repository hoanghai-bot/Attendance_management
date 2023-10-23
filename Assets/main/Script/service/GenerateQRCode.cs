using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class GenerateQRCode : MonoBehaviour
{
    // private DateTime time;
    //
    // private void Start()
    // {
    //     time = DateTime.Now;
    //     StartCoroutine()
    // }
    //
    // private IEnumerator waitForMin()
    // {
    //     yield return new WaitForSeconds(60);
    // }

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
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    void OnGUI()
    {
        Texture2D myQR = generateQR(PlayerPrefs.GetString("id"));
        if (GUI.Button(new Rect(300, 300, 256, 256), myQR, GUIStyle.none)) { }
    }
}


