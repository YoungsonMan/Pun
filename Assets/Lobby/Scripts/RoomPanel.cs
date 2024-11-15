using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] PlayerEntry[] playerEntries;
    [SerializeField] Button startButton;

    // 방에 들어왔었을 때
    private void OnEnable()
    {
        UpdatePlayers();

        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);


        // 레디상태 체크로그
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        Debug.Log($"레디상태: {ready}");
    }

    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;
    }

    public void UpdatePlayers()
    {
        foreach (PlayerEntry entry in playerEntries)
        {
            entry.SetEmpty();
        }

        foreach (Player player in PhotonNetwork.PlayerList) // 현제 방에있는 모든 플레이어들 가져오기
        {
            // 넘버가 할당되기 저에는 -1이니까 그 플레이어는 할당하지 않겠다.
            if (player.GetPlayerNumber() == -1)
                continue;

            int number = player.GetPlayerNumber();
            playerEntries[number].SetPlayer(player);
        }

        // 여기서 몇명이상은 안되게끔 조건문을 걸면 몇명이상부터 하게 할 수 있음.
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {   // 본이이 방장일때만 누를수 있게끔..
            startButton.interactable = CheckAllReady();
        }
        else
        {
            startButton.interactable = false;
        }

    }

    public void EnterPlayer(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 입장!");
        UpdatePlayers();
    }

    public void ExitPlayer(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} 퇴장!");
        UpdatePlayers();
    }

    public void UpdatePlayerProperty(Player targetPlayer, Hashtable properties)
    {
        // Ready Cumstom Property를 변경한 경우면 READY 키가 있음.
        if (properties.ContainsKey(CustomProperty.READY))
        {
            UpdatePlayers();
        }
    }
    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 다 돌때 한명이라도 false면 레디안된사람있는거니까
            if (player.GetReady() == false)
                return false;
        }   // 다 돌았을때 다 레디면
        return true;
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
        PhotonNetwork.CurrentRoom.IsOpen = false; // 게임중에는 못들어오게함, 설정안해서 난입 가능하게도 가능
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
