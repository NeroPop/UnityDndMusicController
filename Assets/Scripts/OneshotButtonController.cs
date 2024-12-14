using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneshotButtonController : MonoBehaviour
{
    public int ButtonIndex;

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
