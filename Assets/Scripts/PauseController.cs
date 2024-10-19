using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    private bool paused;
    private Image currentImage;
    public AudioSource sound;
    public Sprite nextImage;

    public void Start(){
        currentImage = GetComponent<Image>();
    }

    private void ChangeSprite(){
        var temp = currentImage.sprite;
        currentImage.sprite = nextImage;
        nextImage = temp;
    }
    public void PauseGame()
    {
        if (paused) {
            Time.timeScale = 1;
            sound.Play();
        }
        else {
            Time.timeScale = 0;
            sound.Pause();
        }
        paused = !paused;
        ChangeSprite();
    }
}
