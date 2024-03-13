using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FadeImage : MonoBehaviour {
 
    // the image you want to fade, assign in inspector
     public void Flash()
    {
        // fades the image out when you click
        StartCoroutine(ImageFade());
    }
    IEnumerator ImageFade()
    {
        // loop over 1 second
        for (float i = 0; i <= .3; i += Time.deltaTime)
        {
            // set color with i as alpha
            if (i > .15f) {
                GetComponent<RawImage>().color = new Color(1, 1, 1, .3f - i * 6);
            }
            else GetComponent<RawImage>().color = new Color(1, 1, 1, i * 6);
            yield return null;
        }
    }
}