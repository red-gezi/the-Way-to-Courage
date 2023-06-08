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
    public static int maxMainRoadCount = 4;
    public static List<CardRegoin> MainRoadRegoins { get; set; } = new();


    //选择的部署位置
    public static Vector3 seletCardPos;

    //处于等待打出卡牌
    public static bool IsWaitForPlayCard { get; set; } = false;
    //打出卡牌完毕
    public static bool IsPlayCardOver { get; set; } = true;
    //预打出的牌
    public static Card prePlayCard;
    //预打出的牌
    public static Card preDeployCard;
    //处于等待选择部署位置
    public static bool IsWaitForDeploy { get; set; } = false;
    //部署卡牌完毕
    public static bool IsDeployOver { get; set; } = true;

    void Start()
    {
        Instance = this;
        GetComponent<GameProgress>().BattleInit();
    }

    // Update is called once per frame
    void Update()
    {

        ////当打出卡牌时
        //if (!IsPlayCardOver)
        //{
        //    Debug.Log("随机打出一张牌");
        //    Battle.HandCards[0].currentCardState = Card.CardState.AfterPlay;
        //    IsPlayCardOver = true;
        //}
        //if (true && Battle.prePlayCard != null && !IsDeployOver)
        //{
        //    var targetCard = Battle.prePlayCard;
        //    if (targetCard != null)
        //    {
        //        targetCard.currentCardState = Card.CardState.AfterDeploy;
        //        HandCards.Remove(targetCard);
        //        //模拟一个部署效果
        //        if (MainRoadCards.Count < maxMainRoadCardsCount)
        //        {
        //            Debug.Log("加入一个卡牌到主路");
        //            targetCard.MainCard = targetCard;
        //            MainRoadCards.Add(targetCard);
        //        }
        //        else
        //        {
        //            for (int i = 0; i < MainRoadCards.Count; i++)
        //            {
        //                if (MainRoadCards[i].UpLeftCard == null)
        //                {
        //                    Debug.Log("加入一个卡牌到上左");
        //                    MainRoadCards[i].UpLeftCard = targetCard;
        //                    targetCard.MainCard = MainRoadCards[i];
        //                    break;
        //                }
        //                else if (MainRoadCards[i].UpCenterCard == null)
        //                {
        //                    Debug.Log("加入一个卡牌到上中");
        //                    MainRoadCards[i].UpCenterCard = targetCard;
        //                    targetCard.MainCard = MainRoadCards[i];
        //                    break;
        //                }
        //                else if (MainRoadCards[i].UpRightCard == null)
        //                {
        //                    Debug.Log("加入一个卡牌到上右");
        //                    MainRoadCards[i].UpRightCard = targetCard;
        //                    targetCard.MainCard = MainRoadCards[i];
        //                    break;
        //                }
        //                else if (MainRoadCards[i].DownLeftCard == null)
        //                {
        //                    Debug.Log("加入一个卡牌到下左");
        //                    MainRoadCards[i].DownLeftCard = targetCard;
        //                    targetCard.MainCard = MainRoadCards[i];
        //                    break;
        //                }
        //                else if (MainRoadCards[i].DownCenterCard == null)
        //                {
        //                    Debug.Log("加入一个卡牌到下中");
        //                    MainRoadCards[i].DownCenterCard = targetCard;
        //                    targetCard.MainCard = MainRoadCards[i];
        //                    break;
        //                }
        //                else if (MainRoadCards[i].DownRightCard == null)
        //                {
        //                    Debug.Log("加入一个卡牌到下右");
        //                    MainRoadCards[i].DownRightCard = targetCard;
        //                    targetCard.MainCard = MainRoadCards[i];
        //                    break;
        //                }
        //            }
        //        }
        //        IsDeployOver = true;
        //    }

        //}
    }
    private void OnDrawGizmos()
    {
        DrawBattle();
    }
    public void DrawBattle()
    {
        for (int i = 0; i < maxMainRoadCount; i++)
        {
            Gizmos.DrawLine(new Vector3(i * 4 - 2, 0.1f, +3), new Vector3(i * 4 + 2, 0.1f, +3));
            Gizmos.DrawLine(new Vector3(i * 4 - 2, 0.1f, -3), new Vector3(i * 4 + 2, 0.1f, -3));
            Gizmos.DrawLine(new Vector3(i * 4 - 2, 0.1f, +3), new Vector3(i * 4 - 2, 0.1f, -3));
            Gizmos.DrawLine(new Vector3(i * 4 + 2, 0.1f, +3), new Vector3(i * 4 + 2, 0.1f, -3));

            //Debug.DrawLine(new Vector3(i * 4 - 4, 1, 7), new Vector3(i * 4 + 2, 1, 7), Color.red);
            //Debug.DrawLine(new Vector3(i * 4 - 4, 1, 3), new Vector3(i * 4 + 2, 1, 3), Color.red);
            //Debug.DrawLine(new Vector3(i * 4 - 4, 1, 7), new Vector3(i * 4 - 4, 1, 3), Color.red);
            //Debug.DrawLine(new Vector3(i * 4 + 2, 1, 7), new Vector3(i * 4 + 2, 1, 3), Color.red);

            //Debug.DrawLine(new Vector3(i * 4 - 4, 1, -7), new Vector3(i * 4 + 2, 1, -7), Color.red);
            //Debug.DrawLine(new Vector3(i * 4 - 4, 1, -3), new Vector3(i * 4 + 2, 1, -3), Color.red);
            //Debug.DrawLine(new Vector3(i * 4 - 4, 1, -7), new Vector3(i * 4 - 4, 1, -3), Color.red);
            //Debug.DrawLine(new Vector3(i * 4 + 2, 1, -7), new Vector3(i * 4 + 2, 1, -3), Color.red);

            //Debug.DrawLine(new Vector3(i * 4 - 3, 1, 7), new Vector3(i * 4 + 3, 1, 7), Color.green);
            //Debug.DrawLine(new Vector3(i * 4 - 3, 1, 3), new Vector3(i * 4 + 3, 1, 3), Color.green);
            //Debug.DrawLine(new Vector3(i * 4 - 3, 1, 7), new Vector3(i * 4 - 3, 1, 3), Color.green);
            //Debug.DrawLine(new Vector3(i * 4 + 3, 1, 7), new Vector3(i * 4 + 3, 1, 3), Color.green);

            //Debug.DrawLine(new Vector3(i * 4 - 3, 1, -7), new Vector3(i * 4 + 3, 1, -7), Color.green);
            //Debug.DrawLine(new Vector3(i * 4 - 3, 1, -3), new Vector3(i * 4 + 3, 1, -3), Color.green);
            //Debug.DrawLine(new Vector3(i * 4 - 3, 1, -7), new Vector3(i * 4 - 3, 1, -3), Color.green);
            //Debug.DrawLine(new Vector3(i * 4 + 3, 1, -7), new Vector3(i * 4 + 3, 1, -3), Color.green);

            //Debug.DrawLine(new Vector3(i * 4 - 2, 1, 7), new Vector3(i * 4 + 4, 1, 7), Color.blue);
            //Debug.DrawLine(new Vector3(i * 4 - 2, 1, 3), new Vector3(i * 4 + 4, 1, 3), Color.blue);
            //Debug.DrawLine(new Vector3(i * 4 - 2, 1, 7), new Vector3(i * 4 - 2, 1, 3), Color.blue);
            //Debug.DrawLine(new Vector3(i * 4 + 4, 1, 7), new Vector3(i * 4 + 4, 1, 3), Color.blue);

            //Debug.DrawLine(new Vector3(i * 4 - 2, 1, -7), new Vector3(i * 4 + 4, 1, -7), Color.blue);
            //Debug.DrawLine(new Vector3(i * 4 - 2, 1, -3), new Vector3(i * 4 + 4, 1, -3), Color.blue);
            //Debug.DrawLine(new Vector3(i * 4 - 2, 1, -7), new Vector3(i * 4 - 2, 1, -3), Color.blue);
            //Debug.DrawLine(new Vector3(i * 4 + 4, 1, -7), new Vector3(i * 4 + 4, 1, -3), Color.blue);
        }
    }
    private void OnMouseDown()
    {
        //判断点击位置
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out RaycastHit hitInfo))
        //{
        //    Vector3 targetPos = hitInfo.point;
        //    //Debug.Log("Target Position: " + targetPos);
        //}
        if (IsWaitForDeploy)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 targetPos = hitInfo.point;
                if (targetPos.x > -2 && targetPos.x < maxMainRoadCount * 4 + 2)
                {
                    int rank = (int)(targetPos.x + 2) / 4;
                    if (targetPos.z < 3 && targetPos.z > -3)
                    {
                        Debug.Log($"点击了{rank}主区");
                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].MainCards);
                        IsWaitForDeploy = false;
                        Battle.prePlayCard = null;
                    }
                    else if (targetPos.z > 3 && targetPos.z < 7)
                    {
                        Debug.Log($"点击了{rank}上区");
                        if ((targetPos.x + 2) / 4 < 1f / 3)
                        {
                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpLeftCards);
                        }
                        else if ((targetPos.x + 2) / 4 < 2f / 3)
                        {
                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpCenterCards);
                        }
                        else
                        {
                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpRightCards);
                        }
                        IsWaitForDeploy = false;
                        Battle.prePlayCard = null;
                    }
                    else if (targetPos.z < -3 && targetPos.z > -7)
                    {
                        Debug.Log($"点击了{rank}下区");
                        if ((targetPos.x + 2) / 4 < 1f / 3)
                        {
                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownLeftCards);
                        }
                        else if ((targetPos.x + 2) / 4 < 2f / 3)
                        {
                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownCenterCards);
                        }
                        else
                        {
                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownRightCards);
                        }
                        IsWaitForDeploy = false;
                        Battle.prePlayCard = null;
                    }
                    else
                    {
                        Debug.Log("不在可部署范围内");
                    }
                }
                else
                {
                    Debug.Log("不在可部署范围内");
                }
               
                //Debug.Log("Target Position: " + targetPos);
            }
        }
    }
    private void OnMouseOver()
    {


    }
}
