using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _heightY = 3.5f;
    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private GameObject _projectileShadow;
    [SerializeField] private GameObject _grapeSplatterPrefab;
    
    void Start()
    {
        GameObject grapeShadow = 
            Instantiate(_projectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
        Vector3 shadowStartPos = grapeShadow.transform.position;
        
        
        Vector3 playerPos = PlayerController.Instance.transform.position;
        StartCoroutine(ProjectileTrajectoryRoutine(transform.position, playerPos));
        StartCoroutine(ShadowFollowProjectileRoutine(grapeShadow, shadowStartPos, playerPos));
    }

    private IEnumerator ProjectileTrajectoryRoutine(Vector3 startPos, Vector3 endPos)
    {
        float timePassed = 0;
        
        while (timePassed < _duration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / _duration;
            float heightCurveTime = _animCurve.Evaluate(linearTime); // References and adjust to whatever curve we have set in the inspector
            float height = Mathf.Lerp(0f, _heightY, heightCurveTime);
            transform.position = Vector2.Lerp(startPos, endPos, linearTime) + new Vector2(0f, height);
            
            yield return null;
        }
        
        Instantiate(_grapeSplatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    private IEnumerator ShadowFollowProjectileRoutine(GameObject shadow, Vector3 startPos, Vector3 endPos)
    {
        float timePassed = 0;
        
        while (timePassed < _duration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / _duration;
            shadow.transform.position = Vector2.Lerp(startPos, endPos, linearTime);
            
            yield return null;
        }
        
        Destroy(shadow);
    }
}
