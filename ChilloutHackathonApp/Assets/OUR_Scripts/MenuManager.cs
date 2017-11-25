﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuLight;
    public Transform playPosition;
    public GameObject player;
    public GameObject table;
    public GameObject menu;
    public List<GameObject> lights;

    private float secondsToLightUp = 5;
    public MenuManager instance;

    public void PlayerConnected()
    {
        StartCoroutine(StartGameAnimations());
    }

    private void Start()
    {
        instance = this;
    }

    private IEnumerator StartGameAnimations()
    {
        yield return new WaitForSecondsRealtime(secondsToLightUp);
        menuLight.SetActive(true);
        player.GetComponent<Animator>().SetTrigger("MainMenu");
        yield return new WaitForSecondsRealtime(1.5f);
        menu.SetActive(true);
    }

    public void PrepareAndStart()
    {
        menu.SetActive(false);
        StartCoroutine(PrepareLights());
    }

    private IEnumerator PrepareLights()
    {
        menuLight.SetActive(false);
        table.SetActive(true);
        player.transform.position = playPosition.position;

        yield return new WaitForSecondsRealtime(0.5f);
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
            yield return new WaitForSecondsRealtime(0.15f);
        }
        menuLight.SetActive(true);
    }

}
