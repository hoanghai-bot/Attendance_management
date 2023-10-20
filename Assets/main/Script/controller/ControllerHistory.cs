using System;
using Firebase.Firestore;
using main.Script.service;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.controller
{
    public class ControllerHistory : FireBaseCommon
    {
        public Text monthAndYear;
        public int month;
        public int year;
        public string day;
        public GameObject btnManage;
        
        private async void Start()
        {
            base.Start();
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            day = DateTime.Now.Day.ToString();
            UpdateEvent();
            //btnManageCheck();
            if (await CheckPermission("admin"))
            {
                btnManage.SetActive(true);
            }
        }
        public void PlusMonth()
        {
            if (month==12)
            {
                year += 1;
                month = 1;
                UpdateEvent();
                
            }
            else
            {
                month += 1;
                UpdateEvent();
            }
        }

        public void MinusMonth()
        {
            if (month==1)
            {
                year -= 1;
                month = 12;
                UpdateEvent();
            }
            else
            {
                month -= 1;
                UpdateEvent();
            }
        }

        public void ClickButtonDay(Text number)
        {
            day = number.text;
            FindObjectOfType<InformationManagement>().SeachInformation(day,month,year,db);
        }

        private void UpdateEvent()
        {
            monthAndYear.text = "tháng "+month+" năm "+ year;
            FindObjectOfType<CalendarManagement>().DrawCalendar(month,year);
            FindObjectOfType<InformationManagement>().SeachInformation(day,month,year,db);
        }

        // public async void btnManageCheck()
        // {
        //     DocumentReference docref = db.Collection("user").Document(PlayerPrefs.GetString("id"));
        //     DocumentSnapshot snapshot = await docref.GetSnapshotAsync();
        //     Connect connect = snapshot.ConvertTo<Connect>();
        //
        //     if (connect.permission == "admin")
        //     {
        //         btnManage.SetActive(true);
        //     }
        // }
    }
}
