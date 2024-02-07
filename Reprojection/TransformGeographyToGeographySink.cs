using GeoAPI.CoordinateSystems.Transformations;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Reprojection
{
    class TransformGeographyToGeographySink : IGeographySink110
    {
        private readonly ICoordinateTransformation _trans;
        private readonly IGeographySink110 _sink;

        public TransformGeographyToGeographySink(ICoordinateTransformation trans, IGeographySink110 sink)
        {
            _trans = trans;
            _sink = sink;
        }

        public void BeginGeography(OpenGisGeographyType type)
        {
            _sink.BeginGeography(type);
        }

        public void EndGeography()
        {
            _sink.EndGeography();
        }

        public void BeginFigure(double latitude, double longitude, Nullable<double> z, Nullable<double> m)
        {
            double[] fromPoint = { longitude, latitude };
            double[] toPoint = _trans.MathTransform.Transform(fromPoint);
            double tolong = toPoint[0];
            double tolat = toPoint[1];
            _sink.BeginFigure(tolat, tolong, z, m);
        }

        public void AddLine(double latitude, double longitude, Nullable<double> z, Nullable<double> m)
        {
            double[] fromPoint = { longitude, latitude };
            double[] toPoint = _trans.MathTransform.Transform(fromPoint);
            double tolong = toPoint[0];
            double tolat = toPoint[1];
            _sink.AddLine(tolat, tolong, z, m);
        }

        public void EndFigure()
        {
            _sink.EndFigure();
        }

        public void SetSrid(int srid)
        {
            // _sink.SetSrid(srid);
        }

        void IGeographySink110.AddCircularArc(double x1, double y1, double? z1, double? m1, double x2, double y2, double? z2, double? m2)
        {
            throw new NotImplementedException();
        }

        void IGeographySink.SetSrid(int srid)
        {
            throw new NotImplementedException();
        }

        void IGeographySink.BeginGeography(OpenGisGeographyType type)
        {
            throw new NotImplementedException();
        }

        void IGeographySink.BeginFigure(double latitude, double longitude, double? z, double? m)
        {
            throw new NotImplementedException();
        }

        void IGeographySink.AddLine(double latitude, double longitude, double? z, double? m)
        {
            throw new NotImplementedException();
        }

        void IGeographySink.EndFigure()
        {
            throw new NotImplementedException();
        }

        void IGeographySink.EndGeography()
        {
            throw new NotImplementedException();
        }
    }
}
