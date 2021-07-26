namespace MyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [Key]
        public int MaHD { get; set; }

        public DateTime NgayTao { get; set; }

        [Required]
        [StringLength(100)]
        public string HinhThucVanChuyen { get; set; }

        [Required]
        [StringLength(100)]
        public string HinhThucThanhToan { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTien { get; set; }

        public int MaGioHang { get; set; }

        [Required]
        [StringLength(100)]
        public string TinhTrang { get; set; }

        public virtual GioHang GioHang { get; set; }
    }
}
