using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

namespace main.Script.controller
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

        // protected async void ConnertToId(string id, Connect connect)
        // {
        //     DocumentReference doc = db.Collection("user").Document(id);
        //     DocumentSnapshot docsnap =await doc.GetSnapshotAsync();
        //     connect = docsnap.ConvertTo<Connect>();
        // }

        protected async Task<bool> ChenkAccount(string acc,string id)
        {
            bool result = false;
            if (PlayerPrefs.GetString("id")!="")
            {
                DocumentReference doc = db.Collection("user").Document(id);
                DocumentSnapshot docsnap =await doc.GetSnapshotAsync();
                Connect connect = docsnap.ConvertTo<Connect>();
                // Connect connect = default;
                // ConnertToId(PlayerPrefs.GetString("id"),connect);
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
                if (pass == connect.password)
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
                { "password", pass },
                { "name", name },
                {"address",address},
                {"birth",bith},
                {"permission","member"}
            };
            collection.AddAsync(data);
        }
        
        protected void UpdateUser( string dt, string pass, string name, string address, string bith,string mission, string id)
        {
            DocumentReference collection = db.Collection("user").Document(id);
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "account", dt },
                { "password", pass },
                { "name", name },
                {"address",address},
                {"birth",bith},
                {"permission",mission}
            };
            collection.UpdateAsync(data);
        }

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
            
            string check;
            if (time.ToString("tt") == "AM" && time.TimeOfDay < new TimeSpan(8,30,0)
                || time.ToString("tt") == "PM" && time.TimeOfDay <  new TimeSpan(1,30,0))
            {
                check = "check in";
            }
            else
            {
                check = "chech out";
            }
            Debug.Log(PlayerPrefs.GetString("id"));
            DocumentReference doc = db.Collection("user").Document(PlayerPrefs.GetString("id"));
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
            Connect connect = snapshot.ConvertTo<Connect>();
            
            CollectionReference query = db.Collection("attendance data").Document(time.ToString("MM-yyyy")).Collection(time.Day.ToString());
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "timecheck", time },
                {"name",connect.name},
                {"phone", connect.account},
                {"id",PlayerPrefs.GetString("id")},
                {"time",time.ToString("dd/MM/yyyy")},
                {"network",GetDeviceIPAddress()},
                {"device",GetDeviceName()},
                {"check",check}
            };
            await query.AddAsync(data);

            
        }

        protected async Task<bool> CheckPermission(string temp)
        {
            bool result = false;
            DocumentReference doc = db.Collection("user").Document(IDAcc);
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
            Connect connect = snapshot.ConvertTo<Connect>();

            if (connect.permission == temp)
            {
                result = true;
            }
            return result;
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
