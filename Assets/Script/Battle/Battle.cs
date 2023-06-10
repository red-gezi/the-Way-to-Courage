using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Battle : MonoBehaviour
{
    // Start is called before the first frame update
    public static Battle Instance { get; set; }
    public GameObject cardModel;
    public static Chara chara;
    //��ʱ����
    public static GameObject TempCardModel;

    public static List<Card> DeskCards { get; set; } = new();
    public static List<Card> HandCards { get; set; } = new();
   
    public static int maxMainRoadCount = 8;
    public static List<CardRegoin> MainRoadRegoins { get; set; } = new();
    public static List<Card> GetAllCardList() => MainRoadRegoins.Select(region=>region.GetAllCardList()).SelectMany(x => x).ToList();

    void Start()
    {
        Instance = this;
        //������ʱ��
        TempCardModel = Instantiate(cardModel);
        TempCardModel.name = "Temp";
        TempCardModel.GetComponent<BoxCollider>().enabled = false;
        //ս����ʼ��
        GetComponent<GameProgress>().BattleInit();
    }
    //���Ʋ��𿨲�
    private void OnDrawGizmos()
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
    // Update is called once per frame
    //void Update()
    //{
    //    if (IsWaitForDeploy)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out RaycastHit hitInfo))
    //        {
    //            Vector3 targetPos = hitInfo.point;
    //            if (targetPos.x > -2 && targetPos.x < (maxMainRoadCount - 1) * 4 + 2)
    //            {
    //                int rank = (int)(targetPos.x + 2) / 4;
    //                if (targetPos.z < 3 && targetPos.z > -3)
    //                {
    //                    if (Input.GetMouseButtonDown(0))
    //                    {
    //                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].MainCards);
    //                        IsWaitForDeploy = false;
    //                    }
    //                    else
    //                    {
    //                        tempCard.ShowTempCard(rank, CardPosType.Main);
    //                    }
    //                }
    //                else if (targetPos.z > 3 && targetPos.z < 7)
    //                {
    //                    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
    //                    {
    //                        if (Input.GetMouseButtonDown(0))
    //                        {
    //                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpLeftCards);
    //                            IsWaitForDeploy = false;
    //                        }
    //                        else
    //                        {
    //                            tempCard.ShowTempCard(rank, CardPosType.UpLeft);
    //                        }
    //                    }
    //                    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
    //                    {
    //                        if (Input.GetMouseButtonDown(0))
    //                        {
    //                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpCenterCards);
    //                            IsWaitForDeploy = false;
    //                        }
    //                        else
    //                        {
    //                            tempCard.ShowTempCard(rank, CardPosType.UpCenter);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (Input.GetMouseButtonDown(0))
    //                        {
    //                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpRightCards);
    //                            IsWaitForDeploy = false;
    //                        }
    //                        else
    //                        {
    //                            tempCard.ShowTempCard(rank, CardPosType.UpRight);
    //                        }
    //                    }
    //                }
    //                else if (targetPos.z < -3 && targetPos.z > -7)
    //                {
    //                    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
    //                    {
    //                        if (Input.GetMouseButtonDown(0))
    //                        {
    //                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownLeftCards);
    //                            IsWaitForDeploy = false;
    //                        }
    //                        else
    //                        {
    //                            tempCard.ShowTempCard(rank, CardPosType.DownLeft);

    //                        }
    //                    }
    //                    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
    //                    {
    //                        if (Input.GetMouseButtonDown(0))
    //                        {
    //                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownCenterCards);
    //                            IsWaitForDeploy = false;
    //                        }
    //                        else
    //                        {
    //                            tempCard.ShowTempCard(rank, CardPosType.DownCenter);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (Input.GetMouseButtonDown(0))
    //                        {
    //                            prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownRightCards);
    //                            IsWaitForDeploy = false;
    //                        }
    //                        else
    //                        {
    //                            tempCard.ShowTempCard(rank, CardPosType.DownRight);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    //Debug.Log("���ڿɲ���Χ��");
    //                }
    //            }
    //            else
    //            {
    //                //Debug.Log("���ڿɲ���Χ��");
    //            }
    //        }
    //    }
    //    //else
    //    //{
    //    //    TempCardModel.SetActive(false);
    //    //}



    //    ////���������ʱ
    //    //if (!IsPlayCardOver)
    //    //{
    //    //    Debug.Log("������һ����");
    //    //    Battle.HandCards[0].currentCardState = Card.CardState.AfterPlay;
    //    //    IsPlayCardOver = true;
    //    //}
    //    //if (true && Battle.prePlayCard != null && !IsDeployOver)
    //    //{
    //    //    var targetCard = Battle.prePlayCard;
    //    //    if (targetCard != null)
    //    //    {
    //    //        targetCard.currentCardState = Card.CardState.AfterDeploy;
    //    //        HandCards.Remove(targetCard);
    //    //        //ģ��һ������Ч��
    //    //        if (MainRoadCards.Count < maxMainRoadCardsCount)
    //    //        {
    //    //            Debug.Log("����һ�����Ƶ���·");
    //    //            targetCard.MainCard = targetCard;
    //    //            MainRoadCards.Add(targetCard);
    //    //        }
    //    //        else
    //    //        {
    //    //            for (int i = 0; i < MainRoadCards.Count; i++)
    //    //            {
    //    //                if (MainRoadCards[i].UpLeftCard == null)
    //    //                {
    //    //                    Debug.Log("����һ�����Ƶ�����");
    //    //                    MainRoadCards[i].UpLeftCard = targetCard;
    //    //                    targetCard.MainCard = MainRoadCards[i];
    //    //                    break;
    //    //                }
    //    //                else if (MainRoadCards[i].UpCenterCard == null)
    //    //                {
    //    //                    Debug.Log("����һ�����Ƶ�����");
    //    //                    MainRoadCards[i].UpCenterCard = targetCard;
    //    //                    targetCard.MainCard = MainRoadCards[i];
    //    //                    break;
    //    //                }
    //    //                else if (MainRoadCards[i].UpRightCard == null)
    //    //                {
    //    //                    Debug.Log("����һ�����Ƶ�����");
    //    //                    MainRoadCards[i].UpRightCard = targetCard;
    //    //                    targetCard.MainCard = MainRoadCards[i];
    //    //                    break;
    //    //                }
    //    //                else if (MainRoadCards[i].DownLeftCard == null)
    //    //                {
    //    //                    Debug.Log("����һ�����Ƶ�����");
    //    //                    MainRoadCards[i].DownLeftCard = targetCard;
    //    //                    targetCard.MainCard = MainRoadCards[i];
    //    //                    break;
    //    //                }
    //    //                else if (MainRoadCards[i].DownCenterCard == null)
    //    //                {
    //    //                    Debug.Log("����һ�����Ƶ�����");
    //    //                    MainRoadCards[i].DownCenterCard = targetCard;
    //    //                    targetCard.MainCard = MainRoadCards[i];
    //    //                    break;
    //    //                }
    //    //                else if (MainRoadCards[i].DownRightCard == null)
    //    //                {
    //    //                    Debug.Log("����һ�����Ƶ�����");
    //    //                    MainRoadCards[i].DownRightCard = targetCard;
    //    //                    targetCard.MainCard = MainRoadCards[i];
    //    //                    break;
    //    //                }
    //    //            }
    //    //        }
    //    //        IsDeployOver = true;
    //    //    }

    //    //}
    //}

    //private void OnMouseDown()
    //{
    //    if (IsWaitForDeploy)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out RaycastHit hitInfo))
    //        {
    //            Vector3 targetPos = hitInfo.point;
    //            if (targetPos.x > -2 && targetPos.x < (maxMainRoadCount - 1) * 4 + 2)
    //            {
    //                int rank = (int)(targetPos.x + 2) / 4;
    //                if (targetPos.z < 3 && targetPos.z > -3)
    //                {
    //                    Debug.Log($"�����{rank}����");
    //                    prePlayCard.Deploy(rank, MainRoadRegoins[rank].MainCards);
    //                    IsWaitForDeploy = false;
    //                }
    //                else if (targetPos.z > 3 && targetPos.z < 7)
    //                {
    //                    Debug.Log($"�����{rank}����");
    //                    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
    //                    {
    //                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpLeftCards);
    //                    }
    //                    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
    //                    {
    //                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpCenterCards);
    //                    }
    //                    else
    //                    {
    //                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].UpRightCards);
    //                    }
    //                    IsWaitForDeploy = false;
    //                }
    //                else if (targetPos.z < -3 && targetPos.z > -7)
    //                {
    //                    Debug.Log($"�����{rank}����");
    //                    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
    //                    {
    //                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownLeftCards);
    //                    }
    //                    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
    //                    {
    //                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownCenterCards);
    //                    }
    //                    else
    //                    {
    //                        prePlayCard.Deploy(rank, MainRoadRegoins[rank].DownRightCards);
    //                    }
    //                    IsWaitForDeploy = false;
    //                }
    //                else
    //                {
    //                    Debug.Log("���ڿɲ���Χ��");
    //                }
    //            }
    //            else
    //            {
    //                Debug.Log("���ڿɲ���Χ��");
    //            }
    //        }
    //    }
    //}
    //private void OnMouseOver()
    //{
    //    if (prePlayCard != null)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out RaycastHit hitInfo))
    //        {
    //            Vector3 targetPos = hitInfo.point;
    //            Debug.Log($"x:{targetPos.x}-{(targetPos.x + 2) % 4}");

    //            if (targetPos.x > -2 && targetPos.x < (maxMainRoadCount - 1) * 4 + 2)
    //            {
    //                int rank = (int)(targetPos.x + 2) / 4;
    //                var tempCard = TempCardModel.GetComponent<Card>();
    //                if (targetPos.z < 3 && targetPos.z > -3)
    //                {
    //                    Debug.Log($"�����{rank}����");
    //                    tempCard.ShowTempCard(rank, CardPosType.Main);
    //                }
    //                else if (targetPos.z > 3 && targetPos.z < 7)
    //                {
    //                    Debug.Log($"�����{rank}����");
    //                    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
    //                    {
    //                        Debug.Log($"�����{rank}�������");
    //                        tempCard.ShowTempCard(rank, CardPosType.UpLeft);

    //                    }
    //                    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
    //                    {
    //                        Debug.Log($"�����{rank}�����м�");
    //                        tempCard.ShowTempCard(rank, CardPosType.UpCenter);

    //                    }
    //                    else
    //                    {
    //                        Debug.Log($"�����{rank}�����ұ�");
    //                        tempCard.ShowTempCard(rank, CardPosType.UpRight);

    //                    }
    //                }
    //                else if (targetPos.z < -3 && targetPos.z > -7)
    //                {
    //                    Debug.Log($"�����{rank}����");
    //                    if ((targetPos.x + 2) % 4 / 4 < 1f / 3)
    //                    {
    //                        Debug.Log($"�����{rank}�������");
    //                        tempCard.ShowTempCard(rank, CardPosType.DownLeft);

    //                    }
    //                    else if ((targetPos.x + 2) % 4 / 4 < 2f / 3)
    //                    {
    //                        Debug.Log($"�����{rank}�����м�");
    //                        tempCard.ShowTempCard(rank, CardPosType.DownCenter);

    //                    }
    //                    else
    //                    {
    //                        Debug.Log($"�����{rank}�����ұ�");
    //                        tempCard.ShowTempCard(rank, CardPosType.DownRight);

    //                    }
    //                }
    //                else
    //                {
    //                    Debug.Log("���ڿɲ���Χ��");
    //                }
    //            }
    //            else
    //            {
    //                Debug.Log("���ڿɲ���Χ��");
    //            }
    //        }
    //    }
    //    else
    //    {
    //        TempCardModel.SetActive(false);
    //    }
    //}
}
