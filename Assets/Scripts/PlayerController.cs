using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Config")] 
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float lookSpeed = 3;

    private void Update()
    {
        Move();
        Look();
        CheckCursorLock();
    }

    private void Move()
    {
        var forward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        var strafe = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var relativeMovement = new Vector3(strafe, 0, forward);
        var movement = transform.TransformDirection(relativeMovement);
        
        characterController.Move(movement);
        animator.SetFloat("MovementForward",  relativeMovement.normalized.z);
        animator.SetFloat("MovementRight",  relativeMovement.normalized.x);
    }

    private void Look()
    {
        var horizontal = Input.GetAxis("Mouse X") * lookSpeed;
        
        transform.Rotate(0, horizontal, 0);
    }

    private void CheckCursorLock()
    {
        if (Input.GetButton("Fire1"))
        {
            LockCursor();
        }

        if (Input.GetButton("Cancel"))
        {
            UnlockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
