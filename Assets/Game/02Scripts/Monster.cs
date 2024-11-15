using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviourPun
{


    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false)  
            return;
        // IsMine이 결국 방장이니까, 소유권이 나다라는게 사실상 내가 방장이면 동작시켜달라와
        // 동일하게 작동함
        if (PhotonNetwork.IsMasterClient == false)
            return;

        Debug.Log("몬스터 동작 진행");
        
    }
    public void TakeHit()
    {
        Destroy(gameObject);
    }
}
