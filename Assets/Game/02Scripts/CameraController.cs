using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [SerializeField] float rotateSpeed;
    private float _xAxis;
    private float _yAxis;


    Vector3 lastMousePosition;
    
    private bool _isMoving = false;
    private bool _canReset = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        SetCamera();
    }
    void Update()
    {
        // 마우스가 안움직이면 다시 원래 처음 세팅 앞에보고 3인칭 뷰로 고정을 하려는데 잘 안됩니다.
        // 그래서 일단 F누르면 다시 처음 세팅으로 보게하려고했는데도 잘 안되요
        lastMousePosition = Input.mousePosition;
        if (Input.GetKeyDown(KeyCode.F))
        {
            _canReset = true;
        }
        else
            _canReset = false;
        
        InsertRotation();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        if (_isMoving == false)
        {
            SetCamera();
        }
        Rotate();

        SetCamera();

        ResetCamera();

    }
    private void InsertRotation()
    {
         _xAxis = Input.GetAxis("Mouse X");
         _yAxis = Input.GetAxis("Mouse Y");
        if (_xAxis != 0 || _yAxis != 0)   // if (Input.mousePosition != lastMousePosition) 안됨
        {
            _isMoving = true;
            Debug.Log($"마우스움직이는중 : {_isMoving}");
        }
        else _isMoving = false;
        Debug.Log($"Mouse 멈춤 : {_isMoving}");
    }
    private void Rotate()
    {
        transform.RotateAround(Vector3.up, rotateSpeed * _xAxis * Time.deltaTime);
        transform.RotateAround(-Vector3.right, rotateSpeed * _yAxis * Time.deltaTime);
    }
    private void SetCamera()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
    private void ResetCamera()
    {
        if (_canReset)
        {
          
            transform.position = target.position + offset;
            transform.LookAt(transform.forward);
            Debug.Log("CameraReset!");
        }
    }

}
