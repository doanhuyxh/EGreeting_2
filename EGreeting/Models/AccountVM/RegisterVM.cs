using System.ComponentModel.DataAnnotations;

namespace EGreeting.Models.AccountVM
{
    public class RegisterVM
    {
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }


        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
        
       
        public DateTime CreateDate { get; set; }



        public static implicit operator ApplicationUser(RegisterVM vm)
        {
            return new ApplicationUser
            {
                IsActive = true,
                CreateDate = vm.CreateDate,
                FullName = vm.FullName,
                Email = vm.Email,

            };
        }
    }
}
