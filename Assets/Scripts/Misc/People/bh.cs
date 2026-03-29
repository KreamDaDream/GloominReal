using UnityEngine;
using System.Collections;
public class bh : MonoBehaviour, IInteractable
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteract()
    {
        StartCoroutine(seq());
    }

    IEnumerator seq()
    {

        yield return StartCoroutine(UIHandler.instance.speak("Basebol.", "Baseball Man", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Huh?", "Me", plrMovement.instance.gameObject));
        




    }
}
