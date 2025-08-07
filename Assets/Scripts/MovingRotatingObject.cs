using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MovingRotatingObject : MonoBehaviour
{
    [SerializeField] private float movingValueByY;
    [SerializeField] private float movingDuration;
    [SerializeField] private float rotationDuration;
    void Start()
    {
        MoveObject();
        RotateObject();
    }

    private void MoveObject()
    {
        transform.DOMoveY(movingValueByY, movingDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void RotateObject()
    {
        transform.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
}
