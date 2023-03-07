using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnController : MonoBehaviour
{
    public static CustomerSpawnController Instance;

    [SerializeField] GameObject maleCustomerPrefab;
    [SerializeField] GameObject femalePrefab;
    [SerializeField] Transform[] tableSpawnPoints;
    [SerializeField] Transform[] tableFoodSpawnPoints;

    bool[] tableIsTaken;
  
    int maxCustomerCount;
    int currentCustomerCout = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.5f);
        tableIsTaken = new bool[tableSpawnPoints.Length];
        maxCustomerCount = 2 * TableController.Instance.unlockedTableCount;
        StartCoroutine(SpawnCustomers(TableController.Instance.unlockedTableCount));
        StartCoroutine(SittingTable());
    }

    IEnumerator SittingTable()
    {
        yield return new WaitForSeconds(20);
        maxCustomerCount = 2 * TableController.Instance.unlockedTableCount;
        for (int i = 0; i < maxCustomerCount; i++)
        {
            if (tableIsTaken[i] == false)
            {
                yield return new WaitForSeconds(5);
                CreateCustomer(i);
            }
        }
        yield return null;
    }

    IEnumerator SpawnCustomers(int spawnCount)
    { 
        int index;

        while (spawnCount > 0)
        {
            index = Random.Range(0, maxCustomerCount);
            while (tableIsTaken[index])
            {
                index = Random.Range(0, maxCustomerCount);
            }
            CreateCustomer(index);
            spawnCount--;
            yield return new WaitForSeconds(6f);
        }
    }

    public IEnumerator WaitandCreateCustomer(int i)
    {
        yield return new WaitForSeconds(5);
        Debug.Log("works");
        CreateCustomer(i);
    }
    
    void CreateCustomer(int i)
    {
        GameObject spawningGameObject;
        if (Random.Range(0, 2) == 0)
        {
            spawningGameObject = Instantiate(maleCustomerPrefab);
        }
        else
        {
            spawningGameObject = Instantiate(femalePrefab);
        }
        tableIsTaken[i] = true;
        spawningGameObject.GetComponent<CustomerController>().SetDestination(tableSpawnPoints[i], i,tableFoodSpawnPoints[i]);
    }
}


