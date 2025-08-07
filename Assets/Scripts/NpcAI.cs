using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class NpcAI : MonoBehaviour
{
    [SerializeField] private PlayerHarvestingManager playerHarvestingManager;
    [SerializeField] private GameResources gameResources;
    [SerializeField] private Vector3[] wayPoint;
    [SerializeField] private PathType pathType;
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform startRay;
    [SerializeField] private float rayDistance;
    private Animator anim;
    private CapsuleCollider capsuleCollider;
    private Tween walkingTween;
    [SerializeField] private bool isMoving; 
    [SerializeField] private bool isDetectingPlayer;
    [SerializeField] private bool isNPC;
    private bool hasScheduledMove = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            ObjectDetection(); 
        });
    }

    void ObjectDetection()
    {
        RaycastHit hit;

        bool playerHit = Physics.Raycast(startRay.position, Vector3.left, out hit, rayDistance, LayerMask.GetMask("Player"));
        isDetectingPlayer = playerHit;

        if (playerHit && gameResources.grass > 0 && !isMoving)
        {
            playerHarvestingManager.ExchangeGrassToCoin();
            isMoving = true;
            DOVirtual.DelayedCall(1.5f, Move);
        }
        else if (!Physics.Raycast(startRay.position, Vector3.left, out hit, rayDistance, LayerMask.GetMask("NPC")) && isNPC && !isMoving && !hasScheduledMove)
        {
            hasScheduledMove = true;

            DOVirtual.DelayedCall(1, () =>
            {
                anim.SetBool("isWalking", true);
                transform.DOMove(new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z), 1f)
                    .OnComplete(() =>
                    {
                        hasScheduledMove = false;
                        anim.SetBool("isWalking", false);
                    });
            });
        }

        else if (Physics.Raycast(startRay.position, Vector3.left, out hit, rayDistance, LayerMask.GetMask("NPC")))
        {
            isNPC = true;
        }
        else
        {
            isNPC = false;
        }
    }

    private void Move()
    {
        isMoving = true;
        anim.SetBool("isWalking", true);
        if(walkingTween != null && walkingTween.IsActive())
            walkingTween.Kill();
       walkingTween = transform.DOPath(wayPoint, moveDuration, pathType).SetLookAt(0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            anim.SetBool("isWalking", false);
            isMoving = false;
            transform.DORotate(new Vector3(0, -90, 0), 0.5f).SetEase(Ease.Linear);
        });
    }
}
