using System;
using Firebase.Firestore;

[FirestoreData]
public struct Connect
{
    [FirestoreProperty] public string password { get; set; }
    [FirestoreProperty] public string account { get; set; }
    [FirestoreProperty] public string name { get; set; }
    [FirestoreProperty] public string address { get; set; }
    [FirestoreProperty] public string birth { get; set; }
    [FirestoreProperty] public string permission { get; set; }

    [FirestoreProperty] public string check { get; set; }
    [FirestoreProperty] public string id { get; set; }
    [FirestoreProperty] public string time { get; set; }
    [FirestoreProperty] public DateTime timecheck { get; set; }
    [FirestoreProperty] public string network { get; set; }
}
