using System.Collections.Generic;
using AngularJSAuthentication.API.Models;
using System.Web.Http;
using System.Data;
using System.Web.Mvc;
using Utilities;
using System;
using H = System.Web.Http;
using System.Web.Script.Serialization;
using DataAccessLayer;

namespace AngularJSAuthentication.API.Controllers
{
    [H.RoutePrefix("api/Eventing")]
    public class EventingController : ApiController
    {
        DataTableProcesses dataTableProcesses;
        EventManager eventManager;

        public EventingController()
        {
            dataTableProcesses = new DataTableProcesses();
            eventManager = new EventManager();
        }

        public List<EventDetails> GetEventDetails()
        {
            DataTable eventDetails = new DataAccessLayer.EventManager().GetEventDetails();
            List<EventDetails> eventDetailsList = EventDetailEntities(eventDetails);

            return eventDetailsList;
        }

        public List<EventDetails> GetEventDetails(int eventId)
        {
            DataTable eventDetails = eventManager.GetEventDetails(eventId);
            List<EventDetails> eventDetailsList = EventDetailEntities(eventDetails);

            return eventDetailsList;
        }

        public List<EventDetails> GetEventDetails(DateTime eventStartTime)
        {
            DataTable eventDetails = eventManager.GetEventDetails(eventStartTime);
            List<EventDetails> eventDetailsList = EventDetailEntities(eventDetails);

            return eventDetailsList;
        }

        public List<EventDetails> EventDetailEntities(DataTable eventDetails)
        {
            List<EventDetails> eventDetailsEntity = new List<EventDetails>();
            if (eventDetails != null && eventDetails.Rows.Count > 0)
            {
                foreach (DataRow row in eventDetails.Rows)
                {
                    int eventId;
                    DateTime startDate;

                    EventDetails eventDetail = new EventDetails
                    {
                        id = int.TryParse(dataTableProcesses.GetThisColumnValue("eventId", row), out eventId) ? Convert.ToInt16(dataTableProcesses.GetThisColumnValue("eventId", row)) : 0,
                        description = dataTableProcesses.GetThisColumnValue("eventDesc", row),
                        startTime = DateTime.TryParse(dataTableProcesses.GetThisColumnValue("eventStartTime", row), out startDate) ? Convert.ToDateTime(dataTableProcesses.GetThisColumnValue("eventStartTime", row)) : DateTime.MinValue,
                        type = dataTableProcesses.GetThisColumnValue("eventType", row),
                        title = dataTableProcesses.GetThisColumnValue("eventTitle", row),
                        venue = dataTableProcesses.GetThisColumnValue("eventVenue", row),
                        iconImageURL = dataTableProcesses.GetThisColumnValue("eventDspPic", row),
                        duration = dataTableProcesses.GetThisColumnValue("eventDuration", row)
                    };

                    eventDetailsEntity.Add(eventDetail);
                }
            }

            return eventDetailsEntity;
        }

        [H.HttpGet]
        public void SendEMail(string toAddress)
        {
            try
            {
                string from = "";   // Replace with your "From" address. This address must be verified.
                string to = "";  // Replace with a "To" address. If your account is still in the
                                          // sandbox, this address must be verified.

                string subject = "OTP For your Login";
                string body = new Guid().ToString();

                // Supply your SMTP credentials below. Note that your SMTP credentials are different from your AWS credentials.
                const String SMTP_USERNAME = "";  // Replace with your SMTP username. 
                const String SMTP_PASSWORD = "";  // Replace with your SMTP password.

                // Amazon SES SMTP host name. This example uses the US West (Oregon) region.
                const String HOST = "email-smtp.us-west-2.amazonaws.com";

                // The port you will connect to on the Amazon SES SMTP endpoint. We are choosing port 587 because we will use
                // STARTTLS to encrypt the connection.
                const int PORT = 587;

                // Create an SMTP client with the specified host name and port.
                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
                {
                    // Create a network credential with your SMTP user name and password.
                    client.Credentials = new System.Net.NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                    // Use SSL when accessing Amazon SES. The SMTP session will begin on an unencrypted connection, and then 
                    // the client will issue a STARTTLS command to upgrade to an encrypted connection using SSL.
                    client.EnableSsl = true;

                    // Send the email. 
                    try
                    {
                        Console.WriteLine("Attempting to send an email through the Amazon SES SMTP interface...");
                        client.Send(from, to, subject, body);
                        //Console.WriteLine("Email sent!");
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("The email was not sent.");
                        //Console.WriteLine("Error message: " + ex.Message);
                    }
                }

                //Console.Write("Press any key to continue...");
                //Console.ReadKey();
            }
            catch (Exception ex) { }
        }
    }
}
