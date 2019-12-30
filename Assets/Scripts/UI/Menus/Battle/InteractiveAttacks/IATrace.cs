using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IATrace : InteractiveAttack {

    // TEST FOR NOW
    public Slider sliderPrefab;
    public Slider pathSliderPrefab;

    List<Slider> sliderList;
    List<Slider> pathSliderList;
    List<Quality> qualityList;
    int currentSliderPos = 0;

    public enum State { AIM, FIRE }
    State state;
    float rotateSpeed = 100f;
    float originalZRot;
    Vector3 originalSliderPos;
    Vector3 originalPathSliderPos;
    float rotateLimit = 20f;
    float launchSpeed = 8f;
    bool isPastOriginal;

    public class SliderTransform
    {
        public SliderTransform(Vector3 _pos, float _zRot)
        {
            pos = _pos;
            zRot = _zRot;
        }
        public Vector3 pos;
        public float zRot; 
    }

    public List<SliderTransform> threeSliders = new List<SliderTransform>()
    {
        new SliderTransform(new Vector3(0f, 300f), -60f),
        new SliderTransform(new Vector3(375f, -350f), 180f),
        new SliderTransform(new Vector3(-375f, -350f), 60f)
    };

    public List<SliderTransform> fourSliders = new List<SliderTransform>()
    {
        new SliderTransform(new Vector3(-375f, 300f), 0f),
        new SliderTransform(new Vector3(375f, 350f), -90f),
        new SliderTransform(new Vector3(425f, -350f), 180f),
        new SliderTransform(new Vector3(-375f, -400f), 90f)
    };

    Dictionary<int, List<SliderTransform>> sliderTransformDict;

    public void Init(List<float> damage, int numOfSides)
    {
        base.Init(damage);
        CreateSliders(numOfSides);
        qualityList = new List<Quality>();
        originalZRot = sliderList[0].transform.eulerAngles.z;
        //originalZRot = (originalZRot > 180) ? originalZRot - 360 : originalZRot;
    }

    void CreateSliders(int numOfSides)
    {
        sliderList = new List<Slider>();
        pathSliderList = new List<Slider>();

        sliderTransformDict = new Dictionary<int, List<SliderTransform>>();
        sliderTransformDict.Add(3, threeSliders);
        sliderTransformDict.Add(4, fourSliders);

        for(int i = 0; i < numOfSides; i++)
        {
            pathSliderList.Add(Instantiate(pathSliderPrefab, parentRectTransform.transform));
            sliderList.Add(Instantiate(sliderPrefab, parentRectTransform.transform));            
        }

        List<SliderTransform> st = sliderTransformDict[numOfSides];

        for(int i = 0; i < st.Count; i++)
        {
            sliderList[i].transform.localPosition = st[i].pos;
            sliderList[i].transform.localRotation = Quaternion.Euler(0, 0, st[i].zRot);
            pathSliderList[i].transform.localPosition = st[i].pos;
            pathSliderList[i].transform.localRotation = Quaternion.Euler(0, 0, st[i].zRot);
        }
        
    }

    void SetState(State newState)
    {
        switch(newState)
        {
            case State.AIM:
                originalZRot = sliderList[currentSliderPos].transform.eulerAngles.z;
                //originalZRot = (originalZRot > 180) ? originalZRot - 360 : originalZRot;
                break;
            case State.FIRE:
                originalSliderPos = sliderList[currentSliderPos].handleRect.position;
                originalPathSliderPos = pathSliderList[currentSliderPos].handleRect.position;
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

        var currentZRot = sliderList[currentSliderPos].transform.eulerAngles.z;
        currentZRot = (originalZRot == 0 && currentZRot > 180) ? currentZRot - 360 : currentZRot;

        if (Mathf.Abs(originalZRot - currentZRot) <= 1f && !isPastOriginal)
            isPastOriginal = true;
        //print("original Z Rot: " + originalZRot);
        //print("current z rot: " + currentZRot);
        //if ((originalZRot - currentZRot >= rotateLimit && rotateSpeed < 0)
        //  || (currentZRot - originalZRot >= rotateLimit && rotateSpeed > 0))
        if (Mathf.Abs(originalZRot - currentZRot) > rotateLimit
          && isPastOriginal)
        {
            rotateSpeed *= -1;
            isPastOriginal = false;
        }

        sliderList[currentSliderPos].transform.Rotate(Vector3.forward, Time.deltaTime * rotateSpeed);
        if (Input.GetButtonDown("Submit"))
            SetState(State.FIRE);
    }

    void LaunchSlider()
    {
        sliderList[currentSliderPos].value += Time.deltaTime * launchSpeed;        

        if(sliderList[currentSliderPos].value >= sliderList[currentSliderPos].maxValue)
        {
            pathSliderList[currentSliderPos].value = pathSliderList[currentSliderPos].maxValue;
            // get projection
            var sliderPath = sliderList[currentSliderPos].handleRect.position - originalSliderPos;
            var pathSliderPath = pathSliderList[currentSliderPos].handleRect.position - originalPathSliderPos;
            
            var angle = Vector3.Angle(sliderPath, pathSliderPath) * Mathf.Deg2Rad;
            var length = Mathf.Sin(angle);
            
            qualityList.Add(GetSliderQuality(length));

            if (currentSliderPos < sliderList.Count - 1)
            {
                currentSliderPos++;
                SetState(State.AIM);
            }
            else
            {
                quality = DetermineQuality();
                SetAttack(GameObject.FindGameObjectWithTag("MainCanvas"));
                Destroy(gameObject);
            }
        }
    }

    Quality GetSliderQuality(float length)
    {
        Quality tempQ;
        if (length < 0.01f)
            tempQ = Quality.PERFECT;
        else if (length < 0.06f)
            tempQ = Quality.GREAT;
        else if (length < 0.1f)
            tempQ = Quality.GOOD;
        else if (length < 0.14f)
            tempQ = Quality.OKAY;
        else
            tempQ = Quality.POOR;

        return tempQ;

    }

    Quality DetermineQuality()
    {
        int qualityInt = 0;
        for(int i = 0; i < qualityList.Count; i++)
        {
            qualityInt += (int)(qualityList[i]);
        }

        int finalQualityInt = (int)(qualityInt / qualityList.Count);
        Quality finalQuality = (Quality)finalQualityInt;
        return finalQuality;
    }
}
