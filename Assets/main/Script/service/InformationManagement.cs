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
        
        
        public async void SeachInformation(int month,int year,int day, FirebaseFirestore db)
        {
            DelInformation();
            
            string document = month + "-" + year;
            
            Query query = db.Collection("Attendance")
                .Document(document)
                .Collection("data")
                .OrderByDescending("timecheck");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (var doc in snapshot)
            {
                Connect connect = doc.ConvertTo<Connect>();
                if (connect.id == PlayerPrefs.GetString("id") )
                {
                    var temp = FindObjectsOfType<Button>()
                        .Where(x => x.GetComponentInChildren<Text>().text == connect.timecheck.ToString("dd"))
                        .ToList().First();
                    temp.image.color = Color.green;
                    if (connect.timecheck.Day == day)
                    {
                        var clone = Instantiate(prefab, transform);
                        clone.SetActive(true);
                    
                        clone.GetComponentInChildren<Text>().text =
                            connect.check + "\n" +
                            "thời gian: " + ChangeTime(connect.timecheck);
                    }
                }
                if (month == DateTime.Now.Month && year == DateTime.Now.Year)
                {
                    var findButton = FindObjectsOfType<Button>()
                        .Where(x => x.GetComponentInChildren<Text>().text == DateTime.Now.Day.ToString("00"))
                        .ToList().First();
                    findButton.image.color = new Color(0,1,1);
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
