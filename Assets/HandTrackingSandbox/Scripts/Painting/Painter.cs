using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private PaintMode paintMode;

    [SerializeField]
    private Transform paintingTransform;

    [SerializeField]
    private float raycastLength = 0.01f;

    [SerializeField]
    private Texture2D brush;

    [SerializeField]
    private float spacing = 1f;
    
    private float currentAngle = 0f;
    private float lastAngle = 0f;

    private PaintReceiver paintReceiver;
    private Collider paintReceiverCollider;

    private GameObject paintingObject;

    private Stamp stamp;

    private Color color;

    private Vector2? lastDrawPosition = null;

    public void Initialize(PaintReceiver newPaintReceiver)
    {
        stamp = new Stamp(brush);
        stamp.mode = paintMode;

        paintReceiver = newPaintReceiver;
        paintReceiverCollider = newPaintReceiver.GetComponent<Collider>();
    }

    private void Update()
    {
        currentAngle = -transform.rotation.eulerAngles.z;

        Ray ray = new Ray(paintingTransform.position, paintingTransform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * raycastLength);

        if (paintReceiverCollider.Raycast(ray, out hit, raycastLength))
        {
            if (lastDrawPosition.HasValue && lastDrawPosition.Value != hit.textureCoord)
            {
                paintReceiver.DrawLine(stamp, lastDrawPosition.Value, hit.textureCoord, lastAngle, currentAngle, color, spacing);
            }
            else
            {
                paintReceiver.CreateSplash(hit.textureCoord, stamp, color, currentAngle);
            }

            lastAngle = currentAngle;

            lastDrawPosition = hit.textureCoord;
        }
        else
        {
            lastDrawPosition = null;
        }
    }

    public void ChangeColour(Color newColor)
    {
        color = newColor;
    }

    public void SetRotation(float newAngle)
    {
        currentAngle = newAngle;
    }
}
