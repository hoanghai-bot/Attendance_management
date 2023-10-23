using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using main.Script.controller;
using UnityEngine;
using ZXing;

namespace main.Script.service
{
    public class ReadQRCode : MonoBehaviour
    {
        private WebCamTexture webCamTexture;
        private Rect screenRect;
        void Start()
        {
            screenRect = new Rect(0, 0, Screen.width, Screen.height);
            webCamTexture = new WebCamTexture();
            webCamTexture.requestedHeight = Screen.height;
            webCamTexture.requestedWidth = Screen.width;
            if (webCamTexture != null)
            {
                webCamTexture.Play();
            }
        }

        void OnGUI()
        {
            // Vẽ một camera trên màn hình
            GUI.DrawTexture(screenRect, webCamTexture, ScaleMode.ScaleToFit);
            // Đọc nội dung trên màn hình.
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                // Giải mã khung hình hiện tại.
                var result = barcodeReader.Decode(webCamTexture.GetPixels32(), webCamTexture.width, webCamTexture.height);
                if (result != null)
                {
                    Debug.Log("Dữ liệu giải mã được từ mã QR là: " +result.Text);
                    PlayerPrefs.SetString("scan",result.Text);
                    FindObjectOfType<ControllerHome>().ScanCheck();
                    this.enabled = false;
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
        }

       
    }
}