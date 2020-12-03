﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashEffect : MonoBehaviour
{
    public SpriteRenderer myRenderer;
    public Attack myAttack;
    public float flashTime = 0.5f;
    public float counter1 = 0;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    // Start is called before the first frame update
    void Start()
    {
        //myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default"); // or whatever sprite shader is being used
    }

    // Update is called once per frame
    void Update()
    {
        if (myAttack.hitted)
        {
            whiteSprite();
            counter1 = flashTime;
            print("flash");
        }
        else
        {
            if (counter1 <= 0)
            {
                normalSprite();
            }
            else
            {
                counter1 -= Time.deltaTime;
            }
        }
    }

    void whiteSprite()
    {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
    }

    void normalSprite()
    {
        myRenderer.material.shader = shaderSpritesDefault;
        myRenderer.color = Color.white;
    }
}
