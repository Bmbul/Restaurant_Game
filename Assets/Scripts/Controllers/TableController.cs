using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TableController : MonoBehaviour
{
    internal int currentSetIndex = 0;
    internal int unlockedTableCount;
    [SerializeField] Transform tableSetParent;
    [SerializeField] GameObject[] tableSets;
    public static TableController Instance;
    [SerializeField] GameObject[] navMeshes;

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
        SetUpTableSet();
    }

    internal void SetUpTableSet()
    {
        for (int i = 0; i < tableSets.Length; i++)
        {
            if (i == currentSetIndex)
            {
                tableSets[i].SetActive(true);
                SetUpTables();
            }
            else
                tableSets[i].SetActive(false);
        }
    }
    
    void SetUpTables()
    {
        Transform set = tableSetParent.GetChild(currentSetIndex);
        for (int i = 0; i < unlockedTableCount; i++)
            set.GetChild(i).gameObject.SetActive(true);
        for(int i = 0; i < 4; i++)
        {
            if (i == unlockedTableCount - 1)
                navMeshes[i].SetActive(true);
            else
                navMeshes[i].SetActive(false);
        }
    }
}
