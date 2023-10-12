using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

using System.Net;
using System.Net.Sockets;

namespace main.Script
{
    public class FireBaseCommon : MonoBehaviour
    {
        private DateTime time;
        protected string IDAcc ;
        protected FirebaseFirestore db;
        protected void Start()
        {
            db = FirebaseFirestore.DefaultInstance;
            IDAcc = PlayerPrefs.GetString("id");
            Debug.Log(PlayerPrefs.GetString("id"));
            Debug.Log(IDAcc);
            
        }

        private void Update()
        {
            time = DateTime.Now;
        }


        protected async Task<bool> ChenkAccount(string acc)
        {
            bool result = false;
            if (PlayerPrefs.GetString("id")!="")
            {
                DocumentReference doc = db.Collection("user").Document(PlayerPrefs.GetString("id"));
                DocumentSnapshot docsnap =await doc.GetSnapshotAsync();
                Connect connect = docsnap.ConvertTo<Connect>();
                if (acc == connect.account)
                {
                    return false;
                }
            }
        
            Query qref = db.Collection("user");
            QuerySnapshot snapshot = await qref.GetSnapshotAsync();
            foreach (var doc in snapshot)
            {
                Connect connect = doc.ConvertTo<Connect>();
                if (doc.Exists)
                {
                    if (acc == connect.account)
                    {
                        IDAcc = doc.Id;
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    
        protected async Task<bool> ChenkPassword(string pass,string id)
        {
            bool result = false;
            DocumentReference doc = db.Collection("user").Document(id);
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
        
            Connect connect = snapshot.ConvertTo<Connect>();
            if (snapshot.Exists)
            {
                if (pass == connect.Password)
                {
                    result = true;
                }
            }
        
            return result;
        }

    
        protected void NewUser( string dt, string pass, string name, string address, string bith)
        {
            CollectionReference collection = db.Collection("user");
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"device",GetDeviceName()},
                { "account", dt },
                { "Password", pass },
                { "Name", name },
                {"address",address},
                {"birth",bith}
            };
            collection.AddAsync(data);
        }
        
        // protected void UpdateUser( string dt, string pass, string name, string address, string bith)
        // {
        //     DocumentReference collection = db.Collection("user").Document(PlayerPrefs.GetString("id"));
        //     Dictionary<string, object> data = new Dictionary<string, object>()
        //     {
        //         { "account", dt },
        //         { "Password", pass },
        //         { "Name", name },
        //         {"address",address},
        //         {"birth",bith}
        //     };
        //     collection.UpdateAsync(data);
        // }

        string GetDeviceName()
        {
            string deviceName = "N/A";
            try
            {
                deviceName = SystemInfo.deviceName;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error getting device name: " + ex.Message);
            }

            return deviceName;
        }
        string GetDeviceIPAddress()
        {
            string ipAddress = "N/A";
            try
            {
                // Get the device's IP addresses
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);

                // Iterate through the addresses to find a valid IPv4 address
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = address.ToString();
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error getting device IP address: " + ex.Message);
            }

            return ipAddress;
        }

        public async void ScanUpdateCheckTime()
        {
            Debug.Log(PlayerPrefs.GetString("id"));
            DocumentReference doc = db.Collection("user").Document(PlayerPrefs.GetString("id"));
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
            Connect connect = snapshot.ConvertTo<Connect>();
            
            CollectionReference query = db.Collection("attendance data").Document(time.ToString("MM-yyyy")).Collection(time.Day.ToString());
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "timecheck", time },
                {"name",connect.Name},
                {"phone", connect.account},
                {"id",PlayerPrefs.GetString("id")},
                {"time",time.ToString("dd/MM/yyyy")},
                {"network",GetDeviceIPAddress()},
                {"device",GetDeviceName()}
            };
            await query.AddAsync(data);

            
        }
    
        // public async void ScanUpdateCheckOut()
        // {
        //     CollectionReference query = db.Collection("attendance data").Document(time.Year+"/"+time.Month).Collection(time.Day.ToString());
        //     Dictionary<string, object> data = new Dictionary<string, object>()
        //     {
        //         { "checkout", time }
        //     };
        //     await query.AddAsync(data);
        //
        //
        //     
        // }
    }
}
