using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //���Ƶ�ǰ��״̬
    public enum CardState
    {
        OnDeck,      //�ڿ�����
        OnHand,      //��������
        OnDraw,      //ѡȡ��ק�׶�
        AfterDraw,   //��ק���ֽ׶�
        OnDeploy,    //�ڵ�ͼ����ģ�ⲿ��Ч��
        AfterDeploy, //�ڵ�ͼ��ʵ����
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
    //�������е�˳��
    int HandRank => Battle.HandCards.IndexOf(this);
    //��Ӧ�����ɵ��������ɵ��е�˳��
    int MainRoadRank => Battle.MainRoadCards.IndexOf(MainCard);
    // Start is called before the first frame update
    public bool IsOnMainRoad { get; set; }
    //�����ϻ�������ʱλtrue
    public bool IsUpRight { get; set; }

    //�������Ӧ�����ɵ���
    public Card MainCard { get; set; }
    public Card UpLeftCard { get; set; }
    public Card UpCenterCard { get; set; }
    public Card UpRightCard { get; set; }
    public Card DownLeftCard { get; set; }
    public Card DownCenterCard { get; set; }
    public Card DownRightCard { get; set; }
    public CardState currentCardState;
    public CardPosType currentCardPosType
    {
        get
        {
            if (MainCard == this)
            {
                return CardPosType.Main;
            }
            else if (UpRightCard == this)
            {
                return CardPosType.UpRight;
            }
            else if (UpLeftCard == this)
            {
                return CardPosType.UpLeft;
            }
            else if (UpCenterCard == this)
            {
                return CardPosType.UpCenter;
            }
            else if (DownRightCard == this)
            {
                return CardPosType.DownRight;
            }
            else if (DownLeftCard == this)
            {
                return CardPosType.DownLeft;
            }
            else if (DownCenterCard == this)
            {
                return CardPosType.DownCenter;
            }
            else
            {
                Debug.LogError("�쳣");
                return CardPosType.None;
            }
        }
    }
    //�ĸ��ǵ���ɫ,���������� ���� ���� ����
    Color[] colors = new Color[4];

    Color[] CurrentColors
    {
        get
        {
            if (IsOnMainRoad)
            {
                if (!IsUpRight)
                {
                    return new Color[] { colors[0], colors[1], colors[2], colors[3] };
                }
                else
                {
                    return new Color[] { colors[2], colors[3], colors[0], colors[1] };
                }
            }
            else
            {
                if (!IsUpRight)
                {
                    return new Color[] { colors[1], colors[2], colors[3], colors[0] };
                }
                else
                {
                    return new Color[] { colors[3], colors[0], colors[1], colors[2] };

                }
            }
        }
    }
    Vector3 targetPos= Vector3.zero;
    Vector3 targetEuler = Vector3.zero;
    void Update() => RefreshCard();
    //���㿨������ͽǶ�
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
                //float x = Mathf.Lerp(-2, 2, 1f / (Battle.HandCards.Count - 1) * HandRank);
                //float y = 0.3f * Mathf.Sin(Mathf.Lerp(0, Mathf.PI, 1f / (Battle.HandCards.Count - 1) * HandRank));
                //float angel = Mathf.Lerp(25, -25, 1f / (Battle.HandCards.Count - 1) * HandRank);
                //targetPos = new Vector3(x, -5 + y, -HandRank * 0.01f);
                //targetEuler = new Vector3(0, 0, angel);
                break;
            case CardState.OnDraw:
                break;
            case CardState.AfterDraw:
                break;
            case CardState.OnDeploy:
                break;
            case CardState.AfterDeploy:

               Vector3 MainPos = new Vector3(MainRoadRank * 3, 0, 0);
                switch (currentCardPosType)
                {
                    case CardPosType.None:
                        break;
                    case CardPosType.Main:
                        targetPos =  MainPos;
                        targetEuler = new Vector3(0, 0, 0);
                        break;
                    case CardPosType.UpLeft:
                        targetPos = MainPos+new Vector3(-1.5f,0,4.5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.UpCenter:
                        targetPos = MainPos + new Vector3(0, 0, 4.5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.UpRight:
                        targetPos = MainPos + new Vector3(1.5f, 0, 4.5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.DownLeft:
                        targetPos = MainPos + new Vector3(-1.5f, 0,- 4.5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.DownCenter:
                        targetPos = MainPos + new Vector3(0, 0, -4.5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    case CardPosType.DownRight:
                        targetPos = MainPos + new Vector3(-1.5f, 0, -4.5f);
                        targetEuler = new Vector3(0, 90, 0);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        //���¿�������ͽǶ�
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetEuler, Time.deltaTime * 2);
    }

}
