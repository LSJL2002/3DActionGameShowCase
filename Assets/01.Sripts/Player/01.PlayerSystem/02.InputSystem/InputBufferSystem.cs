using UnityEngine;

public class InputBufferSystem
{
    private float bufferTime;
    private float lastInputTime;
    private bool inputQueued;

    public bool HasInput => inputQueued && Time.time - lastInputTime <= bufferTime;

    public InputBufferSystem(float bufferTime = 0.2f)
    {
        this.bufferTime = bufferTime;
    }

    public void RegisterInput()
    {
        lastInputTime = Time.time;
        inputQueued = true;
    }

    public void Consume()
    {
        inputQueued = false;
    }
}
