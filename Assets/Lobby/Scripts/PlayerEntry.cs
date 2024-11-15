using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text readyText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Button readyButton;

    public void SetPlayer(Player player)
    {

        if (player.IsMasterClient)
        {   // 방장 구분하기 쉽게 별달아주기, 특수문자 안되서 그냥 마스터... 
            nameText.text = $"MASTER\n{player.NickName}";
            // <color = red> name </color> html태그도 먹힌다고..
        }
        else
        {
            nameText.text = player.NickName;
        }
        readyButton.gameObject.SetActive(true);
        readyButton.interactable = player == PhotonNetwork.LocalPlayer; // 플레이어가 본인이지 확인 -> 레디버튼 player =isLocal도됨


        if (player.GetReady())
        {
            readyText.text = "Ready";
        }
        else
        {
            readyText.text = "";
        }

    }

    public void SetEmpty()
    {
        readyText.text = "";
        nameText.text = "None";
        readyButton.gameObject.SetActive(false);
    }

    public void Ready()
    {
        // 레디가 아니였으면 레디시키기
        // 레디가 맞았으면 레디풀기
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        ready = !ready;


        PhotonNetwork.LocalPlayer.SetReady(ready);
        if (ready)
        {
            PhotonNetwork.LocalPlayer.SetReady(true);
            readyText.text = "Ready";
            Debug.Log($"Ready: {ready}");
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetReady(false);
            readyText.text = "";
        }
    }
}
