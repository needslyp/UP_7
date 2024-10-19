using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class MuteController : MonoBehaviour
{
    private bool muted;
    private Image currentImage;
    public AudioSource mainSound;
    public AudioSource clickSound;
    public AudioSource raidSound;
    public AudioSource timerSound;
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
        if (muted) {
            mainSound.mute = false;
            clickSound.mute = false;
            raidSound.mute = false;
            timerSound.mute = false;
        }
        else {
            mainSound.mute = true;
            clickSound.mute = true;
            raidSound.mute = true;
            timerSound.mute = true;
        }
        muted = !muted;
        ChangeSprite();
    }
}
