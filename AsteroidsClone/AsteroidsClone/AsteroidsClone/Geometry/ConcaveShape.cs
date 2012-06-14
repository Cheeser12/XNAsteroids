using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AsteroidsClone
{
    public class ConcaveShape : Shape
    {
        public Triangle[] Triangles { get; private set; }

        public ConcaveShape(Vector2 pos, Vector2[] verts, short[] indices)
            : base(verts, indices)
        {
            Position = pos;
            Triangles = Triangulate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Triangle t in Triangles)
            {
                t.Position = Position;
                t.Scale = Scale;
                t.Rotation = Rotation;
                t.Update(gameTime);
            }
        }

        public override bool Intersects(Shape s)
        {
            foreach (Triangle t in Triangles)
            {
                if (t.Intersects(s)) return true;
            }

            return false;
        }

        protected override void CreateShape()
        {
        }

        private Triangle[] Triangulate()
        {
            List<Triangle> triangles = new List<Triangle>();

            Queue<int> ears = new Queue<int>();
            List<int> reflex;
            List<int> convex;
            List<int> removed;
            reflex = new List<int>();

            convex = new List<int>();
            removed = new List<int>();

            // Find convex and reflex verts (initial)
            for (int i = 0; i < Vertices.Length; i++)
            {
                int indexBefore = (i == 0) ? (Vertices.Length - 1) : (i - 1);
                int indexAfter = (i == Vertices.Length - 1) ? (0) : (i + 1);

                if (IsConvexVert(i, indexBefore, indexAfter))
                    convex.Add(i);

                else reflex.Add(i);

            }

            // Find ears (initial)
            for (int i = 0; i < convex.Count; i++)
            {
                int indexBefore = (i == 0) ? (Vertices.Length - 1) : (i - 1);
                int indexAfter = (i == Vertices.Length - 1) ? (0) : (i + 1);

                if (IsEar(i, indexBefore, indexAfter, reflex))
                    ears.Enqueue(i);
            }

            // Something is wrong....no ears!
            if (ears.Count == 0)
            {
                return null;
            }

            while ((Vertices.Length - removed.Count) > 3)
            {

                int i = ears.Dequeue();




                int indexBefore = (i == 0) ? (Vertices.Length - 1) : (i - 1);
                while (removed.Contains(indexBefore))
                {
                    if (indexBefore == 0)
                    {
                        indexBefore = Vertices.Length - 1;

                    }

                    else indexBefore--;
                }

                int indexAfter = (i == Vertices.Length - 1) ? (0) : (i + 1);
                while (removed.Contains(indexAfter))
                {
                    if (indexAfter == Vertices.Length - 1)
                        indexAfter = 0;

                    else indexAfter++;
                }

                Triangle t = new Triangle(GetPos(Vertices[i]), GetPos(Vertices[indexBefore]), GetPos(Vertices[indexAfter]));

                triangles.Add(t);
                removed.Add(i);


                int j = indexBefore;
                int indexBeforeJ = (j == 0) ? (Vertices.Length - 1) : (j - 1);
                while (removed.Contains(indexBeforeJ))
                {
                    if (indexBeforeJ == 0)
                    {
                        indexBeforeJ = Vertices.Length - 1;

                    }

                    else indexBeforeJ--;
                }


                int indexAfterJ = (j == Vertices.Length - 1) ? (0) : (j + 1);
                while (removed.Contains(indexAfterJ))
                {
                    if (indexAfterJ == Vertices.Length - 1)
                        indexAfterJ = 0;

                    else indexAfterJ++;
                }

                int k = indexAfterJ;

                int indexBeforeK = j;

                int indexAfterK = (k == Vertices.Length - 1) ? (0) : (k + 1);
                while (removed.Contains(indexAfterK))
                {
                    if (indexAfterK == Vertices.Length - 1)
                        indexAfterK = 0;

                    else indexAfterK++;
                }


                if (reflex.Contains(j) && IsConvexVert(j, indexBeforeJ, indexAfterJ))
                {
                    reflex.Remove(j);
                    convex.Add(j);

                    if (!ears.Contains(j) && IsEar(j, indexBeforeJ, indexAfterJ, reflex))
                        ears.Enqueue(j);

                }

                if (reflex.Contains(k) && IsConvexVert(k, indexBeforeK, indexAfterK))
                {
                    reflex.Remove(k);
                    convex.Add(k);

                    if (!ears.Contains(k) && IsEar(k, indexBeforeK, indexAfterK, reflex))
                        ears.Enqueue(k);
                }
            }

            // Add the last triangle
            List<int> verts = new List<int>();
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (!removed.Contains(i))
                    verts.Add(i);
            }


            triangles.Add(new Triangle(GetPos(Vertices[verts[0]]), GetPos(Vertices[verts[1]]), GetPos(Vertices[verts[2]])));

            return triangles.ToArray();
        }

        private bool IsEar(int i, int indexBefore, int indexAfter, List<int> reflex)
        {
            if (reflex.Contains(i)) return false;
            Triangle t = new Triangle(GetPos(Vertices[i]), GetPos(Vertices[indexBefore]), GetPos(Vertices[indexAfter]));

            for (int j = 0; j < reflex.Count; j++)
            {
                int index = reflex[j];
                if (index == i || index == indexBefore || index == indexAfter)
                    continue;

                if (t.PointInTriangle(GetPos(Vertices[index])))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsConvexVert(int i, int indexBefore, int indexAfter)
        {
            Vector2 edge1 =
                new Vector2(GetPos(Vertices[i]).X - GetPos(Vertices[indexBefore]).X,
                    GetPos(Vertices[i]).Y - GetPos(Vertices[indexBefore]).Y);

            Vector2 edge2 =
                new Vector2(GetPos(Vertices[indexAfter]).X - GetPos(Vertices[i]).X,
                    GetPos(Vertices[indexAfter]).Y - GetPos(Vertices[i]).Y);

            Vector3 v1 = new Vector3(edge1, 0f);
            v1.Normalize();

            Vector3 v2 = new Vector3(edge2, 0f);
            v2.Normalize();

            float cross =
                ((GetPos(Vertices[i]).X - GetPos(Vertices[indexBefore]).X) *
                (GetPos(Vertices[indexAfter]).Y - GetPos(Vertices[i]).Y)) -
                ((GetPos(Vertices[i]).Y - GetPos(Vertices[indexBefore]).Y) *
                (GetPos(Vertices[indexAfter]).X - GetPos(Vertices[i]).X));

            if (cross < 0f)
                return false;

            else return true;
        }

        public override object Clone()
        {
            Vector2[] verts = new Vector2[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
            {
                verts[i] = new Vector2(Vertices[i].Position.X, Vertices[i].Position.Y);
            }

            return new ConcaveShape(Position, verts, Indices);
        }
    }
}
