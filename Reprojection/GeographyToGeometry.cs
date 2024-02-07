//using GeoAPI.CoordinateSystems;
//using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Types;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Reprojection
{
    public partial class UserDefinedFunctions
    {

        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read)]
        public static SqlGeometry GeographyToGeometry(SqlGeography geog, SqlInt32 toSRID)
        {
            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();

                // Retrieve the parameters of the source spatial reference system
                SqlCommand cmd = new SqlCommand("SELECT well_known_text FROM prospatial_reference_systems WHERE spatial_reference_id = @srid", conn);
                cmd.Parameters.Add(new SqlParameter("srid", geog.STSrid));
                String fromWKT = (String)cmd.ExecuteScalar();

                CoordinateSystemFactory csFact = new CoordinateSystemFactory();
                CoordinateTransformationFactory ctFact = new CoordinateTransformationFactory();

                // Create the source coordinate system from WKT
                ICoordinateSystem fromCS = csFact.CreateFromWkt(fromWKT);

                // Retrieve the parameters of the destination spatial reference system
                cmd.Parameters["srid"].Value = toSRID;
                String toWKT = (String)cmd.ExecuteScalar();
                cmd.Dispose();

                // Create the destination coordinate system from WKT
                ICoordinateSystem toCS = csFact.CreateFromWkt(toWKT);

                // Create a CoordinateTransformationFactory:
                ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctfac = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

                // Create the transformation instance:
                ICoordinateTransformation trans = ctFact.CreateFromCoordinateSystems(fromCS, toCS);

                // create a sink that will create a geometry instance
                SqlGeometryBuilder b = new SqlGeometryBuilder();
                b.SetSrid((int)toSRID);

                // create a sink to do the shift and plug it in to the builder
                TransformGeographyToGeometrySink s = new TransformGeographyToGeometrySink(trans, b);

                // plug our sink into the geometry instance and run the pipeline
                geog.Populate(s);

                // the end of our pipeline is now populated with the shifted geometry instance
                return b.ConstructedGeometry;
            }
        }
    }
}
