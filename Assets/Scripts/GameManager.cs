using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] private GameObject uiHud;
    [SerializeField] private GameObject uiGameOver;
    [SerializeField] private Button uiRetryButton;
    [SerializeField] private PlayerController playerController;
    
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
        
        uiRetryButton.onClick.AddListener(RestartGame);
        playerController.onDie.AddListener(HandleGameOver);
    }

    private void Start()
    {
        ResumeGame();
    }

    private void OnDisable()
    {
        onClick.Disable();
        onCancel.Disable();
        
        uiRetryButton.onClick.RemoveListener(RestartGame);
        playerController.onDie.RemoveListener(HandleGameOver);
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

    private void HandleGameOver()
    {
        uiHud.SetActive(false);
        uiGameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}