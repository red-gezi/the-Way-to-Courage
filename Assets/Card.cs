using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //���Ƶ�ǰ��״̬
    public enum CardState
    {
        OnDeck,      //�ڿ�����,���ڿ���λ��
        OnHand,      //��������,��������λ��
        OnPlay,      //ѡȡ��ק�׶Σ��������
        AfterPlay,   //��ק���ֽ׶�,���ڴ��λ��
        OnDeploy,    //������ٿ����ڵ�ͼ����ģ�ⲿ��Ч��
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
    int RegionRank = 0;
    // Start is called before the first frame update
    public bool IsOnMainRoad { get; set; }
    //�����ϻ�������ʱλtrue
    public bool IsUpRight { get; set; }
    //�����Ƿ�ѡ��
    public bool IsCardSelect { get; set; }

    //������,����
    public void Deploy(int regionRank, List<Card> mainCards)
    {
        RegionRank = regionRank;
        mainCards.Add(this);
        Battle.IsDeployOver = true;
    }
    public CardRegoin BelongCardRegoin { get; set; }

    //�������Ӧ�����ɵ���

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
            //�������Ļ�·����֣��Ż����ƣ��������Ļ�Ϸ����֣����
            if (true)
            {
                currentCardState = CardState.AfterPlay;
                Battle.IsWaitForPlayCard = false;
                Battle.IsPlayCardOver = true;
                Battle.HandCards.Remove(this);
            }
        }
    }
    //ˢ�¼��㿨������ͽǶ�
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
        //���¿�������ͽǶ�
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2);
        transform.eulerAngles = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(targetEuler), Time.deltaTime * 2).eulerAngles;
    }

}
