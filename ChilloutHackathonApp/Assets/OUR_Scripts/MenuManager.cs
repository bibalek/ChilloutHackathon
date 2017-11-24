using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuLight;

    private float secondsToLightUp = 5;

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(secondsToLightUp);
    }
}
