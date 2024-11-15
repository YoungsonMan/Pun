using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickNamePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField nickNameInputField;

    public void Confirm()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        string nickName = nickNameInputField.text;
        if (nickName == "")
        {
            Debug.LogWarning("닉네임을 설정해주세요.");
            return;
        }

        UserProfile profile = new UserProfile();
        profile.DisplayName = nickName;

        BackendManager.Auth.CurrentUser.UpdateUserProfileAsync(profile)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");

                Debug.Log($"Display Name :\t {user.DisplayName}");
                Debug.Log($"Email :\t\t {user.Email}");
                Debug.Log($"Email Verified:\t  {user.IsEmailVerified}");
                Debug.Log($"User ID: \t\t  {user.UserId}");


                PhotonNetwork.LocalPlayer.NickName = nickName;
                PhotonNetwork.ConnectUsingSettings();
                gameObject.SetActive(false);
            });

    }
}
