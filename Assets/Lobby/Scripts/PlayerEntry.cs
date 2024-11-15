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
        {   // ���� �����ϱ� ���� ���޾��ֱ�, Ư������ �ȵǼ� �׳� ������... 
            nameText.text = $"MASTER\n{player.NickName}";
            // <color = red> name </color> html�±׵� �����ٰ�..
        }
        else
        {
            nameText.text = player.NickName;
        }
        readyButton.gameObject.SetActive(true);
        readyButton.interactable = player == PhotonNetwork.LocalPlayer; // �÷��̾ �������� Ȯ�� -> �����ư player =isLocal����


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
        // ���� �ƴϿ����� �����Ű��
        // ���� �¾����� ����Ǯ��
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
