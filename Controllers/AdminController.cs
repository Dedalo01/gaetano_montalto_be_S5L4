using EserS5L4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EserS5L4.Controllers
{
    public class AdminController : Controller
    {

        private string connString = "Server=MSI\\SQLEXPRESS; Initial Catalog=Eser_S5L4; Integrated Security=true; TrustServerCertificate=True";
        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(connString);
            List<Item> items = new List<Item>();

            try
            {
                conn.Open();
                string selectAllItemQuery = "SELECT * FROM Item";

                SqlCommand selectAllItemCmd = new SqlCommand()
            }

            return View();
        }
    }
}
