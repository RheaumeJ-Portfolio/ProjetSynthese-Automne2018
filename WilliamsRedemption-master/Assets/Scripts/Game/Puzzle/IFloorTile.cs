﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Puzzle
{
    public delegate void OnDestinationReachedHandler();

    public abstract class IFloorTile : MonoBehaviour
    {
        public abstract void MoveUp();
        public abstract void MoveDown();
    }
}
