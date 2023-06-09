using System.Linq;
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
            await WaitForDeploy();
            await WaitForSelectMovement();
            //await SettleMovement();
            //await SettleResources();
            //await SettleCardEffects();
            //await TurnEnd();
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
        Battle.MainRoadRegoins.Clear();
        for (int i = 0; i < Battle.maxMainRoadCount; i++)
        {
            Battle.MainRoadRegoins.Add(new CardRegoin());
        }
        //生成牌库卡牌
        for (int i = 0; i < 30; i++)
        {
            var newCardModel = Instantiate(Battle.Instance.cardModel);
            newCardModel.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV();
            newCardModel.SetActive(true);
            newCardModel.transform.position = new Vector3(3f * i, 0, 0);
            newCardModel.name = i.ToString();
            var newCard = newCardModel.GetComponent<Card>();
            newCard.currentCardState = CardState.OnDeck;

            var randomColors = Enumerable.Range(0, 4).Select(i => (CardColor)Random.Range(0, 3));
            newCard.AddColor(randomColors.ToArray());
            Battle.DeskCards.Add(newCard);
        }
        await Task.Delay(1000);
        //布置主线路卡牌
        for (int i = 0; i < Battle.maxMainRoadCount; i++)
        {
            Battle.DeskCards.FirstOrDefault()?.Deploy(i, Battle.MainRoadRegoins[i].MainCards);
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
            targetCard.currentCardState = CardState.OnHand;
            Battle.DeskCards.Remove(targetCard);
            Battle.HandCards.Add(targetCard);
        }
        await Task.Delay(100);
    }
    async Task WaitForPlayCard()
    {
        Battle.IsPlayCardOver = false;
        Battle.IsWaitForPlayCard = true;
        while (!Battle.IsPlayCardOver)
        {
            await Task.Delay(100);
        }
        Debug.Log("卡牌打出完毕");
        await Task.Delay(1000);
    }
    async Task WaitForDeploy()
    {
        Battle.IsDeployOver = false;
        Battle.IsWaitForDeploy = true;
        while (!Battle.IsDeployOver)
        {
            await Task.Delay(100);
        }
        Debug.Log("位置选择完毕");
        Battle.TempCardModel.SetActive(false);
    }
    /// <summary>
    /// 选择路径，存在多条
    /// </summary>
    /// <returns></returns>
    async Task WaitForSelectMovement()
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