public enum CardState
{
    OnDeck,      //在卡组中,放在卡组位置
    OnHand,      //在手牌中,放在手牌位置
    OnPlay,      //选取拖拽阶段，跟随鼠标
    AfterPlay,   //拖拽放手阶段,放在打出位置
    OnDeploy,    //生成虚假卡牌在地图生成模拟部署效果
    AfterDeploy, //在地图真实部署
}
