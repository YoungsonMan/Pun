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
        // 이렇게 이름으로한는것보단 levelRef = BackendManager.Database.RootReference.Child("UserData/choi/level");
        string uID = BackendManager.Auth.CurrentUser.UserId;
        userDataRef = BackendManager.Database.RootReference.Child("UserData").Child(uID);
        levelRef = userDataRef.Child("level");

        // 결과를 받아오는 CallBack
        levelRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("값 가져오기 취소됨");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogWarning($"값 가져오기 실패: {task.Exception.Message}");
                return;
            }
            Debug.Log("값 가져오기 성공");
            DataSnapshot snapShot = task.Result; // result는 데이터베이스의 sanpShot으로나오고
            // snapShot은 "착" 캡쳐해서 들고오는  
            Debug.Log(snapShot.Value);
            if(snapShot.Value == null) // 데이터베이스에 데이터가 없는경우
            {   // 없었으면 초기값으로
                UserData userData = new UserData();
                userData.name = BackendManager.Auth.CurrentUser.DisplayName;
                userData.email = BackendManager.Auth.CurrentUser.Email;
                userData.level = 1;

                // 리스트 의미없는데 지금거는 그냥 테스트용
                userData.list.Add("첫번쨰");
                userData.list.Add("두번쨰");
                userData.list.Add("세번쨰");

                string json = JsonUtility.ToJson(userData);
                userDataRef.SetRawJsonValueAsync(json);
            }
            else
            {
                Debug.Log("있으면 있는거 불러오는 로그");
                foreach(DataSnapshot child in snapShot.Child("list").Children)
                {
                    Debug.Log("테스트테스트");
                    Debug.Log($"{child.Key} : {child.Value}");
                }
                Debug.Log(level);
                Debug.Log(snapShot.Child("level").Value.ToString());
               //  level = int.Parse(snapShot.Child("level").Value.ToString()); //값을 문자열로 변환해서
                level = (long)snapShot.Child("level").Value;
                Debug.Log($"변환된 레벨 : {level}");
                // 레벨만 가져오기
                //문자열로 바꾸는게 좋진않은데 오브젝트가 어떻게 안됨..
                // int level = (int)snapShot.Child("level").Value; // 이렇게 안됨...
                Debug.Log(level);


                /*
                string json = snapShot.GetRawJsonValue();
                Debug.Log(json);    

                UserData userData = JsonUtility.FromJson<UserData>(json);
                Debug.Log(userData.name);
                Debug.Log(userData.email);
                Debug.Log(userData.level);
                Debug.Log($"테스트 테스트 {userData.list[0]}");
                Debug.Log(userData.list[1]);
                Debug.Log(userData.list[2]);
                */

            }
            // snapShot.Child("name"); // 이런식으로 snapShot으로 가져올수있음.
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
        Debug.Log($"값변경 이벤트 확인 로그: {e.Snapshot.Value.ToString()}");
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

        // 유저 레벨별로 분류하기
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
        // userData.name = "최도적";
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        userData.name = user.DisplayName;
        userData.email = "ysc1350@gmail.com";
       // userData.subData.value1 = "텍스트";
       // userData.subData.value2 = 5;
       // userData.subData.value3 = 3.14f;

        string json = JsonUtility.ToJson(userData);
        userDataRef.SetRawJsonValueAsync(json);

    }
    public void LevelDown()
    {
        
        // level--;
        levelRef.SetValueAsync(level --);
        

        /* 한번 쓰기, SetValueAsync
        // 기본 자료형을 통한 저장
        userDataRef.Child("string").SetValueAsync("텍스트");
        userDataRef.Child("long").SetValueAsync(10);
        userDataRef.Child("double").SetValueAsync(3.14);
        userDataRef.Child("bool").SetValueAsync(true);

        // List 자료구조를 통한 순차 저장
        // 여러개 들고있는 아이템 같은거 : 1번에 뭐 2번에 포션 etc...
        List<string> list = new List<string>() { "첫번째", "두번째", "세번째" };
        userDataRef.Child("List").SetValueAsync(list);

        // Dictionary 자료구조를 통한 키&값 저장
        Dictionary<string, object> dictionary = new Dictionary<string, object>()
        {
            { "stringKey", "텍스트" },  // 이거처럼 쓸바에는 위에 기본자료형 저장으로 쓰는게 낫다고함.
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

    /* 위에 SubData
    [Serializable]
    public class SubData
    {
        public string value1;
        public int value2;
        public float value3;
    }
    */
}
