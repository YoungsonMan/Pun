using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform cameraPoint;

    public const string RoomName = "TestRoom";
    Coroutine spawnRoutine;
    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);        // 네트워크 준비에 필요한 시간 살짝 주기
        TestGameStart();
    }
    public void TestGameStart()
    {
        Debug.Log("게임시작");

     
        PlayerSpawn();

       // CameraSpawn();

        if (PhotonNetwork.IsMasterClient == false)
            return;
        // 밑에거는 방장 플레이어인 경우만 진행하는 코드
        MonsterSpawn();
        spawnRoutine = StartCoroutine(MonsterSpawnRoutine());
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {   // 새로운 방장이 되면 방장이 하던 중요한 로직들을 / 해야될 것들을 이어서 할수 있게 추가해주기.
        if (newMasterClient.IsLocal)
        {
            spawnRoutine = StartCoroutine(MonsterSpawnRoutine());
        }

    }
    // 시네머신 버추어캠 써보려했으나 실패
    private void CameraSpawn()
    {
        PhotonNetwork.Instantiate("GameObject/PlayerCamera", cameraPoint.position, Quaternion.identity );
    }
    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        Color color = Random.ColorHSV();
        object[] data = { color.r, color.g, color.b };
        
        PhotonNetwork.Instantiate("GameObject/Player", randomPos, Quaternion.identity, data : data);
        // 위처럼 추가적으로 더 데이터를 실어 보낼수있음
    }

    private void MonsterSpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            PhotonNetwork.Instantiate("GameObject/Monster", randomPos, Quaternion.identity);
            // Instantiate는 내거를 그냥 방에서 같이쓰는건데
            PhotonNetwork.InstantiateRoomObject("GameObject/Monster", randomPos, Quaternion.identity);
            // InstatntiateRoomObject는 다같이 사용하는 방공공Object, 그래도 방장이 control하지만 방나가도 다음 방장한테 migrate됨
        }
    }

    IEnumerator MonsterSpawnRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(60f);

        while (true)
        {
            yield return delay;
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            PhotonNetwork.InstantiateRoomObject("GameObject/Monster", randomPos, Quaternion.identity);
        }
    }
}
