using System.Text.RegularExpressions;
using Firebase.Firestore;
using main.Script.service;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.controller
{
    public class ControllerInformation : FireBaseCommon
    {
        private string id;
        
        public InputField dt;
        public InputField password;
        public InputField confirm;
        public InputField name;
        public InputField address;
        public InputField bith;

        public Text nowMember;
        public Text misionMember;
        public GameObject confirmError;
        public GameObject accError;
        public GameObject priError;

        void Start()
        {
            base.Start();
            InformationUser(PlayerPrefs.GetString("id"));
            
        }

        public async void ChangeUser()
        {
            if (name.text == "" || address.text == "" || bith.text == "")
            {
                priError.SetActive(true);
                return;
            }
            else
            {
                priError.SetActive(false);
            }

            if (!Regex.IsMatch(dt.text, @"^0[0-9]{9}$"))
            {
                accError.SetActive(true);
                confirmError.SetActive(false);
                dt.text = "";
                accError.GetComponentInChildren<Text>().text = "Mời nhập số điện thoại";
                return;
            }

            if (await ChenkAccount(dt.text, id))
            {
                confirmError.SetActive(false);
                IDAcc = PlayerPrefs.GetString("id");
                accError.SetActive(true);
                accError.GetComponentInChildren<Text>().text = "Số điện thoại này đã tồn tại";
            }
            else
            {
                // if (password.text != confirm.text)
                // {
                //     accError.SetActive(false);
                //     Debug.Log("Password has not been confirmed");
                //     confirmError.SetActive(true);
                // }
                // else
                {
                    accError.SetActive(false);
                    confirmError.SetActive(false);
                    UpdateUser(dt.text, password.text, name.text, address.text, bith.text,misionMember.text,id);
                    InformationUser(id);
                    //FindObjectOfType<ButtonFind>().DrawFind();
                    Debug.Log("success");
                }

            }
        }

        public void NoChangeUser()
        {
            InformationUser(id);
        }

        private async void InformationUser(string id)
        {
            DocumentReference doc = db.Collection("user").Document(id);
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();

            Connect connect = snapshot.ConvertTo<Connect>();
            if (snapshot.Exists)
            {
                dt.text = connect.account;
                password.text = connect.password;
                name.text = connect.name;
                address.text = connect.address;
                bith.text = connect.birth;
                nowMember.text = connect.name;
                misionMember.text = connect.permission;
            }

            confirm.text = "";
        }

        public void ListButton(GameObject obj)
        {
            obj.SetActive(true);
        }

        public void ClickPermission(string temp)
        {
            misionMember.text = temp;
        }

        public async void ClickFindName(Text temp)
        {
            Query query = db.Collection("user");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (var doc in snapshot)
            {
                Connect connect = doc.ConvertTo<Connect>();
                if (doc.Exists)
                {
                    if (temp.text == connect.name)
                    {
                        id = doc.Id;
                        InformationUser(id);
                        return;
                    }
                }

            }
            
        }
    }
}
