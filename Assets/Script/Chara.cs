using UnityEngine;

public class Chara : MonoBehaviour
{

    public int Population { get; set; } = 3;
    public int Supplies { get; set; } = 3;
    public int Treasures { get; set; } = 3;
    public int RegionRank { get; set; } = 0;
    public CardPosType cardPosType = CardPosType.None;
    public Card BelongCard => Battle.MainRoadRegoins[RegionRank].GetCard(cardPosType);

    private void Update()
    {
        if (BelongCard!=null)
        {
            transform.position = Vector3.Lerp(transform.position, BelongCard.transform.position, Time.deltaTime*5);
        }
    }
    public void Settlement()
    {
        Population += BelongCard.Population;
        Supplies += BelongCard.Supplies;
        Treasures += BelongCard.Treasures;
    }

}
