using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchillBU : MonoBehaviour
{
    [Tooltip("Set to true if you want the machine to power on at runtime")]
    public bool startWithPowerOn;
    [Tooltip("Set to true if you want everything to power down after correct code is guessed")]
    public bool powerDownAfterAffirmative;
    [Tooltip("The correct code for the machine, 4 digits")]
    public int correctCode;
    [Tooltip("Set to true if you want the correct code to be randomized")]
    public bool randomizeCode;

    private int correctCodeCopy;
    public List<int> correctCodeList;
    public List<TubeControl> tubeControls;
    public GameObject lights;
    public List<Light> indicators;
    public Light readout1;
    public Color affirmativeColor;
    public Color negativeColor;
    public int inputCode;
    public int currentTubeNumber;
    public bool needCodeCheck;

    public GameObject offButton;
    public GameObject onButton;

    public Animator gate;
    public List<Animator> wheelGates;
    public List<int> currentInput;
    public int numbersChecked;
    public bool correctCodeEntered;
    public List<NumberWheel> numberWheels;





    // Start is called before the first frame update
    void Start()
    {

        correctCodeCopy = correctCode;
        if (startWithPowerOn == true)
        {

            readout1.gameObject.SetActive(true);
            lights.SetActive(true);

            for (int i = 0; i < tubeControls.Count; i++)
            {
                tubeControls[i].tubeLight.intensity = tubeControls[i].maxIntensity;
                tubeControls[i].poweringOn = true;
                tubeControls[i].poweringOff = false;
                tubeControls[i].powerisOff = false;
            }

        }
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
        numbersChecked = 0;


    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    public void PowerOn()
    {
        lights.SetActive(true);
        for (int i = 0; i < tubeControls.Count; i++)
        {
            tubeControls[i].PowerOn();
        }

        readout1.gameObject.SetActive(true);

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
                        //light up tube with the number pressed
                        Debug.Log("Number Hit");
                        tubeControls[currentTubeNumber].LightNumber(hit.collider.name);
                        currentInput.Add(int.Parse(hit.collider.name));
                        currentTubeNumber++;
                        if (currentTubeNumber == 4)
                        {
                            Debug.Log("all numbers populated");
                            StartCoroutine(CheckCode());
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


    public IEnumerator CheckCode()
    {
        Debug.Log("Reached Update Indicators");
        for (int i = 0; i < currentInput.Count; i++)
        {

            //set all indicators to green if the input number matches the correct number
            if (currentInput[i] == correctCodeList[i])
            {
                Debug.Log(i + "Correct");
                indicators[i].gameObject.SetActive(true);
                indicators[i].color = affirmativeColor;
                //open the wheel gate if the number is correct
                wheelGates[i].SetTrigger("Open");
                yield return new WaitForSeconds(0.5f);

            }

            //set all indicators to red if the input number does not match the correct number
           if (currentInput[i] != correctCodeList[i])
            {
                Debug.Log(i + "incorrect");
                indicators[i].gameObject.SetActive(true);
                indicators[i].color = negativeColor;
                yield return new WaitForSeconds(0.25f);
            }

            numbersChecked++;

        }

        if (currentInput[0] == correctCodeList[0] && currentInput[1] == correctCodeList[1] && currentInput[2] == correctCodeList[2] && currentInput[3] == correctCodeList[3])
        {
            correctCodeEntered = true;
        }

        if (correctCodeEntered == true)
        {
            bool currentState = false;
            gate.SetTrigger("OpenGate");
            for (int i = 0; i < 8; i++)
            {

                indicators[0].gameObject.SetActive(currentState);
                indicators[1].gameObject.SetActive(currentState);
                indicators[2].gameObject.SetActive(currentState);
                indicators[3].gameObject.SetActive(currentState);
                yield return new WaitForSeconds(.5f);
                currentState = !currentState;
            }

        }

        if (correctCodeEntered == false)
        {
            //if all number have been checked clear the tubes and indicators
            if (numbersChecked == currentInput.Count)
            {
                yield return new WaitForSeconds(2f);
                foreach (Light l in indicators)
                {
                    l.gameObject.SetActive(false);
                }
                ClearTubes();
                currentInput.Clear();
                numbersChecked = 0;
            }

        }
    }

}
