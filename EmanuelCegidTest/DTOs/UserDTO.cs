using Microsoft.AspNetCore.Identity;

namespace EmanuelCegidTest.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


        public IdentityUser DtoToEntity(UserDTO UserDTO)
        {
            IdentityUser userEntity = new IdentityUser
            {
                UserName = UserDTO.Email,
                Email = UserDTO.Email,
                EmailConfirmed = true
            };

            return userEntity;
        }
    }
}
