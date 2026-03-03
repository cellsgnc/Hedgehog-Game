using UnityEngine;

public class Killer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            Kill();
        }
    }

void Kill()
{
        GameManager.instance.RestartGame();
}

}
