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
        // IsMine�� �ᱹ �����̴ϱ�, �������� ���ٶ�°� ��ǻ� ���� �����̸� ���۽��Ѵ޶��
        // �����ϰ� �۵���
        if (PhotonNetwork.IsMasterClient == false)
            return;

        Debug.Log("���� ���� ����");
        
    }
    public void TakeHit()
    {
        Destroy(gameObject);
    }
}
