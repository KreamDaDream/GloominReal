using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CMC : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject guy;
    private bool played = false;
    private bool checke = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!checke && Time.time > 0.5f)
        {
            checke = true;
            if (played == true)
            {
                guy.SetActive(false);
            }

        }

        if (guy.GetComponent<Rigidbody2D>().linearVelocity != Vector2.zero)
        {
            guy.GetComponent<Animator>().SetBool("walking", true);
        }
        else
        {
            guy.GetComponent<Animator>().SetBool("walking", false);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!played)
        {


            if (collision.gameObject == plrMovement.instance.gameObject)
            {
                StartCoroutine(cutscene());
            }
       
     
        } 
    }

    IEnumerator cutscene()
    {
        played = true;

        plrMovement.instance.canMove = false;
        plrMovement.instance.canOpenMenu = false;
        plrMovement.instance.canBeEngaged = false;

        guy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -3);
        yield return new WaitForSeconds(3f);
        guy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        plrMovement.instance.gameObject.GetComponent<Animator>().SetInteger("Dir", 2);
        yield return StartCoroutine(UIHandler.instance.speak("Oh!``` What's up cousin?", "Terry", guy));
        yield return StartCoroutine(UIHandler.instance.speak("What are you doing all the way out here? Aren't you supposed to be protecting the village?", "Terry", guy));
        yield return StartCoroutine(UIHandler.instance.speak("I mean, I guess it's fine, none of the Gloom know the location of our home.", "Terry", guy));
        yield return StartCoroutine(UIHandler.instance.speak("Anyway,`` I have to go.``` I have medicine for my mother.", "Terry", guy));
        AudioManager.Instance.ChangeMusic(null);
        plrMovement.instance.gameObject.GetComponent<Animator>().SetInteger("Dir", 1);

        yield return StartCoroutine(UIHandler.instance.speak(".``.``.``", "Me", plrMovement.instance.gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("Why are you so quiet?``` What's wrong?", "Terry", guy));

        yield return StartCoroutine(UIHandler.instance.speak("Everyone's dead.", "Me", plrMovement.instance.gameObject));
        plrMovement.instance.canMove = false;
        plrMovement.instance.canOpenMenu = false;
        plrMovement.instance.canBeEngaged = false;
        AudioManager.Instance.ChangeMusic(AudioManager.Instance.wind);
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(UIHandler.instance.speak(".```.```.```.```What?", "Terry", guy));
        yield return StartCoroutine(UIHandler.instance.speak("The Gloom.```` They attacked the village.``` An entire swarm of them.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("It was a massacre.", "Me", plrMovement.instance.gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("B``-but,```` I thought you were keeping them away.", "Terry", guy));
        yield return StartCoroutine(UIHandler.instance.speak("There were too many,``` I'm sorry", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Kahoob have mercy.", "Terry", guy));
        guy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -10);
        plrMovement.instance.canMove = false;
        plrMovement.instance.canOpenMenu = false;
        plrMovement.instance.canBeEngaged = false;
        yield return new WaitForSeconds(2);
        guy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
        guy.SetActive(false);
        yield return StartCoroutine(UIHandler.instance.speak("How could I let this happen?", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("I got everyone killed.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("I have nothing left.", "Me", plrMovement.instance.gameObject));
        plrMovement.instance.canMove = false;
        plrMovement.instance.canOpenMenu = false;
        plrMovement.instance.canBeEngaged = false;
        yield return new WaitForSeconds(2);
        plrMovement.instance.gameObject.GetComponent<Animator>().SetInteger("Dir", 2);
        AudioManager.Instance.ChangeMusic(null);
        yield return StartCoroutine(UIHandler.instance.speak("I need to make this right.````` One way or another.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("I need to banish these Gloom back to hell where they belong.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("So I will get to the river.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("I will cross it.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("And I will make the Fragment pay.", "Me", plrMovement.instance.gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("I``` need``` to``` make``` this``` right.", "Me", plrMovement.instance.gameObject));
        yield return new WaitForSeconds(1);
        AudioManager.Instance.ChangeMusic(AudioManager.Instance.background);
        plrMovement.instance.canMove = true;
        plrMovement.instance.canOpenMenu = true;
        plrMovement.instance.canBeEngaged = true;
    }

    public void LoadData(GameData data)
    {
        played = data.cutscenePlayed;
    }
    
    public void SaveData(ref GameData data)
    {
        data.cutscenePlayed = played;
    }
}
