using ShopServiceApi.Infrastructure.Site.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopServiceApi.Infrastructure.Site.Enum
{

    public enum OrderStatus
    {
        [Display(Name = "در انتظار پرداخت")]
        Pending,

        [Display(Name = "پرداخت شده")]
        Paid,

        [Display(Name = "ارسال شده")]
        Shipped,

        [Display(Name = "تکمیل شده")]
        Completed,

        [Display(Name = "لغو شده")]
        Cancelled
    }
}
