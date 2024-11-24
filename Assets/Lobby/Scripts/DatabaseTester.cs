using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseTester : MonoBehaviour
{
    [SerializeField] long level;

    // DatabaseReference levelRef;
    DatabaseReference userDataRef;
    DatabaseReference levelRef;

    private void OnEnable()
    {
        // �̷��� �̸������Ѵ°ͺ��� levelRef = BackendManager.Database.RootReference.Child("UserData/choi/level");
        string uID = BackendManager.Auth.CurrentUser.UserId;
        userDataRef = BackendManager.Database.RootReference.Child("UserData").Child(uID);
        levelRef = userDataRef.Child("level");

        // ����� �޾ƿ��� CallBack
        levelRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("�� �������� ��ҵ�");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogWarning($"�� �������� ����: {task.Exception.Message}");
                return;
            }
            Debug.Log("�� �������� ����");
            DataSnapshot snapShot = task.Result; // result�� �����ͺ��̽��� sanpShot���γ�����
            // snapShot�� "��" ĸ���ؼ� ������  
            Debug.Log(snapShot.Value);
            if(snapShot.Value == null) // �����ͺ��̽��� �����Ͱ� ���°��
            {   // �������� �ʱⰪ����
                UserData userData = new UserData();
                userData.name = BackendManager.Auth.CurrentUser.DisplayName;
                userData.email = BackendManager.Auth.CurrentUser.Email;
                userData.level = 1;

                // ����Ʈ �ǹ̾��µ� ���ݰŴ� �׳� �׽�Ʈ��
                userData.list.Add("ù����");
                userData.list.Add("�ι���");
                userData.list.Add("������");

                string json = JsonUtility.ToJson(userData);
                userDataRef.SetRawJsonValueAsync(json);
            }
            else
            {
                Debug.Log("������ �ִ°� �ҷ����� �α�");
                foreach(DataSnapshot child in snapShot.Child("list").Children)
                {
                    Debug.Log("�׽�Ʈ�׽�Ʈ");
                    Debug.Log($"{child.Key} : {child.Value}");
                }
                Debug.Log(level);
                Debug.Log(snapShot.Child("level").Value.ToString());
               //  level = int.Parse(snapShot.Child("level").Value.ToString()); //���� ���ڿ��� ��ȯ�ؼ�
                level = (long)snapShot.Child("level").Value;
                Debug.Log($"��ȯ�� ���� : {level}");
                // ������ ��������
                //���ڿ��� �ٲٴ°� ���������� ������Ʈ�� ��� �ȵ�..
                // int level = (int)snapShot.Child("level").Value; // �̷��� �ȵ�...
                Debug.Log(level);


                /*
                string json = snapShot.GetRawJsonValue();
                Debug.Log(json);    

                UserData userData = JsonUtility.FromJson<UserData>(json);
                Debug.Log(userData.name);
                Debug.Log(userData.email);
                Debug.Log(userData.level);
                Debug.Log($"�׽�Ʈ �׽�Ʈ {userData.list[0]}");
                Debug.Log(userData.list[1]);
                Debug.Log(userData.list[2]);
                */

            }
            // snapShot.Child("name"); // �̷������� snapShot���� �����ü�����.
            // snapShot.Child("email");

        });

        levelRef.ValueChanged += LevelRef_ValueChanged; ;

    }
    private void OnDisable()
    {
        levelRef.ValueChanged -= LevelRef_ValueChanged; ;
    }
    private void LevelRef_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Debug.Log($"������ �̺�Ʈ Ȯ�� �α�: {e.Snapshot.Value.ToString()}");
        level = int.Parse(e.Snapshot.Value.ToString());
        
    }

    public void LevelUp()
    {
        userDataRef.Child("list").OrderByValue().GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            if(task.IsCanceled)
            {
                return;
            }
            DataSnapshot snapShot = task.Result;
            foreach(DataSnapshot child in snapShot.Children)
            {
                Debug.Log($"{child.Key} {child.Value}");
            }

        });
        // return;

        // ���� �������� �з��ϱ�
        BackendManager.Database.RootReference.Child("UserData")
            .OrderByChild("level")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if(task.IsCanceled)
                    return;
                if (task.IsFaulted)
                    return;
                DataSnapshot snapShot = task.Result;
                foreach(DataSnapshot child in snapShot.Children)
                {
                    Debug.Log($"{child.Key} {child.Value}");
                }

            });

        //level++;
        levelRef.SetValueAsync(level + 1);

        UserData userData = new UserData();
        // userData.name = "�ֵ���";
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        userData.name = user.DisplayName;
        userData.email = "ysc1350@gmail.com";
       // userData.subData.value1 = "�ؽ�Ʈ";
       // userData.subData.value2 = 5;
       // userData.subData.value3 = 3.14f;

        string json = JsonUtility.ToJson(userData);
        userDataRef.SetRawJsonValueAsync(json);

    }
    public void LevelDown()
    {
        
        // level--;
        levelRef.SetValueAsync(level --);
        

        /* �ѹ� ����, SetValueAsync
        // �⺻ �ڷ����� ���� ����
        userDataRef.Child("string").SetValueAsync("�ؽ�Ʈ");
        userDataRef.Child("long").SetValueAsync(10);
        userDataRef.Child("double").SetValueAsync(3.14);
        userDataRef.Child("bool").SetValueAsync(true);

        // List �ڷᱸ���� ���� ���� ����
        // ������ ����ִ� ������ ������ : 1���� �� 2���� ���� etc...
        List<string> list = new List<string>() { "ù��°", "�ι�°", "����°" };
        userDataRef.Child("List").SetValueAsync(list);

        // Dictionary �ڷᱸ���� ���� Ű&�� ����
        Dictionary<string, object> dictionary = new Dictionary<string, object>()
        {
            { "stringKey", "�ؽ�Ʈ" },  // �̰�ó�� ���ٿ��� ���� �⺻�ڷ��� �������� ���°� ���ٰ���.
            { "longKey", 10 },
            { "doubleKey", 3.14 },
            { "boolKey", true },
        };
        userDataRef.Child("Dictionary").SetValueAsync(dictionary);
        */


        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("email", "choi13570@naver.com");
        keyValuePairs.Add("subData/value1", 20);


        userDataRef.UpdateChildrenAsync(keyValuePairs);
    }

    [Serializable]
    public class UserData
    {
        public string name;
        public string email;
        public string job;
        public int level;
        public int strength;
        public int agility;
        public int intelligence;
        public int luck;

        public List<string> list =  new List<string>();

        //public SubData subData = new SubData();
    }

    /* ���� SubData
    [Serializable]
    public class SubData
    {
        public string value1;
        public int value2;
        public float value3;
    }
    */
}
