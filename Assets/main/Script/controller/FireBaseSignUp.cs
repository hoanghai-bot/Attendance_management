using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.controller
{
    public class FireBaseSignUp : FireBaseCommon
    {
    
        public InputField account;
        public InputField password;
        public InputField confirm;
        public InputField name;
        public InputField address;
        public InputField birth;

        public Text textError;
        public GameObject accError;
        public GameObject passError;
        public GameObject confirmError;
        public GameObject priError;
        public GameObject SignInSuccess;
    
        private void Start()
        {
            base.Start();
            SignInSuccess.SetActive(false);
            //GetID();
        }

        public async void SetData()
        {
            if (name.text == ""|| address.text =="" || birth.text =="")
            {
                priError.SetActive(true);
                return;
            }
            else
            {
                priError.SetActive(false);
            }
        
            if (!Regex.IsMatch(account.text, @"^0[0-9]{9}$"))
            {
                account.text = "";
                textError.text = "Mời nhập số điện thoại";
                accError.SetActive(true);
                return;
            }
            else
            {
                accError.SetActive(false);
            }

            if (await ChenkAccount(account.text,IDAcc))
            {
                textError.text = "Số điện thoại này đã được sử dụng";
                accError.SetActive(true);
                IDAcc = null;
                Debug.Log("Số điện thoại này đã được sử dụng");
                return;
            }
            else
            {
                accError.SetActive(false);
            }

            if (password.text == "")
            {
                passError.SetActive(true);
                return;
            }
            else
            {
                passError.SetActive(false);
            }
        

            if (password.text != confirm.text)
            {
                Debug.Log("Password has not been confirmed");
                confirmError.SetActive(true);
            }
            else
            {
                confirmError.SetActive(false);
                //updateId();
                NewUser(account.text, password.text, name.text, address.text, birth.text);
            
                Debug.Log("signup success");
                SignInSuccess.SetActive(true);
                PlayerPrefs.SetString("id", "");
            }
        
        }



        // public async void updateId()
        // {
        //     DocumentReference docref = db.Collection("user").Document("lastID");
        //     Dictionary<string, object> data = new Dictionary<string, object>()
        //     {
        //         { "id", id + 1 }
        //     };
        //
        //     DocumentSnapshot snapshot = await docref.GetSnapshotAsync();
        //     if (snapshot.Exists)
        //     {
        //         await docref.SetAsync(data);
        //     }
        // }

        // public async void GetID()
        // {
        //     DocumentReference docref = db.Collection("user").Document("lastID");
        //     DocumentSnapshot snapshot = await docref.GetSnapshotAsync();
        //     Connect connect = snapshot.ConvertTo<Connect>();
        //
        //     if (snapshot.Exists)
        //     {
        //          id = connect.id;
        //     }
        //    
        // }
    }
}
