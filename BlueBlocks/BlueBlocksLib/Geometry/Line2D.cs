using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.Geometry
{
    public struct Line2D
    {
        IList<Point2D> coords;
        public Line2D(IList<Point2D> coords)
        {
            this.coords = coords;
        }

        public IList<Point2D> GetPoints()
        {
            return coords;
        }

        public IEnumerable<Pair<Point2D, Point2D>> GetSegments()
        {
            for (int i = 0; i < coords.Count - 1; i++)
            {
                yield return new Pair<Point2D, Point2D>() { a = coords[i], b = coords[i + 1] };
            }
        }
    }
}
