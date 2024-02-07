//using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.CoordinateSystems.Transformations;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Reprojection
{
    public class TransformGeometryToGeometrySink : IGeometrySink110
    {
        private readonly ICoordinateTransformation _trans;
        private readonly IGeometrySink110 _sink;

        public TransformGeometryToGeometrySink(ICoordinateTransformation trans, IGeometrySink110 sink)
        {
            _trans = trans;
            _sink = sink;
        }

        public void BeginGeometry(OpenGisGeometryType type)
        {
            _sink.BeginGeometry(type);
        }

        public void EndGeometry()
        {
            _sink.EndGeometry();
        }

        public void BeginFigure(double x, double y, Nullable<double> z, Nullable<double> m)
        {
            double[] fromPoint = { x, y };
            double[] toPoint = _trans.MathTransform.Transform(fromPoint);
            double tox = toPoint[0];
            double toy = toPoint[1];
            _sink.BeginFigure(tox, toy, z, m);
        }

        public void AddLine(double x, double y, Nullable<double> z, Nullable<double> m)
        {
            double[] fromPoint = { x, y };
            double[] toPoint = _trans.MathTransform.Transform(fromPoint);
            double tox = toPoint[0];
            double toy = toPoint[1];
            _sink.AddLine(tox, toy, z, m);
        }

        public void EndFigure()
        {
            _sink.EndFigure();
        }

        public void SetSrid(int srid)
        {
            // _sink.SetSrid(srid);
        }

        public void AddCircularArc(double x1, double y1, double? z1, double? m1, double x2, double y2, double? z2, double? m2)
        {
            throw new NotImplementedException();
        }
    }
}
