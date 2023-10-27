using System;
using System.Linq;
using System.Net;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.service
{
    public class InformationManagement : MonoBehaviour
    {
        public GameObject prefab;
        
        
        public async void SeachInformation(string day,int month,int year, FirebaseFirestore db)
        {
            DelInformation();
            string document = month + "-" + year;
            
            Query query = db.Collection("Attendance")
                .Document(document)
                .Collection("data")
                .OrderBy("timecheck");
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
                       

                        var temp = FindObjectsOfType<Button>()
                            .Where(x => x.GetComponentInChildren<Text>().text == connect.timecheck.ToString("dd"))
                            .ToList().First();
                        temp.image.color = Color.green;

                                clone.GetComponentInChildren<Text>().text =
                            connect.check+ "\n" +
                            "thời gian: " + ChangeTime(connect.timecheck) ;
                        
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
        
        private void DelInformation()
        {
            foreach (Transform i in transform)
            {
                Destroy(i.gameObject);
            }
        }
    }
}
