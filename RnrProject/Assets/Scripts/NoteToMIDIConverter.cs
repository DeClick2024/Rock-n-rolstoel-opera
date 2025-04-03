using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteToMIDIConverter 
{
    [SerializeField] private string noteName; //could be public, need to think about it

    private int _noteValue;

    static Dictionary<string, int> noteDictionary = new Dictionary<string, int>()
    {
        {"C", 24 },
        {"C#", 25 },
        {"D", 26 },
        {"D#", 27 },
        {"E", 28 },
        {"F", 29 },
        {"F#", 30 },
        {"G", 31 },
        {"G#", 32 },
        {"A", 33 },
        {"A#", 34 },
        {"B", 35 }
    };

    /// <summary>
    /// Converts a note name to its MIDI value
    /// </summary>
    public static int NoteToValue(string noteName)
    {
        string note = "";
        string pitch = "";

        for (int i=0; i<noteName.Length;i++)
        {
            if (noteName[i] >= 65 && noteName[i] <= 71 || noteName[i] >= 97 && noteName[i] <= 103 || noteName[i] == 35) note += noteName[i];
            else if (noteName[i] >= 48 && noteName[i] <= 56 || noteName[i] == 45) pitch += noteName[i];
        }

        Int32.TryParse(pitch, out int octaveValue);
        note = note.ToUpper();
        if (note == "") return 0;
        return noteDictionary[note] + (octaveValue * 12);
    }

    /// <summary>
    /// Converts a MIDI value to its note name
    /// </summary>
    public static string ValueToNode(int pitchValue)
    {
        int octaveValue = (pitchValue - 24) / 12;
        int noteNameValue = pitchValue % 12 + 24;
        string noteName = "";
        foreach (KeyValuePair<string, int> note in noteDictionary)
        {
            if (note.Value == noteNameValue)
            {
                noteName = note.Key;
                break;
            }
        }
        noteName += octaveValue.ToString();
        return noteName;
    }
}