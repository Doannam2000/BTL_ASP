namespace MyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [Key]

        public int MaHD { get; set; }

        [DisplayName("Ngày tạo ")]
        public DateTime NgayTao { get; set; }

        [DisplayName("Hình thức vận chuyển")]
        [Required]
        [StringLength(100)]
        public string HinhThucVanChuyen { get; set; }

        [DisplayName("Hình thức thanh toán")]
        [Required]
        [StringLength(100)]
        public string HinhThucThanhToan { get; set; }

        [DisplayName("Tổng tiền")]
        [Column(TypeName = "money")]
        public decimal TongTien { get; set; }

        [DisplayName("Mã giỏ hàng")]
        public int MaGioHang { get; set; }

        [DisplayName("Tình trạng")]
        [Required]
        [StringLength(100)]
        public string TinhTrang { get; set; }

        public virtual GioHang GioHang { get; set; }
    }
}
