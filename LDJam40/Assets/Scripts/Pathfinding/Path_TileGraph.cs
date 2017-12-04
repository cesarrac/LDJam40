using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Path_TileGraph  {


    //          0 -------- 0   <----- 0 = Node / Tile
    //          |          |
    //          |          |   <----- | / - = Edge
    //          |          |
    //          0 -------- 0      

    public Dictionary<Tile, Path_Node<Tile>> nodes; 


    public Path_TileGraph(Area area)
    {
        nodes = new Dictionary<Tile, Path_Node<Tile>>();

        int edgeCount = 0;

        // Use the World Grid to create Nodes from Tile data
        // NODES will contain ONLY tiles that are WALKABLE
        for (int x = 0; x < area.Width; x++)
        {
            for (int y = 0; y < area.Height; y++)
            {
                Tile t = area.GetTile(x, y);
                if (t != null && t.MovementCost > 0)
                {
                    Path_Node<Tile> n = new Path_Node<Tile>();
                    n.data = t;
                    nodes.Add(t, n);
                }
            }
        }

        //Debug.Log("Path_TileGraph -- created " + nodes.Count + " Nodes.");

        // Now loop through nodes to create Edges between them
        foreach (Tile t in nodes.Keys)
        {
            Path_Node<Tile> n = nodes[t];

            Tile[] neighbors = t.GetNeighbors(true);

            List<Path_Edge<Tile>> edges = new List<Path_Edge<Tile>>();

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] != null && neighbors[i].MovementCost > 0)
                {
                    // Is walkable
                    Path_Edge<Tile> edge = new Path_Edge<Tile>();
                    edge.cost = neighbors[i].MovementCost;
                    edge.node = nodes[neighbors[i]];
                    edges.Add(edge);

                    edgeCount++;
                }
            }

            n.edges = edges.ToArray();
        }

        //Debug.Log("Path_TileGraph -- created " + edgeCount + " Edges.");
    }

    //public Path_TileGraph(World world)
    //{
    //    nodes = new Dictionary<Tile, Path_Node<Tile>>();

    //    int edgeCount = 0;

    //    // Use the World Grid to create Nodes from Tile data
    //    // NODES will contain ONLY tiles that are WALKABLE
    //    for (int x = 0; x < world.Width; x++)
    //    {
    //        for (int y = 0; y < world.Height; y++)
    //        {
    //            Tile t = world.GetTileAt(x, y);
    //            if (t != null && t.MovementCost > 0)
    //            {
    //                Path_Node<Tile> n = new Path_Node<Tile>();
    //                n.data = t;
    //                nodes.Add(t, n);
    //            }
    //        }
    //    }

    //    //Debug.Log("Path_TileGraph -- created " + nodes.Count + " Nodes.");

    //    // Now loop through nodes to create Edges between them
    //    foreach(Tile t in nodes.Keys)
    //    {
    //        Path_Node<Tile> n = nodes[t];

    //        Tile[] neighbors = t.GetNeighbors(true);

    //        List<Path_Edge<Tile>> edges = new List<Path_Edge<Tile>>();

    //        for (int i = 0; i < neighbors.Length; i++)
    //        {
    //            if (neighbors[i] != null && neighbors[i].MovementCost > 0)
    //            {
    //                // Is walkable
    //                Path_Edge<Tile> edge = new Path_Edge<Tile>();
    //                edge.cost = neighbors[i].MovementCost;
    //                edge.node = nodes[neighbors[i]];
    //                edges.Add(edge);

    //                edgeCount++;
    //            }
    //        }

    //        n.edges = edges.ToArray();
    //    }

    //      //Debug.Log("Path_TileGraph -- created " + edgeCount + " Edges.");


    //}
}
