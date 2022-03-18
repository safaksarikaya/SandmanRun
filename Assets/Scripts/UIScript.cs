using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIScript : MonoBehaviour
{
  public static UIScript Instance;
  [SerializeField] Text tap2StartText;
  [SerializeField] GameObject successPanel, gameOverPanel;
  private void Awake()
  {
    Instance = this;
  }
  public void CloseTap2Start()
  {
    tap2StartText.enabled = false;
  }
  public void ShowSuccess()
  {
    successPanel.SetActive(true);
  }
  public void ShowGameOver()
  {
    gameOverPanel.SetActive(true);
  }
  public void RestartButton()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}