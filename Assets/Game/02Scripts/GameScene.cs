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
        // 게임신 Start가 돌아간다는건 Loading씬이 끝났다는거, 그러니 SetLoad를 트루로.
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

            Debug.Log($"{targetPlayer.NickName}님이 로딩이 완료 되었습니다.");
            bool allLoaded = CheckAllLoad();
            Debug.Log($"모든 플레이어가 로딩이 완료되었는가??? : {allLoaded}");
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
        PhotonNetwork.CurrentRoom.IsOpen = true; // 끝났을때 들어올수 있게
        PhotonNetwork.LoadLevel("LobbyScene");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void GameStart()
    {
        StartCoroutine(CountDownRoutine());
        // NullReferenceException: Object reference not set to an instance of an object 에러뜸
    }
    IEnumerator CountDownRoutine()
    {
        for (int i = 3; i > 0; i--)
        {
            countDownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countDownText.text = "GAME START!!";
        Debug.Log("게임시작");

    }



    private bool CheckAllLoad()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)  // 다 돌아보는동안 안된사람이 있으면 안된거니까 false
                return false;
        }
        return true;
    }



}
