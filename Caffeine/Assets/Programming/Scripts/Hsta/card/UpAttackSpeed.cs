using UnityEngine;
using Hsta;

public class UpAttackSpeed : MonoBehaviour
{
    void UsingButton()
    {
         if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.attackSpeed += 0.2f;
                Debug.Log($"Attack Speed Up! New Attack Speed: {PlayerManager.Instance.attackSpeed}");
            }
    }
}


