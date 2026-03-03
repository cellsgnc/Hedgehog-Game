using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("dusman Ayarlari")]

    public Transform[] PatrolPoints;
    public short TargetPoint;
    public float speed;
    void Start()
    {
        TargetPoint = 0;
    }

    void Update()
    {
        if (PatrolPoints == null || PatrolPoints.Length == 0) return;

        float distance = Vector2.Distance(transform.position, PatrolPoints[TargetPoint].position);

        if (distance < 0.1f) 
        {
            MoveNextPoint();
        }
        transform.position = Vector2.MoveTowards(transform.position, PatrolPoints[TargetPoint].position, speed * Time.deltaTime);
    }

    void MoveNextPoint()
    {
        TargetPoint++;
        if (TargetPoint >= PatrolPoints.Length) TargetPoint = 0;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
           
            if (player.isDashing)
            {
                player.DashCombo();

                Die();
            }
            else
            {
                GameManager.instance.RestartGame();
            }
        }
    }
 
    void Die()
    {
        Destroy(gameObject);
    }


}
  
