using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] Button gameOverButton;
    [SerializeField] Button leaveRoomButton;


    [SerializeField] TMP_Text countDownText;


    private void Start()
    {
        // ���ӽ� Start�� ���ư��ٴ°� Loading���� �����ٴ°�, �׷��� SetLoad�� Ʈ���.
        PhotonNetwork.LocalPlayer.SetLoad(true);

        if (PhotonNetwork.IsMasterClient)
        {
            gameOverButton.interactable = true;
        }
        else
        {
            gameOverButton.interactable = false;
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {

            Debug.Log($"{targetPlayer.NickName}���� �ε��� �Ϸ� �Ǿ����ϴ�.");
            bool allLoaded = CheckAllLoad();
            Debug.Log($"��� �÷��̾ �ε��� �Ϸ�Ǿ��°�??? : {allLoaded}");
            if (allLoaded)
            {
                GameStart();
            }

        }
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameOverButton.interactable = true;
        }
        else
        {
            gameOverButton.interactable = false;
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    public void GameOver()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = true; // �������� ���ü� �ְ�
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void GameStart()
    {
        StartCoroutine(CountDownRoutine());
        // NullReferenceException: Object reference not set to an instance of an object ������
    }
    IEnumerator CountDownRoutine()
    {
        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countDownText.text = "GAME START!!";
        Debug.Log("���ӽ���");

    }



    private bool CheckAllLoad()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)  // �� ���ƺ��µ��� �ȵȻ���� ������ �ȵȰŴϱ� false
                return false;
        }
        return true;
    }



}
