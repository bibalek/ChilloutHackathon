using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuLight;
    public Transform playPosition;
    public GameObject player;
    public List<GameObject> lights;

    private float secondsToLightUp = 5;

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(secondsToLightUp);
        menuLight.SetActive(true);
        yield return new WaitForSecondsRealtime(10);
        menuLight.SetActive(false);
        player.transform.position = playPosition.position;
        yield return new WaitForSecondsRealtime(0.5f);
        foreach(GameObject light in lights)
        {
            light.SetActive(true);
        }

    }
}
