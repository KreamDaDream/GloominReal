using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.UI;
public class plrMovement : MonoBehaviour, IDataPersistence
{
    public static plrMovement instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputAction moveAction;
    public InputAction interactButton;
    public InputAction menuButton;
    public InputAction dashButton;
    public InputAction sliceButton;

    [SerializeField] private GameObject bb;
    public float speed;
    public float dashSpeed;
    public float rbDamDash;
    public bool dying;
    [SerializeField] private ParticleSystem dashPars;

    [SerializeField] private GameObject pools;

    private void OnDisable() { 
        moveAction.Disable(); 
        interactButton.Disable();
        menuButton.Disable();
        dashButton.Disable();
        sliceButton.Disable();
    }
    private void OnEnable() { 
        moveAction.Enable(); 
        interactButton.Enable();
        menuButton.Enable();
        dashButton.Enable();
        sliceButton.Enable();
    }
    private Rigidbody2D rb;
    public Animator animator;
    public bool menuOpen = false;

    public bool dashing = false;

    public Vector3 spawnPos;
    public float shake;
    public bool canDash;
    public bool canOpenMenu = true;
    public Vector3 dir = new Vector3(0, 1);
    public bool canMove = true;

    public bool canBeEngaged = true;

    public Vector3 camPos;
    public GameObject menu;

    public IHan ihan;
    public bool challenged = false;
    [SerializeField] private GameObject Rooms;

    public GameObject box;
    [SerializeField] private GameObject black;

