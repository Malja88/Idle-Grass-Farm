using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Resources", menuName = "Grass/Coin Data")]
public class GameResources : ScriptableObject
{
    public int coins;
    public int grass;
    public int sickleLevel;
    public int grassPerHarvest;
    public int coinsExchangeRate;
    public int sickleUpgradePrice;
}
