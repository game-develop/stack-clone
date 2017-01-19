using UnityEngine;
using System.Collections;

public class ColorsUtil {

    private static Color32[] stackColors = {
                                               new Color32(251, 15, 15, 0),
                                               new Color32(9, 0, 251, 0),
                                               new Color32(0, 240, 12, 0),
                                               new Color32(24, 255, 236, 0)
                                           };

    public static void ColorMesh(Mesh mesh, int number)
    {
        Color32[] colors = new Color32[mesh.vertices.Length];
        float f = Mathf.Sin(number * 0.05f);
        for (int i = 0; i < colors.Length; ++i)
        {
            colors[i] = Lerp4(stackColors[0], stackColors[1], stackColors[2], stackColors[3], f);
            Debug.Log(colors[i]);
        }
        mesh.colors32 = colors;
    }

    public static Color32 Lerp4(Color32 a, Color32 b, Color32 c, Color32 d, float f)
    {
        if (f < 0.33f)
        {
            return Color.Lerp(a, b, f / 0.33f);
        }

        if (f < 0.66f)
        {
            return Color.Lerp(b, c, (f - 0.33f) / 0.33f);
        }

        return Color.Lerp(c, d, (f - 0.66f) / 0.33f);
    }

}
