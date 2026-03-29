using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class obJBev : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject cill;
    [SerializeField] private AnimationCurve up;

    private Vector3 ogPos;
    void Start()
    {
        ogPos = cube.transform.localPosition;
        cube.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.5f, 0.25f, 0.33f);
        StartCoroutine(upC());
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.localPosition = ogPos + new Vector3(0, Mathf.Sin(Time.time), 0);

    }

    IEnumerator upC()
    {
        for (float i = 0; i < 100; i++)
        {
            gameObject.transform.position = new Vector3(1.34f, (up.Evaluate(i / 100) * 10) - 8, 41.5f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    
}
