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

    

    // 전달을 하고싶으면 콜라이더를 쓰고싶어도 포돈뷰 기준으로
   // [SerializeField] PhotonView otherNetworkObject;
   // [SerializeField] Collider collider;


   // public int value1;
   // public float value2;
   // public float value3;
    [SerializeField] Renderer bodyRenderer;
    [SerializeField] Color color;

    [SerializeField] Color[] colors; // 얘는 따로두는게 좋긴함.

    private void Start()
    {
      //  object[] data = photonView.InstantiationData;
      //  // 다을때 순서 조심해야함(똑같게)
      //  color.r = (float)data[0];
      //  color.g = (float)data[1];
      //  color.b = (float)data[2];
      //  bodyRenderer.material.color = color;


        // 위에 꺼 이렇게도 됨
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
        if (photonView.IsMine == false) // phtonView.Owner.IsLocal == false 도 됨
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


        // 뭔가 잘못되서 저쪽 캐릭터 색을 바꾼다. 아니 그렇게 코드를 짜게된로직은 보니까 알긴하겠는데
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
        // MasterClient 마스터서버로  1. 요청 -> 2. Master가 처리 -> 3. 통보
        // ViaServer    포톤 서버 거쳐서
    }



    [PunRPC]
    private void FireRPC(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = Mathf.Abs((float)PhotonNetwork.Time - (float)info.SentServerTime);
        Debug.Log($"지연시간 : {lag}");
        position += bulletPrefab.Speed * lag * (rotation * Vector3.forward);
        PhotonNetwork.Instantiate("GameObject/Bullet", position, rotation);
        Debug.Log("RPC 총알발사!");

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


            // 콜라이더는 참조형식이라 아이디를 보내서 (참조형식은 다 이런식으로 보내야함)
         //   PhotonView photonView = collider.GetComponent<PhotonView>();
         //   stream.SendNext(otherNetworkObject.ViewID);

        }
        else if (stream.IsReading) 
        {

            color.r = (float)stream.ReceiveNext();
            color.g = (float)stream.ReceiveNext();
            color.b = (float)stream.ReceiveNext();

            // 아이디를 받고
           // int id = (int)stream.ReceiveNext();
           // // 해댱아이디의 포톤뷰를 찾는다.
           // PhotonView target = PhotonView.Find(id);
           // collider = target.GetComponent<Collider>();

        }

        /* 어지간한거 다 있어서 있는거 쓰면됨
        // 보내는 경우는 소유권자 일때.
        if (stream.IsWriting) // IsWriting 보낼때  true일때 보내는 상황이다.
        {
            // 준순서대로 받기때문에
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(bodyRenderer.material.color.r);
            stream.SendNext(bodyRenderer.material.color.g);
            stream.SendNext(bodyRenderer.material.color.b);

            stream.SendNext(value1);
            stream.SendNext(value2);
            stream.SendNext(value3);
        }
        // 받는건 소유권자가 아닐때
        else if (stream.IsReading) // IsReading 받을때, 다른클라이언트가 나한테 데이터를 보낼때
        {
            // 받는 순서를 준순서대로 해야한다.
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
