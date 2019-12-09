using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static bool IsPointVisited(HashSet<SinglyLinkedList<Point>> hs, SinglyLinkedList<Point> p) =>
                    hs.Where(x => x.Value.X == p.Value.X && x.Value.Y == p.Value.Y).Any();

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            HashSet<SinglyLinkedList<Point>> visited = new HashSet<SinglyLinkedList<Point>>();
            Queue<SinglyLinkedList<Point>> queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start, null));
           
            while (queue.Count != 0)
            {
                SinglyLinkedList<Point> point = queue.Dequeue();
                if (point.Value.X < 0 || point.Value.X >= map.Dungeon.GetLength(0)
                    || point.Value.Y < 0 || point.Value.Y >= map.Dungeon.GetLength(1))
                    continue;
                if (map.Dungeon[point.Value.X, point.Value.Y] != MapCell.Empty || IsPointVisited(visited, point))
                    continue;
                visited.Add(point);
                if (chests.Contains(point.Value))
                {
                    yield return point;
                }
                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0) continue;
                        else queue.Enqueue(new SinglyLinkedList<Point>(new Point(point.Value.X + dx, point.Value.Y + dy), point));
            }
            yield break;
        }
    }
}