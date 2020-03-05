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
}
