using System.Collections.Generic;
using System.Linq;

public class CardRegoin
{
    public List<Card> MainCards { get; set; } = new();
    public List<Card> UpLeftCards { get; set; } = new();
    public List<Card> UpCenterCards { get; set; } = new();
    public List<Card> UpRightCards { get; set; } = new();
    public List<Card> DownLeftCards { get; set; } = new();
    public List<Card> DownCenterCards { get; set; } = new();
    public List<Card> DownRightCards { get; set; } = new();
    public Card GetCard(CardPosType cardPosType) => GetCardList(cardPosType)?.LastOrDefault();
    public List<Card> GetCardList(CardPosType cardPosType)
    {
        switch (cardPosType)
        {
            case CardPosType.None: return null;
            case CardPosType.Main: return MainCards;
            case CardPosType.UpLeft: return UpLeftCards;
            case CardPosType.UpCenter: return UpCenterCards;
            case CardPosType.UpRight: return UpRightCards;
            case CardPosType.DownLeft: return DownLeftCards;
            case CardPosType.DownCenter: return DownCenterCards;
            case CardPosType.DownRight: return DownRightCards;
            default: return null;
        }
    }

    public List<Card> GetAllCardList() => new List<List<Card>> { MainCards, UpLeftCards, UpCenterCards, UpRightCards, DownLeftCards, DownCenterCards, DownRightCards }.SelectMany(x => x).ToList();
    public CardRegoin GetNetCardRegoin()
    {
        int index = Battle.MainRoadRegoins.IndexOf(this);
        if (index < Battle.maxMainRoadCount - 1)
        {
            return Battle.MainRoadRegoins[index + 1];
        }
        else
        {
            return null;
        }
    }
    public void Recycle(CardPosType cardPosType)
    {
        GetCardList(cardPosType).ForEach(card =>
        {
            card.currentCardState = CardState.OnDeck;
            Battle.DeskCards.Add(card);
        });
        GetCardList(cardPosType).Clear();
    }
}
