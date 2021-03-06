﻿using UnityEngine;
using System.Collections;

public class StarTexture : MonoBehaviour
{
  public float scale = 100f;
  public float seed = 1337f;

  public GameObject target;
  public float spaceMovementRatio = 0.0001f;

  public float x;
  public float y;

  [Range(0f, 1f)]
  public float density = 0.9f;

  private Color[] buffer;
  private Texture2D texture;
  private Material material;

  private int width = 0;
  private int height = 0;

  public void Awake()
  {
    this.width = Screen.width * 4;
    this.height = Screen.height * 4;

    float distance = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);
    float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
    float width = height * Screen.width / Screen.height;

    transform.localScale = new Vector3(width / 10f, 1.0f, height / 10f);

    Renderer r = GetComponent<Renderer>();
    material = new Material(Shader.Find("Unlit/Transparent"));
    r.material = material;
    buffer = new Color[this.width * this.height];

    material.mainTexture = GenerateTexture();
  }

  public void Update()
  {
    if (target != null)
    {
      x = target.transform.position.x * spaceMovementRatio;
      y = target.transform.position.y * spaceMovementRatio;
    }
    material.SetTextureOffset("_MainTex", new Vector2(x, y));
  }

  private Texture2D GenerateTexture()
  {
    Texture2D texture = new Texture2D(width, height);

    float y = 0f;

    while (y < texture.height)
    {
      float x = 0f;
      while (x < texture.width)
      {
        float xCoord = this.x + x / texture.width * scale;
        float yCoord = this.y + y / texture.height * scale;

        float sample = Mathf.PerlinNoise(xCoord + seed, yCoord + seed);
        Color color = sample >= density ? Color.white : new Color(0f, 0f, 0f, 0f);

        buffer[(int)y * texture.width + (int)x] = color;
        x++;
      }
      y++;
    }

    texture.SetPixels(buffer);
    texture.Apply();

    texture.filterMode = FilterMode.Point;
    texture.wrapMode = TextureWrapMode.Repeat;

    return texture;
  }
}
