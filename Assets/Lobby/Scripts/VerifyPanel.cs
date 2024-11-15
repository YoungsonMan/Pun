using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerifyPanel : MonoBehaviour
{

    [SerializeField] NickNamePanel nickNamePanel;
    private void OnEnable()
    {
        SendVerifyEmail();
    }

    private void OnDisable()
    {
        if (verificationRoutine != null)
        {
            StopCoroutine(verificationRoutine);
        }
    }

    private void SendVerifyEmail()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendEmailVerificationAsync was canceled.");
                gameObject.SetActive(false); // 실패하면 인증기다리는창 비활성화
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                gameObject.SetActive(false);
                return;
            }

            Debug.Log("Email sent successfully.");
            verificationRoutine = StartCoroutine(VerificationRoutine()); 
        });
    }

    Coroutine verificationRoutine;

    IEnumerator VerificationRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(3f);
        while (true)
        {
            // 인증 확인
            BackendManager.Auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task =>
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

                if (BackendManager.Auth.CurrentUser.IsEmailVerified == true)
                {
                    Debug.Log("인증호가인");
                    gameObject.SetActive(false);
                    nickNamePanel.gameObject.SetActive(true);
                }

                Debug.Log("User profile updated successfully.");
            });
            yield return delay; ;
        }
    }
}
