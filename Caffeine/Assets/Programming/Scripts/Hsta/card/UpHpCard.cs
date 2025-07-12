using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpHpCard : MonoBehaviour
{
    public void UsingButton()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.maxHealth += 20;
            PlayerManager.Instance.currentHealth += 20;
            Debug.Log($"Attack Speed Up! New Attack Speed: {PlayerManager.Instance.attackSpeed}");
        }
            
             // TemporaryCardSpawner 오브젝트를 찾아 비활성화
        TemporaryCardSpawner spawner = FindObjectOfType<TemporaryCardSpawner>();
        if (spawner != null)
        {
            spawner.gameObject.SetActive(false);
            Debug.Log("TemporaryCardSpawner deactivated.");
        }
        else
        {
            Debug.LogWarning("TemporaryCardSpawner not found in the scene.");
        }
    }
}
