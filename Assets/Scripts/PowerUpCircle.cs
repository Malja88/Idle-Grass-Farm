using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PowerUpCircle : MonoBehaviour, IStopMovement
{
   private BoxCollider col;
   private bool canEnter = true;
   [SerializeField] private Transform player;
   [SerializeField] private float activeDistance;
   [SerializeField] private int stopMovingTime;
    void Start()
    {
        col = GetComponent<BoxCollider>();

        col.OnTriggerEnterAsObservable().Where(x => canEnter).Subscribe(_ =>
        {
            if (_.TryGetComponent(out PlayerMovement playerMovement))
            {
                canEnter = false;
                StopMovement(playerMovement);
            }
        });

        Observable.EveryUpdate().Subscribe(_ =>
        {
            SetTriggerActive();
        }).AddTo(this);
    }
    

    public async void StopMovement(PlayerMovement playerMovement)
    {
        playerMovement.IsMoving = false;
        await Task.Delay(stopMovingTime);
        playerMovement.IsMoving = true;
    }

    private void SetTriggerActive()
    {
        if (Vector3.Distance(transform.position, player.position) > activeDistance)
        {
            canEnter = true;
        }
    }
}
