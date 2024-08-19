
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private Image titleImage;

    [SerializeField]
    private float startTime = 1.0f;

    [SerializeField]
    private float fadeTime = 1.0f;

    private float timeElapsed = 0.0f;

    private void Awake()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(startTime);
        while (timeElapsed <= fadeTime)
        {
            Color color = titleImage.color;
            color.a = Mathf.Lerp(color.a, 1.0f, fadeTime * Time.deltaTime);
            titleImage.color = color;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("GMTK2024", LoadSceneMode.Single);
    }
}
