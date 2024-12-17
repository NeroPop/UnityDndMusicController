using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientButtonController : MonoBehaviour
{
    public int ButtonIndex;

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
