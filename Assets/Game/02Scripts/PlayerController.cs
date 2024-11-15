using JetBrains.Annotations;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] int hp;
    [SerializeField] float rotateSpeed;
    [SerializeField] float speed;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform muzzlePoint;

    private float _xAxis;
    private float _yAxis;

    private bool _isHit = false;

    

    // ������ �ϰ������ �ݶ��̴��� ����; ������ ��������
   // [SerializeField] PhotonView otherNetworkObject;
   // [SerializeField] Collider collider;


   // public int value1;
   // public float value2;
   // public float value3;
    [SerializeField] Renderer bodyRenderer;
    [SerializeField] Color color;

    [SerializeField] Color[] colors; // ��� ���εδ°� ������.

    private void Start()
    {
      //  object[] data = photonView.InstantiationData;
      //  // ������ ���� �����ؾ���(�Ȱ���)
      //  color.r = (float)data[0];
      //  color.g = (float)data[1];
      //  color.b = (float)data[2];
      //  bodyRenderer.material.color = color;


        // ���� �� �̷��Ե� ��
        int number = photonView.Owner.GetPlayerNumber();
        color = colors[number];
        bodyRenderer.material.color = color;

      //  if (photonView.IsMine)
      //  {
      //      CameraController camController = Camera.main.GetComponent<CameraController>();
      //      camController.target = transform;
      //  }


    }


    private void Update()
    {
        if (photonView.IsMine == false) // phtonView.Owner.IsLocal == false �� ��
            return;
        Move();
        Rotate();
        if (photonView.IsMine == false)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        
        bodyRenderer.material.color = color;


        // ���� �߸��Ǽ� ���� ĳ���� ���� �ٲ۴�. �ƴ� �׷��� �ڵ带 ¥�Եȷ����� ���ϱ� �˱��ϰڴµ�
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     bodyRenderer.material.color = Random.ColorHSV();
        // }
    }
    private void Rotate()
    {
        _xAxis = Input.GetAxis("Mouse X");
        transform.RotateAround(Vector3.up, rotateSpeed * _xAxis * Time.deltaTime);
    }
    public void Move()
    {
        Vector3 moveDir = new Vector3();
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");
       

        // moveDir = transform.TransformDirection(moveDir);

        

        transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);
        if (moveDir == Vector3.zero)
            return;
        Quaternion lookRoatation = Quaternion.LookRotation(moveDir);
        transform.rotation = lookRoatation;

    }
 
    private void Fire()
    {
        photonView.RPC(nameof(FireRPC), RpcTarget.MasterClient, muzzlePoint.position, muzzlePoint.rotation);
        // MasterClient �����ͼ�����  1. ��û -> 2. Master�� ó�� -> 3. �뺸
        // ViaServer    ���� ���� ���ļ�
    }



    [PunRPC]
    private void FireRPC(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = Mathf.Abs((float)PhotonNetwork.Time - (float)info.SentServerTime);
        Debug.Log($"�����ð� : {lag}");
        position += bulletPrefab.Speed * lag * (rotation * Vector3.forward);
        PhotonNetwork.Instantiate("GameObject/Bullet", position, rotation);
        Debug.Log("RPC �Ѿ˹߻�!");

        if (_isHit)
        {
            photonView.RPC(nameof(Success), RpcTarget.All);
        }
        else
        {
            photonView.RPC(nameof(Failure), RpcTarget.All);
        }

    }
    [PunRPC]
    public void Success()
    {
        
    }
    [PunRPC]
    public void Failure()
    {

    }
    [PunRPC]
    public void TakeHit(int damage)
    {
        hp -= damage;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isHit = true;
        }
        else
            return;

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(color.r);
            stream.SendNext(color.g);
            stream.SendNext(color.b);


            // �ݶ��̴��� ���������̶� ���̵� ������ (���������� �� �̷������� ��������)
         //   PhotonView photonView = collider.GetComponent<PhotonView>();
         //   stream.SendNext(otherNetworkObject.ViewID);

        }
        else if (stream.IsReading) 
        {

            color.r = (float)stream.ReceiveNext();
            color.g = (float)stream.ReceiveNext();
            color.b = (float)stream.ReceiveNext();

            // ���̵� �ް�
           // int id = (int)stream.ReceiveNext();
           // // �؈Ծ��̵��� ����並 ã�´�.
           // PhotonView target = PhotonView.Find(id);
           // collider = target.GetComponent<Collider>();

        }

        /* �������Ѱ� �� �־ �ִ°� �����
        // ������ ���� �������� �϶�.
        if (stream.IsWriting) // IsWriting ������  true�϶� ������ ��Ȳ�̴�.
        {
            // �ؼ������ �ޱ⶧����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(bodyRenderer.material.color.r);
            stream.SendNext(bodyRenderer.material.color.g);
            stream.SendNext(bodyRenderer.material.color.b);

            stream.SendNext(value1);
            stream.SendNext(value2);
            stream.SendNext(value3);
        }
        // �޴°� �������ڰ� �ƴҶ�
        else if (stream.IsReading) // IsReading ������, �ٸ�Ŭ���̾�Ʈ�� ������ �����͸� ������
        {
            // �޴� ������ �ؼ������ �ؾ��Ѵ�.
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();


            Color color = new Color();
            color.r = (float)stream.ReceiveNext();
            color.g = (float)stream.ReceiveNext();
            color.b = (float)stream.ReceiveNext();


            value1 = (int)stream.ReceiveNext();
            value2 = (float)stream.ReceiveNext();
            value3 = (float)stream.ReceiveNext();
        }
        */
    }


}
