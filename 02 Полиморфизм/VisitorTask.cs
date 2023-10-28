using Inheritance.Geometry.Virtual;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;

namespace Inheritance.Geometry.Visitor
{
    public interface IVisitor<TResult>
    {
        TResult Visit(Ball ball);
        TResult Visit(RectangularCuboid rectangularCuboid);
        TResult Visit(Cylinder cylinder);
        TResult Visit(CompoundBody compoundBody);
    }

    public abstract class Body
    {
        public abstract TResult Accept<TResult>(IVisitor<TResult> visitor);

        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override TResult Accept<TResult>(IVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override TResult Accept<TResult>(IVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override TResult Accept<TResult>(IVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override TResult Accept<TResult>(IVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class BoundingBoxVisitor : IVisitor<RectangularCuboid>
    {
        public RectangularCuboid Visit(Ball ball)
        {
            return new RectangularCuboid(ball.Position, ball.Radius * 2, ball.Radius * 2, ball.Radius * 2);
        }

        public RectangularCuboid Visit(RectangularCuboid rectangularCuboid)
        {
            return new RectangularCuboid(
                rectangularCuboid.Position, rectangularCuboid.SizeX, rectangularCuboid.SizeY, rectangularCuboid.SizeZ);
        }

        public RectangularCuboid Visit(Cylinder cylinder)
        {
            return new RectangularCuboid(cylinder.Position, cylinder.Radius * 2, cylinder.Radius * 2, cylinder.SizeZ);
        }

        public RectangularCuboid Visit(CompoundBody compoundBody)
        {
            IEnumerable<RectangularCuboid> cubes = compoundBody.Parts.Select((cube) => cube.Accept(this));

            var maxX = cubes.Max(p => p.Position.X + p.SizeX / 2);
            var maxY = cubes.Max(p => p.Position.Y + p.SizeY / 2);
            var maxZ = cubes.Max(p => p.Position.Z + p.SizeZ / 2);

            var minX = cubes.Min(p => p.Position.X - p.SizeX / 2);
            var minY = cubes.Min(p => p.Position.Y - p.SizeY / 2);
            var minZ = cubes.Min(p => p.Position.Z - p.SizeZ / 2);
            var pos = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);

            return new RectangularCuboid(pos, maxX - minX, maxY - minY, maxZ - minZ);
        }
    }

    public class BoxifyVisitor : IVisitor<Body>
    {
        public Body Visit(Ball ball)
        {
            return ball.TryAcceptVisitor<Body>(new BoundingBoxVisitor());
            //return new RectangularCuboid(ball.Position, ball.Radius * 2, ball.Radius * 2, ball.Radius * 2);
        }

        public Body Visit(RectangularCuboid rectangularCuboid)
        {
            return rectangularCuboid;
        }

        public Body Visit(Cylinder cylinder)
        {
            return cylinder.TryAcceptVisitor<Body>(new BoundingBoxVisitor());
            //return new RectangularCuboid(cylinder.Position, cylinder.Radius * 2, cylinder.Radius * 2, cylinder.SizeZ);
        }

        public Body Visit(CompoundBody compoundBody)
        {
            List<Body> parts = new List<Body>();

            foreach (var part in compoundBody.Parts)
            {
                parts.Add(part.TryAcceptVisitor<Body>(new BoxifyVisitor()));
            }

            return new CompoundBody(parts);
        }
    }
}
