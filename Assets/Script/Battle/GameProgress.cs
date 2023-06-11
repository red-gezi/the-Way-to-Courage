using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class GameProgress : MonoBehaviour
{
    public Text progressText;
    //处于等待打出卡牌
    public static bool IsWaitForPlayCard { get; set; } = false;
    //打出卡牌完毕
    public static bool IsPlayCardOver { get; set; } = false;
    //public static bool IsSelectMovementOver { get; set; } = false;
    //预打出的牌
    public static Card PrePlayCard;
    public static Card TempCard => Battle.TempCardModel.GetComponent<Card>();
    //选择的可移动位置
    public static Card SeletSelectMovementCard { get; set; }
    //处于等待选择部署位置
    //public static bool IsWaitForDeploy { get; set; } = false;
    //部署卡牌完毕
    public static bool IsDeployOver { get; set; } = true;
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
        progressText.text = "对局开始";
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

            var randomColors = Enumerable.Range(0, 4).Select(i => (CardColor)Random.Range(0, 3));

            var newCard = newCardModel.GetComponent<Card>();
            newCard.Init(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5), randomColors.ToArray());
            newCard.RefreshUI();
            Battle.DeskCards.Add(newCard);
        }
        await Task.Delay(1000);
        //布置主线路卡牌
        for (int i = 0; i < Battle.maxMainRoadCount; i++)
        {
            Battle.DeskCards.FirstOrDefault()?.Deploy(i, Battle.MainRoadRegoins[i].MainCards);
        }
        //设置角色初始位置
        Chara.Instanc.SetRoad(Battle.MainRoadRegoins[0].GetCard(CardPosType.Main));
        await Task.Delay(1000);
    }
    async Task TurnStart()
    {
        progressText.text = "回合开始";
        await Task.Delay(500);
    }
    async Task DrawCards()
    {
        progressText.text = "玩家抽卡";

        for (int i = Battle.HandCards.Count; i < 5; i++)
        {
            Debug.Log("抽一张卡");

            var targetCard = Battle.DeskCards.FirstOrDefault();
            if (targetCard == null)
            {
                Debug.Log("卡牌已抽完");

            }
            else
            {
                Debug.Log("抽一张卡");
                targetCard.currentCardState = CardState.OnHand;
                Battle.DeskCards.Remove(targetCard);
                Battle.HandCards.Add(targetCard);
            }
        }
        await Task.Delay(100);
    }
    async Task WaitForPlayCard()
    {
        progressText.text = "等待玩家出牌";
        IsPlayCardOver = false;
        IsWaitForPlayCard = true;
        while (!IsPlayCardOver)
        {
            //此处等待玩家在触发卡牌拖拽释放事件
            await Task.Delay(10);
        }
        Debug.Log("卡牌打出完毕");
        await Task.Delay(100);
    }
    async Task WaitForDeploy()
    {
        progressText.text = "等待玩家部署";
        IsDeployOver = false;
        TempCard.SetColor(PrePlayCard.colors);
        TempCard.IsUpRight = PrePlayCard.IsUpRight;
        while (!IsDeployOver)
        {
            if (Input.GetMouseButtonDown(1) && PrePlayCard != null)
            {
                PrePlayCard.IsUpRight = !PrePlayCard.IsUpRight;
                Battle.TempCardModel.GetComponent<Card>().IsUpRight = PrePlayCard.IsUpRight;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 targetPos = hitInfo.point;
                if (targetPos.x > -2 && targetPos.x < (Battle.maxMainRoadCount - 1) * 4 + 2)
                {
                    int rank = (int)(targetPos.x + 2) / 4;
                    CardPosType cardPosType = CardPosType.None;
                    if (targetPos.z < 3 && targetPos.z > -3)
                    {
                        cardPosType = CardPosType.Main;
                    }
                    else if (targetPos.z > 3 && targetPos.z < 7)
                    {
                        if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
                        {
                            cardPosType = CardPosType.UpLeft;
                        }
                        else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
                        {
                            cardPosType = CardPosType.UpCenter;
                        }
                        else
                        {
                            cardPosType = CardPosType.UpRight;
                        }
                    }
                    else if (targetPos.z < -3 && targetPos.z > -7)
                    {
                        if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
                        {
                            cardPosType = CardPosType.DownLeft;
                        }
                        else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
                        {
                            cardPosType = CardPosType.DownCenter;
                        }
                        else
                        {
                            cardPosType = CardPosType.DownRight;
                        }
                    }
                    if (Input.GetMouseButton(0))
                    {
                        if (cardPosType != CardPosType.None)
                        {
                            //做部署有效性判断

                            ////部署在主路时
                            //if (cardPosType== CardPosType.Main)
                            //{
                            //    if (Battle.MainRoadRegoins[rank]==Chara.BelongCard.BelongCardRegoin)
                            //    {
                            //        Debug.LogError("无法放置卡牌到人物脚下");
                            //    }
                            //    else
                            //    {
                            //        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                            //    }
                            //}
                            if (Battle.MainRoadRegoins[rank].GetCardList(cardPosType).Contains(Chara.BelongCard))
                            {
                                Debug.LogError("无法放置卡牌到人物脚下");
                            }
                            else
                            {
                                if (cardPosType == CardPosType.Main)
                                {
                                    PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                                }
                                //部署在上左位置时
                                if (cardPosType == CardPosType.UpLeft)
                                {
                                    if (Battle.MainRoadRegoins[rank].UpCenterCards.Any())
                                    {
                                        Debug.LogError("本区域中侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].UpRightCards.Any())
                                    {
                                        Debug.LogError("本区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpLeftCards.Any()))
                                    {
                                        Debug.LogError("左区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpCenterCards.Any()))
                                    {
                                        Debug.LogError("左区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpRightCards.Any()))
                                    {
                                        Debug.LogError("左区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpLeftCards.Any())
                                    {
                                        Debug.LogError("右区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpCenterCards.Any())
                                    {
                                        Debug.LogError("右区域中间已存在卡牌，无法覆盖");
                                    }
                                    else
                                    {
                                        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                                    }
                                }
                                //部署在下左位置时
                                if (cardPosType == CardPosType.DownLeft)
                                {
                                    if (Battle.MainRoadRegoins[rank].DownCenterCards.Any())
                                    {
                                        Debug.LogError("本区域中侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].DownRightCards.Any())
                                    {
                                        Debug.LogError("本区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownLeftCards.Any()))
                                    {
                                        Debug.LogError("左区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownCenterCards.Any()))
                                    {
                                        Debug.LogError("左区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownRightCards.Any()))
                                    {
                                        Debug.LogError("左区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownLeftCards.Any())
                                    {
                                        Debug.LogError("右区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownCenterCards.Any())
                                    {
                                        Debug.LogError("右区域中间已存在卡牌，无法覆盖");
                                    }
                                    else
                                    {
                                        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                                    }
                                }
                                //部署在上右位置时
                                if (cardPosType == CardPosType.UpRight)
                                {
                                    if (Battle.MainRoadRegoins[rank].UpLeftCards.Any())
                                    {
                                        Debug.LogError("本区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].UpCenterCards.Any())
                                    {
                                        Debug.LogError("本区域中侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpCenterCards.Any()))
                                    {
                                        Debug.LogError("左区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpRightCards.Any()))
                                    {
                                        Debug.LogError("左区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpLeftCards.Any())
                                    {
                                        Debug.LogError("右区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpCenterCards.Any())
                                    {
                                        Debug.LogError("右区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpRightCards.Any()))
                                    {
                                        Debug.LogError("右区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else
                                    {
                                        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                                    }
                                }
                                //部署在下右位置时
                                if (cardPosType == CardPosType.DownRight)
                                {
                                    if (Battle.MainRoadRegoins[rank].DownLeftCards.Any())
                                    {
                                        Debug.LogError("本区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].DownCenterCards.Any())
                                    {
                                        Debug.LogError("本区域中侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownCenterCards.Any()))
                                    {
                                        Debug.LogError("左区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownRightCards.Any()))
                                    {
                                        Debug.LogError("左区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownLeftCards.Any())
                                    {
                                        Debug.LogError("右区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownCenterCards.Any())
                                    {
                                        Debug.LogError("右区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownRightCards.Any()))
                                    {
                                        Debug.LogError("右区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else
                                    {
                                        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                                    }
                                }
                                //部署在上中位置时
                                if (cardPosType == CardPosType.UpCenter)
                                {
                                    if (Battle.MainRoadRegoins[rank].UpLeftCards.Any())
                                    {
                                        Debug.LogError("本区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].UpRightCards.Any())
                                    {
                                        Debug.LogError("本区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpLeftCards.Any()))
                                    {
                                        Debug.LogError("左区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpCenterCards.Any()))
                                    {
                                        Debug.LogError("左区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().UpRightCards.Any()))
                                    {
                                        Debug.LogError("左区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpLeftCards.Any())
                                    {
                                        Debug.LogError("右区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpCenterCards.Any())
                                    {
                                        Debug.LogError("右区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetNextCardRegoin().UpRightCards.Any()))
                                    {
                                        Debug.LogError("右区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else
                                    {
                                        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                                    }
                                }
                                //部署在下中位置时
                                if (cardPosType == CardPosType.DownCenter)
                                {
                                    if (Battle.MainRoadRegoins[rank].DownLeftCards.Any())
                                    {
                                        Debug.LogError("本区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].DownRightCards.Any())
                                    {
                                        Debug.LogError("本区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownLeftCards.Any()))
                                    {
                                        Debug.LogError("左区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownCenterCards.Any()))
                                    {
                                        Debug.LogError("左区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetLastCardRegoin().DownRightCards.Any()))
                                    {
                                        Debug.LogError("左区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownLeftCards.Any())
                                    {
                                        Debug.LogError("右区域左侧已存在卡牌，无法覆盖");
                                    }
                                    else if (Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownCenterCards.Any())
                                    {
                                        Debug.LogError("右区域中间已存在卡牌，无法覆盖");
                                    }
                                    else if ((Battle.MainRoadRegoins[rank].GetNextCardRegoin().DownRightCards.Any()))
                                    {
                                        Debug.LogError("右区域右侧已存在卡牌，无法覆盖");
                                    }
                                    else
                                    {
                                        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].GetCardList(cardPosType));
                                    }
                                }

                            }
                        }
                        else
                        {
                            Debug.Log("请选择一个可部署区域");
                        }
                    }
                    else
                    {
                        TempCard.ShowTempCard(rank, cardPosType);
                    }
                    //if (targetPos.z < 3 && targetPos.z > -3)
                    //{
                    //    if (Input.GetMouseButton(0))
                    //    {
                    //        PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].MainCards);
                    //    }
                    //    else
                    //    {
                    //        TempCard.ShowTempCard(rank, CardPosType.Main);
                    //    }
                    //}
                    //else if (targetPos.z > 3 && targetPos.z < 7)
                    //{
                    //    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
                    //    {
                    //        if (Input.GetMouseButton(0))
                    //        {
                    //            PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].UpLeftCards);
                    //        }
                    //        else
                    //        {
                    //            TempCard.ShowTempCard(rank, CardPosType.UpLeft);
                    //        }
                    //    }
                    //    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
                    //    {
                    //        if (Input.GetMouseButton(0))
                    //        {
                    //            PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].UpCenterCards);
                    //        }
                    //        else
                    //        {
                    //            TempCard.ShowTempCard(rank, CardPosType.UpCenter);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Input.GetMouseButton(0))
                    //        {
                    //            PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].UpRightCards);
                    //        }
                    //        else
                    //        {
                    //            TempCard.ShowTempCard(rank, CardPosType.UpRight);
                    //        }
                    //    }
                    //}
                    //else if (targetPos.z < -3 && targetPos.z > -7)
                    //{
                    //    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
                    //    {
                    //        if (Input.GetMouseButton(0))
                    //        {
                    //            PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].DownLeftCards);
                    //        }
                    //        else
                    //        {
                    //            TempCard.ShowTempCard(rank, CardPosType.DownLeft);

                    //        }
                    //    }
                    //    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
                    //    {
                    //        if (Input.GetMouseButton(0))
                    //        {
                    //            PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].DownCenterCards);
                    //        }
                    //        else
                    //        {
                    //            TempCard.ShowTempCard(rank, CardPosType.DownCenter);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Input.GetMouseButton(0))
                    //        {
                    //            PrePlayCard.Deploy(rank, Battle.MainRoadRegoins[rank].DownRightCards);
                    //        }
                    //        else
                    //        {
                    //            TempCard.ShowTempCard(rank, CardPosType.DownRight);
                    //        }
                    //    }
                    //}
                }
            }
            await Task.Delay(5);
        }
        Debug.Log("部署位置选择完毕");
        Battle.TempCardModel.SetActive(false);
    }
    /// <summary>
    /// 选择路径，存在多条
    /// </summary>
    /// <returns></returns>
    async Task WaitForSelectMovement()
    {
        progressText.text = "等待玩家选择路径";
        //IsSelectMovementOver = false;
        SeletSelectMovementCard = null;
        //所有卡牌设为不可激活
        Battle.GetAllCardList().ForEach(card => card.SetAllRoadSIgnsCannotClick());
        //////////////////////////////////////////判断哪些牌可激活///////////////////////
        var currentCard = Chara.BelongCard;
        List<(bool isEqual, Card targetCard, RoadSignPos targetRoadSignPos)> command = new List<(bool, Card, RoadSignPos)>();
        bool isClickable;
        //当卡牌在主区时
        if (currentCard.currentCardPosType == CardPosType.Main)
        {
            //判断主路线下一张卡
            var targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin()?.GetCard(CardPosType.Main);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));

                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
            //判断上左侧卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.UpLeft);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownRight, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownRight));
            }
            //判断上右测卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.UpRight);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.UpLeft));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
            //判断上张卡的上中间卡
            targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin().GetCard(CardPosType.UpCenter);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }

            //判断下左侧卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.DownLeft);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpRight, currentCard.GetRoadSignColor(RoadSignPos.DownLeft));
                command.Add((isClickable, targetCard, RoadSignPos.UpRight));
            }
            //判断下右测卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.DownRight);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.DownLeft));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));
            }
            //判断下张卡的下中间卡
            targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin().GetCard(CardPosType.DownCenter);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));
            }
        }
        //当卡牌在左上区时
        if (currentCard.currentCardPosType == CardPosType.UpLeft)
        {
            //判断主路线下一张卡的右上卡
            var targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin()?.GetCard(CardPosType.UpRight);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));

                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
            //判断当前卡的中间卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.Main);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpRight, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpRight));
            }
        }

        //当卡牌在左下区时
        if (currentCard.currentCardPosType == CardPosType.DownLeft)
        {
            //判断主路线下一张卡的右下卡
            var targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin()?.GetCard(CardPosType.DownRight);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));

                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
            //判断当前卡的中间卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.Main);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownRight, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownRight));
            }
        }

        //当卡牌在右上区时
        if (currentCard.currentCardPosType == CardPosType.UpRight)
        {
            //判断主路线下一张卡的右上卡
            var targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin()?.GetNextCardRegoin()?.GetCard(CardPosType.UpLeft);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));

                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
            //判断当前卡的中间卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.Main);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.DownLeft));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));
            }
        }

        //当卡牌在右下区时
        if (currentCard.currentCardPosType == CardPosType.DownRight)
        {
            //判断主路线下一张卡的左下卡
            var targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin()?.GetNextCardRegoin()?.GetCard(CardPosType.DownLeft);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));

                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
            //判断当前卡的中间卡
            targetCard = currentCard.BelongCardRegoin.GetCard(CardPosType.Main);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.UpLeft));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
        }

        //当卡牌在中上区时
        if (currentCard.currentCardPosType == CardPosType.UpCenter)
        {
            //判断主路线下一张卡的中间卡
            var targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin()?.GetCard(CardPosType.Main);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.UpLeft, currentCard.GetRoadSignColor(RoadSignPos.DownRight));
                command.Add((isClickable, targetCard, RoadSignPos.UpLeft));
            }
        }

        //当卡牌在中下区时
        if (currentCard.currentCardPosType == CardPosType.DownCenter)
        {
            //判断主路线上一张卡的中间卡
            var targetCard = currentCard.BelongCardRegoin.GetNextCardRegoin()?.GetCard(CardPosType.Main);
            if (targetCard != null)
            {
                isClickable = targetCard.SetRoadSIgnClickable(RoadSignPos.DownLeft, currentCard.GetRoadSignColor(RoadSignPos.UpRight));
                command.Add((isClickable, targetCard, RoadSignPos.DownLeft));
            }
        }


        int colorEqualCount = command.Count(x => x.isEqual);
        Debug.Log("可选择路径数" + colorEqualCount);
        if (colorEqualCount > 0)
        {
            //自动移动
            if (colorEqualCount == 1)
            {
                var targetCommand = command.First(x => x.isEqual);
                SeletSelectMovementCard = targetCommand.targetCard;
            }
            else
            {
                while (SeletSelectMovementCard == null)
                {
                    await Task.Delay(100);
                }
            }
            var targetRegoin = Chara.BelongCard.BelongCardRegoin;
            var targetCardPosType = Chara.BelongCard.currentCardPosType;
            await Task.Delay(500);
            Chara.Instanc.SetRoad(SeletSelectMovementCard);
            Chara.Instanc.Settlement();
            Battle.GetAllCardList().ForEach(card => card.ReSetColor());
            await Task.Delay(2000);
            targetRegoin.Recycle(targetCardPosType);


        }
        else
        {
            Battle.GetAllCardList().ForEach(card => card.ReSetColor());
        }
        Debug.Log("移动位置选择完毕");
        SeletSelectMovementCard = null;
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