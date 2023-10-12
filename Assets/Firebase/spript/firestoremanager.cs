using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Firestoremanager : MonoBehaviour
{
    [SerializeField] Button click;
    [SerializeField] Text number;
    FirebaseFirestore db;

    private ListenerRegistration ListenerRegistration;
    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        click.onClick.AddListener(OnClickDown);
        //GetData();
        ListenerRegistration = db.Collection("user").Document("account").Listen(snapshot =>     //doc data
        {
            Connect connect = snapshot.ConvertTo<Connect>();                                            //lay data vao connect
            number.text = connect.Name.ToString();                                                    //lay data number tu tren firebase
        });
    }

    private void OnDestroy()
    {
        ListenerRegistration.Stop();
    }

    //update
    public void OnClickDown()
    {
        int beforeNumber = int.Parse(number.text);
        Connect connect = new Connect { Name = "", account = "user1" };     //tao du lieu moi
        DocumentReference numberRef = db.Collection("user").Document("account");    //dat documentReference la numberRef
        numberRef.SetAsync(connect).ContinueWithOnMainThread(task =>                        //updata = setAsync
        {
            Debug.Log("updated");                                                    //doan ma continue
            //GetData();
        });

    }

    // void GetData()
    // {
    //     db.Collection("user").Document("account").GetSnapshotAsync().ContinueWithOnMainThread(task =>
    //     {
    //         Connect connect = task.Result.ConvertTo<Connect>();
    //         number.text = connect.number.ToString();
    //     });
    // }
}
