using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PolygonCircles
{
    class PolygonDrawer
    {
        static List<PointF> points = new List<PointF>() { 
            new PointF(0, 0),new PointF(0, 0),new PointF(0, 0),new PointF(0, 0),new PointF(0, 0)
        };
        static Random rnd = new Random(DateTime.Now.Millisecond);
        static void randomValues()
        {
            points[0] = new PointF(rnd.Next(1000, 2000) / 10f, rnd.Next(0, 1000) / 10f);
            points[1] = new PointF(200 + rnd.Next(0, 2000) / 10f, rnd.Next(0, 2000) / 10f);
            points[2] = new PointF(200 + rnd.Next(0, 2000) / 10f, 200 + rnd.Next(0, 2000) / 10f);
            points[3] = new PointF(rnd.Next(1000, 2000) / 10f, 300 + rnd.Next(0, 1000) / 10f);
            points[4] = new PointF(rnd.Next(0, 1000) / 10f, 100 + rnd.Next(0, 2000) / 10f);
        }
        static PointF summPoints(PointF p1, PointF p2)
        {
            return summPoints(p1, p2, 1f, 1f);
        }
        static PointF summPoints(PointF p1, PointF p2, float k1, float k2)
        {
            return new PointF(p1.X * k1 + p2.X * k2, p1.Y * k1 + p2.Y * k2);
        }
        static float scalarMult(PointF p1, PointF p2)
        {
            return (p1.X * p2.X + p1.Y * p2.Y);
        }
        static float length(PointF p1)
        {
            return (float)Math.Sqrt(p1.X * p1.X + p1.Y * p1.Y);
        }
        static PointF norm(PointF p1)
        {
            float le = length(p1);
            return new PointF((float)(p1.X / le), (float)(p1.Y / le));
        }
        static PointF mult(PointF p1, float k)
        {
            return new PointF((float)(p1.X * k), (float)(p1.Y * k));
        }
        static float angleBetween3Points(PointF p1, PointF p2, PointF p3)
        {
            PointF d1 = summPoints(p2, p3, 1f, -1f);
            PointF d2 = summPoints(p1, p2, 1f, -1f);
            float sc = scalarMult(d2, d1);
            float l1 = length(d1);
            float l2 = length(d2);
            float res = sc / (l1 * l2);
            return (float)(Math.PI - Math.Acos(res));
        }



        public static Bitmap execute(float rad, bool drawMore)
        {
            randomValues();
            Bitmap b = new Bitmap(1600, 1600);
            Pen p1 = new Pen(Color.Black);
            Pen p2 = new Pen(Color.Pink, 1f);
            Pen p3 = new Pen(Color.LightGreen, 1.5f);
            Pen p4 = new Pen(Color.Blue, 1.5f);
            Pen pfin = new Pen(Color.Red, 5.5f);

            List<int> indexes = new List<int>();
            for (int i = 0; i < points.Count; ++i)
                indexes.Add(i);
            indexes.Add(0);
            indexes.Add(1);
            using (Graphics G = Graphics.FromImage(b))
            {
                if (drawMore)
                for (int i = 1; i < indexes.Count - 1; ++i)
                {
                    G.DrawLine(p2, points[indexes[i - 1]], points[indexes[i]]);
                    float radd = 10;
                    G.DrawEllipse(p1, points[indexes[i]].X - radd * .5f,
                        points[indexes[i]].Y - radd * .5f, radd, radd);
                }
                PointF prevGran = new PointF(0f, 0f);
                PointF lastGran = new PointF(0f, 0f);
                PointF dLeft = new PointF(0f, 0f);
                for (var i = 0; i < points.Count; ++i)
                {
                    int ind0 = indexes[i], ind1 = indexes[i + 1], ind2 = indexes[i + 2];
                    {
                        float ang = angleBetween3Points(points[ind0], points[ind1], points[ind2]);
                        float d = (float)(rad / Math.Tan(ang * .5f));

                        dLeft = summPoints(points[ind1],
                            mult(norm(summPoints(points[ind0], points[ind1], 1f, -1f)), d));
                        PointF dRight = summPoints(points[ind1],
                            mult(norm(summPoints(points[ind2], points[ind1], 1f, -1f)), d));

                        PointF ots = mult(norm(summPoints(points[ind0], points[ind1], 1f, -1f)), d);
                        PointF mirr = summPoints(dLeft, mult(norm(new PointF(ots.Y, -ots.X)), rad));

                        PointF leftTop = new PointF(mirr.X - rad, mirr.Y - rad);

                        if (drawMore)
                        {
                            G.DrawLine(p4, mirr, dLeft);
                            G.DrawLine(p3, mirr, dRight);
                            G.DrawLine(p3, points[ind1], dLeft);
                        }
                        float ang0 = -angleBetween3Points(summPoints(mirr, new PointF(1.0f, .0f)), mirr, dRight) / (float)Math.PI * 180;
                        float ang1 = -(angleBetween3Points(summPoints(mirr, new PointF(1.0f, .0f)), mirr, dLeft) / (float)Math.PI * 180);
                        if (ang0 < 0 && dRight.Y > mirr.Y) ang0 *= -1;
                        if (ang1 < 0 && dLeft.Y > mirr.Y) ang1 *= -1;
                        if (ang1 > ang0) ang1 -= 360;
                        if (drawMore)
                            G.DrawEllipse(p1, leftTop.X, leftTop.Y, rad * 2, rad * 2);
                        G.DrawArc(pfin, leftTop.X, leftTop.Y, rad * 2, rad * 2, ang0, ang1 - ang0);
                        if (i == 0) lastGran = dLeft;
                        if (i > 0)
                            G.DrawLine(pfin, prevGran, dLeft);
                        prevGran = dRight;

                        //G.DrawString(Math.Round(ang0) + "  " + Math.Round(ang1), new System.Drawing.Font("Arial", 12), new SolidBrush(Color.Black), points[ind1]);

                    }
                }
                G.DrawLine(pfin, prevGran, lastGran);
            }
            return b;
        }

    }
}
