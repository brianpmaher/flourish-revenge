using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Inputs")] 
    [SerializeField] private InputAction onClick;
    [SerializeField] private InputAction onCancel;

    private bool gamePaused => Time.timeScale == 0;

    private void Awake()
    {
        onClick.performed += _ => HandleClick();
        onCancel.performed += _ => HandleCancel();
    }

    private void OnEnable()
    {
        onClick.Enable();
        onCancel.Enable();
    }

    private void Start()
    {
        ResumeGame();
    }

    private void OnDisable()
    {
        onClick.Disable();
        onCancel.Disable();
    }

    private void HandleClick()
    {
        if (gamePaused)
        {
            ResumeGame();
        }
    }

    private void HandleCancel()
    {
        if (!gamePaused)
        {
            PauseGame();
        }
    }

    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}