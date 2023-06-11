using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Card : MonoBehaviour
{
    public int Population { get; set; }
    public int Supplies { get; set; }
    public int Treasures { get; set; }
    public Text PopulationText;
    public Text SuppliesText;
    public Text TreasuresText;

    public CardRegoin BelongCardRegoin => Battle.MainRoadRegoins[RegionRank];

    //在手牌中的顺序
    public int HandRank => Battle.HandCards.IndexOf(this);
    public int RegoinCardListRank => BelongCardRegoin.GetCardList(currentCardPosType).IndexOf(this);
    //对应的主干道牌在主干道中的顺序
    public int RegionRank { get; set; } = 0;
    // Start is called before the first frame update
    public bool IsOnMainRoad { get; set; }
    //当向上或者向左时位true
    public bool IsUpRight { get; set; } = true;
    //卡牌是否被选择
    public bool IsCardSelect { get; set; }
    //自身组对应的主干道牌
    public CardState currentCardState;
    //当卡牌为临时卡时所在的位置
    public CardPosType tempCardPosType;
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
    public List<Texture2D> colorTex;
    //四个角的颜色,依次是左上 右上 右下 左下
    public CardColor[] colors = new CardColor[4];
    Vector3 targetPos = Vector3.zero;
    Vector3 targetEuler = Vector3.zero;

    public void Init(int Population, int Supplies, int Treasures, CardColor[] cardColor)
    {

        this.Population = Population;
        this.Supplies = Supplies;
        this.Treasures = Treasures;
        colors = cardColor;
        SetColor(colors);
    }
    ///////////////////////////////////////////////////////位置刷新////////////////////////////////////////////////////////////////
    void Update() => RefreshCard();
    //刷新计算卡牌坐标和角度
    public void RefreshCard()
    {
        switch (currentCardState)
        {
            case CardState.OnDeck:

                targetPos = new Vector3(31, 0, -11);
                targetEuler = new Vector3(0, 45, 0);
                //transform.position = targetPos;
                break;
            case CardState.OnHand:
                float x = Mathf.Lerp(10, 20, 1f / (Battle.HandCards.Count - 1) * HandRank);
                float y = 0.8f * Mathf.Sin(Mathf.Lerp(0, Mathf.PI, 1f / (Battle.HandCards.Count - 1) * HandRank));
                float angel = Mathf.Lerp(-25, 25, 1f / (Battle.HandCards.Count - 1) * HandRank);
                targetPos = new Vector3(x, 2 + HandRank * 0.2f, -12 + y);
                if (IsCardSelect)
                {
                    targetPos += transform.forward * 2;
                    targetPos += transform.up * 1;
                    angel = 0;
                }
                targetEuler = new Vector3(0, angel, 0);
                break;
            case CardState.OnPlay:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                targetPos = ray.origin + ray.direction * 15;

                break;
            case CardState.AfterPlay:
                targetPos = new Vector3(30, 0, 4);
                targetEuler = new Vector3(0, IsUpRight ? 0 : 180, 0);
                break;
            case CardState.OnDeploy:
                Vector3 MainPos = new Vector3(RegionRank * 4, 0.2f, 0);
                switch (tempCardPosType)
                {
                    case CardPosType.None:
                        break;
                    case CardPosType.Main:
                        targetPos = MainPos;
                        targetEuler = new Vector3(0, IsUpRight ? 0 : 180, 0);
                        break;
                    case CardPosType.UpLeft:
                        targetPos = MainPos + new Vector3(-1f, 0, 5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.UpCenter:
                        targetPos = MainPos + new Vector3(0, 0, 5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.UpRight:
                        targetPos = MainPos + new Vector3(1f, 0, 5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.DownLeft:
                        targetPos = MainPos + new Vector3(-1f, 0, -5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.DownCenter:
                        targetPos = MainPos + new Vector3(0, 0, -5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.DownRight:
                        targetPos = MainPos + new Vector3(1f, 0, -5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    default:
                        break;
                }
                //强制立刻刷新位置
                transform.position = targetPos;
                break;
            case CardState.AfterDeploy:
                MainPos = new Vector3(RegionRank * 4, RegoinCardListRank * 0.01f, 0);
                switch (currentCardPosType)
                {
                    case CardPosType.None:
                        break;
                    case CardPosType.Main:
                        targetPos = MainPos;
                        targetEuler = new Vector3(0, IsUpRight ? 0 : 180, 0);
                        break;
                    case CardPosType.UpLeft:
                        targetPos = MainPos + new Vector3(-1f, 0, 5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.UpCenter:
                        targetPos = MainPos + new Vector3(0, 0, 5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.UpRight:
                        targetPos = MainPos + new Vector3(1f, 0, 5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.DownLeft:
                        targetPos = MainPos + new Vector3(-1f, 0, -5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.DownCenter:
                        targetPos = MainPos + new Vector3(0, 0, -5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    case CardPosType.DownRight:
                        targetPos = MainPos + new Vector3(1f, 0, -5f);
                        targetEuler = new Vector3(0, IsUpRight ? 270 : 90, 0);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        //更新卡牌坐标和角度
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5);
        transform.eulerAngles = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(targetEuler), Time.deltaTime * 5).eulerAngles;
    }
    ///////////////////////////////////////////////////////卡牌交互////////////////////////////////////////////////////////////////

    private void OnMouseEnter() => IsCardSelect = true;
    private void OnMouseExit() => IsCardSelect = false;
    private void OnMouseDown()
    {
        if (GameProgress.IsWaitForPlayCard && currentCardState == CardState.OnHand)
        {
            currentCardState = CardState.OnPlay;
            GameProgress.PrePlayCard = this;
        }
    }
    private void OnMouseUp()
    {
        Debug.Log(Input.mousePosition.y);
        if (GameProgress.PrePlayCard == this)
        {
            //如果在屏幕下方松手，放回手牌，如果在屏幕上方松手，打出
            if (Input.mousePosition.y/Screen.height>0.3)
            {
                currentCardState = CardState.AfterPlay;
                GameProgress.IsWaitForPlayCard = false;
                GameProgress.IsPlayCardOver = true;
                Battle.HandCards.Remove(this);
            }
            else
            {
                currentCardState = CardState.OnHand;
                GameProgress.PrePlayCard = null;
            }
        }
    }

    ///////////////////////////////////////////////////////操作指令////////////////////////////////////////////////////////////////

    /// <summary>
    /// 将卡牌部署到指定位置
    /// </summary>
    /// <param name="regionRank"></param>
    /// <param name="regionCards"></param>
    /// 
    public void Deploy(int regionRank, List<Card> regionCards)
    {
        //Debug.Log($"部署卡牌{name}到{regionRank}区+");
        //清空原来的所属
        if (Battle.HandCards.Contains(this))
        {
            Battle.HandCards.Remove(this);
        }
        if (Battle.DeskCards.Contains(this))
        {
            Battle.DeskCards.Remove(this);
        }
        GameProgress.PrePlayCard = null;
        //设置新所属
        RegionRank = regionRank;
        regionCards.Add(this);
        currentCardState = CardState.AfterDeploy;
        //如果部署在了主区域，则设置参数
        if (BelongCardRegoin.MainCards.Contains(this))
        {
            IsOnMainRoad = true;
        }
        GameProgress.IsDeployOver = true;
    }
    /// <summary>
    /// 以临时牌的形式进行展示
    /// </summary>
    /// <param name="regionRank"></param>
    /// <param name="cardPosType"></param>
    public void ShowTempCard(int regionRank, CardPosType cardPosType)
    {
        //设置新所属
        gameObject.SetActive(true);
        RegionRank = regionRank;
        tempCardPosType = cardPosType;
        currentCardState = CardState.OnDeploy;
        //设置卡牌属性
        Battle.TempCardModel.transform.GetChild(0).GetComponent<Renderer>().material.color = GameProgress.PrePlayCard.transform.GetChild(0).GetComponent<Renderer>().material.color;
    }
    ///////////////////////////////////////////////////////设置卡牌属性和四角路标颜色状态////////////////////////////////////////////////////////////////

    public void RefreshUI()
    {
        PopulationText.text = Mathf.Abs(Population).ToString();
        SuppliesText.text = Mathf.Abs(Supplies).ToString();
        TreasuresText.text = Mathf.Abs(Treasures).ToString();
        PopulationText.color = Population >= 0 ? Color.green : Color.red;
        SuppliesText.color = Population >= 0 ? Color.green : Color.red;
        TreasuresText.color = Population >= 0 ? Color.green : Color.red;
    }
    /// <summary>
    /// 初次设置卡牌颜色
    /// </summary>
    /// <param name="colors"></param>
    public void SetColor(CardColor[] colors)
    {
        this.colors = colors;
        for (int i = 0; i < 4; i++)
        {
            //transform.GetChild(i + 1).GetComponent<Renderer>().material.mainTexture = colorTex[(int)colors[i]];
            switch (colors[i])
            {
                case CardColor.Blue: transform.GetChild(i + 1).GetComponent<RoadSign>().SetColor(Color.cyan); break;
                case CardColor.Red: transform.GetChild(i + 1).GetComponent<RoadSign>().SetColor(Color.red); break;
                case CardColor.Yellow: transform.GetChild(i + 1).GetComponent<RoadSign>().SetColor(Color.yellow); ; break;
                default: transform.GetChild(i + 1).GetComponent<RoadSign>().SetColor(Color.black); break;
            }
        }
    }
    /// <summary>
    /// 重置卡牌颜色
    /// </summary>
    public void ReSetColor()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i + 1).GetComponent<RoadSign>().ResetColor();
        }
    }
    /// <summary>
    /// 重置卡牌颜色
    /// </summary>
    public void SetAllRoadSIgnsCannotClick()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i + 1).GetComponent<RoadSign>().SetCanClick(false);
        }
    }
    public CardColor GetRoadSignColor(RoadSignPos roadSignPos)
    {
        int bias = IsOnMainRoad ? (IsUpRight ? 0 : 2) : (IsUpRight ? 1 : 3);
        int[] index = new int[] { (0 + bias) % 4, (1 + bias) % 4, (2 + bias) % 4, (3 + bias) % 4 };
        CardColor currentColor = colors[index[(int)roadSignPos]];
        return currentColor;
    }
    public bool SetRoadSIgnClickable(RoadSignPos roadSignPos, CardColor currentColor)
    {
        int bias = IsOnMainRoad ? (IsUpRight ? 0 : 2) : (IsUpRight ? 1 : 3);
        int[] index = new int[] { (0 + bias) % 4, (1 + bias) % 4, (2 + bias) % 4, (3 + bias) % 4 };
        CardColor targetColor = GetRoadSignColor(roadSignPos);
        bool isColorEqual = targetColor == currentColor;
        transform.GetChild(index[(int)roadSignPos] + 1).GetComponent<RoadSign>().SetCanClick(isColorEqual);
        return isColorEqual;
    }
}
