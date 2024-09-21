
using src.Application.DTOs;
using src.Domain.Entities;

namespace src.Application.Mappers
{
    public static class DealMapper
    {
        public static DealDTO ToDTO(Deal deal)
        {
            return new DealDTO
            {
                DealId = deal.DealId,
                UserId = deal.UserId,
                Type = deal.Type,
                Value = deal.Value,
                Description = deal.Description,
                TradeFor = deal.TradeFor,
                UrgencyType = deal.UrgencyType,
                Location = new LocationDTO
                {
                    Lat = deal.Location.Lat,
                    Lng = deal.Location.Lng,
                    Address = deal.Location.Address,
                    City = deal.Location.City,
                    State = deal.Location.State,
                    ZipCode = deal.Location.ZipCode
                },
                DealImages = deal.DealImages.Select(di => new DealImageDTO
                {
                    DealImageId = di.DealImageId,
                    DealId = di.DealId,
                    ImageUrl = di.ImageUrl
                }).ToList()
            };
        }

        public static Deal ToEntity(DealDTO dealDTO)
        {
            return new Deal
            {
                DealId = dealDTO.DealId,
                UserId = dealDTO.UserId,
                Type = dealDTO.Type,
                Value = dealDTO.Value,
                Description = dealDTO.Description,
                TradeFor = dealDTO.TradeFor,
                UrgencyType = dealDTO.UrgencyType,
                Location = new DealLocation
                {
                    DealId = dealDTO.DealId,
                    Lat = dealDTO.Location.Lat,
                    Lng = dealDTO.Location.Lng,
                    Address = dealDTO.Location.Address,
                    City = dealDTO.Location.City,
                    State = dealDTO.Location.State,
                    ZipCode = dealDTO.Location.ZipCode
                },
                DealImages = dealDTO.DealImages.Select(di => new DealImage
                {
                    DealImageId = di.DealImageId,
                    DealId = di.DealId,
                    ImageUrl = di.ImageUrl
                }).ToList()
            };
        }

        public static void UpdateEntityFromDTO(Deal deal, DealDTO dealDTO)
        {
            deal.UserId = dealDTO.UserId;
            deal.Type = dealDTO.Type;
            deal.Value = dealDTO.Value;
            deal.Description = dealDTO.Description;
            deal.TradeFor = dealDTO.TradeFor;
            deal.UrgencyType = dealDTO.UrgencyType;

            if (dealDTO.Location != null)
            {
                deal.Location.Lat = dealDTO.Location.Lat;
                deal.Location.Lng = dealDTO.Location.Lng;
                deal.Location.Address = dealDTO.Location.Address;
                deal.Location.City = dealDTO.Location.City;
                deal.Location.State = dealDTO.Location.State;
                deal.Location.ZipCode = dealDTO.Location.ZipCode;
            }

            if (dealDTO.DealImages != null)
            {
                deal.DealImages = dealDTO.DealImages.Select(di => new DealImage
                {
                    DealImageId = di.DealImageId,
                    DealId = di.DealId,
                    ImageUrl = di.ImageUrl
                }).ToList();
            }
        }
    }
}