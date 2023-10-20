using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace main.Script.controller
{
    public class FireBaseSignIn : FireBaseCommon
    {
    
        public GameObject accError;
        public GameObject passError;

        public InputField account;
        public InputField password;

        public Text textError;
        public GameObject SignUpSuccess;
        // Start is called before the first frame update
        async void Start()
        {
            base.Start();
            SignUpSuccess.SetActive(false);
            if (PlayerPrefs.GetString("id")!="" )
            {
                if (await CheckPermission("system"))
                {
                    SceneManager.LoadScene("HomeSystem");
                }
                else
                {
                    SceneManager.LoadScene("Home");
                }
                
            }
            
        }

        public async void CheckSignIn()
        {
            if (!Regex.IsMatch(account.text,@"^0[0-9]{9}$"))
            {
                accError.SetActive(true);
                passError.SetActive(false);
                account.text = "";
                textError.text = "Mời nhập số điện thoại";
                Debug.Log("Mời nhập số điện thoại");
                return;
            }
            if (!await ChenkAccount(account.text,PlayerPrefs.GetString("id")))
            {
                accError.SetActive(true);
                passError.SetActive(false);
                textError.text = "Số điện thoại này chưa được đăng ký";
                Debug.Log("Số điện thoại này chưa được đăng ký");
            }
            else
            {
                if (!await ChenkPassword(password.text, IDAcc))
                {
                    accError.SetActive(false);
                    passError.SetActive(true);
                    Debug.Log("Wrong password");
                }
                else
                {
                    accError.SetActive(false);
                    passError.SetActive(false);
                    Debug.Log("login success");
                    PlayerPrefs.SetString("id", IDAcc);
                    SignUpSuccess.SetActive(true);
                }
            }
        }
    }
}
