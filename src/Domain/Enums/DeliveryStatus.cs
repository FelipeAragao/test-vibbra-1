using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Domain.Enums
{
    public enum DeliveryStatus
    {
        OrderReceived = 1,
        OrderProcessing = 2,
        OrderPacked = 3,
        AwaitingShipment = 4,
        Shipped = 5,
        InTransit = 6,
        DeliveredToLocalDistributionCenter = 7,
        OutForDelivery = 8,
        DeliveryAttemptFailed = 9,
        Delivered = 10,
        DeliveryConfirmed = 11,
        ReturnedOrCanceled = 12
    }
}