using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NekoScript : MonoBehaviour
{
    public Vector2 CutStart { private set; get; }
    public Vector2 CutEnd { private set; get; }

    [SerializeField]
    private GameObject sphereRed;

    //[SerializeField] private GameObject _emptyNekoPartPrefab;

    private List<GameObject> _spheresList;

    private SpriteRenderer spriteRenderer;

    private Texture2D originalTexture;
    private Texture2D currentTexture;
    // - for test
    private System.Diagnostics.Stopwatch watch;
    private int watchCounter = 0;



    private void Awake()
    {
        Debug.Log(string.Format("Neko Sliced: < {0} | {1} >", CutStart, CutEnd));


        spriteRenderer = GetComponent<SpriteRenderer>();

        //new texture instance
        //originalTexture = spriteRenderer.sprite.texture;
        //currentTexture = new Texture2D(originalTexture.width, originalTexture.height);
        //currentTexture.SetPixels(originalTexture.GetPixels());
        //currentTexture.Apply();

        //spriteRenderer.sprite = Sprite
        //    .Create(currentTexture, GetComponent<SpriteRenderer>().sprite.rect, Vector2.one / 2);

        watch = new System.Diagnostics.Stopwatch();
        watch.Start();
    }

    private void Start()
    {
        _spheresList = new List<GameObject>();
    }
    
    //public void SetStartCutLocalPos(Vector3 bladePos)
    //{
    //    CutStart = bladePos - transform.localPosition;
    //}

    //public void SetEndCutLocalPos(Vector3 bladePos)
    //{
    //    CutEnd = bladePos - transform.localPosition;

    //    // Instance red points on Start/End pos
    //    Debug.Log(string.Format("Neko Sliced: < {0} | {1} >", CutStart, CutEnd));

    //    SliceNeko();
    //}



    private void DeleteSpheres()
    {
        foreach(var sphere in _spheresList)
        {
            Destroy(sphere);
        }

        _spheresList.Clear();
    }

    private void CreateSphere(Vector2 position)
    {
        var obj = Instantiate(sphereRed, transform);
        obj.transform.localPosition = position;
        _spheresList.Add(obj);
    }

    private void ShowCutPoints()
    {
        DeleteSpheres();

        CreateSphere(CutStart);
        CreateSphere(CutEnd);
    }

    private void SliceNeko()
    {
        PrintTimeFromLastTimestamp(); //1 ~400ms
        var cuttedTexture = CutSprite(spriteRenderer.sprite);
        //var cuttedTexture = new List<Sprite> { spriteRenderer.sprite, spriteRenderer.sprite }; //test
        PrintTimeFromLastTimestamp(); //2~55ms

        var gameObj1 = CreateNekoPart(cuttedTexture[0]);
        var gameObj2 = CreateNekoPart(cuttedTexture[1]);
        PrintTimeFromLastTimestamp(); //3

        //var punchVec = (CutStart - CutEnd) * -1; //same 
        var punchVec = CutEnd - CutStart;
        gameObj1.GetComponent<Rigidbody2D>().AddForce(punchVec * 100);
        gameObj2.GetComponent<Rigidbody2D>().AddForce(punchVec * 100);
        PrintTimeFromLastTimestamp(); //4

        Destroy(gameObject);
        PrintTimeFromLastTimestamp(); //5
        watchCounter = 0;
    }

    //test returns List [part1, part2];
    private GameObject CreateNekoPart(Sprite sprite, string objName = "PartOfNeko")
    {
        var gameObj = new GameObject(objName //OLD
            , typeof(SpriteRenderer)
            , typeof(Rigidbody2D));

        //var gameObj = Instantiate(_emptyNekoPartPrefab);

        //--- 
        gameObj.transform.localScale = new Vector3(0.5f, 0.5f);
        gameObj.transform.position = this.transform.position;
        gameObj.layer = 7;

        //gameObj.GetComponent<SpriteRenderer>().sprite = test(sr.sprite);
        gameObj.GetComponent<SpriteRenderer>().sprite = sprite;

        gameObj.AddComponent<PolygonCollider2D>();

        //gameObj.AddComponent<NekoScript>(); //?? Creating Empty obj

        return gameObj;
    }

    private float LineFuncPixel(float x, float y, Vector2 pixelStart, Vector2 pixelEnd)
    {
        return (pixelStart.y - pixelEnd.y) * x
            + (pixelEnd.x - pixelStart.x) * y
            + (pixelStart.x * pixelEnd.y - pixelEnd.x * pixelStart.y);
    }

    private Texture2D CopyTexture(Texture2D textureOriginal)
    {
        var texture = new Texture2D(textureOriginal.width, textureOriginal.height);
        texture.SetPixels(textureOriginal.GetPixels());
        texture.Apply();

        return texture;
    }

    private Texture2D CreateTextureWithColors32(int width, int height, Color32[] colors)
    {
        if (width * height != colors.Length)
            throw new System.Exception("Incorect parametres!");

        var texture = new Texture2D(width, height);
        texture.SetPixels32(colors);
        texture.Apply();

        return texture;
    }

    // ?? maybe not working
    // NOT WORKING !!
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

    private List<Sprite> CutSprite(Sprite sprite) //test() 
    {
        //var texture1 = CopyTexture(sprite.texture);
        //var texture2 = CopyTexture(sprite.texture);

        PrintTimeFromLastTimestamp(" CutSprite Start;");

        var height = sprite.texture.height;
        var width = sprite.texture.width;
        var sumPixels = height * width;

        Vector2 bounds = sprite.bounds.extents;
        Vector2 pixelCoefStart = (CutStart + bounds) / (bounds * 2);
        Vector2 pixelCoefEnd = (CutEnd + bounds) / (bounds * 2);

        Vector2 pixelStart = new Vector2(width * pixelCoefStart.x, height * pixelCoefStart.y);
        Vector2 pixelEnd = new Vector2(width * pixelCoefEnd.x, height * pixelCoefEnd.y);
        Debug.Log(string.Format("pixel: < {0} | {1} >", pixelStart, pixelEnd));

        PrintTimeFromLastTimestamp(" Test zone");
        //-- Test zone
        var colors1 = sprite.texture.GetPixels32();
        Color32[] colors2 = new Color32[sumPixels];
        colors1.CopyTo(colors2, 0);

        Debug.Log("colors1: " + colors2.Length);
        Debug.Log("sumPixels: " + sumPixels);

        Color32 Color32clear = new Color32(0, 0, 0, 0);
        int pixelsCounter = 0;

        PrintTimeFromLastTimestamp("  НОВЫЙ Тестовый ");
        //% НОВЫЙ Тестовый 
        for (int i = 0; i < colors1.Length; ++i)
        {
            var y = i / width;
            var x = i % width;
            if ((LineFuncPixel(x, y, pixelStart, pixelEnd)) <= 0)
            {
                colors1[i] = Color32clear;
                pixelsCounter++;
            }
            else
            {
                colors2[i] = Color32clear;
            }
        }
        Debug.Log("pixelsCounter: " + pixelsCounter);
        //------------------

        PrintTimeFromLastTimestamp("set textures");
        var texture1 = CreateTextureWithColors32(width, height, colors1);
        var texture2 = CreateTextureWithColors32(width, height, colors2);
        //-----
        PrintTimeFromLastTimestamp(" before RETURN ");
        return new List<Sprite> {
            Sprite.Create(texture1, sprite.rect, Vector2.one / 2),
            Sprite.Create(texture2, sprite.rect, Vector2.one / 2) }; //te2
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BladeScript component;
        collision.TryGetComponent(out component);

        if (component != null)
        {            
            CutStart = LocalScalingVec(collision.transform.position - transform.localPosition);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BladeScript component;
        collision.TryGetComponent(out component);

        if (component != null)
        {
            CutEnd = LocalScalingVec(collision.transform.position - transform.localPosition);

            if (IsCuttingAvailable())
            {
                SliceNeko();
                //ShowCutPoints(); //for debuging
            }
        }
    }

    private bool IsCuttingAvailable()
    {
        // FOR FUTURE

        // add checking on Length
        // or if startCut Vector negative
        // cutEnd should be positive like (strat -5, 2 ; end 6, 2)
        if (Vector3.Equals(CutStart, CutEnd) || CutStart == null || CutEnd == null)
        {
            return false;
        }

        return true;
    }
    
    private Vector3 LocalScalingVec(Vector3 vec)
    {
        return new Vector3(
                vec.x / transform.localScale.x,
                vec.y / transform.localScale.y,
                vec.z / transform.localScale.z);
    }

    private void PrintTimeFromLastTimestamp(string message = "")
    {
        Debug.Log(">> " + watchCounter + ": " + watch.ElapsedMilliseconds + message);
        watch.Reset();
        watch.Start();
        watchCounter++;
    }

}
