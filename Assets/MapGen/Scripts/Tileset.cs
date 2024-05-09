using System.Collections.Generic;
using UnityEngine;

namespace MapGen
{
    [CreateAssetMenu]
    public class Tileset : ScriptableObject
    {
        [SerializeField]
        private List<Tile> tiles;

        private Dictionary<Tile.ConnectionPoint, List<Tile>> ruleTiles = new();

        private void OnEnable()
        {
            foreach(var t in tiles)
            {
                if (!ruleTiles.ContainsKey(t.ConnectionPoints))
                    ruleTiles[t.ConnectionPoints] = new List<Tile>();
                ruleTiles[t.ConnectionPoints].Add(t);
            }
        }
    }
}