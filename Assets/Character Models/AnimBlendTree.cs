using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBlendTree : MonoBehaviour
{

    public GameObject player;
    public Animator anim;
    public string playerSpeed;

    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", PlayerLocomotionHandler.movementSpeed);
        
    }
}
