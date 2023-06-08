using System.Collections.Generic;

public class CardRegoin
{
    public List<Card> MainCards { get; set; } = new();
    public List<Card> UpLeftCards { get; set; } = new();
    public List<Card> UpCenterCards { get; set; } = new();
    public List<Card> UpRightCards { get; set; } = new();
    public List<Card> DownLeftCards { get; set; } = new();
    public List<Card> DownCenterCards { get; set; } = new();
    public List<Card> DownRightCards { get; set; } = new();
}
