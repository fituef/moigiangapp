using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Models
{
    public class HocVi
    {
        [Key]
        public int Id { get; set; }
        public string TenHocVi { get; set; }
        public string Mota {  get; set; }
    }

}