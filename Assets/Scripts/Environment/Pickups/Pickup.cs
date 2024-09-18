using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Attracting Item Range and Speed")]
    [SerializeField] private float _attractItemRange = 5f;
    [SerializeField] private float _accelerationRate = .2f;
    [SerializeField] private float _attractSpeed = .1f;
    
    [Header("Jumping Animation Curve")]
    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private float _heightY = .1f;
    [SerializeField] private float _duration = 1f;
    
    [Header("Type of Pickup")]
    [SerializeField] private PickUpTypes _pickUpType;
    
    private Vector2 _moveDir;
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        StartCoroutine(ItemSpawnPopUpRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        
        if (Vector3.Distance(transform.position, playerPos) < _attractItemRange)
        {
            _moveDir = (playerPos - transform.position).normalized;
            _attractSpeed += _accelerationRate; 
        }
        else
        {
            Vector2 _moveDir =Vector2.zero;
            _attractSpeed = 0f;
        }
        
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveDir * (_attractSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            DetectPickUpType();
            Destroy(gameObject);
        }
    }
    
    public IEnumerator ItemSpawnPopUpRoutine()
    {
        Vector2 randomEndPos = RandomizeSpawnAxis();
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + randomEndPos;
        
        float timePassed = 0f;
        
        while (timePassed < _duration)
        {
            timePassed += Time.deltaTime;
            float linearTime = timePassed / _duration;
            float heightCurveTime = _animCurve.Evaluate(linearTime);
            float height = Mathf.Lerp(0f, _heightY, heightCurveTime);
            transform.position = Vector2.Lerp(startPos, endPos, linearTime) + new Vector2(0f, height);
            
            yield return null;
        }
    }

    private Vector2 RandomizeSpawnAxis()
    {
        int randomizerX = UnityEngine.Random.Range(-2, 2);
        int randomizerY = UnityEngine.Random.Range(-2, 2);
        
        return new Vector2(randomizerX, randomizerY);
    }

    private void DetectPickUpType()
    {
        switch (_pickUpType)
        {
            case PickUpTypes.HealthOrb:
                PlayerHealth.Instance.HealPlayer(1);
                break;
            case PickUpTypes.StaminaOrb:
                PlayerStamina.Instance.RefreshStamina(1);
                break;
            case PickUpTypes.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold(1);
                break;
            default:
                Debug.Log("In class Pickup no Pickup Type was detected, check the Pickup Type enum for errors.");
                break;
        }
    }
    
    private enum PickUpTypes
    {
        HealthOrb,
        StaminaOrb,
        GoldCoin
    }
}
