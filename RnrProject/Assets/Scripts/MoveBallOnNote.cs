using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace extOSC.Examples
{
    public class MoveBallOnNote : MonoBehaviour
    {
        public Transform[] positions; // Array of positions that the ball can move to
        public float speed = 1.0f;
        public int pitch;
        public int velocity;
        private Vector3 targetPosition;
        public string SendAddress = "/hand0";
        public GameObject ball;
        public GuideBall guideBall;
        

        public string ReceiveNote = "/Note1";
        public string ReceiveVelocity = "/Velocity1";
        public OSCTransmitter Transmitter;
        public OSCReceiver Receiver;

        public int LastReceivedVelocity = 0;
       
        public void MoveToPosition(int index)
        {
            // Set the target position to the position at the given index
            if (index >= 0 && index < positions.Length)
            {
                targetPosition = positions[index].position;
                Debug.Log("Moving to position: " + targetPosition);
            }
        }

        protected virtual void Start()
        {
            targetPosition = transform.position;
            Receiver.Bind(ReceiveNote, ReceivedNote);
            Receiver.Bind(ReceiveVelocity, ReceivedVelocity);
        }
        public void PlayNote()
        {
            SendMidiNote(pitch, velocity);
        }

        public void StopNote()
        {
            SendMidiNote(pitch, 0);
        }
        // Update is called once per frame
        
        private OSCMessage CreateMidiNote(int pitch, int velocity)
        {
            var message = new OSCMessage(SendAddress);
            message.AddValue(OSCValue.Int(pitch));
            message.AddValue(OSCValue.Int(velocity));
            return message;
        }
        private void SendMidiNote(int pitch, int velocity)
        {
            OSCMessage message = CreateMidiNote(pitch, velocity);
            Transmitter.Send(message);
        }
        
        private void ReceivedNote(OSCMessage message)
        {
            //Debug.LogFormat("Received: {0}", message);

            int NoteValue;
            if (message.ToInt(out NoteValue))
            {
                if (NoteValue == pitch)
                {
                    if (LastReceivedVelocity > 0)
                    {
                        int randomIndex = Random.Range(0, guideBall.positions.Length);
                        guideBall.MoveToPosition(randomIndex);
                        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
                        Debug.Log("Received note value: " + NoteValue); // Lauri Code
                    }
                    else
                    {
                       
                    }
                }
            }
        }

        private void ReceivedVelocity(OSCMessage message)
        {
            //Debug.LogFormat("Received: {0}", message);

            int VelocityValue;
            if (message.ToInt(out VelocityValue))
            {
                LastReceivedVelocity = VelocityValue;
            }
        }
    }
}
