using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionScript : MonoBehaviour
{
    [SerializeField] private float size; // 0.1
    [SerializeField] private int sortingOrder;// 1;

    private List<ParticleCollisionEvent> eventsList = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        GameObject[] splats = BloodmarkSystemScript.Instance.BloodSplats;

        gameObject.GetComponent<ParticleSystem>().GetCollisionEvents(other, eventsList);

        foreach(var collisionEvent in eventsList)
        {
            //remake this with sprite Merger!
            // some like: SplatScript.AddSplite(.. instantiate params ..)
            GameObject splat = Instantiate(splats[Random.Range(0, splats.Length)]
                , collisionEvent.intersection
                , Quaternion.Euler(0, 0, Random.Range(0, 360f))
                , BloodmarkSystemScript.Instance.RoomBoundsSplatsHolder);

            splat.transform.localScale = new Vector3(size, size, size);

            var sr = splat.GetComponent<SpriteRenderer>();
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            sr.sortingOrder = sortingOrder;
        }
    }
}
