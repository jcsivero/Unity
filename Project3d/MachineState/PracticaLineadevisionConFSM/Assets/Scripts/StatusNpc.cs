using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(UnityEngine.AI.NavMeshAgent))]
public class StatusNpc : Status
{
    public string name_ = "StatusNpc";
    public bool useNavMeshAI_ = true;
    public bool useNavMeshTarget_ = true;    
    public Animator anim_;
    public UnityEngine.AI.NavMeshAgent agentNavMesh_;

    public CommandAddOrSubEnemy commandAddOrSubEnemy_;
    
   
    public float rotationSpeed_ = 2.0f;
    public float speed_ = 2.0f;
    public float accuracyToWayPoints_ = 1.0f;
    public float visDist_ = 20.0f;
    public float visAngle_ = 30.0f;
    public float visDistToAttack_ = 10.0f;
     
    public int currentWP_;
    public float currentSpeedAI_;
    public float currentSpeedTarget_;    
    public override string GetName()
    {
        return name_;
    }
    public override UnityEngine.AI.NavMeshAgent GetAgentNavMesh()
    {
        return agentNavMesh_;
    }
    public override void InstaciateCommands()
    {
        InstaciateCommands();
        commandAddOrSubEnemy_ = new CommandAddOrSubEnemy();


    }
        /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        InstaciateCommands();       

        
        anim_ = gameObject.GetComponent<Animator>();                        
        agentNavMesh_ = GetComponent<UnityEngine.AI.NavMeshAgent>();


    }
   public override void Start()
    {
       SetTarget(GetStatusWorld().GetTarget());
        
    }
}
