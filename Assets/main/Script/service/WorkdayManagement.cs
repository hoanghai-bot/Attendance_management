using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using main.Script.controller;
using UnityEngine;
using UnityEngine.UI;

public class WorkdayManagement : FireBaseCommon
{
        public GameObject wait;
        public GameObject prefab;
        
        void Start()
        {
            base.Start();
        }

        public async void DrawTableFullMon(int month, int year)
        {
            DelTable();
            

            string document = month + "-" + year;
            Query query = db.Collection("Attendance")
                    .Document(document)
                    .Collection("data")
                    .OrderBy("timecheck");
                QuerySnapshot snapshot = await query.GetSnapshotAsync();
                foreach (var doc in snapshot)
                {
                    Connect connect = doc.ConvertTo<Connect>();

                    if (doc.Exists)
                    {
                        /*
                         lưu giá trị tên và check in vào biến / kiểm tra check in lưu biến đi muộn 
                         kiểm tra giá trị tên và chenk our 
                         thời gian check in - check out
                         lưu số công vào biến 
                        cộng tất cả / xuất giá trị 
                        (tối ưu chạy 1 vòng query)
                         */
                    }
                }

                wait.SetActive(false);
        }

        public async void DrawTableWithName(int month, int year, string name)
        {
            DelTable();
            
            string document = month + "-" + year;

            
            Query query = db.Collection("Attendance")
                .Document(document)
                .Collection("data")
                .OrderBy("timecheck");
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
                        if (connect.check == "check in")
                        {
                            clone.GetComponent<Image>().color = Color.green;
                        }
                        else
                        {
                            clone.GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }

            wait.SetActive(false);
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
