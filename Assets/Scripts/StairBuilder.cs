using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class StairBuilder : MonoBehaviour
{

    public GameObject stepPrefab;
    public float stepOffset;
    Vector3 stepPosition;
    Queue<GameObject> movableSteps;
    Queue<GameObject> allSteps;
    public UnityEngine.UI.Text distance;
    CharacterController controller;

    public AudioClip bling;
    AudioSource audios;

    int steps;


    public int numberOfSteps;

    Renderer stepRenderer;
    public Color activeStepColor;
    public Color movableStepColor;
    public Color defaultStepColor;
    float origSpeed;

    
    public float playerSpeed;
    public Camera playerCamera;
    public GameObject playerPrefab;

    private GameObject player;

    private bool done;

    private Vector3 playerTargetPosition;

    public float range;

    void Start()
    {
        movableSteps = new Queue<GameObject>();
        stepRenderer = stepPrefab.GetComponent<Renderer>();
        allSteps = new Queue<GameObject>();

        audios = playerCamera.GetComponent<AudioSource>();

        stepPosition = gameObject.transform.position;


        for (int i = 0; i < numberOfSteps; i++)
        {
            Renderer rend;
            GameObject stepInstance = GameObject.Instantiate(stepPrefab);

            rend = stepInstance.GetComponent<Renderer>();
                rend.material.color = movableStepColor;
           // rend.enabled = false;
            stepInstance.transform.parent = gameObject.transform;
            allSteps.Enqueue(stepInstance);
        }

        for (int i = 0; i < numberOfSteps; i++)
        {
            makeStep(true);
        }

            GameObject playerInstance = Instantiate(playerPrefab);

        playerInstance.transform.position = transform.position + transform.TransformDirection(Vector3.up) * (1+ stepRenderer.bounds.size.y);
        player = playerInstance;
        controller = player.GetComponent<CharacterController>();

        playerTargetPosition = stepRenderer.bounds.size.y * transform.TransformDirection(Vector3.up) + transform.TransformDirection(Vector3.forward) * stepRenderer.bounds.size.z;

        playerCamera.transform.position = player.transform.position + (transform.TransformDirection(Vector3.up) * 2) + (transform.TransformDirection(Vector3.back) * 3);
        playerCamera.transform.rotation = Quaternion.AngleAxis(20F, Vector3.right);
        playerCamera.transform.parent = player.transform;
        steps = 0;

        origSpeed = playerSpeed;

    }


    private void makeStep(bool solid)
    {
        GameObject stepInstance = allSteps.Dequeue();
        int offsetDirection = 0;
        float multipy = (steps / 1000f) / 2f;

        float random = Random.Range(0.0f, 1.0f);

        



        if (solid)
        {
            offsetDirection = 0;
        }
        else if(random >= 0.0f && random < 0.10f *(1+ multipy))
        {
            offsetDirection = 1;
        }
        else if (random >= 0.10f * (1 + multipy) && random < 0.20f * (1 + multipy))
        {
            offsetDirection = -1;
        }
        else if (random >= 0.20f * (1 + (2*multipy))  && random <=1.0)
        {
            offsetDirection = 0;
        }



        stepInstance.transform.position = stepPosition + (transform.TransformDirection(Vector3.left) * stepOffset * offsetDirection);
            if (offsetDirection != 0)
            {
            stepInstance.GetComponent<Renderer>().material.color = movableStepColor;
            movableSteps.Enqueue(stepInstance);
            }
            else
            {
                stepInstance.GetComponent<Renderer>().material.color = defaultStepColor;
            }

        allSteps.Enqueue(stepInstance);

    
        stepPosition += (stepRenderer.bounds.size.y * transform.TransformDirection(Vector3.up) + transform.TransformDirection(Vector3.forward) * stepRenderer.bounds.size.z);

    }

    // Update is called once per fram
    void Update()
    {
        if (steps > numberOfSteps) {
            playerSpeed += (steps / numberOfSteps) * 0.0001f;
    }

        controller.Move(playerTargetPosition * playerSpeed * Time.deltaTime);


        if (done == false && movableSteps.Count > 0 && movableSteps.Peek().transform.position.z - player.transform.position.z < range)
        {
            movableSteps.Peek().GetComponent<Renderer>().material.color = activeStepColor;
            done = true;
        }

        RaycastHit hit;

        if (!Physics.Raycast(new Ray(player.transform.position, player.transform.TransformDirection(Vector3.down)), out hit))
        {
            playerTargetPosition = Vector3.zero;
        }
        else {
            playerTargetPosition = transform.TransformDirection(stepRenderer.bounds.size.y * transform.TransformDirection(Vector3.up) + transform.TransformDirection(Vector3.forward) * stepRenderer.bounds.size.z);

        }

        if (stepPosition.z - player.transform.position.z < 10)
        {
            makeStep(false);
        }
        if (playerTargetPosition != Vector3.zero) {
            steps = Mathf.RoundToInt(player.transform.position.z / stepRenderer.bounds.size.z);
              distance.text =  steps.ToString();

            if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (movableSteps.Count > 0)
            {
                moveStep(Vector3.left, stepOffset);
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (movableSteps.Count > 0) {
                    moveStep(Vector3.right, stepOffset);
                }
            }
        }
        }
        else
        {
            distance.text = "Game over :( Press Space to restart!";
        }

        if (Input.GetKey(KeyCode.Space))
        {
            playerSpeed = 0;
            stepPosition = gameObject.transform.position;
            player.transform.position = transform.position + transform.TransformDirection(Vector3.up) * (1 + stepRenderer.bounds.size.y);
            playerTargetPosition = stepRenderer.bounds.size.y * transform.TransformDirection(Vector3.up) + transform.TransformDirection(Vector3.forward) * stepRenderer.bounds.size.z;
            movableSteps.Clear();
            done = false;
            distance.text = "";
            steps = 0;

            for (int i = 0; i < numberOfSteps; i++)
            {
                makeStep(true);
            }

            playerSpeed = origSpeed;
        }

    }

    void moveStep( Vector3 direction, float offset)
    {
        if (movableSteps.Peek().GetComponent<Renderer>().material.color == activeStepColor)
        {
            GameObject stepInstance = movableSteps.Dequeue();
            stepInstance.transform.position += transform.TransformDirection(direction) * offset;
            stepInstance.GetComponent<Renderer>().material.color = defaultStepColor;
            done = false;

            float pitch = Random.Range( 1.0f, 1.15f);

            audios.clip = bling;
            audios.pitch = pitch;
            audios.Play();
           
        }
    }
}
