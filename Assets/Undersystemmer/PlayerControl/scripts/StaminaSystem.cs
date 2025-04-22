using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;
    public float staminaDrainRate = 20f;

    [Header("References")]
    public Image staminaBarImage; // Change from Slider to Image

    void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaUI();
    }

    void Update()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;

        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }

        UpdateStaminaUI();
    }

    void UpdateStaminaUI()
    {
        if (staminaBarImage != null)
            staminaBarImage.fillAmount = currentStamina / maxStamina;
    }

    public bool HasStamina(float amount)
    {
        return currentStamina >= amount;
    }

    public void UseStamina(float amount)
    {
        currentStamina = Mathf.Max(currentStamina - amount, 0f);
        UpdateStaminaUI();
    }
}