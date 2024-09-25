using src.Application.DTOs;
using src.Domain.Entities;
using src.Domain.Enums;

namespace src.Application.Mappers
{
    public static class DeliveryMapper
    {
        public static DeliveryDTO ToDTO(Delivery delivery)
        {
            return new DeliveryDTO
            {
                DeliveryId = delivery.DeliveryId,
                DealId = delivery.DealId,
                UserId = delivery.UserId,
                DeliveryPrice = delivery.DeliveryPrice,
                Steps = delivery.Steps != null ? MapStepsToDTO(delivery.Steps) : null
            };
        }

        public static Delivery FromDTO(DeliveryDTO deliveryDTO)
        {
            return new Delivery
            {
                DeliveryId = deliveryDTO.DeliveryId,
                DealId = deliveryDTO.DealId,
                UserId = deliveryDTO.UserId,
                DeliveryPrice = deliveryDTO.DeliveryPrice,
                Steps = deliveryDTO.Steps != null ? MapStepsFromDTO(deliveryDTO.Steps) : null
            };
        }

        private static List<DeliveryStepsDTO> MapStepsToDTO(List<DeliverySteps> steps)
        {
            var stepsDTO = new List<DeliveryStepsDTO>();
            foreach (var step in steps)
            {
                if (step != null)
                {
                    stepsDTO.Add(new DeliveryStepsDTO
                    {
                        DeliveryStepsId = step.DeliveryStepsId,
                        DeliveryId = step.DeliveryId,
                        Date = step.Date,
                        DeliveryStatus = step.DeliveryStatus.ToString(),
                        Active = step.Active
                    });
                }
            }
            return stepsDTO;
        }

        private static List<DeliverySteps> MapStepsFromDTO(List<DeliveryStepsDTO> stepsDTO)
        {
            var steps = new List<DeliverySteps>();
            foreach (var stepDTO in stepsDTO)
            {
                if (stepDTO != null)
                {
                    steps.Add(new DeliverySteps
                    {
                        DeliveryStepsId = stepDTO.DeliveryStepsId,
                        DeliveryId = stepDTO.DeliveryId,
                        Date = stepDTO.Date,
                        DeliveryStatus = Enum.Parse<DeliveryStatus>(stepDTO.DeliveryStatus),
                        Active = stepDTO.Active
                    });
                }
            }
            return steps;
        }
    }
}