using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    public CardScript cardYouSee;

    [SerializeField] private int[] _cardValues;
    [SerializeField] private int _currentIndex = 1;

    //При старте
    private void Start()
    {
        GetCardValues();
        Shuffle();
        Shuffle();
    }


    //Этот метод задаёт каждому элементу массива _cardValues значение, соотвутствующий цфире на спрайте карты.
    //Для примера, в основной колоде минимальное значение карты 1, а максимальное 10:
    //Сначала объявляем переменную num. В цикле он понадобится для высчитывания остатка, по которому будем задавать значения элементам массива
    //Вычисляем остаток i элемента массива от его деления на 10
    //Если остаток будет равен нулю, то мы меняем значение num на 10, если нет, оставляем значение i
    //После, присваиваем i элементу массива значение num

    public void GetCardValues()
    {
        int num;

        if (cardSprites.Length > 25)
        {
            for (int i = 1; i < cardSprites.Length; i++)
            {
                num = i;

                num %= 10;

                if (num == 0)
                {
                    num = 10;
                }
                _cardValues[i] = num;
            }
        }
        else
        {
            for (int i = 1; i < cardSprites.Length; i++)
            {
                num = i;

                num %= 6;

                if (num == 0)
                {
                    num = 6;
                }
                _cardValues[i] = num;
            }
            for (int i = 13; i < cardSprites.Length; i++)
            {
                _cardValues[i] *= -1; 
            }
            
        }
    }
    //Перемешивает значения в массивах cardSprites и _cardValues
    //Пример со спрайтом. В цикле мы идём от большого значения к меньшему , не затрагивая 0-ой элемент(это рубкашка карты)
    //Объявляем переменную j и присваиваем ей рандомное значение от 1 до длины элемента массива cardSprites
    //Затем, в спрайтовую переменную face присваиваем значение i элемента массива cardSprites, затем, присваиваем значению 
    //элемента i того же массива значение элемента j массива
    //И жуе в конце, мы присваиваем значение j элементу массива значение face
    public void Shuffle()
    {
        for(int i = cardSprites.Length - 1; i > 0; --i)
        {
            int j = Random.Range(1, cardSprites.Length);
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = _cardValues[i];
            _cardValues[i] = _cardValues[j];
            _cardValues[j] = value;
        }
    }
    //Метод, который задаёт карте индекс, спрайт, и значение для обычной карте в поле или для карты в руке.
    //Возвращает значение выложенной карты
    public int DealCard(CardScript cardScript)
    {
        cardScript.SetIndex(_currentIndex);
        cardScript.SetSprite(cardSprites[_currentIndex]);
        cardScript.SetValue(_cardValues[_currentIndex++]);
        return cardScript.GetValueOfCard();
    }
    //Метод для выкладывания карт из руки.
    //Возвращает значение карты
    public int DealHandCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[cardYouSee.GetComponent<CardScript>().cardIndex]);
        cardScript.SetValue(_cardValues[cardYouSee.GetComponent<CardScript>().cardIndex]);
        cardYouSee.DeleteCard();
        return cardScript.GetValueOfCard();
    }
}
