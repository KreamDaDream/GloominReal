using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject bar;
    [SerializeField] private Button neweg;
    [SerializeField] private Button cont;

    private void Awake()
    {
    }

    void Start()
    {
        AudioManager.Instance.ChangeMusic(AudioManager.Instance.mainMenu);
        StartCoroutine(toggleFade(false));
        if (!DataPersistenceManager.instance.hasGameData())
        {
            cont.interactable = false;
        }
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

    public void pullUpControls()
    {
        if (bar.activeSelf)
        {
            bar.SetActive(false);
        }
        else
        {
            bar.SetActive(true);
        }
    }
    public void doit()
    {
        StartCoroutine(start(false));
    }

    public void newGame()
    {
        
        DataPersistenceManager.instance.NewGame();
        StartCoroutine(start(true));
    }
    private IEnumerator start(bool newe)
    {
        cont.interactable = false;
        neweg.interactable = false;
        yield return StartCoroutine(toggleFade(true));
        yield return new WaitForSeconds(2f);
        if (!newe)
        {
            SceneManager.LoadSceneAsync("Map");
        } else
        {
            SceneManager.LoadScene("Slides");

        }
    }

    
}
