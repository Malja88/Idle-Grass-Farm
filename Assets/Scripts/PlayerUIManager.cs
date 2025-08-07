using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]  private TextMeshProUGUI grassText;
    [SerializeField]  private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI sickleLevelText;
    [SerializeField] private GameResources gameResources;
    private PlayerHarvestingManager playerHarvestingManager;
    private int currentGrassCount = 0;
    private Tween grassTween;
    private Tween coinTween;
    private Tween sickleTween;

    private void Start()
    {
        playerHarvestingManager = GetComponent<PlayerHarvestingManager>();
        grassText.text = gameResources.grass.ToString();
        coinText.text = gameResources.coins.ToString();
        sickleLevelText.text = gameResources.sickleLevel.ToString();
    }

    public void AddGrassUI()
    {
        if (grassTween != null && grassTween.IsActive())
            grassTween.Kill();
        
        grassTween = DOTween.To(() => gameResources.grass, x => {
            grassText.text = x.ToString();
        }, playerHarvestingManager.GrassEndValue, 1f);
    }

    public void ExchangeGrassToCoinUI()
    {
        if (coinText != null)
        {
            if (coinTween != null && coinTween.IsActive())
                coinTween.Kill();
            
            coinTween = DOTween.To(() => gameResources.coins, x =>
            {
                coinText.text = x.ToString();
            }, playerHarvestingManager.ExchangeEndValue, 1f);
            
            if (grassTween != null && grassTween.IsActive())
                grassTween.Kill();
            
            grassTween = DOTween.To(() => gameResources.grass, x =>
            {
                grassText.text = x.ToString();
            }, playerHarvestingManager.GrassExchangeLimit, 1f);
        }
    }

    public void UpgradeSickleUI()
    {
        if (sickleTween != null && sickleTween.IsActive())
            sickleTween.Kill();
        
        sickleTween = DOTween.To(() => gameResources.coins, x => {
            coinText.text = x.ToString();
        }, playerHarvestingManager.SickleEndValue, 1f);
        sickleLevelText.text = gameResources.sickleLevel.ToString();
    }
}
