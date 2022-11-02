using UnityEngine;
using UnityEngine.UI; 

public class CardScript : MonoBehaviour
{
    public int value = 0;
    public PlayScript cardOwner;
    public CardScript card; 

    public int cardIndex;



    private void Start()
    {
        //���� � ����� ���� ��������� Button, �� ��� ������� ���������� ����������� ����� GetHandCard
        if (GetComponent<Button>())
        {
            GetComponent<Button>().onClick.AddListener(() => cardOwner.GetComponent<PlayScript>().GetHandCard());
        }
    }

    //���� ���������� ������ �� �����, �� ���������� cardYouSee �� ������� ������ ����� �������� ������� ������,
    //� �������� ���� ��������� CardScript
    public void OnMouseOver()
    {
        GameObject.Find("HandDeck").GetComponent<DeckScript>().cardYouSee = card;
    }
    //����� ������ �����
    public void SetIndex(int newCardIndex)
    {
        cardIndex = newCardIndex;
    }
    
    //���������� �������� �����
    public int GetValueOfCard()
    {
        return value;
    }
    //����� �������� ��� �����
    public void SetValue(int newValue)
    {
        value = newValue;
    }
    //����� ������ ��� �����
    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
    //������� ����� �� ����
    public void DeleteCard()
    {
        gameObject.SetActive(false);
    }

}
