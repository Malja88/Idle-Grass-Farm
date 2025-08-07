using UniRx;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Controls controls;
    private Rigidbody rb;
    private Animator anim;
    private bool isMoving = true;
    private bool isRotating = true;
    [SerializeField] private float speed;

    #region Booleans Properties

    public bool IsMoving { get => isMoving; set => isMoving = value; }

    #endregion
    private void Awake()
    {
        controls = new Controls();
    }

    void Start()
    {
        controls.Enable();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Observable.EveryFixedUpdate().Subscribe(_ => { PlayerMove();});
    }

    void PlayerMove()
    {
        if (!isMoving)
        {
            anim.SetFloat("Speed", 0);
            rb.linearVelocity = Vector3.zero;
            return;
        }
        Vector3 direction = controls.Player.Move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3(direction.x, 0, direction.y).normalized * speed;
        anim.SetFloat("Speed", direction.magnitude);
        PlayerRotation(direction);
    }

    void PlayerRotation(Vector3 inputVector)
    {
        if (!isRotating) return;
        Vector3 goPos = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        if(goPos != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Normalize(goPos)), Time.deltaTime * 10);
        }
    }
}
