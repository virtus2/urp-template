
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField]
    private GameObject dragon;


    public float angle = 15f;

    private void Awake()
    {
        StartCoroutine(GameStart());
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            Application.Quit();
        }
    }

    private IEnumerator GameStart()
    {
        while (true)
        {
            dragon.transform.Rotate(Vector3.up, angle * Time.deltaTime);
            yield return null;
        }
    }

}
