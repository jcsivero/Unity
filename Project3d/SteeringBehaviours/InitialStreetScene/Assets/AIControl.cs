﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

	public GameObject[] goalLocations;
	UnityEngine.AI.NavMeshAgent agent;
	Animator anim;


	// Use this for initialization
	void Start () {
		goalLocations = GameObject.FindGameObjectsWithTag("goal");
		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.SetDestination(goalLocations[Random.Range(0,goalLocations.Length)].transform.position);
		anim =  GetComponent<Animator>();
		anim.SetFloat("wOffset",Random.Range(0.0f,1.0f));		
		anim.SetTrigger("isWalking");
		float sm = Random.Range(0.1f,2.0f);
		anim.SetFloat("speedMult",sm);
		agent.speed *= sm;
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < 2)
		{
			agent.SetDestination(goalLocations[Random.Range(0,goalLocations.Length)].transform.position);
			//anim.SetTrigger("isIdle");
		}
	}
}
