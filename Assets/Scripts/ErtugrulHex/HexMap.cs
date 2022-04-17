using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public GameObject hexPrefab;
    // Start is called before the first frame update
    void Start()
    {
        generateMap();
    }
    public void generateMap()
    {
        for (int column = 0; column < 10; column++)
        {
            for (int row = 0;row<10;row++)
            {
                HexObject h = new HexObject(column,row);
                Instantiate(hexPrefab,h.Position(),Quaternion.identity,this.transform);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
