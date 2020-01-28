using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Churchill : MonoBehaviour
{
    [Tooltip("Set to true if you want the machine to power on at runtime")]
    public bool startWithPowerOn;
    [Tooltip("Set to true if you want everything to power down after correct code is guessed")]
    public bool powerDownAfterAffirmative;
    [Tooltip("The correct code for the machine, 4 digits")]
    public int correctCode;
    [Tooltip("Set to true if you want the correct code to be randomized")]
    public bool randomizeCode;

    public List<int> correctCodeList;
    public List<TubeControl> tubeControls;
    public GameObject lights;
    public List<Light> indicators;
    public Light readout1;
    public int inputCode;
    public int currentTubeNumber;

    public GameObject offButton;
    public GameObject onButton;

    public Animator gate;
    public int currentInput;
    public bool correctCodeEntered;
    public bool firstTry;
    public int tries;
    public int currentTry;
    public int highNumber;
    public int lowNumber;


    //variables for guage dial
    public GameObject guageDial;
    public float rotationTarget;
    public float dialRotationSpeed;
    public bool rotating;
    public Quaternion destinationRotation;
    public float currentRotation;
    public Quaternion currentRotationQuant;
    public NumberWheel dialControl;

    // Start is called before the first frame update
    void Start()
    {

        //allow the user to choose the code to be randomized
        if (randomizeCode)
        {
            correctCode = Random.Range(1000, 9999);
        }

        //convert the correct code to a list of integers
        while (correctCode > 0)
        {
            correctCodeList.Add(correctCode % 10);
            correctCode /= 10;
        }

        correctCodeList.Reverse();

        //set the current tube number to default (0)
        currentTubeNumber = 0;
        tries = 5;
        currentTry = 5;



        if (startWithPowerOn == true)
        {

            readout1.gameObject.SetActive(true);
            lights.SetActive(true);

            tubeControls[currentTubeNumber].PowerOn();

        }

        dialRotationSpeed = 2;
        currentRotation = -270;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();

    }

    public void PowerOn()
    {
        lights.SetActive(true);

        readout1.gameObject.SetActive(true);

        for (int i = 0; i < tubeControls.Count; i++)
        {
            if (i <= currentTubeNumber)
            {
                tubeControls[i].PowerOn();
            }
        }

    }

    public void PowerOff()
    {
        lights.SetActive(false);

        for (int i = 0; i < tubeControls.Count; i++)
        {
            tubeControls[i].PowerOff();
        }

        foreach (Light light in indicators)
        {
            light.gameObject.SetActive(false);
        }

        readout1.gameObject.SetActive(false);
    }


    public void checkInput()
    {


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))

                if (hit.transform.name != null)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.transform.tag == "Number")
                    {
                        Debug.Log("Number Hit");
                        currentInput = int.Parse(hit.collider.name);
                        DetermineHighLowNumber(currentInput, hit.collider.name);
                        currentTry = currentTry - 1;
                        dialControl.RotateWheel(tries, currentTry);


                        if (currentTubeNumber == 4)
                        {
                            Debug.Log("WIN");
                        }
                    }

                    if (hit.transform.name == "PowerOff")
                    {
                        PowerOff();
                        onButton.transform.localPosition = new Vector3(onButton.transform.localPosition.x, 0.2713f, onButton.transform.localPosition.z);
                        offButton.transform.localPosition = new Vector3(offButton.transform.localPosition.x, 0.2774984f, offButton.transform.localPosition.z);
                    }

                    if (hit.transform.name == "PowerOn")
                    {
                        PowerOn();
                        onButton.transform.localPosition = new Vector3(onButton.transform.localPosition.x, 0.2774984f, onButton.transform.localPosition.z);
                        offButton.transform.localPosition = new Vector3(offButton.transform.localPosition.x, 0.2713f, offButton.transform.localPosition.z);
                    }
                }
        }
    }

    public void ClearTubes()
    {
        for (int i = 0; i < tubeControls.Count; i++)
        {
            tubeControls[i].ClearTube();
            currentTubeNumber = 0;
        }
    }

    public void DetermineHighLowNumber(int numberEntered, string buttonPressed)
    {

        if (currentTry == tries)
        {
            //if the correct number is less than the current input number set the low number to be the lowest it can be
            if (correctCodeList[currentTubeNumber] < numberEntered)
            {
                lowNumber = 0;
                highNumber = numberEntered;
                StartCoroutine(UpdateIndicators(lowNumber, highNumber, numberEntered));

            }

            if (correctCodeList[currentTubeNumber] > numberEntered)
            {
                lowNumber = numberEntered;
                highNumber = 9;
                StartCoroutine(UpdateIndicators(lowNumber, highNumber, numberEntered));
            }
        }

        if (currentTry != tries)
        {
            if (correctCodeList[currentTubeNumber] < numberEntered)
            {
                highNumber = numberEntered;
                StartCoroutine(UpdateIndicators(lowNumber, highNumber, numberEntered));
            }

            if (correctCodeList[currentTubeNumber] > numberEntered)
            {
                lowNumber = numberEntered;
                StartCoroutine(UpdateIndicators(lowNumber, highNumber, numberEntered));
            }
        }

        if (correctCodeList[currentTubeNumber] == numberEntered)
        {
            tubeControls[currentTubeNumber].LightNumber(buttonPressed);
            currentTubeNumber++;
            
            tries = tries - 1; 
            currentTry = tries;

            //turn all the indicators back on
            for (int i = 0; i < indicators.Count; i++)
            {
                indicators[i].gameObject.SetActive(true);
            }

            tubeControls[currentTubeNumber].PowerOn();

        }
    }

    public IEnumerator UpdateIndicators(int lowNumber, int highNumber, int numberEntered)
    {
        indicators[numberEntered].gameObject.SetActive(false);

        for (int i = 0; i < indicators.Count; i++)
        {
            if (i < lowNumber || i > highNumber)
            {
                indicators[i].gameObject.SetActive(false);
            }

        }

        yield return null;
    }

}
