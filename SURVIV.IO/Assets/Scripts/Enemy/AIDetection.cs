using UnityEngine;

public class AIDetection : MonoBehaviour
{
    // set up detection here

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Human>() != null)
        {
            this.GetComponentInParent<EnemyFSM>().target = other.gameObject.GetComponent<Human>();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Human>())
        {
            this.GetComponentInParent<EnemyFSM>().target = null;
        }
    }
}
