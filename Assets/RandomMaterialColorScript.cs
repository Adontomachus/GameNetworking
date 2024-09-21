using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using GNW2.GameManager;
using UnityEngine;
public class RandomMaterialColorScript : NetworkBehaviour
{


    public MeshRenderer renderer;
    public Color NetworkedColor { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        ChangeColors();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.color = NetworkedColor;
        if (GameManager.changeColors == true)
        {
            Debug.Log("Color Changed!");
            ChangeColors();

        }

    }
    public void ChangeColors()
    {
        NetworkedColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        GameManager.changeColors = false;
        
    }
}
