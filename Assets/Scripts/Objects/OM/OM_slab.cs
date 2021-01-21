using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OM.OM_utils;

namespace OM
{

    public class OM_slab
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }

        // positions
        public Rotations axis_x { get; set; }
        public Rotations axis_y { get; set; }
        public Rotations axis_z { get; set; }
    }
}
