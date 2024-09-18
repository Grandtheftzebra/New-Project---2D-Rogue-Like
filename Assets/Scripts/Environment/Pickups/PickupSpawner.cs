using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject _goldCoin, _healthOrb, _staminaOrb;
    [SerializeField] private float _spawnCountWhenLucky = 3f;

    public void SpawnItem()
    {
        int randomNum = Random.Range(0, 3);
        
        switch (randomNum)
        {
            case 0:
                Instantiate(_healthOrb, transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(_staminaOrb, transform.position, Quaternion.identity);
                break;
            case 2:
                int getLucky = Random.Range(1, 7);
                
                if (getLucky == 6)
                    for (int i = 0; i < _spawnCountWhenLucky; i++)
                        Instantiate(_goldCoin, transform.position, Quaternion.identity);
                else
                    Instantiate(_goldCoin, transform.position, Quaternion.identity);
                
                break;
        }
    }
}
