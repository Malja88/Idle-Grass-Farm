using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerHarvestingManager : MonoBehaviour, IHarvest, ISickleUpgrade, IExchangeGrassToCoin
{
    [Header("Body Components")]
    private Animator anim;
    private CapsuleCollider capsuleCol;
    private PlayerMovement playerMovement;
    private PlayerUIManager playerUIManager;
    private Rigidbody rb;
    [Header("Scene Components")]
    [SerializeField] private GameObject[] grass;
    [SerializeField] private GameResources gameResources;
    [SerializeField] private GameObject sickle;
    [Header("FX Components")]
    [SerializeField] private ParticleSystem exchangeGrassParticle;
    [SerializeField] private ParticleSystem sickleParticle;
    [SerializeField] private ParticleSystem sickleSwoosh;
    private int sickleEndValue;
    private int grassEndValue;
    private int exchangeEndValue;
    private int grassExchangeLimit;
    
    public int SickleEndValue { get => sickleEndValue; set => sickleEndValue = value; }
    public int GrassEndValue { get => grassEndValue; set => grassEndValue = value; }
    public int ExchangeEndValue { get => exchangeEndValue; set => exchangeEndValue = value; }
    public int GrassExchangeLimit { get => grassExchangeLimit; set => grassExchangeLimit = value; }
    void Start()
    {
        anim = GetComponent<Animator>();   
        capsuleCol = GetComponent<CapsuleCollider>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        playerUIManager = GetComponent<PlayerUIManager>();
        capsuleCol.OnTriggerEnterAsObservable().Subscribe(_ =>
        {
            if(_.CompareTag("Harvest"))
            {
                Harvest();
            }
            
            else if (_.CompareTag("Sickle Upgrade"))
            {
                UpgradeSickle();
            }
        } );
    }

    public async Task Harvest()
    {
        sickle.SetActive(true);
        anim.SetBool("isHarvest", true);
        anim.SetFloat("Speed",0);
        playerMovement.IsMoving = false;
        rb.linearVelocity = Vector3.zero;
        for (int i = 0; i < grass.Length; i++)
        {
            grass[i].SetActive(false);
            await Task.Delay(500);
        }
        anim.SetBool("isHarvest", false);
        sickle.SetActive(false);
        playerMovement.IsMoving = true;
        GrowGrassBack();
        
        
        grassEndValue = gameResources.grass + gameResources.grassPerHarvest;
        gameResources.grass = grassEndValue;
        playerUIManager.AddGrassUI();
    }

    private async Task GrowGrassBack()
    {
        await Task.Delay(5000);
        foreach (GameObject g in grass)
        {
            g.SetActive(true);
        }
    }

    public void UpgradeSickle()
    {
        if (gameResources.coins < gameResources.sickleUpgradePrice) return;
        sickleParticle.Play();
        sickleEndValue = gameResources.coins -= gameResources.sickleUpgradePrice;
        gameResources.sickleLevel++;
        gameResources.grassPerHarvest++;
        playerUIManager.UpgradeSickleUI();
    }

    public void ExchangeGrassToCoin()
    {
        if (gameResources.grass <= 0) return;
        exchangeGrassParticle.Play();
        exchangeEndValue = gameResources.coins + gameResources.coinsExchangeRate;
        grassExchangeLimit = Mathf.Max(0, gameResources.grass - gameResources.coinsExchangeRate);
        gameResources.coins = exchangeEndValue;
        gameResources.grass = grassExchangeLimit;
        playerUIManager.ExchangeGrassToCoinUI();
    }

    public void SickleSwooshPlay()
    {
        sickleSwoosh.Play();
    }
}
