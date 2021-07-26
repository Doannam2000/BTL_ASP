namespace MyPham.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            Chi_Tiet_Gio_Hang = new HashSet<Chi_Tiet_Gio_Hang>();
        }

        [Key]
        public int MaSP { get; set; }

        [Required]
        [StringLength(80)]
        public string TenSP { get; set; }

        [Column(TypeName = "money")]
        public decimal Gia { get; set; }

        [StringLength(80)]
        public string ThuongHieu { get; set; }

        public int SoLuongTon { get; set; }

        [StringLength(80)]
        public string XuatXu { get; set; }

        public int? DungTich { get; set; }

        [StringLength(80)]
        public string HangSX { get; set; }

        public int? TrongLuong { get; set; }

        [StringLength(60)]
        public string ChatLieu { get; set; }

        [Column(TypeName = "ntext")]
        public string CongDung { get; set; }

        [Column(TypeName = "ntext")]
        public string ThanhPhan { get; set; }

        [Column(TypeName = "ntext")]
        public string HuongDanSD { get; set; }

        [Column(TypeName = "ntext")]
        public string QuyCachDongGoi { get; set; }

        public double? GiamGia { get; set; }

        public int MaDM { get; set; }

        [Required]
        [StringLength(100)]
        public string AnhSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chi_Tiet_Gio_Hang> Chi_Tiet_Gio_Hang { get; set; }

        public virtual DanhMucSP DanhMucSP { get; set; }
    }
}
