public static class Utils
{
    public static UnityEngine.Vector3 GetRandomDir()
    {
        return new UnityEngine.Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
}
