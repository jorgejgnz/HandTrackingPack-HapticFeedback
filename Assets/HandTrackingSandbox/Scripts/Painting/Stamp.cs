using UnityEngine;

public enum PaintMode : byte
{
    Draw,
    Erase
}

public class Stamp
{
    private float[] Pixels;
    public float[] CurrentPixels;

    public int Width;
    public int Height;

    public PaintMode mode = PaintMode.Draw;

    private float currentAngle = 0f;

    public Stamp(Texture2D stampTexture)
    {
		Width = stampTexture.width;
		Height = stampTexture.height;

		Pixels = new float[Width * Height];
        CurrentPixels = new float[Width * Height];

        float alphaValue;

		for(int x = 0; x < Width; x++)
		{
			for(int y = 0; y < Height; y++)
			{
                alphaValue = stampTexture.GetPixel(x, y).a;
                Pixels[x + y * Width] = alphaValue;
                CurrentPixels[x + y * Width] = alphaValue;
            }
		}
    }

    public void SetRotation(float targetAngle)
    {
        if (targetAngle != currentAngle)
        {
            float sin = Mathf.Sin(Mathf.Deg2Rad * targetAngle);
            float cos = Mathf.Cos(Mathf.Deg2Rad * targetAngle);

            float x0 = Width / 2f;
            float y0 = Height / 2f;

            float deltaX, deltaY;

            int xp, yp;

            float rotatedPixelValue;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    deltaX = x - x0;
                    deltaY = y - y0;

                    xp = (int)(deltaX * cos - deltaY * sin + x0);
                    yp = (int)(deltaX * sin + deltaY * cos + y0);

                    if (xp >= 0 && xp < Width && yp >= 0 && yp < Height)
                        rotatedPixelValue = Pixels[xp + Width * yp];
                    else
                        rotatedPixelValue = 0f;

                    CurrentPixels[x + Width * y] = rotatedPixelValue;
                }
            }

            currentAngle = targetAngle;
        }
    }
}
