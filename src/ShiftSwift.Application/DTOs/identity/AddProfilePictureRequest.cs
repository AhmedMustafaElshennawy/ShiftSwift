using Microsoft.AspNetCore.Http;

namespace ShiftSwift.Application.DTOs.identity
{
    public sealed record AddProfilePictureRequest(IFormFile FormFile);
   
    
}
