using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Firebase.Firestore;
using main.Script.service;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace main.Script.controller
{
    public class ControllerHome : FireBaseCommon
    {

        public GameObject success;
        public GameObject failure;
        public GameObject errorNetwork;
        public GameObject repeatError;
        public Text successText;
        public Text repeatText;
        
        
        string networkAddress = "192.168.1.0";  // Địa chỉ mạng
        string netmask = "255.255.255.0";

        async void Start()
        {
            base.Start();
            if (await CheckPermission("system"))
            {
                GetComponent<ReadQRCode>().camTexture.Stop();
                SceneManager.LoadScene("CreateQR");
            }
            
        }

        public async void ScanCheck()
        {
            DocumentReference docref = db.Collection("User").Document(IDAcc);
            DocumentSnapshot docsnapshot = await docref.GetSnapshotAsync();
            Connect connect = docsnapshot.ConvertTo<Connect>();
            
            // Query query = db.Collection("User");
            // QuerySnapshot snapshot = await query.GetSnapshotAsync();
            // foreach (var doc in snapshot)
            // {
                
                try
                {
                    if (PlayerPrefs.GetString("scan").StartsWith("Checkin")
                        || PlayerPrefs.GetString("scan").StartsWith("Checkout"))
                    {
                        
                        if (!IsIPInNetwork(GetDeviceIPAddress(),networkAddress,netmask))
                        {
                            errorNetwork.SetActive(true);
                            return;
                        }
                        var text =FindLastPart(PlayerPrefs.GetString("scan"));
                        DateTime temp = DateTime.Parse(text);
                        string check;
                        if (PlayerPrefs.GetString("scan").StartsWith("Checkin"))
                        {
                            check = "check in";
                            ChangeText(check,temp);
                        }
                        else
                        {
                            check = "check out";
                            ChangeText(check,temp);
                        }

                        if (check ==connect.check)
                        {
                            repeatError.SetActive(true);
                            return;
                        }
                        else
                        {

                            Dictionary<string, object> data = new Dictionary<string, object>()
                            {
                                { "check", check }
                            };

                            await db.Collection("User")
                                .Document(IDAcc)
                                .UpdateAsync(data);
                            
                            ScanUpdateCheckTime(temp,check,success);
                            return;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    failure.SetActive(true);
                    return;
                    throw;
                }
            //}
            failure.SetActive(true);
            
        }
        
        static bool IsIPInNetwork(string ipAddress, string networkAddress, string netmask)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            IPAddress network = IPAddress.Parse(networkAddress);
            IPAddress mask = IPAddress.Parse(netmask);

            // Thực hiện phép AND giữa địa chỉ IP và netmask, sau đó so sánh với địa chỉ mạng
            return (ip.Address & mask.Address) == network.Address;
        }

        public void clickButtonFailure(GameObject temp)
        {
            temp.SetActive(false);
            FindObjectOfType<ReadQRCode>().enabled = true;
            FindObjectOfType<ReadQRCode>().scanning = true;
            FindObjectOfType<ReadQRCode>().camTexture.Play();
        }

        private string FindLastPart(string text)
        {
            int startIndex = text.IndexOf(" ") + 1; // Tìm vị trí cuối cùng của khoảng trắng
            string lastPart = text.Substring(startIndex);
            Debug.Log(lastPart);
            return lastPart;
        }

        private void ChangeText(string check ,DateTime temp)
        {
            successText.text = check + " thành công\n"+ temp.ToString("hh:mm:ss");
            repeatText.text = "Bạn đã " + check + "\ntrước đấy";
        }
    }
}
