using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : BaseUI
{
    [SerializeField] GameObject roomList;
    [SerializeField] GameObject leaveButton;
    [SerializeField] GameObject scrollbarVertical;

    void Start()
    {
        roomList = GetUI("RoomList");
        // Image roomListImage = GetUI<Image>("RoomList");
        // roomListImage.color = Color.red;
        leaveButton = GetUI("LeaveButton");
        scrollbarVertical = GetUI("Scrollbar Vertical");


        roomList.SetActive(false);
        leaveButton.SetActive(false);
    }

}
