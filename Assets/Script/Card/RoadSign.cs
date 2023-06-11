using UnityEngine;

public class RoadSign : MonoBehaviour
{
    public bool IsCanClick { get; set; } = false;
    public Color currentColor { get; set; }
    private void OnMouseUp()
    {
        if (IsCanClick)
        {
            GameProgress.SeletSelectMovementCard = transform.parent.GetComponent<Card>();
        }
    }
    public void SetColor(Color color)
    {
        currentColor = color;
        ResetColor();
    }

    public void ResetColor()
    {
        GetComponent<Renderer>().material.SetColor("_Color", currentColor*5);
    }

    public void SetCanClick(bool isCanClick)
    {
        IsCanClick = isCanClick;
        GetComponent<Renderer>().material.SetColor("_Color", currentColor * (isCanClick ? 4 : 0.5f));
    }
    
}
