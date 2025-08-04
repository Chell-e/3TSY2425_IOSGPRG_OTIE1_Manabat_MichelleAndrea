using UnityEngine;

public class FloatingEnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        Camera camera = GetComponent<Camera>();
    }

    private void Update()
    {
        this.transform.rotation = camera.transform.rotation;
        this.transform.position = target.transform.position + offset;
    }
}
