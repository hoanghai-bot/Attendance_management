using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script
{
    public class ControllerHome : FireBaseCommon
    {
        
        // public InputField dt;
        // public InputField password;
        // public InputField confirm;
        // public InputField name;
        // public InputField address;
        // public InputField bith;
    
        //public Text nameUser;
        public GameObject menuUser;
        // public GameObject confirmError;
        // public GameObject accError;
    
        void Start()
        {
            base.Start();
            //InformationUser();
            
        }

        // public async void ChangeUser()
        // {
        //     if (await ChenkAccount(dt.text))
        //     {
        //         confirmError.SetActive(false);
        //         Debug.Log("This account already exists");
        //         IDAcc = PlayerPrefs.GetString("id");
        //         accError.SetActive(true);
        //
        //     }
        //     else
        //     {
        //         if (password.text != confirm.text)
        //         {
        //             accError.SetActive(false);
        //             Debug.Log("Password has not been confirmed");
        //             confirmError.SetActive(true);
        //         }
        //         else
        //         {
        //             accError.SetActive(false);
        //             confirmError.SetActive(false);
        //             UpdateUser(dt.text, password.text, name.text, address.text, bith.text);
        //             InformationUser();
        //             Debug.Log("success");
        //         }
        //
        //     }
        // }

        // public void NoChangeUser()
        // {
        //     InformationUser();
        // }
        // private async void InformationUser( )
        // {
        //     DocumentReference doc = db.Collection("user").Document(IDAcc);
        //     DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
        //
        //     Connect connect = snapshot.ConvertTo<Connect>();
        //     if (snapshot.Exists)
        //     {
        //         dt.text = connect.account;
        //         password.text = connect.Password;
        //         name.text = connect.Name;
        //         address.text = connect.address;
        //         bith.text = connect.birth;
        //         nameUser.text = name.text;
        //     }
        //     confirm.text = "";
        // }

        public void UserButton()
        {
            menuUser.SetActive(true);
        }
        
    }
}
