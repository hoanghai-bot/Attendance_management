using System;
using Firebase.Firestore;
using main.Script.controller;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.service
{
    public class AttendanceTableManagement : FireBaseCommon
    {
        public GameObject wait;
        public GameObject prefab;
        //public GameObject pb;
        void Start()
        {
            base.Start();
        }

        public async void DrawTableFullDay(int month, int year, int day)
        {
            DelTable();
            //Instantiate(pb, transform);
            //int daysInMonth = DateTime.DaysInMonth(year, month);

            string document = month + "-" + year;
            // for (int i = daysInMonth; i > 0; i--)
            // {
                Query query = db.Collection("Attendance")
                    .Document(document)
                    .Collection("data")
                    .OrderByDescending("timecheck");
                QuerySnapshot snapshot = await query.GetSnapshotAsync();

                
                
                foreach (var doc in snapshot)
                {
                    Connect connect = doc.ConvertTo<Connect>();
                    if (connect.timecheck.Day == day)
                    {
                        
                        DocumentReference docuser = db.Collection("User")
                            .Document(connect.id);
                        DocumentSnapshot snapshotuser = await docuser.GetSnapshotAsync();
                        Connect connectUser = snapshotuser.ConvertTo<Connect>();

                        var clone = Instantiate(prefab, transform);
                        clone.SetActive(true);
                        Text[] children = clone.GetComponentsInChildren<Text>();
                        children[0].text = connectUser.name;
                        children[1].text = ChangeTime(connect.timecheck);
                        if (connect.check == "check in")
                        {
                            clone.GetComponent<Image>().color = Color.green;
                        }
                        else
                        {
                            clone.GetComponent<Image>().color = Color.yellow;
                        }
                    }
                }
            //}
            
            wait.SetActive(false);
        }

        public async void DrawTableWithName(int month, int year,int day, string id,string Name)
        {
            DelTable();
            //Instantiate(pb, transform);
            //int daysInMonth = DateTime.DaysInMonth(year, month);
            string document = month + "-" + year;

            // for (int i = daysInMonth; i > 0; i--)
            // {
                Query query = db.Collection("Attendance")
                    .Document(document)
                    .Collection("data")
                    .OrderByDescending("timecheck");
                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                foreach (var doc in snapshot)
                {
                    Connect connect = doc.ConvertTo<Connect>();
                    if (connect.id == id && connect.timecheck.Day == day)
                    {
                        var clone = Instantiate(prefab, transform);
                        clone.SetActive(true);
                        Text[] children = clone.GetComponentsInChildren<Text>();
                        children[0].text = Name;
                        children[1].text = ChangeTime(connect.timecheck);
                        if (connect.check == "check in")
                        {
                            clone.GetComponent<Image>().color = Color.green;
                        }
                        else
                        {
                            clone.GetComponent<Image>().color = Color.yellow;
                        }

                    }
                }
            //}
            
            wait.SetActive(false);
        }

        private string ChangeTime(DateTime time)
        {

            // Chuyển đổi thời gian sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local);

            // Định dạng thời gian thành chuỗi với múi giờ của Việt Nam (ICT)
            string formattedTimeVietnam = vietnamTime.ToString("HH:mm dd/MM/yyyy");
            return formattedTimeVietnam;
        }
        
        private void DelTable()
        {
            wait.SetActive(true);
            foreach (Transform i in transform)
            {
                Destroy(i.gameObject);
            }
        }
    }
}