using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeControl : MonoBehaviour
{
    public List<GameObject> numbers;
    public List<Renderer> numberMaterials;
    public Material litMaterial;
    public Material unlitMaterial;
    public int tracker = 1;
    public Light tubeLight;
    public float speed;
    public float step;
    public float currentIntensity;
    public float maxIntensity;
    public float minIntensity;
    public bool powerisON;
    public bool powerisOff;
    public bool poweringOn;
    public bool poweringOff;

    // Start is called before the first frame update
    void Start()
    {
        speed = .10f;
        maxIntensity = 150;

        for (int i = 0; i < 10; i++)
        {
            numberMaterials.Add(gameObject.transform.GetChild(i).GetComponent<Renderer>());
            numbers.Add(gameObject.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < numberMaterials.Count; i++)
        {
            numberMaterials[i].material = unlitMaterial;
        }

        tubeLight = gameObject.transform.GetChild(17).GetComponent<Light>();
        tubeLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {



        if (poweringOn)
        {
            if (currentIntensity > maxIntensity - 1)
            {
                powerisON = true;
                poweringOn = false;
                powerisOff = false;
            }

            if (powerisON == false)
            {
                currentIntensity = tubeLight.intensity;
                step = speed * Time.deltaTime;
                tubeLight.intensity = Mathf.Lerp(currentIntensity, maxIntensity, speed);
            }

            else
            {
                return;
            }
        }

        if (poweringOff)
        {
            if (currentIntensity < minIntensity + .2f)
            {
                powerisOff = true;
                poweringOff = false;
                powerisON = false;
            }

            if (powerisOff == false)
            {
                currentIntensity = tubeLight.intensity;
                step = speed * Time.deltaTime;
                tubeLight.intensity = Mathf.Lerp(currentIntensity, minIntensity, speed);
            }

            else
            {
                return;
            }
        }

    }

    private void OnGUI()
    {

    }


    public void PowerOn()
    {
        poweringOn = true;
        poweringOff = false;
        powerisOff = false;
    }

    public void PowerOff()
    {
        poweringOff = true;
        poweringOn = false;
        powerisON = false;
    }

    public void LightNumber(string number)
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            numbers[i].SetActive(true);
            if (number == "6" || number == "7")
            {
                numbers[3].SetActive(false);
                numbers[5].SetActive(false);
                numbers[4].SetActive(false);
            }


            if (number == "8" || number == "9")
            {
                numbers[2].SetActive(false);
                numbers[3].SetActive(false);
                numbers[5].SetActive(false);
                numbers[4].SetActive(false);
                numbers[6].SetActive(false);

            }


            if (numberMaterials[i].name == number.ToString())
            {
                numberMaterials[i].material = litMaterial;
            }

            else
            {
                numberMaterials[i].material = unlitMaterial;



            }
        }
    }
    

    //clear the tube
    public void ClearTube()
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            numberMaterials[i].material = unlitMaterial;
        }
    }
}
