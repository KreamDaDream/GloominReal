using UnityEngine;

public class swordplace : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject sword;
    private bool taken;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (taken)
        {
            if (sword != null)
            {
                Destroy(sword);
            }
        } else
        {
            if (sword == null)
            {
                taken = true;
            }
        }
    }

    public void LoadData(GameData data)
    {
        taken = data.swordTaken;
    }

    public void SaveData(ref GameData data)
    {
        data.swordTaken = taken;

    }
}
