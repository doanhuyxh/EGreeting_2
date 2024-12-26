using EGreeting.Models;
using EGreeting.Models.EmailListViewModels;

namespace EGreeting.Services
{
    public interface ICommon
    {
        string GetSHA256(string str);
        string GetMD5(string str);
        string GenerateRandomString(int lenght);
        Task<string> UploadedFile(IFormFile ProfilePicture);
        Task<List<CategoriesCard>> MenuNav();
        Task SendEmail(Cards request, EmailListCRUD model);

    }

}
