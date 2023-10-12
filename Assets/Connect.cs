using System;
using Firebase.Firestore;

[FirestoreData]
public struct Connect
{
    [FirestoreProperty] public string Password { get; set; }
    [FirestoreProperty] public string account { get; set; }
    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public string address { get; set; }
    [FirestoreProperty] public string birth { get; set; }


    [FirestoreProperty] public string id { get; set; }
    [FirestoreProperty] public string time { get; set; }
    [FirestoreProperty] public DateTime timecheck { get; set; }
    [FirestoreProperty] public string network { get; set; }
}
