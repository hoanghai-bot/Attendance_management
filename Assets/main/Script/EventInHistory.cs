using System;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script
{
    public class EventInHistory : FireBaseCommon
    {
        public Text monthAndYear;
        public int month;
        public int year;
        public string day;

        private void Start()
        {
            base.Start();
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            day = DateTime.Now.Day.ToString();
            UpdateEvent();
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
        
        
    }
}
