using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private float platformScaleVariance = 0.3f;

    [SerializeField] private float distanceUp = 0.25f;
    [SerializeField] private float secondsBtwnMoveUp = 2f;
    [SerializeField] private float maxHeight = 10f;

    [SerializeField] private float circleRadius = 5f;
    [SerializeField] private float secondBtwnGenerate = 4f;

    private float timeLastGenerated;
    private bool firstTime = true;

    private Queue<GameObject> platforms;
    
    // Start is called before the first frame update
    void Start()
    {
        platforms = new Queue<GameObject>();
        StartCoroutine(MovePlatformsUp());
        StartCoroutine(GeneratePlatform());
        //StartCoroutine(RotatePlatforms(3f));


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    IEnumerator RotatePlatforms(float angle)
    {
        foreach (var platform in platforms)
        {
            var pos = platform.transform.position;
            platform.transform.position = new Vector3(pos.x + Mathf.Cos(angle) * circleRadius, pos.y, pos.z + Mathf.Sin(angle) * circleRadius);
        }
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(RotatePlatforms(angle));
    }
    
    IEnumerator MovePlatformsUp()
    {
        int dequeue = 0;
        if (platforms != null && platforms.Count > 0)
        {
            foreach (var platform in platforms)
            {
                platform.transform.position += Vector3.up * distanceUp;
                if (platform.transform.position.y > maxHeight)
                {
                    dequeue += 1;
                    Destroy(platform.gameObject);
                }
            }
        }

        for (int i = 0; i < dequeue; ++i)
        {
            platforms.Dequeue();
        }
        
        yield return new WaitForSeconds(secondsBtwnMoveUp);

        StartCoroutine(MovePlatformsUp());
    }

    IEnumerator GeneratePlatform()
    {
        var pos = Random.insideUnitCircle.normalized * circleRadius;
        var platform = Instantiate(platformPrefab);
        
        platform.transform.position = new Vector3(pos.x, 0, pos.y);
        var platformScaleVariance = Random.Range(-this.platformScaleVariance, this.platformScaleVariance);
        platform.transform.localScale *= (1f + platformScaleVariance);
        
        platforms.Enqueue(platform);

        var generateTimeVariance = Random.Range(-1f, 1f) * secondBtwnGenerate * 0.3f;
        yield return new WaitForSeconds(secondBtwnGenerate + generateTimeVariance);

        StartCoroutine(GeneratePlatform());
    }
}
