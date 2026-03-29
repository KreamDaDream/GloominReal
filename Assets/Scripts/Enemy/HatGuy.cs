using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HatGuy : MonoBehaviour, IEnemy
{
    public Vector3 boundCenter { get; set; }
    public Vector3 bounds { get; set; }

    public int MaxHealth { get; set; }
    public bool dead = false;
    public int health { get; set; }
    private Rigidbody2D rb;
    public ParticleSystem hurtPars { get; set; }
    private int worth;
    private ProjPool hatPool;
    private ProjPool sharpHatPool;

    private ProjPool coinPool;

    private GameObject coinImage;

    private GameObject plr;

    private bool attacking;

    private float lastAttack;

    private GameObject thing;
    private bool overide = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hatPool = GameObject.Find("Pools").transform.Find("Hat").GetComponent<ProjPool>();
        sharpHatPool = GameObject.Find("Pools").transform.Find("PointyHat").GetComponent<ProjPool>();

        coinPool = GameObject.Find("Pools").transform.Find("Coin").GetComponent<ProjPool>();
        MaxHealth = 30;
        worth = 50;
        health = MaxHealth;
        hurtPars = transform.Find("hurtPars").GetComponent<ParticleSystem>();
        coinImage = GameObject.Find("BattleScreen").transform.Find("PointOfCoin").gameObject;

        thing = transform.Find("HatOrbThing").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null)
        {
            plr = GameObject.Find("PLAYER");
        }

        if (!overide)
        {
            thing.transform.localPosition = new Vector3(0, Mathf.Sin(2 * Time.time) / 4, 0);
            thing.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(4 * Time.time) * 8);
        }


        if (rb.linearVelocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }



        if (Time.time - lastAttack > 1 && !attacking && !dead && !plr.GetComponent<plrMovement>().dying)
        {
            attacking = true;
            int ran = Random.Range(1, 4);
            if (ran == 1)
            {
                StartCoroutine(pointyHat());
            }
            else if (ran == 2)
            {

                StartCoroutine(hatAttack());

            }
            else if (ran == 3)
            {
                StartCoroutine(bite());

            }
        }
    }

    IEnumerator bite()
    {
        for (int j = 0; j < 5; j++)
        {
            Vector3 dir = (plr.transform.position - transform.position).normalized;
            Vector3 des = plr.transform.position + (dir * 2);
            des = new Vector3(Mathf.Clamp(des.x, boundCenter.x - bounds.x / 2, boundCenter.x + bounds.x / 2), Mathf.Clamp(des.y, boundCenter.y - bounds.y / 2, boundCenter.y + bounds.y / 2), 0);
            float newAng = Mathf.MoveTowardsAngle(
                rb.rotation,
                (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg),
                500


            );
            rb.MoveRotation(newAng);
            for (int i = 0; i < 2; i++)
            {
                if (plr.GetComponent<plrMovement>().dying || dead)
                {
                    break;
                }
                thing.transform.localPosition = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
                yield return new WaitForSeconds(0.1f);
            }

            float start = Time.time;

            thing.GetComponent<Animator>().SetBool("biting", true);

            GetComponent<OuchBox>().active = true;

            while ((transform.position - des).magnitude > 0.5f && Time.time - start < 1f)
            {
                rb.linearVelocity = (des - transform.position).normalized * 20;

                yield return new WaitForEndOfFrame();
            }
            rb.linearVelocity = Vector3.zero;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.bite);

            thing.GetComponent<Animator>().SetBool("biting", false);


            GetComponent<OuchBox>().active = false;

            newAng = Mathf.MoveTowardsAngle(
                rb.rotation,
                0,
                500

            );

            rb.MoveRotation(newAng);



            lastAttack = Time.time;
            attacking = false;
            yield return new WaitForEndOfFrame();
        }
    }


    IEnumerator hatAttack()
    {

      
          for (int i = 0; i < 10; i++)
            {
                if (plr.GetComponent<plrMovement>().dying || dead)
                {
                    break;
                }

                StartCoroutine(summonHat());
                yield return new WaitForSeconds(0.5f);
            }
          
        
        lastAttack = Time.time;
        attacking = false;
        yield return new WaitForEndOfFrame();

    }

    IEnumerator pointyHat()
    {
        float angleStep = 360f / 10;
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(summonPointHat(i * angleStep));
            yield return new WaitForSeconds(0.5f);

        }
        lastAttack = Time.time;
        attacking = false;
        yield return new WaitForEndOfFrame();

    }
    IEnumerator summonPointHat(float step)
    {
        
            Vector3 direction = new Vector3(Mathf.Cos(step * Mathf.Deg2Rad), Mathf.Sin(step * Mathf.Deg2Rad), 0);
            GameObject hatt = sharpHatPool.getObj();
            if (hatt != null)
            {
                hatt.SetActive(true);
                hatt.transform.position = transform.position + (direction * 4);
                hatt.transform.rotation = Quaternion.Euler(0, 0, step + 90);
                hatt.GetComponent<Rigidbody2D>().linearVelocity = (direction * -20f);
                yield return new WaitForSeconds(2f);
                hatt.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            hatt.SetActive(false);

        }



        yield return new WaitForEndOfFrame();
    }




    
        IEnumerator summonHat()
    {
        int left = Random.Range(0, 2);
        if (left == 0)
        {
            left = -1;
        } else
        {
            left = 1;
        }
        Vector3 location = boundCenter + new Vector3(bounds.x / 2 * left, Random.Range(-bounds.y / 2, bounds.y / 2), Random.Range(-bounds.z / 2, bounds.z / 2));
        GameObject spik = hatPool.getObj();
        if (spik != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.summon);
            spik.GetComponent<SpriteRenderer>().sprite = spik.GetComponent<Hat>().hats[Random.Range(0, spik.GetComponent<Hat>().hats.Count)];

            spik.SetActive(true);
            spik.transform.position = location;
            spik.GetComponent<Rigidbody2D>().linearVelocity = new Vector3(-10 * left, 0, 0);
            yield return new WaitForSeconds(1f);
            spik.GetComponent<Rigidbody2D>().linearVelocity = new Vector3(0, 0, 0);
            spik.SetActive(false);
        }
    }



    public void damage(int amt)
    {

        health = Mathf.Clamp(health - amt, 0, MaxHealth);
        if (health == 0 && !dead)
        {
            StartCoroutine(die());
        }
        hurtPars.Emit(20);
    }

    public IEnumerator die()
    {
        dead = true;
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>())
        {


            t.enabled = false;

        }

        foreach (Transform po in hatPool.transform)
        {

            if (po.gameObject.activeSelf)
            {
                po.gameObject.SetActive(false);
            }

        }
        foreach (Transform po in sharpHatPool.transform)
        {

            if (po.gameObject.activeSelf)
            {
                po.gameObject.SetActive(false);
            }

        }
        IHan.instance.titleCard = true;

        float angleStep = 360f / worth;
        List<GameObject> coins = new List<GameObject>();
        for (int i = 0; i < worth; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            GameObject coin = coinPool.getObj();
            if (coin != null)
            {
                coin.SetActive(true);
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(direction * 10f, ForceMode2D.Impulse);
                coin.GetComponent<coinDissa>().dissPoint = coinImage;
                coins.Add(coin);
            }
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < coins.Count; i++)
        {
            yield return new WaitForSeconds(1 / coins.Count);
            coins[i].GetComponent<Rigidbody2D>().linearDamping = 0;

            coins[i].GetComponent<Rigidbody2D>().linearVelocity = (coinImage.transform.position - coins[i].transform.position).normalized * 10f;

        }
        yield return new WaitForSeconds(2f);
        BossFight.instance.d = true;
        yield return GameObject.Find("BattleScreen").GetComponent<BattleBox>().toggleBox(false, gameObject);
        GameObject.Find("BattleScreen").SetActive(false);
        IHan.instance.titleCard = false;

        Destroy(gameObject); 
    }
}
