using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    public float FadeRate;
    private Image image;
    private float targetAlpha;
    public delegate void DoneFade();
    public event DoneFade onFadeDone;
    void Start()
    {
        this.image = this.GetComponent<Image>();
        if (this.image == null)
        {
            Debug.LogError("Error: No image on " + this.name);
        }
        this.targetAlpha = this.image.color.a;
    }

    IEnumerator FadeControl()
    {
        while(true)
        {
            Color curColor = this.image.color;
            float alphaDiff = Mathf.Abs(curColor.a - this.targetAlpha);
            if (alphaDiff > 0.1f)
            {
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, this.FadeRate * Time.deltaTime);
                this.image.color = curColor;
            }
            else
            {
                Debug.Log("Stopping fade");
                if (onFadeDone != null){
                    onFadeDone();
                }
                yield break;

            }

            yield return null;
        }
    }

    public void FadeOut()
    {
        this.targetAlpha = 0.0f;
        StartCoroutine("FadeControl");
    }

    public void FadeIn()
    {
        this.targetAlpha = 1.0f;
        StartCoroutine("FadeControl");
    }
}
