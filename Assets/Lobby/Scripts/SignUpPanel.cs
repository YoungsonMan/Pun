using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] TMP_InputField passwordConfirmInputField;

    [SerializeField] GameObject checkPopup;
    public void SignUp()
    {
        // Debug.Log("ȸ�������� �Ϸ�ƽ��ϴ�!");

        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirm = passwordConfirmInputField.text;

        if (email.IsNullOrEmpty())
        {
            Debug.LogWarning("�̸����� �Է����ּ���.");
            return;
        }

        if(password != confirm)
        {
            Debug.LogWarning("�н����尡 ��ġ���� �ʽ��ϴ�.");
            return;
        }
        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>  //�׳� ContinueWith���ϸ� ������ �־ ...OnMainThread���� �ؾ� �����ϴ�.
            {

               //  �̸��� �ߺ� ����: AuthError.EmailAlreadyInUse;
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Firebase.FirebaseException exception = task.Exception.InnerException as Firebase.FirebaseException;
                    switch((AuthError)exception.ErrorCode)
                    {

                        case AuthError.EmailAlreadyInUse:
                        Debug.LogWarning($"�̸����� �̹� ������Դϴ�.");
                        checkPopup.SetActive(true);
                        break;
                    }
                    
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                checkPopup.SetActive(true);
                AuthResult result = task.Result;
                Debug.Log($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
                gameObject.SetActive(false);
            });

    }
}
