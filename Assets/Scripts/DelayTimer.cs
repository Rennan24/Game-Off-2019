public struct DelayTimer
{
    public float Length;
    private float timer;

    private void Tick(float dt)
    {
        timer += dt;
    }
}
