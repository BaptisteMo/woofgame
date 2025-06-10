public static class BoostHelper
{
    public static float ApplyMultiplier(float baseValue)
    {
        return baseValue * GameSession.Instance.boostPowerMultiplier;
    }
}