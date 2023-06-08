using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //卡牌当前的状态
    public enum CardState
    {
        OnDeck,      //在卡组中,放在卡组位置
        OnHand,      //在手牌中,放在手牌位置
        OnPlay,      //选取拖拽阶段，跟随鼠标
        AfterPlay,   //拖拽放手阶段,放在打出位置
        OnDeploy,    //生成虚假卡牌在地图生成模拟部署效果
        AfterDeploy, //在地图真实部署
    }
    public enum CardPosType
    {
        None,
        Main,
        UpLeft,
        UpCenter,
        UpRight,
        DownLeft,
        DownCenter,
        DownRight,
    }
    //在手牌中的顺序
    int HandRank => Battle.HandCards.IndexOf(this);
    //对应的主干道牌在主干道中的顺序
    int RegionRank = 0;
    // Start is called before the first frame update
    public bool IsOnMainRoad { get; set; }
    //当向上或者向左时位true
    public bool IsUpRight { get; set; }
    //卡牌是否被选择
    public bool IsCardSelect { get; set; }

    //部署卡牌,参数
    public void Deploy(int regionRank, List<Card> mainCards)
    {
        RegionRank = regionRank;
        mainCards.Add(this);
        Battle.IsDeployOver = true;
    }
    public CardRegoin BelongCardRegoin { get; set; }

    //自身组对应的主干道牌

    public CardState currentCardState;
    public CardPosType currentCardPosType
    {
        get
        {
            if (BelongCardRegoin.MainCards.Contains(this))
            {
                return CardPosType.Main;
            }
            else if (BelongCardRegoin.UpRightCards.Contains(this))
            {
                return CardPosType.UpRight;
            }
            else if (BelongCardRegoin.UpLeftCards.Contains(this))
            {
                return CardPosType.UpLeft;
            }
            else if (BelongCardRegoin.UpCenterCards.Contains(this))
            {
                return CardPosType.UpCenter;
            }
            else if (BelongCardRegoin.DownLeftCards.Contains(this))
            {
                return CardPosType.DownLeft;
            }
            else if (BelongCardRegoin.DownCenterCards.Contains(this))
            {
                return CardPosType.DownCenter;
            }
            else if (BelongCardRegoin.DownRightCards.Contains(this))
            {
                return CardPosType.DownRight;
            }
            else
            {
                Debug.LogError("异常");
                return CardPosType.None;
            }
        }
    }
    //四个角的颜色,依次是左上 右上 右下 左下
    Color[] colors = new Color[4];

    Color[] CurrentColors
    {
        get
        {
            if (IsOnMainRoad)
            {
                return IsUpRight ? new Color[] { colors[2], colors[3], colors[0], colors[1] } : new Color[] { colors[0], colors[1], colors[2], colors[3] };
            }
            else
            {
                return IsUpRight ? new Color[] { colors[3], colors[0], colors[1], colors[2] } : new Color[] { colors[1], colors[2], colors[3], colors[0] };
            }
        }
    }
    Vector3 targetPos = Vector3.zero;
    Vector3 targetEuler = Vector3.zero;
    void Update() => RefreshCard();

    private void OnMouseEnter() => IsCardSelect = true;
    private void OnMouseExit() => IsCardSelect = false;
    private void OnMouseDown()
    {
        if (Battle.IsWaitForPlayCard)
        {
            currentCardState = CardState.OnPlay;
            Battle.prePlayCard = this;
        }
    }
    private void OnMouseUp()
    {
        if (Battle.prePlayCard == this)
        {
            //如果在屏幕下方松手，放回手牌，如果在屏幕上方松手，打出
            if (true)
            {
                currentCardState = CardState.AfterPlay;
                Battle.IsWaitForPlayCard = false;
                Battle.IsPlayCardOver = true;
                Battle.HandCards.Remove(this);
            }
        }
    }
    //刷新计算卡牌坐标和角度
    public void RefreshCard()
    {
        switch (currentCardState)
        {
            case CardState.OnDeck:

                targetPos = new Vector3(30, 0, -8);
                targetEuler = new Vector3(0, 90, 0);
                transform.position = targetPos;
                break;
            case CardState.OnHand:
                float x = Mathf.Lerp(10, 20, 1f / (Battle.HandCards.Count - 1) * HandRank);
                float y = 0.3f * Mathf.Sin(Mathf.Lerp(0, Mathf.PI, 1f / (Battle.HandCards.Count - 1) * HandRank));
                float angel = Mathf.Lerp(-25, 25, 1f / (Battle.HandCards.Count - 1) * HandRank);
                targetPos = new Vector3(x, 5 + HandRank * 0.01f, -6 + y);
                if (IsCardSelect)
                {
                    targetPos += transform.forward * 3;
                }
                targetEuler = new Vector3(0, angel, 0);
                break;
            case CardState.OnPlay:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                targetPos = ray.origin + ray.direction * 15;

                break;
            case CardState.AfterPlay:
                targetPos = new Vector3(30, 0, 8);
                break;
            case CardState.OnDeploy:
                break;
            case CardState.AfterDeploy:

                Vector3 MainPos = new Vector3(RegionRank * 4, 0, 0);
                switch (currentCardPosType)
                {
                    case CardPosType.None:
                        break;
                    case CardPosType.Main:
                        targetPos = MainPos;
                        targetEuler = new Vector3(0, 0, 0);
                        break;
                    case CardPosType.UpLeft:
                        targetPos = MainPos + new Vector3(-2f, 0, 5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.UpCenter:
                        targetPos = MainPos + new Vector3(0, 0, 5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.UpRight:
                        targetPos = MainPos + new Vector3(2f, 0, 5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.DownLeft:
                        targetPos = MainPos + new Vector3(2f, 0, -5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.DownCenter:
                        targetPos = MainPos + new Vector3(0, 0, -5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.DownRight:
                        targetPos = MainPos + new Vector3(-2f, 0, -5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        //更新卡牌坐标和角度
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2);
        transform.eulerAngles = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(targetEuler), Time.deltaTime * 2).eulerAngles;
    }

}
