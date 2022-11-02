using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Button endTurnBtn;
    public Button standBtn;
    public Button quitBtn;
    public Button restartBtn;


    [SerializeField] private bool _opponentTurn = true;
    [SerializeField] private bool _opponentStand = false;
    [SerializeField] private bool _playerTurn = false;
    [SerializeField] private bool _playerStand = false;

    [SerializeField] private int _standClicks = 0;


    public Text turnText;
    public Text resultText;
    public Text playerScoreText;
    public Text opponentScoreText;

    public PlayScript playerScript;
    public PlayScript opponentScript;

    //��������� ����� � ���-��� ����� ��� ������ � ���������(����� �����, � �������� ������)
    public void UpdateScore()
    {
        opponentScoreText.text = opponentScript.scoreValue.ToString();
        playerScoreText.text = playerScript.scoreValue.ToString();
    }
    
    private void Start()
    {
        //���������� ������� ������, ��� ������� ��� ���� �������. 
        endTurnBtn.onClick.AddListener(() => EndTurnClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        quitBtn.onClick.AddListener(() => QuitApplicationClicked());
        restartBtn.onClick.AddListener(() => RestartClicked());
        //����� � �������� �������� �� ���� �����
        playerScript.GetHand();
        opponentScript.GetHand();
        //����� � ������ ���� ����� � ������. �������� ������ ����������� ����� ��������� �� ����
        //������ turnText ������� ��������, ������������, ��� ������ ��� ������
        playerScript.GetCard();
        AllowHandInteractions(playerScript, opponentScript);
        turnText.text = "��� ������ 1";
        //����� ����� �����������
        UpdateScore();        
    }

    //����� ��� ������ �� ����������
    private void QuitApplicationClicked()
    {
        Application.Quit();
    }

    //����� ��� �������� ������
    //���������� _standClicks ������������� �� 1
    //������� ���������� _playerStand(����� ������) = _playerTurn(��� ������). �����, _playerTurn �� ��������� _playerStand 
    //���� ����� ����� ������ "���", �� ��� ��������� �������� ������ ����� �������� ��� ���� � ����� �� ����� ��� ��� 
    //�� ����� ��������� ��������� ��������
    //���� _standClicks ����� �������� 2(��� ������������� ������ �� ��), �� ����� ����������, ���� ���, �� ����� ������� ����. 
    private void StandClicked()
    {
        _standClicks++;

        _playerStand = _playerTurn;
        _playerTurn = !_playerStand;

        _opponentStand = _opponentTurn;
       _opponentTurn = !_opponentStand;

        if (_standClicks > 1) RoundOver();
        else EndTurnClicked();
    }

    //����� ��� ������ "��������� ���"
    private void EndTurnClicked()
    {
        //���� ����� ��������� �����, �� ��� ������� ������ "��������� ���" �������� ����� �������� �����
        if (_playerStand)
        {
            opponentScript.GetCard();
        }
        //���� �������� ��������� �����, �� ��� ������� ������ "��������� ���" ����� ����� �������� �����
        else if (_opponentStand)
        {
            playerScript.GetCard();
        }
        //���� ��� ������ � �������� �� ��������� ���, �� �������� �������� �����, � ������ �������� ���
        //��� ��������� ����� �������������� ��� ��������� ��������
        if (_playerTurn && !_opponentStand)
        {
            opponentScript.GetCard();
            _playerTurn = false;
            _opponentTurn = !_opponentStand;
        }
        //���� ��� ������ � �������� �� ��������� ���, �� �������� �������� �����, � ������ �������� ���
        //��� ��������� ����� �������������� ��� ��������� ��������   
        else if (_opponentTurn && !_playerStand)
        {
            playerScript.GetCard();
            _opponentTurn = false;
            _playerTurn = !_playerStand;
        }
        UpdateScore();
        //���� ������ ��� ������, �� ����� ������ �� ����. ��������� ������ ����������������� �� ������ �������
        //� ��������� ����������������� � ������� ���������
        if (_playerTurn) 
        {
            turnText.text = "��� ������ 1";
            AllowHandInteractions(playerScript, opponentScript);
        }
        //���� ������ ��� ���������, �� ����� ������ �� ����. ��������� ��������� ����������������� �� ������ �������
        //� ��������� ����������������� � ������� ������
        else
        { 
            turnText.text = "��� ������ 2";
            AllowHandInteractions(opponentScript, playerScript);
        }
        //���� ���� ������ ��� ���� ��������� ���� ���������, �� ������������� ������������ ����� ��� ������ �� ���
        if (playerScript.cardIndex >= 9 || opponentScript.cardIndex >= 9)
        {
            StandClicked();
        }
        //���� � ������ ��� ��������� ����� ������ 20, �� ����� ���������
        if (playerScript.scoreValue > 20 || opponentScript.scoreValue > 20) 
            RoundOver();

    }

    //��������� ����������������� � ������� ������ �����, ���� �������� ��� ���, � ����� ��������� ������ � ������ ������ �����. � ��������. 
    private void AllowHandInteractions(PlayScript toRecive, PlayScript toReject)
    {
        for (int i = 1; i <= toRecive.hand.Length; i++)
        {
            GameObject.Find($"{toRecive.forDirectory}/HandField/HandCard{i}").GetComponent<Button>().interactable = true;
            GameObject.Find($"{toReject.forDirectory}/HandField/HandCard{i}").GetComponent<Button>().interactable = false;
        }
    }

    //����� ����� ������
    private void RoundOver()
    {
        bool roundOver = true; 
        bool playerBust = playerScript.scoreValue > 20;
        bool opponentBust = opponentScript.scoreValue > 20;
        //���� � ����� ���-�� ����� ������ 20 ��� �����, �� ��������� �����.
        if (playerBust && opponentBust || playerScript.scoreValue == opponentScript.scoreValue)
        {
            resultText.text = "�����!";
        }
        //���� � ������ ������ 20 ����� ��� � ��������� ������ 20 ����� � � ���� ������ �����, ��� � ������
        //�� �������� ���������
        else if (playerBust || (!opponentBust && opponentScript.scoreValue > playerScript.scoreValue))
        {
            resultText.text = "�������� �������!";
        }
        //���� � ��������� ������ 20 ����� ��� � ������ ������ 20 ����� � � ���� ������ �����, ��� � ���������
        //�� ����� ���������
        else if (opponentBust || playerScript.scoreValue > opponentScript.scoreValue)
        {
            resultText.text = "����� �������!";
        }

        //���� ����� ��������, �� ������ ����� ���� � �������� ������ ��������
        //������ ����������� ����������
        //����� ����� � ������ �������� � �� ����� ���� ���������� ����� � ����������
        if (roundOver)
        {
            endTurnBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            restartBtn.gameObject.SetActive(true);

            turnText.gameObject.SetActive(false); 
            resultText.gameObject.SetActive(true);

        } 
    }
    //�����, ������� ������������� �����.
    private void RestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
