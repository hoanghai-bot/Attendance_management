using System;
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
                SceneManager.LoadScene("HomeSystem");
            }
            
        }

        public async void ScanCheck()
        {
            Query query = db.Collection("User");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (var doc in snapshot)
            {
                
                try
                {
                    if (PlayerPrefs.GetString("scan").StartsWith(doc.Id) )
                    {
                        if (!IsIPInNetwork(GetDeviceIPAddress(),networkAddress,netmask))
                        {
                            errorNetwork.SetActive(true);
                            return;
                        }
                        var text =FindLastPart(PlayerPrefs.GetString("scan"));
                        DateTime temp = DateTime.Parse(text);
                        string check;
                        if (temp.TimeOfDay < new TimeSpan(11,0,0)
                            || temp.TimeOfDay >new TimeSpan(13,0,0) && temp.TimeOfDay <  new TimeSpan(16,0,0))
                        {
                            check = "check in";
                            ChangeText(check);
                        }
                        else
                        {
                            check = "chech out";
                            ChangeText(check);
                        }

                        if (check == PlayerPrefs.GetString("check"))
                        {
                            repeatError.SetActive(true);
                            return;
                        }
                        else
                        {
                            PlayerPrefs.SetString("check",check);
                            ScanUpdateCheckTime(temp,check);
                            success.SetActive(true);
                            Debug.Log("check thanh cong " + DateTime.Now.TimeOfDay);
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
            }
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

        private void ChangeText(string check)
        {
            successText.text = check + "\nthành công";
            repeatText.text = "Bạn đã " + check + "\ntrước đấy";
        }
    }
}
