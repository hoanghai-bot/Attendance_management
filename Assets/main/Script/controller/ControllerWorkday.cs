using System;
using main.Script.service;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.controller
{
    public class ControllerWorkday : FireBaseCommon
    {
        public Text monthAndYear;
        public int month;
        public int year;
        public GameObject tableFind;
        public Text find;
        
        
        
        private void Start()
        {
            base.Start();

            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
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

        private void UpdateEvent()
        {
            monthAndYear.text = "tháng "+month+" năm "+ year;
            FindObjectOfType<AttendanceTableManagement>().DrawTableFullMon(month,year);
        
        }

        public void ClickFind()
        {
            tableFind.SetActive(true);
        }

        public void Find(Text temp)
        {
            find.text = temp.text;
            FindObjectOfType<AttendanceTableManagement>().DrawTableWithName(month,year,temp.text);
        }
        public void AllFind(Text temp)
        {
            find.text = temp.text;
            FindObjectOfType<AttendanceTableManagement>().DrawTableFullMon(month,year);
        }
    }
}