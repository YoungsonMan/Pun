using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Bullet : MonoBehaviourPun, IPunObservable
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float speed;
    public float Speed { get { return speed; }}

    public int damage;



    [SerializeField] Renderer bodyRenderer;
    [SerializeField] Color color;

    [SerializeField] Color[] colors; // 얘는 따로두는게 좋긴함.
    private bool _isHit = false;
   

    void Start()
    {
        rigid.velocity = transform.forward * speed;

        Destroy(gameObject, 3f);
        int number = photonView.Owner.GetPlayerNumber();
        color = colors[number];
        bodyRenderer.material.color = color;
    }

    void Update()
    {
        
    }

    [PunRPC]
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            Destroy(gameObject);

            if (collision.gameObject.tag == "Player")
            {
                GameObject instance = collision.gameObject;
                PlayerController player = instance.GetComponent<PlayerController>();
                _isHit = true;
                player.TakeHit(damage);
            }
            else if(collision.gameObject.tag == "Monster")
            {
                GameObject instance = collision.gameObject;
                Monster monster = instance.GetComponent<Monster>();
                monster.TakeHit();
                Destroy(collision.gameObject);
            }
            else
                return;
            _isHit = false;



        }

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(color.r);
            stream.SendNext(color.g);
            stream.SendNext(color.b);

        }
        else if (stream.IsReading)
        {

            color.r = (float)stream.ReceiveNext();
            color.g = (float)stream.ReceiveNext();
            color.b = (float)stream.ReceiveNext();

        }

    }

}
