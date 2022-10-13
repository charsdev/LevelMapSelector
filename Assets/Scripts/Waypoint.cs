using System.Collections.Generic;
using UnityEngine;

public class Waypoint : Node
{
    public List<Path> Paths = new List<Path>();

    public Path GetPath(Direction value) => Paths.Find(item => item.Direction == value);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < Paths.Count; i++)
        {
            var nodes = Paths[i].Nodes;
            for (int j = 0; j < nodes.Count; j++)
            {
                if (DebugTools.InBounds(j + 1, nodes))
                {
                    Gizmos.DrawLine(nodes[j].position, nodes[j + 1].position);
                }
            }
        }
    }

}
