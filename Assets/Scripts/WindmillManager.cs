using DG.Tweening;
using UnityEngine;

public class WindmillManager : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    void Start()
    {
        Vector3 currentRotation = transform.eulerAngles;
        transform.DORotate(new Vector3(currentRotation.x, currentRotation.y, currentRotation.z + 360), rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
}
