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

    // �濡 ���Ծ��� ��
    private void OnEnable()
    {
        UpdatePlayers();

        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);


        // ������� üũ�α�
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        Debug.Log($"�������: {ready}");
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

        foreach (Player player in PhotonNetwork.PlayerList) // ���� �濡�ִ� ��� �÷��̾�� ��������
        {
            // �ѹ��� �Ҵ�Ǳ� ������ -1�̴ϱ� �� �÷��̾�� �Ҵ����� �ʰڴ�.
            if (player.GetPlayerNumber() == -1)
                continue;

            int number = player.GetPlayerNumber();
            playerEntries[number].SetPlayer(player);
        }

        // ���⼭ ����̻��� �ȵǰԲ� ���ǹ��� �ɸ� ����̻���� �ϰ� �� �� ����.
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {   // ������ �����϶��� ������ �ְԲ�..
            startButton.interactable = CheckAllReady();
        }
        else
        {
            startButton.interactable = false;
        }

    }

    public void EnterPlayer(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} ����!");
        UpdatePlayers();
    }

    public void ExitPlayer(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} ����!");
        UpdatePlayers();
    }

    public void UpdatePlayerProperty(Player targetPlayer, Hashtable properties)
    {
        // Ready Cumstom Property�� ������ ���� READY Ű�� ����.
        if (properties.ContainsKey(CustomProperty.READY))
        {
            UpdatePlayers();
        }
    }
    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // �� ���� �Ѹ��̶� false�� ����ȵȻ���ִ°Ŵϱ�
            if (player.GetReady() == false)
                return false;
        }   // �� �������� �� �����
        return true;
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
        PhotonNetwork.CurrentRoom.IsOpen = false; // �����߿��� ����������, �������ؼ� ���� �����ϰԵ� ����
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
