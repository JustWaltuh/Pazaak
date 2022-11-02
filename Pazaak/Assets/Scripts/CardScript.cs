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
        //если у карты есть компонент Button, то для данного компонента назначается метод GetHandCard
        if (GetComponent<Button>())
        {
            GetComponent<Button>().onClick.AddListener(() => cardOwner.GetComponent<PlayScript>().GetHandCard());
        }
    }

    //Если разместить курсор на карте, то переменной cardYouSee из боковой колоды будет присвоен игровой объект,
    //у которого есть компонент CardScript
    public void OnMouseOver()
    {
        GameObject.Find("HandDeck").GetComponent<DeckScript>().cardYouSee = card;
    }
    //задаёт индекс карте
    public void SetIndex(int newCardIndex)
    {
        cardIndex = newCardIndex;
    }
    
    //возвращает значение карты
    public int GetValueOfCard()
    {
        return value;
    }
    //задаёт значение для карты
    public void SetValue(int newValue)
    {
        value = newValue;
    }
    //задаёт спрайт для карты
    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
    //удаляет карту из руки
    public void DeleteCard()
    {
        gameObject.SetActive(false);
    }

}
