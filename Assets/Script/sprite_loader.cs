using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sprite_loader : MonoBehaviour {
    public RectTransform image;
    private List<string> ImageName;
    private int imageNum = 0;

    // Use this for initialization
    void Start()
    {
        ImageName = new List<string>();

        ImageName.Add("1");
        ImageName.Add("2");
        ImageName.Add("3");
        ImageName.Add("4");

        Debug.Log(ImageName.Count);
    }

    public void LoadNextPic(bool val)
    {
      if (val)    // right if true
        {
            imageNum++;
            if (imageNum > ImageName.Count - 1)
                imageNum = 0;
        }
        else
        {
            imageNum--;
            if (imageNum < 0)
                imageNum = ImageName.Count - 1;
        }
        
        string tempName = ImageName[imageNum];

        Sprite mySprite = Resources.Load<Sprite>(tempName);
        if (mySprite)
        {

            image.GetComponent<Image>().sprite = mySprite;
        }
        else
        {
            Debug.LogError("no sprite found ImageName = " + ImageName[imageNum]);
        }
    }
}
