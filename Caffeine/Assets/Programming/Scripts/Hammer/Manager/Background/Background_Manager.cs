using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Manager : MonoBehaviour
{
    public float groundAddPosition = 20.0f;
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        if (GameObject.FindWithTag("Player") != null)
        {
            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
            Vector3 myPos = transform.position;
            float diffx = Mathf.Abs(playerPosition.x - myPos.x);
            float diffy = Mathf.Abs(playerPosition.y - myPos.y);

            Vector3 playerDir = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().moveDirection;
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;

            switch (transform.tag)
            {
                case "Ground":
                    if (diffx > diffy)
                    {
                        transform.Translate(Vector3.right * dirX * groundAddPosition);
                    }
                    else if (diffx < diffy)
                    {
                        transform.Translate(Vector3.up * dirY * groundAddPosition);
                    }
                    break;
                case "Enemy":
                    
                    break;
            }
        }
    }
}
