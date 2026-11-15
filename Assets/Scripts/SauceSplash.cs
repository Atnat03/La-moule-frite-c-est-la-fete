using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SauceSplash : MonoBehaviour
{
    public void SetDecalSplash(Material splashMat)
    {
        DecalProjector decalProjector = GetComponent<DecalProjector>();

        decalProjector.material = splashMat;
    }
}
