using System;
using System.Linq;
using main.Script.service;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.controller
{
    public class ControllerAttendanceTable : FireBaseCommon
    {
        public Text monthAndYear;
        public int day;
        public int month;
        public int year;
        public Text find;
        public GameObject listBtn;
        public GameObject Qr;
        private string name = null;
        private string idname;

        private async void Start()
        {
            base.Start();
            if (await CheckPermission("admin"))
            {
                listBtn.SetActive(true);
            }

            if (await CheckPermission("system"))
            {
                Qr.SetActive(true);
            }

            day = DateTime.Now.Day;
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
        
        public void PlusDay()
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day == daysInMonth)
            {
                if (month==12)
                {
                    year += 1;
                    month = 1;
                    day = 1;
                    UpdateEvent();
                
                }
                else
                {
                    month += 1;
                    day = 1;
                    UpdateEvent();
                }
                
            }
            else
            {
                day += 1;
                UpdateEvent();
            }
        }
        
        public void MinusDay()
        {
            if (day==1)
            {
                if (month==1)
                {
                    year -= 1;
                    month = 12;
                    day = 31;
                    UpdateEvent();
                }
                else
                {
                    month -= 1;
                    int daysInMonth = DateTime.DaysInMonth(year, month);
                    day = daysInMonth;
                    UpdateEvent();
                }
            }
            else
            {
                day -= 1;
                UpdateEvent();
            }
        }

        private void UpdateEvent()
        {
            monthAndYear.text = day+"/"+month+"/"+ year;
            if (name!=null)
            {
                FindObjectOfType<AttendanceTableManagement>().DrawTableWithName(month,year,day,idname,name);
            }
            else
            {
                FindObjectOfType<AttendanceTableManagement>().DrawTableFullDay(month,year,day);
            }
            
        
        }

        public void ClickAtive(GameObject temp)
        {
            temp.SetActive(true);
        }

        public void Find(GameObject temp)
        {
            var t = temp.GetComponentInChildren<Text>().text;
            name = t;
            find.text = t;
            idname = temp.name;
            FindObjectOfType<AttendanceTableManagement>().DrawTableWithName(month,year,day,idname,name);
        }
        public void AllFind(Text temp)
        {
            name = null;
            find.text = temp.GetComponentInChildren<Text>().text;
            FindObjectOfType<AttendanceTableManagement>().DrawTableFullDay(month,year,day);
            
        }

        public void ClickTime()
        {
            FindObjectOfType<CalendarManagement>().DrawCalendar(month,year);
            if (month == DateTime.Now.Month && year == DateTime.Now.Year)
            {
                var findButton = FindObjectsOfType<Button>()
                    .Where(x => x.GetComponentInChildren<Text>().text == DateTime.Now.Day.ToString("00"))
                    .ToList().First();
                findButton.image.color = Color.green;
            }
        }

        public void ClickDay(GameObject temp)
        {
            day = Convert.ToInt32(temp.GetComponentInChildren<Text>().text);
            UpdateEvent();
        }
    }
}

