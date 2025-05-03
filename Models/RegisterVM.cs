using System.ComponentModel.DataAnnotations;
namespace Htime.Models
{
    public class RegisterVM
    {
        
       

        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage ="Tối Đa 100 kí tự")]
        public string Name { get; set; }
  
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối Đa 100 kí tự")]
        public string Email { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "Tối Đa 50 kí tự")]
        public string MatKhau { get; set; }

        
    }
}
