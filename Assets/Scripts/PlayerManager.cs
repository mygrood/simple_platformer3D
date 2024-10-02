using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int playerIndex = 0;
        
        public int GetPlayerIndex()
        {
            return playerIndex;
        }
        
    }
}
