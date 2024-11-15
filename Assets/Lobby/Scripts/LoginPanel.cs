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
        if (user == null)  //Ȥ�� �α�ȵǼ� ���� ������ ����
            return;
        // Firebase�� UserId ������ �����Ѱ� �ο��޾Ƽ� �̰� ������.

        Debug.Log($"Display Name :\t {user.DisplayName}");
        Debug.Log($"Email :\t\t {user.Email}");
        Debug.Log($"Email Verified:\t  {user.IsEmailVerified}");
        Debug.Log($"User ID: \t\t  {user.UserId}");
        
        if (user.IsEmailVerified == false)
        {
            // TODO: �̸��� ���� ����
            verifyPanel.gameObject.SetActive(true);
        }
        else if (user.DisplayName == "")
        {
            // ���� ����
            nickNamePanel.gameObject.SetActive(true);
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = user.DisplayName;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    /* ������
    private void Start()
    {
        idInputField.text = $"Player {Random.Range(1000, 10000)}";
    }

    public void Login()
    {
        if (idInputField.text == "")
        {
            Debug.LogWarning("���̵� �Է��ؾ� ������ �����մϴ�.");
            return;
        }

        // ������ ��û
        // PhotonNetwork._____ �� ������ ��û�� ������ �� ����
        PhotonNetwork.LocalPlayer.NickName = idInputField.text; // �÷��̾� �г��� ����
        Debug.Log("�α��� ��û");
        PhotonNetwork.ConnectUsingSettings();                   // ���� ���������� �������� ���� ��û
    }
    */
}
