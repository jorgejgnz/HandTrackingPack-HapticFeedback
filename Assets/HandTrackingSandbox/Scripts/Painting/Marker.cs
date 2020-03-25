using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private Color color;

    [SerializeField]
    private MeshRenderer[] colouredParts;

    [SerializeField]
    private Painter painter;

    [SerializeField]
    private PaintReceiver paintReceiver;

    void Awake()
    {

        foreach (MeshRenderer renderer in colouredParts)
        {
            renderer.material.color = color;
        }

        painter.Initialize(paintReceiver);
        painter.ChangeColour(color);
    }

    public void SetColor(string nameColor)
    {
        switch (nameColor)
        {
            case "red":
                color = Color.red;
                break;
            case "green":
                color = Color.green;
                break;
            case "blue":
                color = Color.blue;
                break;
        }

        foreach (MeshRenderer renderer in colouredParts)
        {
            renderer.material.color = color;
        }

        painter.ChangeColour(color);
    }
}
