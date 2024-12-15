using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDestroyed = false;

    public void OnDestroy()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
        }
    }
}
