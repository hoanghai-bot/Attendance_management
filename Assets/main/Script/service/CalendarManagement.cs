using System;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.service
{
    public class CalendarManagement : MonoBehaviour
    {
        
        public GameObject space;
        public GameObject prefab;
        
        public void DrawCalendar(int month,int year)
        {
            DelCalendar();
            
            
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int i = 0; i< (int)firstDayOfMonth.DayOfWeek; i++)
            {
                var temp =Instantiate( space, transform);
                temp.AddComponent<Image>();
                temp.SetActive(true);
            }

            for (int day = 1; day <= daysInMonth; day++)
            {
                var clone = Instantiate(prefab, transform);
                clone.SetActive(true);
                clone.GetComponentInChildren<Text>().text = day.ToString("00");
            }
        }

        

        private void DelCalendar()
        {
            foreach (Transform i in transform)
            {
                Destroy(i.gameObject);
            }
        }
    }
}
