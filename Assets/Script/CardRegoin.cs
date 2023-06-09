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
    public Card GetCard(CardPosType cardPosType)
    {
        switch (cardPosType)
        {
            case CardPosType.None: return null;
            case CardPosType.Main: return MainCards.LastOrDefault();
            case CardPosType.UpLeft: return UpLeftCards.LastOrDefault();
            case CardPosType.UpCenter: return UpCenterCards.LastOrDefault();
            case CardPosType.UpRight: return UpRightCards.LastOrDefault();
            case CardPosType.DownLeft: return DownLeftCards.LastOrDefault();
            case CardPosType.DownCenter: return DownCenterCards.LastOrDefault();
            case CardPosType.DownRight: return DownRightCards.LastOrDefault();
            default: return null;
        }
    }
}
