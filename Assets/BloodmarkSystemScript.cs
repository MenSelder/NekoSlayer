using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Slicer2D;

public class BloodmarkSystemScript : MonoBehaviour
{
    /*
     * Make it as visual 
     * on event in blade slash
     * like OnBladeStrke event
     * 
     * separate by 
     * 1) instantiate partices
     * 2) make drips on face
     */
    public static BloodmarkSystemScript Instance { get; private set; }

    [SerializeField] private GameObject[] bloodSplats; //remake with Sprites ??
    [SerializeField] private GameObject[] bloodSprays; //still not usage -> delete
    //use it for making splats on the background (with splats on face)
    [SerializeField] private Sprite[] bloodSpraySprites;

    [SerializeField] private GameObject face; // rename like "on face splats holder..." ?

    //make small blood partice
    //which instantiate along the cut

    [SerializeField] private ParticleSystem particleBlood; //big particle onSliced
    
    //implement Sprite Merger for it!!!
    [SerializeField] public Transform roomBoundsSplatsHolder;
    [SerializeField] public Transform backgroundSplatsHolder;
    
    //[SerializeField] GameObject blade;

    [SerializeField, Range(0, 5f)] float disappearanceSpeed;        
    [SerializeField] float intensivity; //13
    [SerializeField] float errorCoef; //1f
    [SerializeField, Range(0, 0.1f)] float timeIntervalBtweenSplats; //0f


    public GameObject[] BloodSplats => bloodSplats;
    public GameObject[] BloodSprays => bloodSprays;
    public Transform RoomBoundsSplatsHolder => roomBoundsSplatsHolder;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //JobHandle job?? (unnecessary with it)
        foreach (SpriteRenderer splashSpriteRender in face.GetComponentsInChildren(typeof(SpriteRenderer)))
        {
            //Debug.Log(splashSpriteRender);

            if (splashSpriteRender.color.a <= 0)
            {
                Destroy(splashSpriteRender.gameObject);
                continue;
            }

            splashSpriteRender.color = new Color(splashSpriteRender.color.r, splashSpriteRender.color.g, splashSpriteRender.color.b, splashSpriteRender.color.a - Time.deltaTime * disappearanceSpeed);
        }
    }

    public void OnSlicing(Slice2D slice)
    {
        var bladeSegment = slice.slice;

        // Drawing splats on face
        var pointsList = GenerateIntermediatePoints(bladeSegment[0].ToVector2(), bladeSegment[1].ToVector2());
        StartCoroutine(InstantiateSplats(pointsList, timeIntervalBtweenSplats));

        //Particle
        var directionVector = bladeSegment[1].ToVector2() - bladeSegment[0].ToVector2();

        var position = slice.originGameObject.transform.position;
        var particle = Instantiate(particleBlood, position, Quaternion.LookRotation(directionVector));
        particle.Play();

        BloodSpraysOnBackground(pointsList);
    }

    private void BloodSpraysOnBackground(List<Vector2> pointsList)
    {
        //splats on background (bg.layerOrder = -10)
        foreach (var positionPoint in pointsList)
        {
            string splatName = "backgrondSplat";
            var newSplatGameObject = new GameObject(splatName, typeof(SpriteRenderer));
            var spriteRenderer = newSplatGameObject.GetComponent<SpriteRenderer>();

            var spriteRandom = bloodSpraySprites[Random.Range(0, bloodSpraySprites.Length)];
            spriteRenderer.sprite = spriteRandom;

            int sortingOrder = -9;
            spriteRenderer.sortingOrder = sortingOrder;

            newSplatGameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            newSplatGameObject.transform.SetParent(backgroundSplatsHolder);

            newSplatGameObject.transform.position = positionPoint;

            float size = 0.1f;
            Vector3 splatSize = new Vector3(size, size, size);
            newSplatGameObject.transform.localScale = splatSize;

            Color color = Color.red;
            spriteRenderer.color = color;
        }
    }

    private List<Vector2> GenerateIntermediatePoints(Vector2 a, Vector2 b)
    {
        var distance = Vector2.Distance(a, b);
        //float intensivity = 13f;
        float delta = distance / intensivity;
        float step = 1f / intensivity;

        List<Vector2> pointsList = new List<Vector2>();

        float i = 0;
        while(i < 1)
        {
            var newPoint = Vector2.Lerp(a, b, i);
            var pointError = GenPointError(delta);
            newPoint += pointError * errorCoef; // TURN ON (TEST)

            pointsList.Add(newPoint);
            i += step;
        }

        return pointsList;
    }

    private Vector2 GenPointError(float range)
    {
        Vector2 point = new Vector2(Random.Range(-range / 2, range / 2), Random.Range(-range / 2, range / 2));
        return point.normalized; // is it should be normalized?
    }

    private IEnumerator InstantiateSplats(List<Vector2> pointsList, float intervalSeconds)
    {
        foreach (var positionPoint in pointsList)
        {
            var obj = bloodSplats[Random.Range(0, bloodSplats.Length)]; //rnd obj from bloodSlashes list

            Instantiate(obj, positionPoint, Quaternion.Euler(0, 0, Random.Range(0, 360f)), face.transform);
            yield return new WaitForSeconds(intervalSeconds);
        }
    }
}
