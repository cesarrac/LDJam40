using UnityEngine;
using System.Collections.Generic;
using Priority_Queue;
using System.Linq; 

public class Path_AStar  {

    Queue<Tile> path;

    public int Length()
    {
        if (path != null && path.Count > 0)
            return path.Count;
        else
            return 0;
    }

    public Path_AStar(Area area, Tile startTile, Tile endTile)
    {
        //if (world.path_graph == null)
        //    world.path_graph = new Path_TileGraph(world);

        if (area.path_graph == null)
            area.path_graph = new Path_TileGraph(area);

        // A dictionary of Walkable Nodes (just a reference to path_graph.nodes)
        Dictionary<Tile, Path_Node<Tile>> nodes = area.path_graph.nodes;

        if (nodes.ContainsKey(startTile) == false)
        {
            Debug.LogError("PathAStar could not find starting tile in path graph!");
            return;
        }

        // Set the starting tile's node
        Path_Node<Tile> start = nodes[startTile];

        // If end tile is null, keep this goal null
        Path_Node<Tile> goal = null;
        if (endTile != null)
        {
            if (nodes.ContainsKey(endTile) == false)
            {
                Debug.LogError("PathAStar could not find end tile in path graph!");
                return;
            }

            goal = nodes[endTile];
        }

        // Nodes already verified
        List<Path_Node<Tile>> ClosedSet = new List<Path_Node<Tile>>();

        //List<Path_Node<Tile>> OpenSet = new List<Path_Node<Tile>>();
        //OpenSet.Add( start );

        // Nodes being verified
        PathfindingPriorityQueue<Path_Node<Tile>> OpenSet = new PathfindingPriorityQueue<Path_Node<Tile>>();
        OpenSet.Enqueue(start, 0);

        // Nodes we just came from
        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From = new Dictionary<Path_Node<Tile>, Path_Node<Tile>>();

        Dictionary<Path_Node<Tile>, float> g_score = new Dictionary<Path_Node<Tile>, float>();
        // Set default values to Infinity (so all nodes have a score in the Map)
        foreach (Path_Node<Tile> n in nodes.Values)
        {
            g_score[n] = Mathf.Infinity;
        }
        g_score[ start ] = 0;

        Dictionary<Path_Node<Tile>, float> f_score = new Dictionary<Path_Node<Tile>, float>();
        // Set default values to Infinity (so all nodes have a score in the Map)
        foreach (Path_Node<Tile> n in nodes.Values)
        {
            f_score[n] = Mathf.Infinity;
        }
        f_score[ start ] = HeuristicCostEstimate(start, goal);

        while(OpenSet.Count > 0)
        {
            Path_Node<Tile> current = OpenSet.Dequeue();

            if (goal != null)
            {
                if (current == goal)
                {
                    ReconstructPath(Came_From, current);
                    return;
                }
            }
            else
            {
                // TODO: goal is null, meaning the goal is to walk on a tile that contains some THING on it
            }

            ClosedSet.Add(current);

            foreach(Path_Edge<Tile> edge_neighbor in current.edges)
            {
                Path_Node<Tile> neighbor = edge_neighbor.node;

                if (ClosedSet.Contains(neighbor))
                    continue; // Ignore this already verified node

                float tentative_g_score = g_score[current] + DistBetween(current, neighbor);

                // Check this Node (neighbor) has not been check yet
                if (OpenSet.Contains(neighbor) && tentative_g_score >= g_score[neighbor])
                    continue;

                Came_From[neighbor] = current;
                g_score[neighbor] = tentative_g_score;
                f_score[neighbor] = g_score[neighbor] + HeuristicCostEstimate(neighbor, goal);

                if (OpenSet.Contains(neighbor) == false)
                    OpenSet.Enqueue(neighbor, f_score[neighbor]);
                else
                    OpenSet.UpdatePriority(neighbor, f_score[neighbor]);
            }
        }

        // No Path from Start to Goal was found if we reach here
        return;
    }

    float HeuristicCostEstimate(Path_Node<Tile> a, Path_Node<Tile> b)
    {
        return Mathf.Sqrt(
            Mathf.Pow(a.data.X - b.data.X, 2) +
            Mathf.Pow(a.data.Y - b.data.Y, 2)
         );
    }

    float DistBetween(Path_Node<Tile> a, Path_Node<Tile> b)
    {
        // On a grid:
        // Hori / Vert neighbors has a distance of 1
        if (Mathf.Abs(a.data.X - b.data.X)  + Mathf.Abs(a.data.Y - b.data.Y) == 1)
        {
            return 1f;
        }

        // Diag neighbors have a distance of 1.41421356237
        if (Mathf.Abs(a.data.X - b.data.X) == 1 &&  Mathf.Abs(a.data.Y - b.data.Y) == 1)
        {
            return 1.41421356237f;
        }

        // Otherwise do the math
        return Mathf.Sqrt(
          Mathf.Pow(a.data.X - b.data.X, 2) +
          Mathf.Pow(a.data.Y - b.data.Y, 2)
       );

    }

    void ReconstructPath(
        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From,
        Path_Node<Tile> current)
    {
        // Current begins as the goal tile
        Queue<Tile> total_path = new Queue<Tile>();
        total_path.Enqueue(current.data);
        // Go back through the Came From map until we find the start node
        while (Came_From.ContainsKey(current))
        {
            current = Came_From[current];
            total_path.Enqueue(current.data);
        }

        // Now that total_path is populated it contains all the Nodes in the path
        // but FROM the goal to the START (in reverse). 
        // So we have to reverse it to get it in the right order

        path = new Queue<Tile>(total_path.Reverse());
    }

    public Tile Dequeue()
    {
        if (path == null)
        {
            Debug.LogError("Attempting to Dequeue from a null path!");
            return null;
        }

        if (path.Count <= 0)
        {
            Debug.LogError("Path contains zero or less Tiles!");
            return null;
        }
            
        return path.Dequeue();
    }

	
}
