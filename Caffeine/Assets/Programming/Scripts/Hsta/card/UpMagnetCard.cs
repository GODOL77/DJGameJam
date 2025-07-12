using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpMagnetCard : MonoBehaviour
{
     public void UsingButton()
    {
        if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.attractionRange += 0.3f;
                Debug.Log($"Magnet Range Up! New Range: {PlayerManager.Instance.attractionRange}");
            }

            // TemporaryCardSpawner 오브젝트를 찾아 비활성화
            TemporaryCardSpawner spawner = FindObjectOfType<TemporaryCardSpawner>();
            if (spawner != null)
            {
                spawner.gameObject.SetActive(false);
                Debug.Log("TemporaryCardSpawner deactivated by UpMagnetCard.");
            }
            else
            {
                Debug.LogWarning("TemporaryCardSpawner not found in the scene. Cannot deactivate from UpMagnetCard.");
            }
    }
}
