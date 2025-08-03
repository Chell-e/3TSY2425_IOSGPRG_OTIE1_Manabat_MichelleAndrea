using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Seek,
    Attack
}

public class EnemyFSM : MonoBehaviour
{
    [SerializeField] EnemyState currState;
    [SerializeField] public Human target;

    float waitTime = 2f;
    float idleTime = 0f;

    Vector2 randomPos = Vector2.zero;
    bool isMoving = false;

    private void Update()
    {
        switch(currState)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Wander:
                WanderUpdate();
                break;
            case EnemyState.Seek:
                SeekUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
        }
    }

    void ChangeState(EnemyState state)
    {
        currState = state;
    }

    void IdleUpdate() 
    {
        if (target != null)
        {
            ChangeState(EnemyState.Seek);
            idleTime = 0f;
            return;
        }

        idleTime += Time.deltaTime;

        if (idleTime > waitTime)
        {
            ChangeState(EnemyState.Wander);
            idleTime = 0f;
        }
    }

    void WanderUpdate() 
    {
        if (target != null)
        {
            ChangeState(EnemyState.Seek);
            isMoving = false;
            return;
        }

        if (!isMoving)
        {
            randomPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            isMoving = true;
        }
        else
        {
            Vector3 look = transform.InverseTransformPoint(randomPos);
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90;
            transform.Rotate(0, 0, angle);

            this.transform.position = Vector2.MoveTowards(this.transform.position, randomPos, 5 * Time.deltaTime);
        }

        if (Vector2.Distance(this.transform.position, randomPos) < 1)
        {
            ChangeState(EnemyState.Idle);
            isMoving = false;
        }
    }

    void SeekUpdate() 
    {
        if (target == null)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        Vector3 targetPos = target.transform.position;
        Vector3 look = transform.InverseTransformPoint(targetPos);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90;
        transform.Rotate(0, 0, angle);

        this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, 5 * Time.deltaTime);
    }

    void AttackUpdate() 
    {

    }


}
