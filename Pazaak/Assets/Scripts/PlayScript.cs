using UnityEngine;

public class PlayScript : MonoBehaviour
{

    public DeckScript handDeckScript;
    public DeckScript mainDeckScript;

    public int scoreValue = 0;

    public GameObject[] field;
    public GameObject[] hand;

    public int cardIndex = 0;

    public string forDirectory; 

    //����� ��� ������ ����� �� �������� ������
    public int GetCard()
    {
        int cardValue = mainDeckScript.DealCard(field[cardIndex++].GetComponent<CardScript>());
        scoreValue += cardValue;
        return scoreValue;
    }
    //����� ��� ����������� ���� ������/���������
    public void GetHand()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            handDeckScript.DealCard(hand[i].GetComponent<CardScript>());
        }
    }

    //����� ������������ ����� �� "����" �� ���� � ��������� ����� ���-�� ����� ������/���������. 
    public int GetHandCard()
    {
        int cardValue = handDeckScript.DealHandCard(field[cardIndex++].GetComponent<CardScript>());
        scoreValue += cardValue;
        GameObject.Find("GameManager").GetComponent<GameManager>().UpdateScore();
        return scoreValue;
    }
   
}
