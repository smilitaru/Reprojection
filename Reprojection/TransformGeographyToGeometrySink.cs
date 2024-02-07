using GeoAPI.CoordinateSystems.Transformations;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Reprojection
{
    public class TransformGeographyToGeometrySink : IGeographySink110
    {
        private readonly ICoordinateTransformation _trans;
        private readonly IGeometrySink110 _sink;

        public TransformGeographyToGeometrySink(ICoordinateTransformation trans, IGeometrySink110 sink)
        {
            _trans = trans;
            _sink = sink;
        }

        public void BeginGeography(OpenGisGeographyType type)
        {
            _sink.BeginGeometry((OpenGisGeometryType)type);
        }

        public void EndGeography()
        {
            _sink.EndGeometry();
        }

        public void BeginFigure(double latitude, double longitude, Nullable<double> z, Nullable<double> m)
        {
            double[] fromPoint = { longitude, latitude };
            double[] toPoint = _trans.MathTransform.Transform(fromPoint);
            double x = toPoint[0];
            double y = toPoint[1];
            _sink.BeginFigure(x, y, z, m);
        }

        public void AddLine(double latitude, double longitude, Nullable<double> z, Nullable<double> m)
        {
            double[] fromPoint = { longitude, latitude };
            double[] toPoint = _trans.MathTransform.Transform(fromPoint);
            double x = toPoint[0];
            double y = toPoint[1];
            _sink.AddLine(x, y, z, m);
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
