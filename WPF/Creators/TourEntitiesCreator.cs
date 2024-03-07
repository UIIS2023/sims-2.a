using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.Converters
{
    class TourEntitiesCreator
    {
        public static List<Checkpoint> CreateCheckpoints(ObservableCollection<CheckpointCardViewModel> checkpointCards, int tourId)
        {
            var checkpoints = new List<Checkpoint>();
            foreach (var checkpointCard in checkpointCards)
            {
                var checkpoint = new Checkpoint
                {
                    Name = checkpointCard.Name,
                    Type = checkpointCard.Type,
                    TourId = tourId
                };

                checkpoints.Add(checkpoint);
            }
            return checkpoints;
        }

        public static List<Appointment> CreateAppointments(ObservableCollection<AppointmentCardViewModel> appointmentCards, int tourId, int userId)
        {
            var appointments = new List<Appointment>();
            foreach (var appointmentCard in appointmentCards)
            {
                var checkpoint = new Appointment
                {
                    Start = appointmentCard.Start,
                    Occupancy = 0,
                    Started = false,
                    Finished = false,
                    TourId = tourId,
                    UserId = userId
                };

                appointments.Add(checkpoint);
            }
            return appointments;
        }

        public static List<Image> CreateImages(List<string> imagePaths)
        {
            var images = new List<Image>();
            if (imagePaths.Count > 0)
            {
                foreach (var imagePath in imagePaths)
                {
                    var image = new Image();
                    image.Path = imagePath;
                    if (imagePath == imagePaths[0])
                    {
                        image.Cover = true;
                    }
                    image.Cover = false;
                    image.Type = ImageType.TOUR;

                    images.Add(image);
                }
            }
            return images;
        }

    }
}
