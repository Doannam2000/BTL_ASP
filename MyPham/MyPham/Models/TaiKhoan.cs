namespace MyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            GioHang = new HashSet<GioHang>();
        }

        [Key]
        public int MaTK { get; set; }

        [Required(ErrorMessage ="Bạn chưa nhập Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Bạn chưa nhập Mật Khẩu")]
        [StringLength(20, MinimumLength = 6 , ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }


        [Required(ErrorMessage = "Bạn chưa nhập Họ tên")]
        [StringLength(50)]
        public string HoTen { get; set; }

        [Column(TypeName = "ntext")]
        public string DiaChi { get; set; }

        [StringLength(10)]
        public string SoDienThoai { get; set; }

        [StringLength(50)]
        public string Anh { get; set; }

        public bool TinhTrang { get; set; }

        public int MaQuyen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHang { get; set; }

        public virtual PhanQuyen PhanQuyen { get; set; }
    }
}
