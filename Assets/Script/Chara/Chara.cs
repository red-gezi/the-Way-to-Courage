using UnityEngine;
using UnityEngine.UI;

public class Chara : MonoBehaviour
{
    public static Chara Instanc { get; set; }
    public int Population { get; set; } = 3;
    public int Supplies { get; set; } = 3;
    public int Treasures { get; set; } = 3;
    public Text PopulationText ;
    public Text SuppliesText ;
    public Text TreasuresText ;
    public static int RegionRank { get; set; } = 0;
    public static CardPosType cardPosType = CardPosType.None;
    public static Card BelongCard;

    private void Awake() => Instanc = this;
    private void Start() => RefreshUI();
    private void Update()
    {
        if (BelongCard != null)
        {
            transform.position = Vector3.Lerp(transform.position, BelongCard.transform.position + Vector3.up * 0.4f, Time.deltaTime * 5);
        }
    }

    private void RefreshUI()
    {
        PopulationText.text = Population.ToString();
        SuppliesText.text = Supplies.ToString();
        TreasuresText.text = Treasures.ToString();
    }

    public  void SetRoad(Card card)
    {
        BelongCard = card;
        cardPosType = card.currentCardPosType;
        RegionRank = card.RegionRank;
    }
    public  void Settlement()
    {
        Population += BelongCard.Population;
        Supplies += BelongCard.Supplies;
        Treasures += BelongCard.Treasures;
        RefreshUI();
    }

}
