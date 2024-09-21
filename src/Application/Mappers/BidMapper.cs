using src.Domain.Entities;
using src.Application.DTOs;

public static class BidMapper
{
    public static BidDTO ToDTO(Bid bid)
    {
        return new BidDTO
        {
            BidId = bid.BidId,
            UserId = bid.UserId,
            DealId = bid.DealId,
            Accepted = bid.Accepted,
            Value = bid.Value,
            Description = bid.Description
        };
    }

    public static Bid ToEntity(BidDTO bidDTO)
    {
        return new Bid
        {
            BidId = bidDTO.BidId,
            UserId = bidDTO.UserId,
            DealId = bidDTO.DealId,
            Accepted = bidDTO.Accepted,
            Value = bidDTO.Value,
            Description = bidDTO.Description
        };
    }

    public static void UpdateEntityFromDTO(Bid bid, BidDTO bidDTO)
    {
        bid.UserId = bidDTO.UserId;
        bid.DealId = bidDTO.DealId;
        bid.Accepted = bidDTO.Accepted;
        bid.Value = bidDTO.Value;
        bid.Description = bidDTO.Description;
    }
}
