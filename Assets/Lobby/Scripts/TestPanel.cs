using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : BaseUI
{
    [SerializeField] GameObject roomList;
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject scrollbarVertical;

    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;
    private Dictionary<string, RoomEntry> roomDictionary = new Dictionary<string, RoomEntry>();
    void Start()
    {
        roomList = GetUI("RoomList");
        // Image roomListImage = GetUI<Image>("RoomList");
        // roomListImage.color = Color.red;
        leaveButton = GetUI("LeaveButton");
        scrollbarVertical = GetUI("Scrollbar Vertical");

        roomContent = GetUI<RectTransform>("Content");

        roomList.SetActive(true);
        leaveButton.SetActive(true);
    }
    void OnEnable()
    {
        roomList.SetActive(true);
        leaveButton.SetActive(true);
    }
    public void SubscirbesEvents()
    {
        GetUI<Button>("LeaveButton").onClick.AddListener(LeaveLobby);
    }
    public void LeaveLobby()
    {
        Debug.Log("�κ� ���� ��û");
        PhotonNetwork.LeaveLobby();
    }
    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // ���� ����� ��� + ���� ������� ��� + ������ �Ұ����� ���� ���
            if (info.RemovedFromList == true || info.IsVisible == false || info.IsOpen == false)
            {
                // ���� ��Ȳ : �κ� ���ڸ��� ������� ���� ���
                if (roomDictionary.ContainsKey(info.Name) == false)
                    continue;

                Destroy(roomDictionary[info.Name].gameObject);
                roomDictionary.Remove(info.Name);
            }

            // ���ο� ���� ������ ���
            else if (roomDictionary.ContainsKey(info.Name) == false)
            {
                RoomEntry roomEntry = Instantiate(roomEntryPrefab, roomContent);
                roomDictionary.Add(info.Name, roomEntry);
                roomEntry.SetRoomInfo(info);
            }

            // ���� ������ ����� ���
            else if (roomDictionary.ContainsKey((string)info.Name) == true)
            {
                RoomEntry roomEntry = roomDictionary[info.Name];
                roomEntry.SetRoomInfo(info);
            }
        }
    }

    public void ClearRoomEntries()
    {
        foreach (string name in roomDictionary.Keys)
        {
            Destroy(roomDictionary[name].gameObject);
        }
        roomDictionary.Clear();
    }

}
