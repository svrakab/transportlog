using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Transport.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        Transport.Models.TransportLogEntities appointmentContext = new Transport.Models.TransportLogEntities();
        Transport.Models.TransportLogEntities resourceContext = new Transport.Models.TransportLogEntities();

        public ActionResult SchedulerPartial()
        {
            var appointments = appointmentContext.Load;
            var resources = resourceContext.LoadType;

            ViewData["Appointments"] = appointments.ToList();
            ViewData["Resources"] = resources.ToList();

            return PartialView("_SchedulerPartial");
        }
        public ActionResult SchedulerPartialEditAppointment()
        {
            var appointments = appointmentContext.Load;
            var resources = resourceContext.LoadType;

            try
            {
                HomeControllerSchedulerSettings.UpdateEditableDataObject(appointmentContext, resourceContext);
            }
            catch (Exception e)
            {
                ViewData["SchedulerErrorText"] = e.Message;
            }

            ViewData["Appointments"] = appointments.ToList();
            ViewData["Resources"] = resources.ToList();

            return PartialView("_SchedulerPartial");
        }
    }
    public class HomeControllerSchedulerSettings
    {
        static DevExpress.Web.Mvc.MVCxAppointmentStorage appointmentStorage;
        public static DevExpress.Web.Mvc.MVCxAppointmentStorage AppointmentStorage
        {
            get
            {
                if (appointmentStorage == null)
                {
                    appointmentStorage = new DevExpress.Web.Mvc.MVCxAppointmentStorage();
                    appointmentStorage.Mappings.AppointmentId = "ID";
                    appointmentStorage.Mappings.Start = "PlannedTime";
                    appointmentStorage.Mappings.End = "DockOff";
                    appointmentStorage.Mappings.Subject = "IDUser";
                    appointmentStorage.Mappings.Description = "";
                    appointmentStorage.Mappings.Location = "";
                    appointmentStorage.Mappings.AllDay = "";
                    appointmentStorage.Mappings.Type = "IDLoadType";
                    appointmentStorage.Mappings.RecurrenceInfo = "";
                    appointmentStorage.Mappings.ReminderInfo = "";
                    appointmentStorage.Mappings.Label = "";
                    appointmentStorage.Mappings.Status = "IDStatus";
                    appointmentStorage.Mappings.ResourceId = "";
                }
                return appointmentStorage;
            }
        }

        static DevExpress.Web.Mvc.MVCxResourceStorage resourceStorage;
        public static DevExpress.Web.Mvc.MVCxResourceStorage ResourceStorage
        {
            get
            {
                if (resourceStorage == null)
                {
                    resourceStorage = new DevExpress.Web.Mvc.MVCxResourceStorage();
                    resourceStorage.Mappings.ResourceId = "ID";
                    resourceStorage.Mappings.Caption = "Name";
                }
                return resourceStorage;
            }
        }

        public static void UpdateEditableDataObject(Transport.Models.TransportLogEntities appointmentContext, Transport.Models.TransportLogEntities resourceContext)
        {
            InsertAppointments(appointmentContext, resourceContext);
            UpdateAppointments(appointmentContext, resourceContext);
            DeleteAppointments(appointmentContext, resourceContext);
        }

        static void InsertAppointments(Transport.Models.TransportLogEntities appointmentContext, Transport.Models.TransportLogEntities resourceContext)
        {
            var appointments = appointmentContext.Load.ToList();
            var resources = resourceContext.LoadType;

            var newAppointments = DevExpress.Web.Mvc.SchedulerExtension.GetAppointmentsToInsert<Transport.Models.Load>("Scheduler", appointments, resources,
                AppointmentStorage, ResourceStorage);
            foreach (var appointment in newAppointments)
            {
                appointmentContext.Load.Add(appointment);
            }
            appointmentContext.SaveChanges();
        }
        static void UpdateAppointments(Transport.Models.TransportLogEntities appointmentContext, Transport.Models.TransportLogEntities resourceContext)
        {
            var appointments = appointmentContext.Load.ToList();
            var resources = resourceContext.LoadType;

            var updAppointments = DevExpress.Web.Mvc.SchedulerExtension.GetAppointmentsToUpdate<Transport.Models.Load>("Scheduler", appointments, resources,
                AppointmentStorage, ResourceStorage);
            foreach (var appointment in updAppointments)
            {
                var origAppointment = appointments.FirstOrDefault(a => a.ID == appointment.ID);
                appointmentContext.Entry(origAppointment).CurrentValues.SetValues(appointment);
            }
            appointmentContext.SaveChanges();
        }

        static void DeleteAppointments(Transport.Models.TransportLogEntities appointmentContext, Transport.Models.TransportLogEntities resourceContext)
        {
            var appointments = appointmentContext.Load.ToList();
            var resources = resourceContext.LoadType;

            var delAppointments = DevExpress.Web.Mvc.SchedulerExtension.GetAppointmentsToRemove<Transport.Models.Load>("Scheduler", appointments, resources,
                AppointmentStorage, ResourceStorage);
            foreach (var appointment in delAppointments)
            {
                var delAppointment = appointments.FirstOrDefault(a => a.ID == appointment.ID);
                if (delAppointment != null)
                    appointmentContext.Load.Remove(delAppointment);
            }
            appointmentContext.SaveChanges();
        }
    }

}