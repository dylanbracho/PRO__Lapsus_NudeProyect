using UnityEngine;

public class condicionspawn : MonoBehaviour
{
    [SerializeField] GameObject trampa;
    [SerializeField] GameObject spawn;
    [SerializeField] GameObject spawnTrampa;
    [SerializeField] PlayerController pC;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trampa.activeInHierarchy)
        {
            spawn.SetActive(false);
            spawnTrampa.SetActive(true);
            pC.spawnpoint = spawnTrampa;
        }
        else
        {
            spawn.SetActive(true);
            spawnTrampa.SetActive(false);
            pC.spawnpoint = spawn;
        }
    }
}
