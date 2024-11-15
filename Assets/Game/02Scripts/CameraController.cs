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
        // ���콺�� �ȿ����̸� �ٽ� ���� ó�� ���� �տ����� 3��Ī ��� ������ �Ϸ��µ� �� �ȵ˴ϴ�.
        // �׷��� �ϴ� F������ �ٽ� ó�� �������� �����Ϸ����ߴµ��� �� �ȵǿ�
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
        if (_xAxis != 0 || _yAxis != 0)   // if (Input.mousePosition != lastMousePosition) �ȵ�
        {
            _isMoving = true;
            Debug.Log($"���콺�����̴��� : {_isMoving}");
        }
        else _isMoving = false;
        Debug.Log($"Mouse ���� : {_isMoving}");
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
