using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI levelText;

    public float fadeSpeed = 1f;

    public void FadeToScene(string sceneName, string levelName)
    {
        StartCoroutine(FadeRoutine(sceneName, levelName));
    }

    IEnumerator FadeRoutine(string sceneName, string levelName)
    {
        // FADE OUT
        yield return StartCoroutine(Fade(1));

        SceneManager.LoadScene(sceneName);

        // küçük bekleme (scene otursun)
        yield return new WaitForSeconds(0.2f);

        // LEVEL TEXT
        if (levelText != null)
        {
            levelText.text = levelName;
            yield return StartCoroutine(FadeText(1));

            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(FadeText(0));
        }

        // FADE IN
        yield return StartCoroutine(Fade(0));
    }

    IEnumerator Fade(float target)
    {
        float t = fadeImage.color.a;

        while (!Mathf.Approximately(t, target))
        {
            t = Mathf.MoveTowards(t, target, Time.deltaTime * fadeSpeed);
            fadeImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }

    IEnumerator FadeText(float target)
    {
        Color c = levelText.color;
        float t = c.a;

        while (!Mathf.Approximately(t, target))
        {
            t = Mathf.MoveTowards(t, target, Time.deltaTime * fadeSpeed);
            levelText.color = new Color(c.r, c.g, c.b, t);
            yield return null;
        }
    }
}