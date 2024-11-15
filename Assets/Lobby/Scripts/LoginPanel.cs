using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passwordField;

    [SerializeField] NickNamePanel nickNamePanel;
    [SerializeField] VerifyPanel verifyPanel;

    //[SerializeField] TMP_InputField idInputField;

    public void Login()
    {
        string email = emailInputField.text;
        string password = passwordField.text;



        BackendManager.Auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task  =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                AuthResult result = task.Result;
                Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
                CheckUserInfo();
            });
    }
    public void CheckUserInfo()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)  //혹시 로긴안되서 유저 없으면 리턴
            return;
        // Firebase는 UserId 생성시 고유한걸 부여받아서 이게 유용함.

        Debug.Log($"Display Name :\t {user.DisplayName}");
        Debug.Log($"Email :\t\t {user.Email}");
        Debug.Log($"Email Verified:\t  {user.IsEmailVerified}");
        Debug.Log($"User ID: \t\t  {user.UserId}");
        
        if (user.IsEmailVerified == false)
        {
            // TODO: 이메일 인증 진행
            verifyPanel.gameObject.SetActive(true);
        }
        else if (user.DisplayName == "")
        {
            // 접속 진행
            nickNamePanel.gameObject.SetActive(true);
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = user.DisplayName;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    /* 구버전
    private void Start()
    {
        idInputField.text = $"Player {Random.Range(1000, 10000)}";
    }

    public void Login()
    {
        if (idInputField.text == "")
        {
            Debug.LogWarning("아이디를 입력해야 접속이 가능합니다.");
            return;
        }

        // 서버에 요청
        // PhotonNetwork._____ 로 서버에 요청을 진행할 수 있음
        PhotonNetwork.LocalPlayer.NickName = idInputField.text; // 플레이어 닉네임 설정
        Debug.Log("로그인 요청");
        PhotonNetwork.ConnectUsingSettings();                   // 포톤 설정파일을 내용으로 접속 신청
    }
    */
}
