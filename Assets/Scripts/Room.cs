using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public Links links;

    public int linkedRooms = 0;

    public Vector2Int number = Vector2Int.zero;

    public RoomLinks attachedRooms;











    [System.Serializable]
    public class RoomLinks
    {
        public Room left = null;
        public Room right = null;
        public Room up=null;
        public Room down=null;
    }

    [System.Serializable]
    public class Links
    {
        public bool top;
        public bool bottom;
        public bool left;
        public bool right;
    }
}
