using System;
using System.Net;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.service
{
    public class InformationManagement : MonoBehaviour
    {
        public GameObject prefab;
        
        string networkAddress = "192.168.1.0";  // Địa chỉ mạng
        string netmask = "255.255.255.0";
        
        public async void SeachInformation(string day,int month,int year, FirebaseFirestore db)
        {
            DelInformation();
            Query query = db.Collection("attendance data").Document(month + "-" + year).Collection(day);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (var doc in snapshot)
            {
                Connect connect = doc.ConvertTo<Connect>();
                if (doc.Exists)
                {
                    if (connect.id == PlayerPrefs.GetString("id"))
                    {
                        var clone = Instantiate(prefab, transform);
                        clone.SetActive(true);
                        string net;
                        if (IsIPInNetwork(connect.network,networkAddress,netmask))
                        {
                            net = "PUTOEDU";
                        }
                        else
                        {
                            net = "không rõ";
                        }

                        clone.GetComponentInChildren<Text>().text =
                            connect.check+ "\n" +
                            "thời gian: " + ChangeTime(connect.timecheck) + "\n" +
                            "mạng : " + net;

                    }
                }
            }
            
        }
        
        private string ChangeTime(DateTime time)
        {

            // Chuyển đổi thời gian sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local);

            // Định dạng thời gian thành chuỗi với múi giờ của Việt Nam (ICT)
            string formattedTimeVietnam = vietnamTime.ToString("HH:mm dd/MM/yyyy");
            return formattedTimeVietnam;
        }
        
        static bool IsIPInNetwork(string ipAddress, string networkAddress, string netmask)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            IPAddress network = IPAddress.Parse(networkAddress);
            IPAddress mask = IPAddress.Parse(netmask);

            // Thực hiện phép AND giữa địa chỉ IP và netmask, sau đó so sánh với địa chỉ mạng
            return (ip.Address & mask.Address) == network.Address;
        }
        private void DelInformation()
        {
            foreach (Transform i in transform)
            {
                Destroy(i.gameObject);
            }
        }
    }
}
