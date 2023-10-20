using System;
using Firebase.Firestore;
using main.Script.controller;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace main.Script.service
{
    public class TableManagement : FireBaseCommon
    {
        public GameObject wait;
        public GameObject prefab;
        public GameObject pb;
        void Start()
        {
            base.Start();
        }

        public async void DrawTableFullMon(int month, int year)
        {
            DelTable();
            Instantiate(pb, transform);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int i = daysInMonth; i > 0; i--)
            {
                Query query = db.Collection("attendance data").Document(month + "-" + year).Collection(i.ToString());
                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                foreach (var doc in snapshot)
                {
                    Connect connect = doc.ConvertTo<Connect>();
                    if (doc.Exists)
                    {
                        var clone = Instantiate(prefab, transform);
                        clone.SetActive(true);
                        Text[] children = clone.GetComponentsInChildren<Text>();
                        children[0].text = connect.check;
                        children[1].text = connect.name;
                        children[2].text = ChangeTime(connect.timecheck);
                        children[3].text = "";
                    }
                }
            }
            
            wait.SetActive(false);
        }

        public async void DrawTableWithName(int month, int year, string name)
        {
            DelTable();
            Instantiate(pb, transform);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int i = daysInMonth; i > 0; i--)
            {
                Query query = db.Collection("attendance data").Document(month + "-" + year).Collection(i.ToString());
                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                foreach (var doc in snapshot)
                {
                    Connect connect = doc.ConvertTo<Connect>();
                    if (name == connect.name)
                    {
                        if (doc.Exists)
                        {
                            var clone = Instantiate(prefab, transform);
                            clone.SetActive(true);
                            Text[] children = clone.GetComponentsInChildren<Text>();
                            children[0].text = connect.check;
                            children[1].text = connect.name;
                            children[2].text = ChangeTime(connect.timecheck);
                            children[3].text = "";
                        }
                    }
                }
            }
            
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