    private float lastSlice = 0;
    public bool canInteract = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(UIHandler.instance.toggleFade(false));
        AudioManager.Instance.ChangeMusic(AudioManager.Instance.background);
        if (instance == null)
        {
            instance = this;
        }
    }


    private void Awake()
    {
        interactButton.performed += ctx => Interact();
        menuButton.performed += ctx => ToggleMenu();
        dashButton.performed += ctx => StartCoroutine(dash());
        sliceButton.performed += ctx => StartCoroutine(slice());

    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.GetComponent<CinemachineBrain>().enabled == false)
        {
            Camera.main.transform.position = camPos + new Vector3(Random.Range(-shake, shake), Random.Range(-shake, shake), -10);
            shake *= Mathf.Pow(0.95f, Time.deltaTime * 60f);
            if (shake < 0.01f)
            {
                shake = 0;
            }
        }

        if (!dying)
        {
            if (canMove)
            {
                rb.linearVelocity = Vector3.Normalize(moveAction.ReadValue<Vector2>()) * speed;
            }
            else
            {
                if (!dashing)
                {
                    rb.linearVelocity = Vector3.Normalize(moveAction.ReadValue<Vector2>()) * 0;
                }
                if (rb.linearVelocity == Vector2.zero)
                {
                    animator.SetBool("Walking", false);

                }
            }

            if (rb.linearVelocity == Vector2.zero)
            {
                animator.SetBool("Walking", false);

            }
            if (rb.linearVelocityX > 0)
                {
                    if (animator.GetBool("Walking") != true)
                    {
                        animator.SetBool("Walking", true);
                    }
                    animator.SetInteger("Dir", 4);
                    dir = new Vector3(1, 0);
                }
                else if (rb.linearVelocityX < 0)
                {
                    if (animator.GetBool("Walking") != true)
                    {
                        animator.SetBool("Walking", true);
                    }
                    animator.SetInteger("Dir", 3);
                    dir = new Vector3(-1, 0);
                }

                if (rb.linearVelocityY < 0)
                {
                    if (animator.GetBool("Walking") != true)
                    {
                        animator.SetBool("Walking", true);
                    }
                    if (!canDash)
                    {
                        animator.SetInteger("Dir", 1);
                    }
                    dir = new Vector3(0, -1);
                }
                else if (rb.linearVelocityY > 0)
                {
                    if (animator.GetBool("Walking") != true)
                    {
                        animator.SetBool("Walking", true);
                    }
                    if (!canDash)
                    {
                        animator.SetInteger("Dir", 2);
                    }
                    dir = new Vector3(0, 1);
                }

               

            
           
        } else
        {
            animator.SetBool("dying", true);
            animator.SetBool("Walking", false);
            animator.SetInteger("Dir", -1);
            transform.rotation = new Quaternion(0, 0, 180, 0);
        }

    }
    public void toggleLockCamera(bool locking)
    {
        if (locking)
        {
            camPos = Camera.main.transform.position;

            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            foreach (Transform tran in Rooms.transform)
            {
                if (tran.Find("Camera") != null)
                {
                    
                    tran.Find("Camera").GetComponent<CinemachineCamera>().enabled = false;
                }
            }
        }
        else
        {
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
            foreach (Transform tran in Rooms.transform)
            {
                if (tran.Find("Camera") != null)
                {
                    tran.Find("Camera").GetComponent<CinemachineCamera>().enabled = true;
                }
            }
        }
    }
    private void Interact()
    {
        if (canInteract)
        {
            Collider2D[] stuff = Physics2D.OverlapBoxAll(transform.position + dir, new Vector2(0.5f, 0.5f), 0f);
            foreach (Collider2D col in stuff)
            {
                if (col.GetComponent<IInteractable>() != null)
                {
                    col.GetComponent<IInteractable>().OnInteract();                  
                    break;
                }
            }
        }
    }

    public void checky(GameObject flag)
    {
        spawnPos = flag.transform.position;
        IHan.instance.health = IHan.instance.maxHealth;
        DataPersistenceManager.instance.SaveGame();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.checkPoint);
    }
    private void ToggleMenu()
    {
        if (menuOpen == true)
        {
            menuOpen = false;
            canInteract = true;
            menu.SetActive(false);
        } else
        {
            if (canOpenMenu)
            {
                menuOpen = true;
                canInteract = false;

                menu.SetActive(true);
            }
        }
    }
    
    public void die()
    {
        dying = true;
        canDash = false;
        AudioManager.Instance.ChangeMusic(AudioManager.Instance.wind);

        foreach (Transform po in pools.transform)
        {
            foreach (Transform p in po.transform)
            {
                if (p.gameObject.activeSelf)
                {
                    p.gameObject.SetActive(false);
                }
            }
        }
        StartCoroutine(bb.GetComponent<BattleBox>().gameOver());
    }
    IEnumerator slice()
    {
        if (canDash)
        {
            if (Time.time - lastSlice > 0.71f)
            {
                if (ihan.requestAP())
                {
                    lastSlice = Time.time;
                    animator.SetBool("SliceRequest", true);
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.slice);

                    Collider2D[] stuff = Physics2D.OverlapBoxAll(transform.position + (dir/2), new Vector2(1f, 1f), 0f);
                    foreach (Collider2D col in stuff)
                    {
                        if (col.gameObject.GetComponent<IEnemy>() != null)
                        {
                            AudioManager.Instance.PlaySFX(AudioManager.Instance.impact);

                            col.gameObject.GetComponent<IEnemy>().damage(ihan.attackPoints);
                            shake += 0.3f;

                            break;
                        }
                    }

                    yield return new WaitForSeconds(0.7f);
                    animator.SetBool("SliceRequest", false);
                }
            }
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator dash()
    {
        if (dashing == false && canDash && ihan.requestAP() && !dying)
        {
            dashing = true;
            canMove = false;
            dashPars.Play();
            float sIDD = Vector2.Dot(rb.linearVelocity, Vector3.Normalize(moveAction.ReadValue<Vector2>()));
            sIDD = Mathf.Clamp01(sIDD);
            float mul = 1 - (sIDD / dashSpeed);
            rb.AddForce(Vector3.Normalize(moveAction.ReadValue<Vector2>()) * dashSpeed * mul, ForceMode2D.Impulse);
            rb.linearDamping = rbDamDash;
            yield return new WaitForSeconds(0.2f);
            rb.linearDamping = 2;
            canMove = true;
            dashPars.Stop();
            dashing = false;

        }

    }

    public void LoadData(GameData data)
    {

        spawnPos = data.point;
        transform.position = spawnPos;

        for (int i = 0; i < 8; i++)
        {
            if (data.inventory[i] != "")
            {
                ihan.inventory[i] = data.inventory[i];
            }
        }
        if (data.inventory[8] != "")
        {
            ihan.equippedW = data.inventory[8];
        } else
        {
            ihan.equippedW = "Sword";
        }
        if (data.inventory[9] != "")
        {
            ihan.equippedD = data.inventory[9];
        } else
        {
            ihan.equippedW = "Chainmail Shirt";

        }
        ihan.coin = data.Coin;
    }

    public void SaveData(ref GameData data)
    {
        data.point = spawnPos;

        string[] invent = new string[] { "", "", "", "", "", "", "", "", "Sword", "Chainmail Shirt" };

        for (int i = 0; i < ihan.inventory.Count; i++)
        {
            invent[i] = ihan.inventory[i];
        }
        invent[8] = ihan.equippedW;
        invent[9] = ihan.equippedD;
        data.inventory = invent;

        data.Coin = ihan.coin;
    }
}
