using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.ContentSizeFitter;

public class slideManager : MonoBehaviour
{
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI speech;
    [SerializeField] private List<Sprite> imgs;

    [SerializeField] private Image img;

    private bool white = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(toggleFade(false));

        StartCoroutine(seq());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator toggleFade(bool fade)
    {
        if (fade)
        {
            black.SetActive(true);
            for (float i = 0; i <= 100; i++)
            {
                black.GetComponent<Image>().color = new Color(0, 0, 0, i / 100);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            black.SetActive(true);
            for (float i = 100; i >= 0; i--)
            {
                black.GetComponent<Image>().color = new Color(0, 0, 0, i / 100);
                yield return new WaitForSeconds(0.01f);

            }
            black.SetActive(false);

        }
    }

    public IEnumerator seq()
    {
       img.sprite = imgs[0];

        yield return StartCoroutine(say("In the beginning, there was light", true, 1f));
        yield return StartCoroutine(say("And dark", false, 1f));

        yield return StartCoroutine(say("Day", true, 1f));
        yield return StartCoroutine(say("And Night", false, 1f));
        yield return StartCoroutine(say("Karoob", true, 2f));
        yield return StartCoroutine(say("And Amrath", false, 2f));
        img.sprite = imgs[1];
        yield return StartCoroutine(say("Karoob gave man spirit and love", true, 2f));
        yield return StartCoroutine(say("Amrath gave man depth and sorrow", false, 2f));
        yield return StartCoroutine(say("The world was a place of harmony", true, 2f));
        yield return StartCoroutine(say("Of perfect balance between light and darkness", true, 2f));
        img.sprite = imgs[2];

        yield return StartCoroutine(say("Karoob decided to give humanity free will", true, 3f));
        yield return StartCoroutine(say("Under the sun, billions started to pray to Karoob", true, 3f));
        img.sprite = imgs[3];

        yield return StartCoroutine(say("Under the moon, humanity hid away, hiding from Amrath and their creations", false, 3f));

        yield return StartCoroutine(say("Amrath was enraged. Couldn’t they see all they did for them?", false, 3f));
        img.sprite = imgs[4];

        yield return StartCoroutine(say("In a fit of rage, they unleashed the Gloom onto the world", false, 3f));

        yield return StartCoroutine(say("Air turned poison", false, 1f));
        yield return StartCoroutine(say("Water turned to blood", false, 1f));
        yield return StartCoroutine(say("Dreams became nightmares", false, 1f));
        img.sprite = imgs[5];

        yield return StartCoroutine(say("When people started to die, Karoob had to do something", true, 2f));
        yield return StartCoroutine(say("They taught the greatest minds of Earth at the time to create a box of material so strong", true, 3f));
        yield return StartCoroutine(say("That could bound soul itself", true, 1f));

        yield return StartCoroutine(say("But that still wasn’t enough", true, 1f));
        img.sprite = imgs[6];

        yield return StartCoroutine(say("After luring Amrath to the center of the world,", true, 2f));

        yield return StartCoroutine(say("Karoob ignited their existence", true, 1f));

        yield return StartCoroutine(say("And, as a white hole of gravity, tackled Amrath into the box", true, 2f));

        yield return StartCoroutine(say("Fusing with the box in the process", true, 1f));
        img.sprite = imgs[7];

        yield return StartCoroutine(say("Humanity lived in peace once more", false, 1f));

        yield return StartCoroutine(say("But unbeknownst to them, some of Amrath’s soul remained", false, 2f));
        yield return StartCoroutine(say("Outside the confines of the box", false, 1f));
        img.sprite = imgs[8];

        yield return StartCoroutine(say("For centuries, the fragment bided its time, and got stronger", false, 2f));
        yield return StartCoroutine(say("Until finally, it unleashed the Gloom once more", false, 2f));
        yield return StartCoroutine(say("In the form of horrible beasts", false, 1f));
        img.sprite = imgs[9];

        yield return StartCoroutine(say("With Karoob gone, Gloom slowly became a fact of life", false, 2f));
        yield return StartCoroutine(say("And no body or thing was equipped to end it", false, 2f));
        yield return StartCoroutine(toggleFade(true));
        SceneManager.LoadScene("Map");

    }

    public IEnumerator say(string text, bool color, float wait)
    {
        if (color != white)
        {
            if (color)
            {
                speech.color = Color.black;
                for (float i = 0; i <= 20; i++)
                {
                    background.GetComponent<Image>().color = Color.Lerp(Color.black, Color.white, i/20);
                    yield return new WaitForSeconds(0.01f);
                }
                white = true;
            }
            else
            {
                speech.color = Color.white;
                for (float i = 0; i <= 20; i++)
                {
                    background.GetComponent<Image>().color = Color.Lerp(Color.white, Color.black, i / 20);
                    yield return new WaitForSeconds(0.01f);
                }
                white = false;
            }
        }
            string msg = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != '`')
                {
                    msg += text[i];
                    speech.text = msg;

                }
                yield return new WaitForSeconds(0.05f);

        }
        yield return new WaitForSeconds(wait);
        }
    
}
