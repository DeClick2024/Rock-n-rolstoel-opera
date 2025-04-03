using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class SetOSCIPAddress : MonoBehaviour
{
    //adresses
    public const string OSC_GET_IP = "/rocknrollstoelIP";

    [Header("OSC Settings")]
    public OSCTransmitter Transmitter;
    public OSCReceiver Receiver;

    // Start is called before the first frame update
    void Start()
    {
        Receiver.Bind(OSC_GET_IP, ReceivedIP);
    }

    /*
     * Other host sends message with UDP broadcast 255.255.255.255 to give IP address
     * Address of Transmitter is set to that addres
     */
    void ReceivedIP(OSCMessage message)
    {
        string otherIPAddress = message.Ip.ToString();
        Debug.Log("IPAddress OSC transmitter set to:" + otherIPAddress);
        if (otherIPAddress != Transmitter.RemoteHost)
        {
            Transmitter.RemoteHost = otherIPAddress.ToString();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
