using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStamina : Singleton<PlayerStamina>
{
   [SerializeField] private Sprite _fullStaminaSpriteOrb, _emptyStaminaSpriteOrb;
   [SerializeField] private int _staminaRegen = 10;
   public int CurrentStamina { get; private set; }

   private Transform _staminaOrbs;
   const string STAMINA_ORB_TEXT = "Stamina_Orb_Container";

   private int _initalStamina = 3;
   private int _maxStamina;

   private void Start()
   {
      _staminaOrbs = GameObject.Find(STAMINA_ORB_TEXT).transform;
   }

   protected override void Awake()
   {
      base.Awake();
      
      _maxStamina = _initalStamina;
      CurrentStamina = _initalStamina;
   }
   
   public void UseStamina(int amount)
   {
      if (CurrentStamina - amount >= 0)
      {
         CurrentStamina -= amount;
         UpdateStaminaUIImages();
         StopAllCoroutines();
         StartCoroutine(StaminaRegenRoutine());
      }
   }

   public void RefreshStamina(int amount)
   {
      if (CurrentStamina < _maxStamina && !PlayerHealth.Instance.IsDead)
      {
         CurrentStamina += amount;
         UpdateStaminaUIImages();
      }
   }

   public void UpdateStaminaUIImages()
   {
      for (int i = 0; i < _maxStamina; i++)
      {
         Transform child = _staminaOrbs.GetChild(i);
         Image image = child?.GetComponent<Image>();
         
         if (i <= CurrentStamina - 1)
            image.sprite = _fullStaminaSpriteOrb;
         else
            image.sprite = _emptyStaminaSpriteOrb;
      }

      // if (CurrentStamina < _maxStamina)
      // {
      //    StopAllCoroutines();
      //    StartCoroutine(StaminaRegenRoutine());
      // }
   }

   public void RefillStaminaOnPlayerDeath()
   {
      CurrentStamina = _initalStamina;
      UpdateStaminaUIImages();
   }
   private IEnumerator StaminaRegenRoutine()
   {
      while (CurrentStamina < _maxStamina)
      {
         yield return new WaitForSeconds(_staminaRegen);
         RefreshStamina(1);
      }
   }
}
