using System;
using Firebase.Firestore;
using main.Script.controller;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.service
{
    public class WorkdayManagement : FireBaseCommon
    {
        public GameObject wait;
        // public Text wn;
        // public Text wd;
        // public Text late;
        // public Text lasttime;
        public GameObject prefab;
        
        private float workday;
        private int timelate;
        private TimeSpan sumlast;
        private TimeSpan checkout;
        void Start()
        {
            base.Start();
        }

        public async void DrawReportWithName(int month, int year)
        {
            DelTable();
            string id =null;
            string str;
            string document = month + "-" + year;

            Query userQuery = db.Collection("User");
            QuerySnapshot userSnapshot = await userQuery.GetSnapshotAsync();
            foreach (var useDoc in userSnapshot)
            {
                Connect userConnect = useDoc.ConvertTo<Connect>();
                id = useDoc.Id;
                str = userConnect.name;
                
                Query query = db.Collection("Attendance")
                    .Document(document)
                    .Collection("data")
                    .WhereEqualTo("id",id)
                    .OrderByDescending("timecheck");
                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                
                foreach (var doc in snapshot)
                {
                    Connect connect = doc.ConvertTo<Connect>();
                    CalculateWork(connect);
                }
                WriteResults(str);
                ResetCalculate();
                wait.SetActive(false);
            }
            
            // Query query = db.Collection("Attendance")
            //     .Document(document)
            //     .Collection("data")
            //     .OrderByDescending("timecheck")
            //     .OrderByDescending("id");
            // QuerySnapshot snapshot = await query.GetSnapshotAsync();
            //
            //
            // foreach (var doc in snapshot)
            // {
            //     Connect connect = doc.ConvertTo<Connect>();
            //
            //     if (id == null)
            //     {
            //         id = connect.id;
            //     }
            //     if (id ==connect.id)
            //     {
            //         CalculateWork(connect);
            //     }
            //     else
            //     {
            //         WriteResults(id);
            //         id = connect.id;
            //         ResetCalculate();
            //         CalculateWork(connect);
            //     }
            //
            // }
            // WriteResults(id);
            // wait.SetActive(false);
        }

        private void ResetCalculate()
        {
            workday = 0;
            timelate = 0;
            sumlast = TimeSpan.Zero;
        }

        private async void WriteResults(string id)
        {
            // DocumentReference docRef = db.Collection("User")
            //     .Document(id);
            // DocumentSnapshot docsnap = await docRef.GetSnapshotAsync();
            // Connect c = docsnap.ConvertTo<Connect>();
                    
                    
            var clone = Instantiate(prefab, transform);
            clone.SetActive(true);
            Text[] children = clone.GetComponentsInChildren<Text>();
            children[0].text = id; //c.name;
            children[1].text = workday.ToString();
            children[2].text = timelate.ToString();
            children[3].text = sumlast.ToString();
        }
        
        private void CalculateWork(Connect connect)
        {
            TimeSpan temp = connect.timecheck.TimeOfDay;
            if (connect.check == "check out")
            {
                checkout = temp;
            }
            else if (connect.check == "check in")
            {
                if (ChangeTime(connect.timecheck) > new TimeSpan(13, 30, 0))
                {
                    timelate++;
                    sumlast = sumlast.Add(ChangeTime(connect.timecheck) - new TimeSpan(13, 30, 0));
                }
                else if (ChangeTime(connect.timecheck) > new TimeSpan(8, 30, 0)
                         && ChangeTime(connect.timecheck) < new TimeSpan(11, 0, 0))
                {
                    timelate++;
                    sumlast = sumlast.Add(ChangeTime(connect.timecheck) - new TimeSpan(8, 30, 0));
                }

                TimeSpan calculate = checkout - temp;

                if (calculate > new TimeSpan(2, 0, 0)
                    && calculate < new TimeSpan(6, 0, 0))
                {
                    workday += 0.5f;
                }

                if (calculate > new TimeSpan(6, 0, 0))
                {
                    workday += 1;
                }
            }
        }

        private TimeSpan ChangeTime(DateTime time)
        {

            // Chuyển đổi thời gian sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local);

            // Định dạng thời gian thành chuỗi với múi giờ của Việt Nam (ICT)
            TimeSpan formattedTimeVietnam = vietnamTime.TimeOfDay;
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
