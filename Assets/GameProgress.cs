using System.Threading.Tasks;
using UnityEngine;

class GameProgress : MonoBehaviour
{
    public async void BattleInit()
    {
        await BattleStart();
        while (true)
        {
            await TurnStart();
            await DrawCards();
            await WaitForPlayCard();
            await WaitForSelectCardPos();
            await CheckMovement();
            await SettleMovement();
            await SettleResources();
            await SettleCardEffects();
            await TurnEnd();
            //达成中断条件，跳出
            if (false)
            {
                break;
            }
        }
        await BattleEnd();
    }
    async Task BattleStart()
    {
        Battle.MainRoadCards.Clear();
        for (int i = 0; i < 30; i++)
        {
            var newCardModel = Instantiate(Battle.Instance.cardModel);
            newCardModel.SetActive(true);
            newCardModel.transform.position = new Vector3(3f * i, 0, 0);
            var newCard = newCardModel.GetComponent<Card>();
            newCard.currentCardState = Card.CardState.OnDeck;

            Battle.DeskCards.Add(newCard);
        }
        await Task.Delay(1000);
    }
    async Task TurnStart()
    {
        await Task.Delay(100);
    }
    async Task DrawCards()
    {
        for (int i = Battle.HandCards.Count; i < 5; i++)
        {
            Debug.Log("抽一张卡");

            var targetCard = Battle.DeskCards[0];
            targetCard.currentCardState = Card.CardState.OnHand;
            Battle.DeskCards.Remove(targetCard);
            Battle.HandCards.Add(targetCard);
        }
        await Task.Delay(100);
    }
    async Task WaitForPlayCard()
    {
        Battle.IsPlayCardOver = false;
        while (!Battle.IsPlayCardOver)
        {
            await Task.Delay(100);
        }
        Debug.Log("卡牌打出完毕");
    }
    async Task WaitForSelectCardPos()
    {
        Battle.IsSeletCardPosOver = false;
        while (!Battle.IsSeletCardPosOver)
        {
            await Task.Delay(100);
        }
        Debug.Log("位置选择完毕");
    }
    async Task CheckMovement()
    {
        await Task.Delay(100);
    }
    async Task SettleMovement()
    {
        await Task.Delay(100);
    }
    async Task SettleResources()
    {
        await Task.Delay(100);
    }
    async Task SettleCardEffects()
    {
        await Task.Delay(100);
    }
    async Task TurnEnd()
    {
        await Task.Delay(100);
    }
    async Task BattleEnd()
    {
        await Task.Delay(100);
    }
}