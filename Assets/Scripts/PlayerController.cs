using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterController characterController;

    [Header("Config")] 
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float lookSpeed = 3;

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        var forward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        var strafe = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var movement = transform.TransformDirection(new Vector3(strafe, 0, forward));
        
        characterController.Move(movement);
    }

    private void Look()
    {
        var horizontal = Input.GetAxis("Mouse X") * lookSpeed;
        
        transform.Rotate(0, horizontal, 0);
    }
}
