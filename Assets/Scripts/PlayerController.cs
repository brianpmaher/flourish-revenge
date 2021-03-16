using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private VisualEffect flamethrowerEffect;
    [SerializeField] private AudioSource flamethrowerSoundEffect;

    [Header("Config")] 
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float lookSpeed = 3;

    [Header("Input")] 
    [SerializeField] private InputAction onClick;
    [SerializeField] private InputAction onPause;
    [SerializeField] private InputAction onMove;
    [SerializeField] private InputAction onLook;
    [SerializeField] private InputAction onFire;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;

    private void Awake()
    {
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();

        // Handle inputs
        onClick.performed += _ => ResumeGame();
        onPause.performed += _ => PauseGame();
        onMove.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        onMove.canceled += ctx => moveDirection = Vector2.zero;
        onLook.performed += ctx => lookDirection = ctx.ReadValue<Vector2>();
        onLook.canceled += ctx => lookDirection = Vector2.zero;
        onFire.performed += _ => StartFiring();
        onFire.canceled += _ => StopFiring();
    }

    private void OnEnable()
    {
        onClick.Enable();
        onPause.Enable();
        onMove.Enable();
        onLook.Enable();
        onFire.Enable();
    }
    
    private void Update()
    {
        Move();
        Look();
    }

    private void OnDisable()
    {
        onClick.Disable();
        onPause.Disable();
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
        animator.SetFloat("MovementForward",  localMovement.normalized.z);
        animator.SetFloat("MovementRight",  localMovement.normalized.x);
    }

    private void Look()
    {
        var normalizedLook = lookDirection * (lookSpeed * Time.deltaTime);
        transform.Rotate(0, normalizedLook.x, 0);
    }

    private void StartFiring()
    {
        flamethrowerEffect.Play();
        flamethrowerSoundEffect.Play();
    }

    private void StopFiring()
    {
        flamethrowerEffect.Stop();
        flamethrowerSoundEffect.Stop();
    }

    // TODO: This should be in a game manager
    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    // TODO: This should be in a game manager
    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
