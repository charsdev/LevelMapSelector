using UnityEngine;

public abstract class Node : MonoBehaviour
{
    [HideInInspector] public Vector3 position;

    protected void Awake()
    {
        position = transform.position;
    }
}
