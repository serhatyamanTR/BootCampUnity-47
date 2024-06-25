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

    public class Labirent_Runner_Ai_script : Agent
    {
        private Rigidbody   rbody;
        public  Transform   Hedef1;
        public  Transform   Hedef2;
        public Transform    labirent;
        public  float       carpan;
        public  float       timeRemain;
            void Start()
            {
                rbody = GetComponent<Rigidbody>();  
            }

        // Update is called once per frame
        void Update()
            {
                if(timeRemain>0)
                    {
                        timeRemain -= Time.deltaTime;
                    }
            }
        public override void OnEpisodeBegin()
            {
                rbody.angularVelocity = Vector3.zero;
                rbody.velocity  = Vector3.zero;
                transform.localPosition = new Vector3(0,1,0);
                Debug.Log("Labirent  Lokasyonu = "+ labirent.transform.localPosition);
                

            }
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(Hedef1.localPosition);
            sensor.AddObservation(Hedef2.localPosition);
            sensor.AddObservation(transform.localPosition);

            sensor.AddObservation(rbody.velocity.x);
            sensor.AddObservation(rbody.velocity.z);
        }
        public override void OnActionReceived(ActionBuffers actions)
        {
            var moveX = actions.DiscreteActions[0]; // 0: İleri, 1: geri,
            var moveY = actions.DiscreteActions[1]; // 2: Sola, 3: Sağa

            Vector3 move = Vector3.zero;

            switch (moveX)
                {
                    case 1:
                        move = new Vector3(0, 0, 1);
                        break;
                    case 2:
                        move = new Vector3(0, 0, -1);
                        break;
                }

            switch (moveY)
                {
                    case 3:
                        move = new Vector3(-1, 0, 0);
                        break;
                    case 4:
                        move = new Vector3(1, 0, 0);
                        break;
                }
            
            

            rbody.velocity = move*carpan;

            float   HedefeFark1  =  Vector3.Distance(transform.localPosition,Hedef1.localPosition);
            float   HedefeFark2 =  Vector3.Distance(transform.localPosition,Hedef2.localPosition);
            Debug.Log("Ai  Lokasyonu = "+ transform.localPosition);

            if (timeRemain <=0)
                {
                    SetReward(-1f);
                    EndEpisode();
                }

        }

        // Ödül ve ceza sistemi
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("target"))
                {
                    SetReward(1.0f);  // Hedefe ulaştığında ödül ver
                    EndEpisode();
                }
            else if (other.CompareTag("wall"))
                {
                    SetReward(-1.0f);  // Duvara çarptığında ceza ver
                    EndEpisode();
                }
        }

    private Vector3 YonDegistir(Vector3 currentMove)
        {
            // Sağ el kuralına göre 90 derece döndür
            return Vector3.Cross(currentMove, Vector3.up);
        }


        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut[0] = (int)Input.GetAxisRaw("Vertical");
            discreteActionsOut[1] = (int)Input.GetAxisRaw("Horizontal");
        }
    }
