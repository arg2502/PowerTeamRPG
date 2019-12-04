using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IATrace : InteractiveAttack {

	// TEST FOR NOW
	public Slider slider1, slider2, slider3;
	public Image image1, image2, image3;

    List<Slider> sliderList;
    int currentSliderPos = 0;

    public enum State { AIM, FIRE }
    State state;
    float rotateSpeed = 50f;
    float originalZRot;
    float rotateLimit = 10f;
    float launchSpeed = 8f;

    public void Init(List<float> damage)
    {
        base.Init(damage);
        sliderList = new List<Slider>() { slider1, slider2, slider3 };
        originalZRot = slider1.transform.localEulerAngles.z;
    }

    void SetState(State newState)
    {
        switch(newState)
        {
            case State.AIM:
                originalZRot = sliderList[currentSliderPos].transform.localEulerAngles.z;
                break;
                
        }
        state = newState;
    }

    private void Update()
    {
        if(state == State.AIM)
        {
            RotateSlider();
        }
        else if (state == State.FIRE)
        {
            LaunchSlider();
        }
    }

    void RotateSlider()
    {
        sliderList[currentSliderPos].transform.Rotate(Vector3.forward, Time.deltaTime * rotateSpeed);

        var currentZRot = sliderList[currentSliderPos].transform.localEulerAngles.z;
        if ((originalZRot - currentZRot >= rotateLimit && rotateSpeed < 0)
            || (currentZRot - originalZRot >= rotateLimit && rotateSpeed > 0))
            rotateSpeed *= -1;

        if (Input.GetButtonDown("Submit"))
            SetState(State.FIRE);
    }

    void LaunchSlider()
    {
        sliderList[currentSliderPos].value += Time.deltaTime * launchSpeed;

        if(sliderList[currentSliderPos].value >= sliderList[currentSliderPos].maxValue)
        {
            if (currentSliderPos < sliderList.Count - 1)
            {
                currentSliderPos++;
                SetState(State.AIM);
            }
            else
            {
                quality = Quality.PERFECT; // test
                SetAttack(GameObject.FindGameObjectWithTag("MainCanvas"));
                Destroy(gameObject);
            }
        }
    }
}
