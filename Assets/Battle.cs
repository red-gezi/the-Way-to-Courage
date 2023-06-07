using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Battle : MonoBehaviour
{
    // Start is called before the first frame update
    public static Battle Instance { get; set; }
    public GameObject cardModel;
    public static List<Card> DeskCards { get; set; } = new();
    public static List<Card> HandCards { get; set; } = new();
    public static List<Card> MainRoadCards { get; set; } = new();
     int maxCardsCount=10;

    //打出的牌
    Card playCard;
    //选择的部署位置
    Vector3 seletCardPos;


    public static bool IsPlayCardOver { get; set; } = true;
    public static bool IsSeletCardPosOver { get; set; } = true;

    void Start()
    {
        Instance = this;
        GetComponent<GameProgress>().BattleInit();
    }

    // Update is called once per frame
    void Update()
    {
        //当打出卡牌时
        if (!IsPlayCardOver)
        {
            Debug.Log("随机打出一张牌");
            Battle.HandCards[0].currentCardState = Card.CardState.AfterDraw;
            IsPlayCardOver = true;
        }
        if (!IsSeletCardPosOver)
        {
            var targetCard = HandCards.FirstOrDefault();
            if (targetCard != null)
            {
                targetCard.currentCardState = Card.CardState.AfterDeploy;
                HandCards.Remove(targetCard);
                //模拟一个部署效果
                if (MainRoadCards.Count < maxCardsCount)
                {
                    Debug.Log("加入一个卡牌到主路");
                    targetCard.MainCard = targetCard;
                    MainRoadCards.Add(targetCard);
                }
                else
                {
                    for (int i = 0; i < MainRoadCards.Count; i++)
                    {
                        if (MainRoadCards[i].UpLeftCard == null)
                        {
                            Debug.Log("加入一个卡牌到上左");
                            MainRoadCards[i].UpLeftCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].UpCenterCard == null)
                        {
                            Debug.Log("加入一个卡牌到上中");
                            MainRoadCards[i].UpCenterCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].UpLeftCard == null)
                        {
                            Debug.Log("加入一个卡牌到上右");
                            MainRoadCards[i].UpRightCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].DownLeftCard == null)
                        {
                            Debug.Log("加入一个卡牌到下左");
                            MainRoadCards[i].DownLeftCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].DownCenterCard == null)
                        {
                            Debug.Log("加入一个卡牌到下中");
                            MainRoadCards[i].DownCenterCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].DownLeftCard == null)
                        {
                            Debug.Log("加入一个卡牌到下右");
                            MainRoadCards[i].DownRightCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                    }
                }
                IsPlayCardOver = true;
            }
            
        }
    }
    private void OnMouseDown()
    {
        //判断点击位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPos = hitInfo.point;
            Debug.Log("Target Position: " + targetPos);
        }
    }
    private void OnMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPos = hitInfo.point;
            Debug.Log("Target Position: " + targetPos);
        }
    }
}
