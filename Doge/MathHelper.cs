namespace Silk.NET_Tutorials;

public static class MathHelper
{
    public static float DegreesToRadians(float degrees) => MathF.PI / 180f * degrees;
    
    public static float RadiansToDegrees(float degrees) => (180f / MathF.PI) * degrees;

    public static float Clamp(float value, float min, float max)
    {
        if (value > max)
        {
            return max;
        }
        
        if (value < min)
        {
            return min;
        }

        return value;
    }
}
