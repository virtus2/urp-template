using rds;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILootTableEntry
{
    void OnPreResultEvaluation(EventArgs e);
    void OnHit(EventArgs e);
    void OnPostResultEvaluation(ResultEventArgs e);
    string ToString(int indentationlevel);
}

[System.Serializable]
public class LootTableEntry : ILootTableEntry
{
    public double Probability;
    public bool UniqueDrop;
    public bool DropAlways;
    public bool DropEnabled;
    public LootTable LootTable;

    public void OnHit(EventArgs e)
    {
    }

    public void OnPreResultEvaluation(EventArgs e)
    {
    }

    public void OnPostResultEvaluation(ResultEventArgs e)
    {
    }

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int indentationlevel)
    {
        string indent = "".PadRight(4 * indentationlevel, ' ');

        return string.Format(indent + "(RDSObject){0} Prob:{1},UAE:{2}{3}{4}",
            this.GetType().Name, Probability,
            (UniqueDrop ? "1" : "0"), (DropAlways? "1" : "0"), (DropEnabled? "1" : "0"));
    }
}

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptable Objects/LootTable")]
public class LootTable : ScriptableObject
{
    public List<LootTableEntry> Loots;
}
