using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DrawLine : MonoBehaviour
{
    [SerializeField] Transform first;
    [SerializeField] Transform second;

    void Start()
    {
        
    }

    private void OnGUI()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(first.position, second.position, Color.red, 0.5f);
    }
}
