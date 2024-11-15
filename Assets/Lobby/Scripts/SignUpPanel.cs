using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] TMP_InputField passwordConfirmInputField;

    public void SignUp()
    {
        // Debug.Log("회원가입이 완료됐습니다!");

        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirm = passwordConfirmInputField.text;

        if (email.IsNullOrEmpty())
        {
            Debug.LogWarning("이메일을 입력해주세요.");
            return;
        }

        if(password != confirm)
        {
            Debug.LogWarning("패스워드가 일치하지 않습니다.");
            return;
        }
        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>  //그냥 ContinueWith만하면 터질수 있어서 ...OnMainThread까지 해야 안전하다.
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                AuthResult result = task.Result;
                Debug.Log($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
                gameObject.SetActive(false);
            });


    }
}
