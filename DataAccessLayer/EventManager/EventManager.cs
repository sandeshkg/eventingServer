using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EventManager
    {
        SqlConnection connection;
        SqlCommand command;
        DataTable eventDetails;

        public EventManager()
        {
            try
            {
                eventDetails = new DataTable();
                connection = new SqlConnection();
                connection.ConnectionString = @"";
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
            }
            catch (SqlException ex)
            {
                // Todo : logging
            }
        }

        /// <summary>
        /// Get all event details at a single hit
        /// </summary>
        /// <returns></returns>
        public DataTable GetEventDetails()
        {
            eventDetails = GetEventDetailsForQuery("Select * from EventDetails");
            return eventDetails;
        }

        /// <summary>
        /// Get event details based on the Event ID
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public DataTable GetEventDetails(int eventID)
        {
            eventDetails = GetEventDetailsForQuery("select * from EventDetails where eventId = '" + eventID + "'");
            return eventDetails;
        }

        /// <summary>
        /// Method to get Event details starting from the given start date
        /// </summary>
        /// <param name="eventStartTime"></param>
        /// <returns></returns>
        public DataTable GetEventDetails(DateTime eventStartTime)
        {
            try
            {
                string sqlCom = "select * from EventDetails where eventStartTime = @eventStartTime";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlCom, connection);
                cmd.Parameters.Add("@eventStartTime", System.Data.SqlDbType.Date);
                cmd.Parameters["@eventStartTime"].Value = eventStartTime;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    eventDetails.Load(reader);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            catch (SqlException sqlEx)
            { }
            catch (Exception ex) { }
            return eventDetails;
        }

        private DataTable GetEventDetailsForQuery(string query)
        {
            try
            {
                command.CommandText = query;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    eventDetails.Load(reader);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            catch (SqlException sqlEx) { }
            catch (Exception ex) { }
            return eventDetails;
        }
    }
}
