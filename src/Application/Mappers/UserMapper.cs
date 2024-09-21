using src.Application.DTOs;
using src.Domain.Entities;

namespace src.Application.Mappers
{
public static class UserMapper
{
    public static UserDTO ToDTO(User user)
    {
        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Login = user.Login,
            Password = user.Password,
            Location = user.Locations?.Select(l => new LocationDTO
            {
                Lat = l.Lat,
                Lng = l.Lng,
                Address = l.Address,
                City = l.City,
                State = l.State,
                ZipCode = l.ZipCode
            }).FirstOrDefault()
        };
    }

    public static User ToEntity(UserDTO userDTO)
    {
        UserLocation? userLocation = null;
        if(userDTO.Location != null)
        {
            userLocation = new UserLocation
            {
                UserId = userDTO.UserId,
                Lat = userDTO.Location.Lat,
                Lng = userDTO.Location.Lng,
                Address = userDTO.Location.Address,
                City = userDTO.Location.City,
                State = userDTO.Location.State,
                ZipCode = userDTO.Location.ZipCode,
                Active = true
            };
        }

        return new User
        {
            UserId = userDTO.UserId,
            Name = userDTO.Name,
            Email = userDTO.Email,
            Login = userDTO.Login,
            Password = userDTO.Password,
            Locations = [ userLocation ]
        };
    }

    public static void UpdateEntityFromDTO(User userInto, UserDTO userFrom)
    {
        if(userFrom.Location != null)
        {
            userInto.Locations[0].Lat = userFrom.Location.Lat;
            userInto.Locations[0].Lng = userFrom.Location.Lng;
            userInto.Locations[0].Address = userFrom.Location.Address;
            userInto.Locations[0].City = userFrom.Location.City;
            userInto.Locations[0].State = userFrom.Location.State;
            userInto.Locations[0].ZipCode = userFrom.Location.ZipCode;
            userInto.Locations[0].Active = true;
        }

        userInto.Name = userFrom.Name;
        userInto.Email = userFrom.Email;
        userInto.Login = userFrom.Login;
        userInto.Password = userFrom.Password;
    }
}

}