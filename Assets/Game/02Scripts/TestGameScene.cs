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
        yield return new WaitForSeconds(1f);        // ��Ʈ��ũ �غ� �ʿ��� �ð� ��¦ �ֱ�
        TestGameStart();
    }
    public void TestGameStart()
    {
        Debug.Log("���ӽ���");

     
        PlayerSpawn();

       // CameraSpawn();

        if (PhotonNetwork.IsMasterClient == false)
            return;
        // �ؿ��Ŵ� ���� �÷��̾��� ��츸 �����ϴ� �ڵ�
        MonsterSpawn();
        spawnRoutine = StartCoroutine(MonsterSpawnRoutine());
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {   // ���ο� ������ �Ǹ� ������ �ϴ� �߿��� �������� / �ؾߵ� �͵��� �̾ �Ҽ� �ְ� �߰����ֱ�.
        if (newMasterClient.IsLocal)
        {
            spawnRoutine = StartCoroutine(MonsterSpawnRoutine());
        }

    }
    // �ó׸ӽ� ���߾�ķ �Ẹ�������� ����
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
        // ��ó�� �߰������� �� �����͸� �Ǿ� ����������
    }

    private void MonsterSpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            PhotonNetwork.Instantiate("GameObject/Monster", randomPos, Quaternion.identity);
            // Instantiate�� ���Ÿ� �׳� �濡�� ���̾��°ǵ�
            PhotonNetwork.InstantiateRoomObject("GameObject/Monster", randomPos, Quaternion.identity);
            // InstatntiateRoomObject�� �ٰ��� ����ϴ� �����Object, �׷��� ������ control������ �泪���� ���� �������� migrate��
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
