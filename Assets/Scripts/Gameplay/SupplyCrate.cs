using UnityEngine;

public class SupplyCrate : MonoBehaviour
{
    public enum CrateReward { Health, Ammo }
    public CrateReward rewardType;

    public void GiveReward(PlayerHealth playerHealth, PlayerCombat playerCombat)
    {
        if (rewardType == CrateReward.Health)
        {
            playerHealth.Heal(0.25f);
        }
        else if (rewardType == CrateReward.Ammo)
        {
            playerCombat.AddAmmo(15);
        }

        WaveManager.Instance.HideCrates();
    }
}