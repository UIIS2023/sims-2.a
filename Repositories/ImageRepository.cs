using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private const string FilePath = "../../../Resources/Data/images.csv";

        private readonly Serializer<Image> _serializer;

        private List<Image> _images;

        public ImageRepository()
        {
            _serializer = new Serializer<Image>();
            _images = _serializer.FromCSV(FilePath);
        }


        public void Delete(int id)
        {
            _images = _serializer.FromCSV(FilePath);
            Image founded = _images.Find(i => i.Id == id) ?? throw new ArgumentException();
            _images.Remove(founded);
            _serializer.ToCSV(FilePath, _images);
        }

        public List<Image> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<Image> GetAllForTours()
        {
            _images = _serializer.FromCSV(FilePath);
            return _images.FindAll(i => i.Type == ImageType.TOUR);
        }
        public List<Image> GetAllForAccommodations()
        {
            _images = _serializer.FromCSV(FilePath);
            return _images.FindAll(i => i.Type == ImageType.ACCOMMODATION);
        }

        public Image? GetById(int id)
        {
            _images = _serializer.FromCSV(FilePath);
            return _images.Find(i => i.Id == id);
        }


        public int NextId()
        {
            _images = _serializer.FromCSV(FilePath);
            if (_images.Count < 1)
            {
                return 1;
            }
            return _images.Max(t => t.Id) + 1;
        }

        public void Save(Image image)
        {
            image.Id = NextId();
            _images = _serializer.FromCSV(FilePath);
            _images.Add(image);
            _serializer.ToCSV(FilePath, _images);
        }

        public void SaveAll(List<Image> images)
        {
            foreach (Image image in images)
            {
                Save(image);
            }
        }

        public void Update(Image image)
        {
            _images = _serializer.FromCSV(FilePath);
            Image current = _images.Find(t => t.Id == image.Id) ?? throw new ArgumentException();
            int index = _images.IndexOf(current);
            _images.Remove(current);
            _images.Insert(index, image);
            _serializer.ToCSV(FilePath, _images);
        }
    }
}
