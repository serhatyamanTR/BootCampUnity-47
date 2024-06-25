    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Unity.MLAgents;
    using Unity.MLAgents.Sensors;
    using Unity.MLAgents.Actuators;
    using UnityEngine.InputSystem;
    using System.Buffers.Text;
    using System;
    using NUnit.Framework;
    using Random = UnityEngine.Random;

public class MonsterAgent : Agent
{
    public Transform target;
    public float moveSpeed = 1.0f;
    private Vector3 startPosition;
    private int stuckCounter = 0;
    private Vector3 lastPosition;
    private float stuckThreshold = 0.1f; // Bir konum değişikliği algılama eşiği
    private int maxStuckSteps = 5; // Sıkışma adım sayısı

    public override void Initialize()
    {
        startPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startPosition;
        stuckCounter = 0;
        lastPosition = transform.localPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (Time.frameCount % 10 == 0) // Her 10 frame'de bir hareket
        {
            int moveX = actions.DiscreteActions[0];
            int moveZ = actions.DiscreteActions[1];

            Vector3 move = new Vector3(moveX - 1, 0, moveZ - 1);
            transform.localPosition += move * moveSpeed;

            CheckForStuck();

            float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);
            if (distanceToTarget < 1.0f)
            {
                SetReward(1.0f);
                EndEpisode();
            }
            else
            {
                SetReward(-0.01f);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = (int)Input.GetAxisRaw("Horizontal") + 1;
        discreteActionsOut[1] = (int)Input.GetAxisRaw("Vertical") + 1;
    }

    private void CheckForStuck()
    {
        if (Vector3.Distance(transform.localPosition, lastPosition) < stuckThreshold)
        {
            stuckCounter++;
            if (stuckCounter >= maxStuckSteps)
            {
                // Canavarın sıkıştığını anladığımız nokta
                SetReward(-0.5f);
                EndEpisode();
            }
        }
        else
        {
            stuckCounter = 0;
        }
        lastPosition = transform.localPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            SetReward(-0.1f);
            // Canavarı geri çevir
            Vector3 moveDirection = (transform.localPosition - collision.contacts[0].point).normalized;
            transform.localPosition += moveDirection * moveSpeed;
        }
    }
}