using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent<int> onScoreChanged;
    
    [Header("Dependencies")] 
    [SerializeField] private GameObject uiHud;
    [SerializeField] private GameObject uiGameOver;
    [SerializeField] private Button uiRetryButton;
    [SerializeField] private PlayerController playerController;
    
    [Header("Inputs")] 
    [SerializeField] private InputAction onClick;
    [SerializeField] private InputAction onCancel;
    
    private bool GamePaused => Time.timeScale == 0;
    private int _score;

    public int Score => _score;

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
        _score = 0;
        ResumeGame();
    }

    private void OnDisable()
    {
        onClick.Disable();
        onCancel.Disable();
        
        uiRetryButton.onClick.RemoveListener(RestartGame);
        playerController.onDie.RemoveListener(HandleGameOver);
    }

    public void IncreaseScore(int score)
    {
        _score += score;
        onScoreChanged.Invoke(_score);
    }
    
    private void HandleClick()
    {
        if (GamePaused)
        {
            ResumeGame();
        }
    }

    private void HandleCancel()
    {
        if (!GamePaused)
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