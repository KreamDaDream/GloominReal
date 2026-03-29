using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject black;

    [SerializeField] private GameObject textOb;

    [SerializeField] private TextMeshProUGUI textPerson;

    [SerializeField] private TextMeshProUGUI textContent;



    public GameObject coinImg;

    public InputAction advance;
    [SerializeField] private GameObject dascene;

    [SerializeField] private GameObject boat;
    public static UIHandler instance;
    [SerializeField] private GameObject aftertext;
    private bool pressedE = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void OnEnable()
    {
        advance.Enable();
    }

    private void OnDisable()
    {
        advance.Disable();
    }


    private void Awake()
    {
        advance.performed += ctx => toggleFor();
    }

    void toggleFor()
    {
        pressedE = true;
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

    public void itsover()
    {
        StartCoroutine(gameOver());
    }

    private IEnumerator gameOver()
    {
        StartCoroutine(UIHandler.instance.toggleFade(true));
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator speak(string text, string person, GameObject tar)
    {
        float start = Time.time;
        plrMovement.instance.canInteract = false;
        plrMovement.instance.canMove = false;
        plrMovement.instance.canOpenMenu = false;
        plrMovement.instance.canBeEngaged = false;

        textPerson.text = person;
        textOb.SetActive(true);
        string msg = "";
        bool fastMode = false;
        Vector3[] corns = { new Vector3(0, 0, 0), new Vector3(424, -278, 0), new Vector3(0, -278, 0), new Vector3(424, 0, 0) };

        Vector2 screenPos = Camera.main.WorldToScreenPoint(tar.transform.position);

        Vector3 farthest = corns[0];
        foreach (var corn in corns)
        {
            if ((corn - (Vector3)screenPos).magnitude > (farthest - (Vector3)screenPos).magnitude)
            {
                farthest = corn;
            }
        }
        textOb.GetComponent<RectTransform>().anchoredPosition = farthest;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '`')
            {
                msg += text[i];

                if (!fastMode)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.talk);
                }
            }


            textContent.text = msg;
            if (pressedE == true)
            {
                pressedE = false;
                if (Time.time - start > 0.1f)
                {
                    fastMode = true;
                }
            }
            if (!fastMode)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        while (pressedE == false)
        {
            yield return new WaitForEndOfFrame();
        }
        pressedE = false;
        textOb.SetActive(false);
        plrMovement.instance.canInteract = true;
        plrMovement.instance.canMove = true;
        plrMovement.instance.canOpenMenu = true;
        plrMovement.instance.canBeEngaged = true;

    }

    public IEnumerator endingSequence()
    {
        plrMovement.instance.canInteract = false;
        plrMovement.instance.canMove = false;
        plrMovement.instance.canOpenMenu = false;
        plrMovement.instance.canBeEngaged = false;


        yield return StartCoroutine(toggleFade(true));
        dascene.SetActive(true);
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(toggleFade(false));

        for (float i = 0; i <= 200; i++)
        {
            boat.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(new Vector3(-529.54f, -41, 0), new Vector3(532.9f, -41, 0), i / 200);
            yield return new WaitForSeconds(0.06f);
        }
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(toggleFade(true));
        dascene.SetActive(false);

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(UIHandler.instance.speak("I've made a mistake.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Nothing can erase it.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("But I will avenge my people.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("If it's the last thing I do.", "Me", plrMovement.instance.gameObject));
        yield return new WaitForSeconds(2f);
        aftertext.SetActive(true);
        aftertext.GetComponent<TextMeshProUGUI>().text = "TO BE CONTINUED";
        yield return new WaitForSeconds(2f);
        aftertext.GetComponent<TextMeshProUGUI>().text = "THANK YOU FOR PLAYING";
        yield return new WaitForSeconds(2f);
        aftertext.GetComponent<TextMeshProUGUI>().text = "SPECIAL THANKS TO WB AND DOGGOBLUE";
        yield return new WaitForSeconds(2f);
        aftertext.GetComponent<TextMeshProUGUI>().text = "AND JOHNNY RAZER FOR MAKING THE GAME JAM IG";
        yield return new WaitForSeconds(2f);

        aftertext.SetActive(false);
        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("MainMenu");

    }
}
