using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevExpress.DevAV {
    public class Product : DatabaseObject {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ProductionStart { get; set; }
        public bool Available { get; set; }
        public byte[] Image { get; set; }
        public virtual Employee Support { get; set; }
        public long? SupportId { get; set; }
        public virtual Employee Engineer { get; set; }
        public long? EngineerId { get; set; }
        public int? CurrentInventory { get; set; }
        public int Backorder { get; set; }
        public int Manufacturing { get; set; }
        public byte[] Barcode { get; set; }
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }
        [DataType(DataType.Currency)]
        public decimal SalePrice { get; set; }
        [DataType(DataType.Currency)]
        public decimal RetailPrice { get; set; }
        public double ConsumerRating { get; set; }
        public ProductCategory Category { get; set; }
    }

    public enum ProductCategory {
        [Display(Name = "Automation")]
        Automation,
        [Display(Name = "Monitors")]
        Monitors,
        [Display(Name = "Projectors")]
        Projectors,
        [Display(Name = "Televisions")]
        Televisions,
        [Display(Name = "Video Players")]
        VideoPlayers,
    }

}
