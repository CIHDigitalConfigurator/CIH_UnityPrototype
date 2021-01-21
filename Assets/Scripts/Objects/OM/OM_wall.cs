using System.Collections.Generic;
using static OM.OM_utils;

namespace OM
{

    public class OM_wall
    {
        // Dimensions
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }

        // Rotations
        public Rotations axis_x { get; set; }
        public Rotations axis_y { get; set; }
        public Rotations axis_z { get; set; }

        // List of possible allowed collision objects
        public List<string> allowed_colliders { get; set; }
        

    }
}
