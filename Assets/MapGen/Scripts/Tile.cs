using NaughtyAttributes;
using NUnit.Framework;
using System;
using UnityEngine;

namespace MapGen
{
    public class Tile : MonoBehaviour
    {
        [Flags]
        public enum ConnectionPoint
        {
            // 3 Connection points per edge
            None = 0,
            
            // Top Edge
            TopLeft = 1 << 0,
            TopMiddle = 1 << 1,
            TopRight = 1 << 2,

            // Left Edge
            LeftTop = 1<<3,
            LeftMiddle = 1<<4,
            LeftBotton = 1<<5,

            // Right Edge
            RightTop = 1<<6,
            RightMiddle = 1<<7,
            RightBotton = 1<<8,

            // Bottom Edge
            BottomLeft = 1<<9,
            BottomMiddle = 1<<10,
            BottomRight = 1<<11,
        }

        [EnumFlags]
        public ConnectionPoint ConnectionPoints;

        [SerializeField]
        private bool CanFlip = false;

        [SerializeField]
        private uint TileWidth = 5;

        [SerializeField]
        private uint TileHeight = 5;

        //    +z
        // -x  T +x
        //    -z 

        private const float gizmoLineLength = 5;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            if(ConnectionPoints.HasFlag(ConnectionPoint.TopMiddle))
            {
                Vector3 top = transform.position + new Vector3(0, 0, TileHeight / 2f);
                Gizmos.DrawLine(top, top + new Vector3(0, 0, gizmoLineLength));
            }
            if (ConnectionPoints.HasFlag(ConnectionPoint.BottomMiddle))
            {
                Vector3 bottom = transform.position - new Vector3(0, 0, TileHeight / 2f);
                Gizmos.DrawLine(bottom, bottom - new Vector3(0, 0, gizmoLineLength));
            }
            if (ConnectionPoints.HasFlag(ConnectionPoint.LeftMiddle))
            {
                Vector3 left = transform.position - new Vector3(TileWidth / 2f, 0, 0);
                Gizmos.DrawLine(left, left - new Vector3(gizmoLineLength, 0, 0));
            }
            if (ConnectionPoints.HasFlag(ConnectionPoint.RightMiddle))
            {
                Vector3 right = transform.position + new Vector3(TileWidth / 2f, 0, 0);
                Gizmos.DrawLine(right, right + new Vector3(gizmoLineLength, 0, 0));
            }
        }
    }
}