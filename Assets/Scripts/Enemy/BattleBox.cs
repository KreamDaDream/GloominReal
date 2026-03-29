using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BattleBox : MonoBehaviour
{
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject bg;
    [SerializeField] private GameObject go;

    [SerializeField] private ParticleSystem ps;

    [SerializeField] private List<GameObject> toggleCols = new List<GameObject>();

    [SerializeField] private GameObject battleUI;
    private GameObject plr;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private GameObject ene;
    [SerializeField] private List<Sprite> particleLoop = new List<Sprite>();

    private Vector3 ogPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr = GameObject.Find("PLAYER");
    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null)
        {
            plr = GameObject.Find("PLAYER");
        }
    }
    public IEnumerator gameOver()
    {

        Vector3 enePos = ene.transform.position;
        ogPos = plr.transform.position;
        ene.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        plr.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        plr.GetComponent<plrMovement>().canOpenMenu = false;
        IHan.instance.inventoryUI.SetActive(false);

        for (float i = 0; i < 50; i++)
        {
            battleUI.GetComponent<RectTransform>().localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, 400, 0), (i) / 50);
            bg.transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 10, 0), (i) / 50);
            ene.transform.position = Vector3.Lerp(enePos, enePos + new Vector3(0, 20, 0), (i) / 50);
            yield return new WaitForSeconds(0.008f + ((i / 120) * 0.02f));

        }
        for (float i = 0; i < 10; i++)
        {
            var vel = ps.velocityOverLifetime;
            vel.y = new ParticleSystem.MinMaxCurve(4 + 2 * i, 6 + 2 * i);

            yield return new WaitForSeconds((0.1f));

        }
        yield return new WaitForSeconds((2f));

        for (float i = 100; i > 0; i--)
        {
            go.SetActive(true);

            go.GetComponent<RectTransform>().localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, -296, 0), (i) / 50);

            plr.transform.position = Vector3.Lerp(ogPos - new Vector3(0, 14, 0), ogPos, (i) / 80);

            var vel = ps.velocityOverLifetime;
            vel.y = new ParticleSystem.MinMaxCurve(4 + 2 * i / 10, 6 + 2 * i/10);
            yield return new WaitForSeconds((0.01f));

        }
    }
    public IEnumerator toggleBox(bool open, GameObject chall)
    {
        if (plr == null)
        {
            plr = GameObject.Find("PLAYER");
        }
        bg.GetComponent<EdgeCollider2D>().enabled = false;
        foreach (var col in toggleCols)
        {
            foreach (var col2 in col.GetComponents<Collider2D>())
            {
                col.GetComponent<Collider2D>().enabled = false;
            }
        }


        if (open)
        {
            plr.GetComponent<plrMovement>().canOpenMenu = false;
            plr.GetComponent<plrMovement>().menu.SetActive(false);
            plr.GetComponent<SpriteRenderer>().sortingLayerName = "BB";
            plr.layer = LayerMask.NameToLayer("Box");
            transform.position = plr.transform.position;
            plr.GetComponent<plrMovement>().toggleLockCamera(true);
            plr.GetComponent<plrMovement>().speed = 5;
            plr.GetComponent<plrMovement>().camPos = plr.transform.position + new Vector3(0, 0, -10);
            if (chall.name == "hatOrbOb")
            {
            }
            else
            {
                chall.GetComponent<SpriteRenderer>().sortingLayerName = "BB";
            }


            ogPos = plr.transform.position;
            Vector3 ogChal = chall.transform.position;
            battleUI.SetActive(true);
            for (float i = 0; i < 50; i++)
            {
                battleUI.GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(0, -450, 0), Vector3.zero, i / 50);
                black.transform.localPosition = Vector3.Lerp(new Vector3(0, -15, 0), Vector3.zero, i / 50);
                bg.transform.localPosition = Vector3.Lerp(new Vector3(0, -15, 0), Vector3.zero, i / 50);
                plr.transform.position = Vector3.Lerp(ogPos, transform.TransformPoint(new Vector3(-1, 0, 0)), i / 50);
                chall.transform.position = Vector3.Lerp(ogChal, transform.TransformPoint(new Vector3(1, 0, 0)), i / 50);

                plr.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one / 2, i / 50);

                yield return new WaitForSeconds(0.008f);

            }
            ps.Play();
            AudioManager.Instance.ChangeMusic(AudioManager.Instance.FIGHT);
            Destroy(chall);
            if (chall.name == "hatOrbOb")
            {
                ene = Instantiate(enemies[3]);

            }
            else
            {
                ene = Instantiate(enemies[Random.Range(0, enemies.Count - 1)]);
            }
            ene.transform.parent = null;
            ene.transform.position = transform.TransformPoint(new Vector3(1, 0, 0));
            ene.GetComponent<IEnemy>().boundCenter = transform.position;
            ene.GetComponent<IEnemy>().bounds = new Vector3(5.75f, 5.75f, 0);
            plr.GetComponent<plrMovement>().canMove = true;
            plr.GetComponent<plrMovement>().canDash = true;
            plr.GetComponent<Animator>().SetInteger("Dir", 1);

            bg.GetComponent<EdgeCollider2D>().enabled = true;

        }
        else
        {
            plr.GetComponent<plrMovement>().canMove = false;
            plr.GetComponent<plrMovement>().canDash = false;
            ps.Stop();
            ps.Clear();
            AudioManager.Instance.ChangeMusic(AudioManager.Instance.background);

            for (float i = 0; i < 50; i++)
            {
                battleUI.GetComponent<RectTransform>().localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, -700, 0), i / 50);
                black.transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, -15, 0), i / 50);
                bg.transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, -15, 0), i / 50);
                plr.transform.localScale = Vector3.Lerp(Vector3.one / 2, Vector3.one, i / 50);
                plr.transform.position = Vector3.Lerp(transform.TransformPoint(new Vector3(-1, 0, 0)), ogPos, i / 50);

                yield return new WaitForSeconds(0.008f);
            }
            plr.GetComponent<plrMovement>().canMove = true;
            plr.GetComponent<plrMovement>().canOpenMenu = true;

            plr.GetComponent<plrMovement>().speed = 10;
            battleUI.SetActive(false);
            foreach (var col in toggleCols)
            {
                foreach (var col2 in col.GetComponents<Collider2D>())
                {
                    col.GetComponent<Collider2D>().enabled = true;
                }
            }
            plr.GetComponent<plrMovement>().toggleLockCamera(false);
            plr.GetComponent<plrMovement>().challenged = false;
            plrMovement.instance.canBeEngaged = true;

            plr.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            plr.layer = LayerMask.NameToLayer("Default");
            plr.GetComponent<plrMovement>().canInteract = true;


        }




    }

    
 
}
