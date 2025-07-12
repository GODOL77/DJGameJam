using UnityEngine;
using Hsta;

public class UpAttackSpeed : MonoBehaviour
{
    //public TemporaryCardSpawner temporaryCardSpawner; // TemporaryCardSpawner 직접 참조

    public void UsingButton()
    {
         if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.attackSpeed += 0.2f;
                Debug.Log($"Attack Speed Up! New Attack Speed: {PlayerManager.Instance.attackSpeed}");
            }

TemporaryCardSpawner spawner = FindObjectOfType<TemporaryCardSpawner>();
        // TemporaryCardSpawner 오브젝트를 찾아 비활성화
        if (spawner != null)
        {
            spawner.gameObject.SetActive(false);
            Debug.Log("TemporaryCardSpawner deactivated.");
        }
        else
        {
            Debug.LogWarning("TemporaryCardSpawner reference is not set in UpAttackSpeed. Cannot deactivate.");
        }
    }
}