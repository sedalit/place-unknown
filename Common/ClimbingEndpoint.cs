using UnityEngine;
using DG.Tweening;

public class ClimbingEndpoint : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void OnTriggerEnter(Collider other) => other.transform.DOMove(target.position, 0.5f);

}
