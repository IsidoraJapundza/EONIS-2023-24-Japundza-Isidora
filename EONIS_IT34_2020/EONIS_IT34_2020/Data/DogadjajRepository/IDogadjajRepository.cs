﻿using EONIS_IT34_2020.Models.DTOs.Dogadjaj;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Data.DogadjajRepository
{
    public interface IDogadjajRepository
    {
        List<Dogadjaj> GetDogadjaj();
        Dogadjaj GetDogadjajById(Guid Id_dogadjaj);
        Dogadjaj CreateDogadjaj(Dogadjaj dogadjaj);
        void UpdateDogadjaj(Dogadjaj dogadjaj); // izmeniti
        void DeleteDogadjaj(Guid Id_dogadjaj);
        bool SaveChanges();
    }
}
