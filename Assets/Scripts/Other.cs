using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other
{
    /*
     * MATIRX Convert 
     */
    private Color32[,] ConvertColorsToMatrix(int width, int height, Color32[] colors)
    {
        if (width * height != colors.Length)
            throw new System.Exception("Incorect parametres!");

        var matrix = new Color32[height, width];
        var counter = 0;
        //----
        //for(int y = height - 1; y > 0; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        matrix[y, x] = colors[counter];
        //        counter++;
        //    }
        //}
        //----
        for (int y = 0; y < height; y++) //need testing
        {
            for (int x = 0; x < width; x++)
            {
                matrix[y, x] = colors[counter];
                counter++;
            }
        }

        return matrix;
    }

    private Color32[] ConvertMatrixToColors(Color32[,] matrix)
    {
        var colors = new Color32[matrix.Length];
        int counter = 0;
        foreach (var item in matrix)
        {
            colors[counter] = item;
            counter++;
        }

        return colors;
    }

    private bool ConverterTest(int w, int h, Color32[] colors)
    {
        var matrix = ConvertColorsToMatrix(w, h, colors);
        var colorsTest = ConvertMatrixToColors(matrix);

        Debug.Log("---- test ---- ");
        Debug.Log("Length: 1: " + colors.Length + "2: " + colorsTest.Length);
        if (colors.Length != colorsTest.Length)
            return false;

        for (int i = 0; i < colors.Length; i++)
        {
            if (!Color32.Equals(colors[i], colorsTest[i]))
            {
                Debug.Log("ERROR! on pixel: " + i);
                return false;
            }
        }

        return true;
    }
}
