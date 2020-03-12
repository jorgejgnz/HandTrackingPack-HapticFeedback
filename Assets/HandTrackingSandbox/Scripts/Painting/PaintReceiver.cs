using UnityEngine;

using System.IO;

[RequireComponent(typeof(MeshRenderer))]
public class PaintReceiver : MonoBehaviour
{
    [SerializeField]
    private Texture2D initialTexture;

    private Texture2D texture;
    private Texture2D newTexture;
	private Color32[] originalTexture;
	private Color32[] currentTexture;
    private float currentStampRotation = 0f;

	private int textureWidth;
	private int textureHeight;

    private bool wasModified = false;

    private void Awake()
    {
        texture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;

        textureWidth = texture.width;
        textureHeight = texture.height;

        originalTexture = texture.GetPixels32();

        newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false, true);
		newTexture.SetPixels32(initialTexture.GetPixels32());
        newTexture.Apply();

        currentTexture = new Color32[textureWidth * textureHeight];
        newTexture.GetPixels32().CopyTo(currentTexture, 0);

        GetComponent<MeshRenderer>().material.mainTexture = newTexture;
    }

    // Apply changes only once per frame when all the pixels are drawn into the currentTexture
    private void LateUpdate()
    {
        if(wasModified)
        {
            newTexture.SetPixels32(currentTexture);
            newTexture.Apply();

            wasModified = false;
        }
    }

    /// <summary>
    /// Paints one stamp
    /// </summary>
    /// <param name="uvPosition">Position to be painted</param>
    /// <param name="stamp">Stamp instance</param>
    /// <param name="color">Colour used to paint over - applied only if PaintMode of stamp is set to PaintOver</param>
    /// <param name="stampRotation">Rotation of stamp</param>
    public void CreateSplash(Vector2 uvPosition, Stamp stamp, Color color, float stampRotation = 0f)
    {
        stamp.SetRotation(stampRotation);

		PaintOver (stamp, (Color32)color, uvPosition);
    }

    /// <summary>
    /// Paints a line that consist of stamps
    /// </summary>
    /// <param name="stamp">Stamp instance</param>
    /// <param name="startUVPosition">start UV position of the line</param>
    /// <param name="endUVPosition">End UV position of the line</param>
    /// <param name="startStampRotation">Rotation of stamp at the beginning</param>
    /// <param name="endStampRotation">Rotation of stamp at the end</param>
    /// <param name="color">Colour used to paint over - applied only if PaintMode of stamp is set to PaintOver</param>
    /// <param name="spacing">The smaller the value, the more dense the line is</param>
    public void DrawLine(Stamp stamp, Vector2 startUVPosition, Vector2 endUVPosition, float startStampRotation, float endStampRotation, Color color, float spacing)
    {
        Vector2 uvDistance = endUVPosition - startUVPosition;

        Vector2 pixelDistance = new Vector2(Mathf.Abs(uvDistance.x) * textureWidth, Mathf.Abs(uvDistance.y) * textureHeight);
        float stampDistance = stamp.Width > stamp.Height ? stamp.Height : stamp.Width;

        int stampsNo = Mathf.FloorToInt((pixelDistance.magnitude / stampDistance) / spacing) + 1;

        for (int i = 0; i <= stampsNo; i++)
        {
            float lerp = i / (float)stampsNo;

            Vector2 uvPosition = Vector2.Lerp(startUVPosition, endUVPosition, lerp);
            
            stamp.SetRotation(Mathf.Lerp(startStampRotation, endStampRotation, lerp));

            PaintOver(stamp, color, uvPosition);
        }
    }

    private void PaintOver(Stamp stamp, Color32 color, Vector2 uvPosition)
    {
        int paintStartPositionX = (int)((uvPosition.x * textureWidth) - stamp.Width / 2f);
		int paintStartPositionY = (int)((uvPosition.y * textureHeight) - stamp.Height / 2f);

        // Checking manually if int is bigger than 0 is faster than using Mathf.Clamp
        int paintStartPositionXClamped = paintStartPositionX;
        if (paintStartPositionXClamped < 0)
            paintStartPositionXClamped = 0;
        int paintStartPositionYClamped = paintStartPositionY;
        if (paintStartPositionYClamped < 0)
            paintStartPositionYClamped = 0;

        // Check manually if end position doesn't exceed texture size
        int paintEndPositionXClamped = paintStartPositionX + stamp.Width;
        if (paintEndPositionXClamped >= textureWidth)
            paintEndPositionXClamped = textureWidth - 1;
        int paintEndPositionYClamped = paintStartPositionY + stamp.Height;
        if (paintEndPositionYClamped >= textureHeight)
            paintEndPositionYClamped = textureHeight - 1;

        int totalWidth = paintEndPositionXClamped - paintStartPositionXClamped;
        int totalHeight = paintEndPositionYClamped - paintStartPositionYClamped;

		Color32 newColor = default(Color);
        Color32 textureColor;
        float alpha;
        int aChannel;
        
        for (int x = 0; x < totalWidth; x++)
        {
            for(int y = 0; y < totalHeight; y++)
            {
                int stampX = paintStartPositionXClamped - paintStartPositionX + x;
                int stampY = paintStartPositionYClamped - paintStartPositionY + y;

                alpha = stamp.CurrentPixels[stampX + stampY * stamp.Width];

                // There is no need to do further calculations if this stamp pixel is transparent
                if (alpha < 0.001f)
                    continue;

                int texturePosition = paintStartPositionXClamped + x + (paintStartPositionYClamped + y) * textureWidth;

                if (stamp.mode == PaintMode.Erase)
                    color = originalTexture[texturePosition];

                aChannel = (int)(alpha * 255f);

				textureColor = currentTexture[texturePosition];

                newColor.r = (byte)(color.r * aChannel / 255 + textureColor.r * textureColor.a * (255 - aChannel) / (255 * 255));
                newColor.g = (byte)(color.g * aChannel / 255 + textureColor.g * textureColor.a * (255 - aChannel) / (255 * 255));
                newColor.b = (byte)(color.b * aChannel / 255 + textureColor.b * textureColor.a * (255 - aChannel) / (255 * 255));
                newColor.a = (byte)(aChannel + textureColor.a * (255 - aChannel) / 255);

                currentTexture[texturePosition] = newColor;
            }
        }

        wasModified = true;
    }

    public void Clean()
    {
        Awake();
    }
}
