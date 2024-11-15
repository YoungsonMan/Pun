using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    public static BackendManager instance { get; private set; }

    private FirebaseApp app; // �⺻
    public static FirebaseApp App { get {  return instance.app; } }

    private FirebaseAuth auth;
    public static FirebaseAuth Auth { get { return instance.auth; } }

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
        // ȣȯ�� üũ���ϰ� ���������ִ� �ҽ��ڵ�. ó���� �˾�â��
        // Async, �񵿱�� (Task, MultiThread������� üũ�ϰ�����)
        // �񵿱�� �۾��̳�������(�α������) �̰��ϰڴ�.
        // CheckAndFixDependenciesAsync()�̰� �ϰ� ������ContinueWithOnMainThread(task =>... �̰��Ѵ� ������ ����
        // �ڿ� �ٿ��� �ϳ��� task ���ٽ�����
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance; //�����ϸ� DefaultInstance ��
                auth = FirebaseAuth.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase dependencies check success");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
                // Firebase Unity SDK is not safe to use here.
                app = null; // ���� null �̴� ? ������ �ȵƴ�.
            }
        });
    }
}
