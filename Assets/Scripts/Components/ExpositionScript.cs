using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpositionScript : MonoBehaviour
{
    public string nextScene = "GGJ";
    public List<GameObject> slides = new List<GameObject>();
    public Animator transition = null;
    public float SlideTime = 2f;
    [SerializeField] private float slideTimer = 0f;
    [SerializeField] private int index = 0;

    public void ChangeSlide()
    {
        slides[index].SetActive(false);
        index++;
        if (index < slides.Count)
        {
            slides[index].SetActive(true);
        }
        else
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeScene(nextScene);
            }
        }
    }
}
