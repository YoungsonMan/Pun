using Firebase;
using Firebase.Auth;  // 인증관련
using Firebase.Database; // 데이터베이스
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    // public const string UserDataPath = "UserData"; 이렇게 상수로 만들어 쓰는것도 괜츈
    public static BackendManager instance { get; private set; }

    private FirebaseApp app; // 기본
    public static FirebaseApp App { get {  return instance.app; } }

    private FirebaseAuth auth;
    public static FirebaseAuth Auth { get { return instance.auth; } }

    private FirebaseDatabase database;
    public static FirebaseDatabase Database { get { return instance.database; } }

    private void Awake()
    {
        SetSingleton();

    }
    private void Start()
    {
        CheckDependency();
    }


    private void SetSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CheckDependency()
    {
        // 호환성 체크도하고 수정도해주는 소스코드. 처음에 팝업창그
        // Async, 비동기식 (Task, MultiThread방식으로 체크하고있음)
        // 비동기식 작업이끝났을떄(로긴됐을떄) 이거하겠다.
        // CheckAndFixDependenciesAsync()이거 하고 끝나면ContinueWithOnMainThread(task =>... 이거한다 식으로 구현
        // 뒤에 붙여서 하나로 task 람다식으로
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance; //성공하면 DefaultInstance 로
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase dependencies check success");


                /*
                DatabaseReference root = BackendManager.Database.RootReference; // 최상위 위치, root
                DatabaseReference userData = root.Child("UserData");
                DatabaseReference choiLevel = root.Child("UserData").Child("choi").Child("Level");
                DatabaseReference gildongLevel = userData.Child("gildong").Child("Level");

                // 한번쓰기
                choiLevel.SetRawJsonValueAsync(3);
                */
               

            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
                // Firebase Unity SDK is not safe to use here.
                app = null; // 앱이 null 이다 ? 연동이 안됐다.
                auth = null;
                database = null;
            }
        });

        // 이런식으로 사용될 예정 BackendManager.Database.RootReference.Child("Name").GetValueAsync();

    }

    

}
