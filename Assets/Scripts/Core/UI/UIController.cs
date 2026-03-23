using UnityEngine;
using TMPro;

namespace NeonProtocol.Core.UI
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance;

        [Header("Interaction Prompt")]
        [SerializeField] private GameObject promptPanel;
        [SerializeField] private TextMeshProUGUI promptText;

        [Header("Game Over")]
        [SerializeField] private GameObject gameOverPanel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            if (promptPanel != null) promptPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }

        public void ShowPrompt(string text)
        {
            if (promptText != null) promptText.text = text;
            if (promptPanel != null) promptPanel.SetActive(true);
        }

        public void HidePrompt()
        {
            if (promptPanel != null) promptPanel.SetActive(false);
        }

        public void ShowGameOver()
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }
    }
}
