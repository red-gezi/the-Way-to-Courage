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

    //�������
    Card playCard;
    //ѡ��Ĳ���λ��
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
        //���������ʱ
        if (!IsPlayCardOver)
        {
            Debug.Log("������һ����");
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
                //ģ��һ������Ч��
                if (MainRoadCards.Count < maxCardsCount)
                {
                    Debug.Log("����һ�����Ƶ���·");
                    targetCard.MainCard = targetCard;
                    MainRoadCards.Add(targetCard);
                }
                else
                {
                    for (int i = 0; i < MainRoadCards.Count; i++)
                    {
                        if (MainRoadCards[i].UpLeftCard == null)
                        {
                            Debug.Log("����һ�����Ƶ�����");
                            MainRoadCards[i].UpLeftCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].UpCenterCard == null)
                        {
                            Debug.Log("����һ�����Ƶ�����");
                            MainRoadCards[i].UpCenterCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].UpLeftCard == null)
                        {
                            Debug.Log("����һ�����Ƶ�����");
                            MainRoadCards[i].UpRightCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].DownLeftCard == null)
                        {
                            Debug.Log("����һ�����Ƶ�����");
                            MainRoadCards[i].DownLeftCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].DownCenterCard == null)
                        {
                            Debug.Log("����һ�����Ƶ�����");
                            MainRoadCards[i].DownCenterCard = targetCard;
                            targetCard.MainCard = MainRoadCards[i];
                            break;
                        }
                        else if (MainRoadCards[i].DownLeftCard == null)
                        {
                            Debug.Log("����һ�����Ƶ�����");
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
        //�жϵ��λ��
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
