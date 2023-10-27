using Firebase.Firestore;
using main.Script.controller;
using UnityEngine;
using UnityEngine.UI;

namespace main.Script.service
{
    public class ButtonFind : FireBaseCommon
    {
        public GameObject prefab;
        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            DrawFind();
        }

        public async void DrawFind()
        {
            Query query = db.Collection("User");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (var doc in snapshot)
            {
                Connect connect = doc.ConvertTo<Connect>();
                if (doc.Exists)
                {
                    var clone = Instantiate(prefab, transform);
                    clone.GetComponentInChildren<Text>().text = connect.name;
                }
            }
        }
    }
}
