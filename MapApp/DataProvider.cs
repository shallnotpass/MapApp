using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapApp
{
    public class DataProvider
    {
        public List<Coordinates> ReadCoordinatesFromDatabase(string connectionString)
        {
            string commandText = @"SELECT * FROM dbo.Coordinates;";
            List<Coordinates> result = new List<Coordinates>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[3];
                            reader.GetValues(row);
                            result.Add(new Coordinates {Latitude = (float)row[1], Longitude = (float)row[2] });
                        }
                        return result;
                    }
                }
            }
        }
        public void SaveCoordinatesToDatabase(string connectionString, List<Coordinates> coordinates)
        {
            string output = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(output, connection))
                {
                    connection.Open();
                    for (int i = 0; i < coordinates.Count; i++)
                    {
                        output = String.Format("UPDATE dbo.Coordinates SET latitude = {1}, longitude = {2} WHERE idcoordinates = {0};", i.ToString(), coordinates[i].Latitude.ToString().Replace(',','.'), coordinates[i].Longitude.ToString().Replace(',', '.'));
                        command.CommandText = output;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
