using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIHealthBar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Damageable playerHealth;
    [SerializeField] private GameObject healthImage;
    [SerializeField] private Sprite healthFullSprite;
    [SerializeField] private Sprite healthEmptySprite;

    private RectTransform _healthPanelRectTransform;
    private int _maxHealth;

    private void Awake()
    {
        _healthPanelRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _maxHealth = playerHealth.MaxHealth;

        for (int i = 0; i < _maxHealth; i++)
        {
            var image = Instantiate(healthImage, transform);
            image.GetComponent<Image>().sprite = healthFullSprite;
            var rectTransform = image.GetComponent<RectTransform>().rect;
            rectTransform.position = new Vector2(100 * i, 0);
            rectTransform.size = new Vector2(100, 100);
        }

        var rect = _healthPanelRectTransform.rect;
        rect.width = _maxHealth * 100;
    }

    private void OnEnable()
    {
        playerHealth.onHealthChanged.AddListener(UpdateHealthBar);
    }

    private void OnDisable()
    {
        playerHealth.onHealthChanged.RemoveListener(UpdateHealthBar);
    }

    private void UpdateHealthBar(int health)
    {
    }
}