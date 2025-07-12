using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpXPCard : MonoBehaviour
{
    public void UsingButton()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.plusXP+=2; // 경험치 2 추가
            //Debug.Log($"Gained 2 additional XP. Current XP: {PlayerManager.Instance.playerXP}");
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