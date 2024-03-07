using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using System.Collections.Generic;


namespace SOSTeam.TravelAgency.Application.Services
{
    public class ImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService()
        {
            _imageRepository = Injector.CreateInstance<IImageRepository>();
        }

        public void Delete(int id)
        {
            _imageRepository.Delete(id);
        }

        public List<Image> GetAll()
        {
            return _imageRepository.GetAll();
        }

        public List<Image> GetAllForTours()
        {
            return _imageRepository.GetAllForTours();
        }

        public List<Image> GetAllForAccommodations()
        {
            return _imageRepository.GetAllForAccommodations();
        }

        public Image? GetTourCover(int tourId)
        {
            return _imageRepository.GetAllForTours().Find(i => i.EntityId == tourId && i.Cover);
        }

        public Image? GetAccommodationCover(int accommodationId)
        {
            return _imageRepository.GetAllForAccommodations().Find(i => i.EntityId == accommodationId && i.Cover);
        }

        public Image? GetById(int id)
        {
            return _imageRepository.GetById(id);
        }

        public void Save(Image image)
        {
            _imageRepository.Save(image);
        }

        public void SaveAll(List<Image> images)
        {
            _imageRepository.SaveAll(images);
        }

        public void Update(Image image)
        {
            _imageRepository.Update(image);
        }

    }
}
