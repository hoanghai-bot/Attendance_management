using System;
using Firebase.Firestore;
using main.Script.controller;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.service
{
    public class WorkdayManagement : FireBaseCommon
    {
        public GameObject wait;
        public Text wn;
        public Text wd;
        public Text late;
        public Text lasttime;
        
        private float workday;
        private int timelate;
        private TimeSpan sumlast;
        private TimeSpan checkout;
        void Start()
        {
            base.Start();
        }

        public async void DrawReportWithName(int month, int year, string name)
        {
            DelTable();
            string document = month + "-" + year;
            workday = 0;
            timelate = 0;
            sumlast = TimeSpan.Zero;
            checkout= TimeSpan.Zero;
            
            Query query = db.Collection("Attendance")
                .Document(document)
                .Collection("data")
                .OrderByDescending("timecheck");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();


            foreach (var doc in snapshot)
            {
                Connect connect = doc.ConvertTo<Connect>();
                if (doc.Exists)
                {
                    if (name == connect.name)
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
                                sumlast += ChangeTime(connect.timecheck) - new TimeSpan(13, 30, 0);
                            }
                            else if(ChangeTime(connect.timecheck) > new TimeSpan(8, 30, 0)
                                    && ChangeTime(connect.timecheck)< new TimeSpan(11,0,0))
                            {
                                timelate++;
                                sumlast += ChangeTime(connect.timecheck) - new TimeSpan(8, 30, 0);
                            }

                            TimeSpan calculate = checkout - temp;

                            if (calculate > new TimeSpan(2, 0, 0)
                                && calculate < new TimeSpan(5, 0, 0))
                            {
                                workday += 0.5f;
                            }

                            if (calculate > new TimeSpan(5, 0, 0))
                            {
                                workday += 1;
                            }
                        }
                    }
                }
            }

            wn.text = (workday*2).ToString();
            wd.text = workday.ToString();
            late.text = timelate.ToString();
            lasttime.text = sumlast.ToString();
                
            wait.SetActive(false);
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
        }
    }
}
