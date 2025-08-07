using UniRx;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - player.position;

        Observable.EveryLateUpdate().Subscribe(_ => { IdleCameraMovement(); });
    }

    void IdleCameraMovement()
    {
        Vector3 _newPosition = new(offset.x + player.position.x, transform.position.y, offset.z + player.position.z);
        transform.position = _newPosition;
    }
    
}
