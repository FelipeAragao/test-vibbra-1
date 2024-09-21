using src.Application.DTOs;
using src.Domain.Entities;

namespace src.Application.Mappers
{
    public static class InviteMapper
    {
        public static InviteDTO ToDTO(Invite invite)
        {
            return new InviteDTO
            {
                InviteId = invite.InviteId,
                UserId = invite.UserId,
                UserInvitedId = invite.UserInvitedId,
                Name = invite.Name,
                Email = invite.Email,
                User = invite.User != null ? UserMapper.ToDTO(invite.User) : null,
                UserInvited = invite.UserInvited != null ? UserMapper.ToDTO(invite.UserInvited) : null
            };
        }

        public static Invite ToEntity(InviteDTO inviteDTO)
        {
            return new Invite
            {
                InviteId = inviteDTO.InviteId,
                UserId = inviteDTO.UserId,
                UserInvitedId = inviteDTO.UserInvitedId,
                Name = inviteDTO.Name,
                Email = inviteDTO.Email
            };
        }
        public static void UpdateEntityFromDTO(Invite invite, InviteDTO inviteDTO)
        {
            invite.UserId = inviteDTO.UserId;
            invite.UserInvitedId = inviteDTO.UserInvitedId;
            invite.Name = inviteDTO.Name;
            invite.Email = inviteDTO.Email;
        }
    }
}