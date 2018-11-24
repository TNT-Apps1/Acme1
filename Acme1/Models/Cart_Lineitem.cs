using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Acme1.Models
{
    public class Cart_Lineitem
    {


        [Required, Key, Column(Order = 1)]
        public int CartNumber { get; set; }
        [Required, Key, Column(Order = 2), MaxLength(10)]
        public string ProductId { get; set; }
        [Required, Range(minimum: 1, maximum: 99, ErrorMessage = "Qty sb 1-99")]
        public int Quantity { get; set; }


        public static Int32 CartUpSert(SqlConnection dbcon, Cart_Lineitem cart)
        {
            SqlCommand cmd = new SqlCommand("sp_cart_upsert", dbcon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cartid", SqlDbType.Int).Value = cart.CartNumber;
            cmd.Parameters.Add("@prodid", SqlDbType.VarChar).Value = cart.ProductId;
            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = cart.Quantity;
            int intCnt = cmd.ExecuteNonQuery();
            return intCnt;
        }



    }
}