using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using main.Script.controller;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace main.Script.service
{
    public class ReadQRCode : MonoBehaviour
    {

        public bool scanning = true;
        public WebCamTexture camTexture;
        private Rect screenRect;
        public RawImage rawImage;
        
        void Start()
        {
            RequestCameraPermission();
            screenRect = new Rect(0, 0, Screen.width, Screen.height);
            camTexture = new WebCamTexture();
            camTexture.requestedHeight = Screen.height;
            camTexture.requestedWidth = Screen.width;
            if (camTexture != null)
            {
                camTexture.Play();
                rawImage.texture = camTexture;
                rawImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)camTexture.height / camTexture.width;
            }
        }
        
        void OnGUI()
        {
            // Vẽ một camera trên màn hình
            //GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
            // Đọc nội dung trên màn hình.
            try
            {
                if (scanning)
                {
                    IBarcodeReader barcodeReader = new BarcodeReader();
                    // Giải mã khung hình hiện tại.
                    var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
                    if (result != null)
                    {
                        Debug.Log("Dữ liệu giải mã được từ mã QR là: " + result.Text);
                        PlayerPrefs.SetString("scan", result.Text);
                        FindObjectOfType<ControllerHome>().ScanCheck();
                        scanning = false; // Dừng quét sau khi tìm thấy mã QR
                        camTexture.Stop();
                        enabled = false;
                    }
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
        }

        public void Buttonclick()
        {
            camTexture.Stop();
        }
        
        public void RequestCameraPermission()
        {
            Debug.Log("checkPermission");
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Application.RequestUserAuthorization(UserAuthorization.WebCam);
            }
        }
    }
}