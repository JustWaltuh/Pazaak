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

    //обновляет текст с кол-вом очков для игрока и оппонента(игрок слева, а оппонент справа)
    public void UpdateScore()
    {
        opponentScoreText.text = opponentScript.scoreValue.ToString();
        playerScoreText.text = playerScript.scoreValue.ToString();
    }
    
    private void Start()
    {
        //Назначение кнопкам методы, для которых они были созданы. 
        endTurnBtn.onClick.AddListener(() => EndTurnClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        quitBtn.onClick.AddListener(() => QuitApplicationClicked());
        restartBtn.onClick.AddListener(() => RestartClicked());
        //Игрок и оппонент получают на руку карты
        playerScript.GetHand();
        opponentScript.GetHand();
        //Игрок в начале берёт карту с колоды. Запрещат игроку выкладывать карты оппонента на стол
        //тексту turnText задаётся значение, показывающее, что сейчас ход игрока
        playerScript.GetCard();
        AllowHandInteractions(playerScript, opponentScript);
        turnText.text = "ХОД ИГРОКА 1";
        //текст очков обновляется
        UpdateScore();        
    }

    //метод для выхода из приложения
    private void QuitApplicationClicked()
    {
        Application.Quit();
    }

    //Метод для пропуска раунда
    //переменная _standClicks увеличивается на 1
    //Булевая переменная _playerStand(игрок пасует) = _playerTurn(ход игрока). Также, _playerTurn не равняется _playerStand 
    //Если игрок нажмёт кнопку "Пас", то его состояние пропуска раунда будет равнятся его ходу и сразу же после его ход 
    //не будет равняться состоянию пропуска
    //Если _standClicks будет равнятся 2(оба пользователей нажмут на неё), то раунд закончится, если нет, то будет пропуск хода. 
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

    //Метод для кнопки "Закончить ход"
    private void EndTurnClicked()
    {
        //Если игрок пропустил раунд, то при нажатии кнопки "Закончить ход" оппонент будет получать карты
        if (_playerStand)
        {
            opponentScript.GetCard();
        }
        //Если оппонент пропустил раунд, то при нажатии кнопки "Закончить ход" игрок будет получать карты
        else if (_opponentStand)
        {
            playerScript.GetCard();
        }
        //Если ход игрока и оппонент не пропустил ход, то оппонент получает карту, у игрока отнимают ход
        //ход оппонента будет противоположен его состоянию пропуска
        if (_playerTurn && !_opponentStand)
        {
            opponentScript.GetCard();
            _playerTurn = false;
            _opponentTurn = !_opponentStand;
        }
        //Если ход игрока и оппонент не пропустил ход, то оппонент получает карту, у игрока отнимают ход
        //ход оппонента будет противоположен его состоянию пропуска   
        else if (_opponentTurn && !_playerStand)
        {
            playerScript.GetCard();
            _opponentTurn = false;
            _playerTurn = !_playerStand;
        }
        UpdateScore();
        //Если сейчас ход игрока, то текст скажет об этом. Позволяет игроку взаимодействовать со своими картами
        //и запрещает взаимодействовать с кратами оппонента
        if (_playerTurn) 
        {
            turnText.text = "ХОД ИГРОКА 1";
            AllowHandInteractions(playerScript, opponentScript);
        }
        //Если сейчас ход оппонента, то текст скажет об этом. Позволяет оппоненту взаимодействовать со своими картами
        //и запрещает взаимодействовать с кратами игрока
        else
        { 
            turnText.text = "ХОД ИГРОКА 2";
            AllowHandInteractions(opponentScript, playerScript);
        }
        //Если поле игрока или поле оппонента было заполнено, то автоматически пропускается раунд для одного из них
        if (playerScript.cardIndex >= 9 || opponentScript.cardIndex >= 9)
        {
            StandClicked();
        }
        //Если у игрока или оппонента очков больше 20, то раунд кончается
        if (playerScript.scoreValue > 20 || opponentScript.scoreValue > 20) 
            RoundOver();

    }

    //Позволяет взаимодействовать с картами игрока слева, если наступил его ход, а также закрывает доступ к картам игроку слева. И наоборот. 
    private void AllowHandInteractions(PlayScript toRecive, PlayScript toReject)
    {
        for (int i = 1; i <= toRecive.hand.Length; i++)
        {
            GameObject.Find($"{toRecive.forDirectory}/HandField/HandCard{i}").GetComponent<Button>().interactable = true;
            GameObject.Find($"{toReject.forDirectory}/HandField/HandCard{i}").GetComponent<Button>().interactable = false;
        }
    }

    //Метод конца раунда
    private void RoundOver()
    {
        bool roundOver = true; 
        bool playerBust = playerScript.scoreValue > 20;
        bool opponentBust = opponentScript.scoreValue > 20;
        //если у обоих кол-во очков больше 20 или равны, то выводится ничья.
        if (playerBust && opponentBust || playerScript.scoreValue == opponentScript.scoreValue)
        {
            resultText.text = "НИЧЬЯ!";
        }
        //если у игрока больше 20 очков или у оппонента меньше 20 очков и у него больше очков, чем у игрока
        //то оппонент побеждает
        else if (playerBust || (!opponentBust && opponentScript.scoreValue > playerScript.scoreValue))
        {
            resultText.text = "ОППОНЕНТ ПОБЕДИЛ!";
        }
        //если у оппонента больше 20 очков или у игрока меньше 20 очков и у него больше очков, чем у оппонента
        //то игрок побеждает
        else if (opponentBust || playerScript.scoreValue > opponentScript.scoreValue)
        {
            resultText.text = "ИГРОК ПОБЕДИЛ!";
        }

        //Если раунд закончен, то кнопки конца хода и пропуска раунда исчезают
        //кнопка перезапуска появляется
        //Также текст с ходами исчезает и за место него появляется текст с резульатом
        if (roundOver)
        {
            endTurnBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            restartBtn.gameObject.SetActive(true);

            turnText.gameObject.SetActive(false); 
            resultText.gameObject.SetActive(true);

        } 
    }
    //Метод, который перезапускает сцену.
    private void RestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
