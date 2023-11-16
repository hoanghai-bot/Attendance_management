using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

namespace main.Script.service
{
    public class GenerateQRCode : MonoBehaviour
    {
        public Text check;
        public DateTime time;
        public string checkQR;
    
        private void Start()
        {
            time = DateTime.Now;
            var temp = SceneManager.GetActiveScene();
            if (temp.name == "CreateQR")
            {
                UpdateTime();
            }
            
        }

        private void UpdateTime()
        {
            StartCoroutine(WaitForMin());
            if ( time.TimeOfDay < new TimeSpan(11,0,0)
                 || time.TimeOfDay >new TimeSpan(13,0,0) && time.TimeOfDay <  new TimeSpan(16,0,0))
            {
                check.text = "Check in";
                checkQR = "Checkin";
            }
            else
            {
                check.text = "Check out";
                checkQR = "Checkout";
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
            
            Texture2D myQR = generateQR(checkQR + " " + time.ToString("dd/MM/yyyy HH:mm:ss"));
        
            GUI.DrawTexture(new Rect((Screen.width-256)/2, (Screen.height-256)/2, 256, 256), myQR, ScaleMode.ScaleToFit);
        }
    
        
    }
}


