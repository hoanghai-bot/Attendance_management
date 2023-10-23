using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using main.Script.service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace main.Script.controller
{
    public class ControllerHome : FireBaseCommon
    {

        public GameObject success;
        public GameObject failure;

        async void Start()
        {
            base.Start();
            if (await CheckPermission("system"))
            {
                SceneManager.LoadScene("HomeSystem");
            }
            
        }

        public async void ScanCheck()
        {
            
            if (PlayerPrefs.GetString("scan")=="")
            {
                failure.SetActive(true);
                return;
            }
            Query query = db.Collection("user");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (var doc in snapshot)
            {
                if (PlayerPrefs.GetString("scan") == doc.Id)
                {
                    ScanUpdateCheckTime();
                    success.SetActive(true);
                    Debug.Log("check thanh cong " + DateTime.Now.TimeOfDay);
                }
            }
            
            
        }

        public void clickButton()
        {
            failure.SetActive(false);
            FindObjectOfType<ReadQRCode>().enabled = true;
        }
        
    }
}
