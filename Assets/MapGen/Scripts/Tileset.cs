using System.Collections.Generic;
using UnityEngine;

namespace MapGen
{
    [CreateAssetMenu]
    public class Tileset : ScriptableObject
    {
        [SerializeField]
        private List<Tile> Tiles;

        public Dictionary<Tile.ConnectionPoint, List<Tile>> ruleTiles = new();

        public void Initialize()
        {
            foreach (var t in Tiles)
            {
                if (!ruleTiles.ContainsKey(t.ConnectionPoints))
                    ruleTiles[t.ConnectionPoints] = new List<Tile>();
                ruleTiles[t.ConnectionPoints].Add(t);
            }
        }
    }
}