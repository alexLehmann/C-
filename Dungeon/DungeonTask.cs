using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class DungeonTask
	{        
        private static List<List<Point>> PathCollectionToListPath(IEnumerable<SinglyLinkedList<Point>> path, bool flagReverse)
        {
            List<List<Point>> list = new List<List<Point>>();
            foreach (var item in path)
            {
                list.Add(item.ToList());
                if (flagReverse) list[list.Count - 1].Reverse();
            }
            return list;
        }

        private static List<Point> SearchShortesPath(List<List<Point>> list)
        {
            int minCount = Int32.MaxValue;
            int indexList = 0;
            for (int i = 0; i < list.Count; i++)
            {               
                if (minCount > list[i].Count)
                {
                    minCount = list[i].Count;
                    indexList = i;
                }
            }
            return list[indexList].ToList();
        }

        private static IEnumerable<List<Point>> JoinStartPathAndEndPath(List<List<Point>> pathStart, List<List<Point>> pathEnd) =>
            pathStart.Join(pathEnd,
                x => x.Last(),
                y => y.First(),
                (x, y) => x.Concat(y.Skip(1)).ToList());

        private static IEnumerable<MoveDirection> PathConvertToMoveDirection(List<Point> path)=>
            path.Zip(path.Skip(1),
                      (a, b) => Walker.ConvertOffsetToDirection(new Size(b) - new Size(a)));


        public static MoveDirection[] FindShortestPath(Map map)
		{            
            var pathStart = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            var pathEnd = BfsTask.FindPaths(map, map.Exit, map.Chests);
            var listStart = PathCollectionToListPath(pathStart,true);
            var listEnd = PathCollectionToListPath(pathEnd,false);
            var result = JoinStartPathAndEndPath(listStart,listEnd);
            if (result.Count() == 0)
            {
                var path = BfsTask.FindPaths(map, map.InitialPosition, new Point[] { map.Exit });
                result = PathCollectionToListPath(path, true).ToList();                 
            }
            if (result.Count() == 0) return new MoveDirection[0];
            List<Point> shortesPath = SearchShortesPath(result.ToList());
            var pathMoveDirection = PathConvertToMoveDirection(shortesPath);                     
            return pathMoveDirection.ToArray();
        }
	}
}
