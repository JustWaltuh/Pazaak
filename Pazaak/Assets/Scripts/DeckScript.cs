using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    public CardScript cardYouSee;

    [SerializeField] private int[] _cardValues;
    [SerializeField] private int _currentIndex = 1;

    //��� ������
    private void Start()
    {
        GetCardValues();
        Shuffle();
        Shuffle();
    }


    //���� ����� ����� ������� �������� ������� _cardValues ��������, ��������������� ����� �� ������� �����.
    //��� �������, � �������� ������ ����������� �������� ����� 1, � ������������ 10:
    //������� ��������� ���������� num. � ����� �� ����������� ��� ������������ �������, �� �������� ����� �������� �������� ��������� �������
    //��������� ������� i �������� ������� �� ��� ������� �� 10
    //���� ������� ����� ����� ����, �� �� ������ �������� num �� 10, ���� ���, ��������� �������� i
    //�����, ����������� i �������� ������� �������� num

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
    //������������ �������� � �������� cardSprites � _cardValues
    //������ �� ��������. � ����� �� ��� �� �������� �������� � �������� , �� ���������� 0-�� �������(��� �������� �����)
    //��������� ���������� j � ����������� �� ��������� �������� �� 1 �� ����� �������� ������� cardSprites
    //�����, � ���������� ���������� face ����������� �������� i �������� ������� cardSprites, �����, ����������� �������� 
    //�������� i ���� �� ������� �������� �������� j �������
    //� ��� � �����, �� ����������� �������� j �������� ������� �������� face
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
    //�����, ������� ����� ����� ������, ������, � �������� ��� ������� ����� � ���� ��� ��� ����� � ����.
    //���������� �������� ���������� �����
    public int DealCard(CardScript cardScript)
    {
        cardScript.SetIndex(_currentIndex);
        cardScript.SetSprite(cardSprites[_currentIndex]);
        cardScript.SetValue(_cardValues[_currentIndex++]);
        return cardScript.GetValueOfCard();
    }
    //����� ��� ������������ ���� �� ����.
    //���������� �������� �����
    public int DealHandCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[cardYouSee.GetComponent<CardScript>().cardIndex]);
        cardScript.SetValue(_cardValues[cardYouSee.GetComponent<CardScript>().cardIndex]);
        cardYouSee.DeleteCard();
        return cardScript.GetValueOfCard();
    }
}
