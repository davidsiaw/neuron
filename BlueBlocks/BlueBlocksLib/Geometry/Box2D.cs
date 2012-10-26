using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.Geometry
{

    public struct Box2D
    {
        public readonly long x0, y0, x1, y1;
        readonly long w, h;
        public Box2D(long x, long y, long w, long h)
        {
            this.x0 = x;
            this.y0 = y;
            this.x1 = x + w;
            this.y1 = y + h;
            this.w = w;
            this.h = h;

            if (x0 > x1)
            {
                long m = x0;
                x0 = x1;
                x1 = m;
            }

            if (y0 > y1)
            {
                long m = y0;
                y0 = y1;
                y1 = m;
            }
        }

        [Flags]
        public enum Position
        {
            Inside = 0x0,
            Top = 0x1,
            Bottom = 0x2,
            Left = 0x4,
            Right = 0x8

        }

        public Position PositionOf(Point2D coord)
        {
            Position pos = Position.Inside;

            if (coord.x < this.x0)
            {
                pos |= Position.Left;
            }
            if (coord.x > this.x1)
            {
                pos |= Position.Right;
            }
            if (coord.y < this.y0)
            {
                pos |= Position.Top;
            }
            if (coord.y > this.y1)
            {
                pos |= Position.Bottom;
            }
            return pos;
        }

        public bool IsInside(Point2D coord)
        {
            return PositionOf(coord) == (Position)0;
        }

        // clip off everything not to the right of mid
        public bool ClipRight(ref Decimal a, ref Decimal b, Decimal mid)
        {
            if (a < mid && b < mid)
            {
                a = 0;
                b = 0;
                return false;
            }

            if (a <= mid && b >= mid)
            {
                a = mid;
            }

            else if (b <= mid && a >= mid)
            {
                b = mid;
            }
            return true;
        }

        // clip off everything not to the left of mid
        public bool ClipLeft(ref Decimal a, ref Decimal b, Decimal mid)
        {
            if (a > mid && b > mid)
            {
                a = 0;
                b = 0;
                return false;
            }

            if (a <= mid && b >= mid)
            {
                b = mid;
            }

            else if (b <= mid && a >= mid)
            {
                a = mid;
            }
            return true;
        }

        public bool ClipSegment(Pair<Point2D, Point2D> segment, out Pair<Point2D, Point2D> clipped, out bool aClipped, out bool bClipped)
        {
            Position apos = PositionOf(segment.a);
            Position bpos = PositionOf(segment.b);

            aClipped = apos != Position.Inside;
            bClipped = bpos != Position.Inside;

            if (apos == Position.Inside && bpos == Position.Inside)
            {
                clipped = segment;
                return true;
            }

            if (apos == bpos)
            {
                clipped = null;
                return false;
            }

            Decimal xa = segment.a.x;
            Decimal ya = segment.a.y;
            Decimal xb = segment.b.x;
            Decimal yb = segment.b.y;

            Decimal dy = yb - ya;
            Decimal dx = xb - xa;

            Decimal m = 0;
            Decimal c = 0;
            if (dx != 0 && dy != 0)
            {
                m = dy / dx;
                c = ya - m * xa; 
            }


            bool resultx = true;
            bool resulty = true;


            // clip the top
            resulty &= ClipRight(ref ya, ref yb, y0);
            if (dx != 0 && dy != 0)
            {
                xa = (ya - c) / m;
                xb = (yb - c) / m;
            }

            // clip the left
            resultx &= ClipRight(ref xa, ref xb, x0);
            if (dx != 0 && dy != 0)
            {
                ya = m * xa + c;
                yb = m * xb + c;
            }

            // clip the right
            resultx &= ClipLeft(ref xa, ref xb, x1);
            if (dx != 0 && dy != 0)
            {
                ya = m * xa + c;
                yb = m * xb + c;
            }

            // clip the bottom
            resulty &= ClipLeft(ref ya, ref yb, y1);
            if (dx != 0 && dy != 0)
            {
                xa = (ya - c) / m;
                xb = (yb - c) / m;
            }

            if ((resultx | resulty) == false)
            {
                clipped = null;
                return false;
            }

            clipped = new Pair<Point2D, Point2D>() { 
                a = new Point2D() {
                    x = (uint)Math.Abs(Math.Round(xa)),
                    y = (uint)Math.Abs(Math.Round(ya)) }, 

                b = new Point2D() { 
                    x = (uint)Math.Abs(Math.Round(xb)), 
                    y = (uint)Math.Abs(Math.Round(yb)) } };

            return resultx | resulty;
        }

        public IEnumerable<Line2D> Clip(Line2D line)
        {
            List<Line2D> lines = new List<Line2D>();

            List<Pair<Point2D, Point2D>> segments = new List<Pair<Point2D, Point2D>>(line.GetSegments());
            for (int i = 0; i < segments.Count; i++)
            {
                List<Pair<Point2D, Point2D>> currline = new List<Pair<Point2D, Point2D>>();
                for (; i < segments.Count; i++)
                {
                    bool aclipped,bclipped;
                    Pair<Point2D,Point2D> clipped;
                    bool lineExists = ClipSegment(segments[i], out clipped, out aclipped, out bclipped);
                    if (lineExists)
                    {
                        currline.Add(clipped);
                    }
                    if (!lineExists || bclipped)
                    {
                        break;
                    }
                }

                List<Point2D> points = new List<Point2D>();
                if (currline.Count > 0)
                {
                    points.Add(currline[0].a);
                    for (int c = 0; c < currline.Count; c++)
                    {
                        points.Add(currline[c].b);
                    }
                }
                if (points.Count > 0)
                {
                    lines.Add(new Line2D(points));
                }
            }

            return lines;
        }
    }
}
