using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private Fireable weapon;
    [SerializeField] private Damageable damageable;

    [Header("Config")] 
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float lookSpeed = 3;

    [Header("Input")] 
    [SerializeField] private InputAction onMove;
    [SerializeField] private InputAction onLook;
    [SerializeField] private InputAction onFire;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private static readonly int MovementForward = Animator.StringToHash("MovementForward");
    private static readonly int MovementRight = Animator.StringToHash("MovementRight");

    private void Awake()
    {
        damageable.onDie.AddListener(Die);
        
        // Handle inputs
        onMove.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        onMove.canceled += ctx => moveDirection = Vector2.zero;
        onLook.performed += ctx => lookDirection = ctx.ReadValue<Vector2>();
        onLook.canceled += ctx => lookDirection = Vector2.zero;
        onFire.performed += _ => weapon.StartFiring();
        onFire.canceled += _ => weapon.StopFiring();
    }

    private void OnEnable()
    {
        onMove.Enable();
        onLook.Enable();
        onFire.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            var fuel = other.GetComponent<Fuel>().PickUp();
            weapon.IncreaseFuel(fuel);
        }
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void OnDisable()
    {
        onMove.Disable();
        onLook.Disable();
        onFire.Disable();
    }

    private void Move()
    {
        var normalizedMovement = moveDirection * (moveSpeed * Time.deltaTime);
        var localMovement = new Vector3(normalizedMovement.x, 0, normalizedMovement.y);
        var globalMovement = transform.TransformDirection(localMovement);
        
        characterController.Move(globalMovement);
        animator.SetFloat(MovementForward,  localMovement.normalized.z);
        animator.SetFloat(MovementRight,  localMovement.normalized.x);
    }

    private void Look()
    {
        var normalizedLook = lookDirection * (lookSpeed * Time.deltaTime);
        transform.Rotate(0, normalizedLook.x, 0);
    }

    private void Die()
    {
        Debug.Log("I died");
    }
}
