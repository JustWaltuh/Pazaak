using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _rulesBtn;

    [SerializeField] private Button _toMenuBtn;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _ruleMenu;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => StartGame());
        _exitBtn.onClick.AddListener(() => ExitApplication());
        _rulesBtn.onClick.AddListener(() => ReadRules());
        _toMenuBtn.onClick.AddListener(() => BackToMenu());
    }


    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ReadRules()
    {
        _ruleMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }

    private void BackToMenu()
    {
        _mainMenu.SetActive(true);
        _ruleMenu.SetActive(false);
    }

    private void ExitApplication()
    {
        Application.Quit();
    }
}
