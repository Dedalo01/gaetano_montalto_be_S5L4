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

                SqlCommand selectAllItemCmd = new SqlCommand(selectAllItemQuery, conn);

                SqlDataReader itemTableReader = selectAllItemCmd.ExecuteReader();

                if (itemTableReader.HasRows)
                {
                    while (itemTableReader.Read())
                    {
                        Item item = new Item(
                            (int)itemTableReader["Id"],
                            (string)itemTableReader["Name"],
                            Convert.ToDouble(itemTableReader["Price"]),
                            (string)itemTableReader["Description"],
                            itemTableReader["ImageMain"] != DBNull.Value ? (string)itemTableReader["ImageMain"] : string.Empty,
                            itemTableReader["ImageSecond"] != DBNull.Value ? (string)itemTableReader["ImageSecond"] : string.Empty,
                            itemTableReader["ImageThird"] != DBNull.Value ? (string)itemTableReader["ImageThird"] : string.Empty
                        );

                        items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }
            finally
            {
                conn.Close();
            }

            return View(items);
        }

        public IActionResult Details([FromRoute] int id)
        {
            Item item;

            if (id > 0)
            {
                SqlConnection conn = new SqlConnection(connString);

                try
                {
                    conn.Open();

                    string selectItemByIdQuery = "SELECT * FROM Item WHERE Id = @ItemId";
                    SqlCommand selectItemByIdCmd = new SqlCommand(selectItemByIdQuery, conn);
                    selectItemByIdCmd.Parameters.AddWithValue("ItemId", id);

                    SqlDataReader itemTableReader = selectItemByIdCmd.ExecuteReader();

                    if (itemTableReader.HasRows)
                    {
                        itemTableReader.Read();
                        item = new Item(
                            (int)itemTableReader["Id"],
                            (string)itemTableReader["Name"],
                            Convert.ToDouble(itemTableReader["Price"]),
                            (string)itemTableReader["Description"],
                            itemTableReader["ImageMain"] != DBNull.Value ? (string)itemTableReader["ImageMain"] : string.Empty,
                            itemTableReader["ImageSecond"] != DBNull.Value ? (string)itemTableReader["ImageSecond"] : string.Empty,
                            itemTableReader["ImageThird"] != DBNull.Value ? (string)itemTableReader["ImageThird"] : string.Empty);

                        return View(item);
                    }
                }
                catch (Exception ex)
                {

                }
                finally { conn.Close(); }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Error");
        }


        public IActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddItem(Item item, IFormFile image)
        {
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string fileName = Path.GetFileName(image.FileName);
                string fullFilePath = Path.Combine(path, fileName);

                FileStream stream = new FileStream(fullFilePath, FileMode.Create);
                image.CopyTo(stream);

                string insertIntoItemTableQuery = @"INSERT INTO Item
                    (Name, Price, Description, ImageMain, ImageSecond, ImageThird)
                    VALUES
                    (@name, @price, @description, @imageMain, @imageSecond, @imageThird)";
                SqlCommand insertIntoItemTableCmd = new SqlCommand(insertIntoItemTableQuery, conn);
                insertIntoItemTableCmd.Parameters.AddWithValue("name", item.Name);
                insertIntoItemTableCmd.Parameters.AddWithValue("price", item.Price);
                insertIntoItemTableCmd.Parameters.AddWithValue("description", item.Description);
                insertIntoItemTableCmd.Parameters.AddWithValue("imageMain", fileName);
                insertIntoItemTableCmd.Parameters.AddWithValue("imageSecond", "");
                insertIntoItemTableCmd.Parameters.AddWithValue("imageThird", "");

                int rowsNumber = insertIntoItemTableCmd.ExecuteNonQuery();

            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return RedirectToAction("Index");
        }

        public IActionResult Edit([FromRoute] int id)
        {
            Item item;

            if (id > 0)
            {
                SqlConnection conn = new SqlConnection(connString);

                try
                {
                    conn.Open();

                    string selectItemByIdQuery = "SELECT * FROM Item WHERE Id = @ItemId";
                    SqlCommand selectItemByIdCmd = new SqlCommand(selectItemByIdQuery, conn);
                    selectItemByIdCmd.Parameters.AddWithValue("ItemId", id);

                    SqlDataReader itemTableReader = selectItemByIdCmd.ExecuteReader();

                    if (itemTableReader.HasRows)
                    {
                        itemTableReader.Read();
                        item = new Item(
                            (int)itemTableReader["Id"],
                            (string)itemTableReader["Name"],
                            Convert.ToDouble(itemTableReader["Price"]),
                            (string)itemTableReader["Description"],
                            itemTableReader["ImageMain"] != DBNull.Value ? (string)itemTableReader["ImageMain"] : string.Empty,
                            itemTableReader["ImageSecond"] != DBNull.Value ? (string)itemTableReader["ImageSecond"] : string.Empty,
                            itemTableReader["ImageThird"] != DBNull.Value ? (string)itemTableReader["ImageThird"] : string.Empty);

                        return View(item);
                    }
                }
                catch (Exception ex)
                {

                }
                finally { conn.Close(); }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Error");
        }

        [HttpPost]
        public IActionResult Edit(Item item, IFormFile image)
        {

            string editItemFromDb = @"
                UPDATE Item 
                SET Name = @name, Price = @price, Description = @desc, ImageMain = @imgMain
                WHERE Id = @itemId
                ";

            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string fileName = Path.GetFileName(image.FileName);
                string fullFilePath = Path.Combine(path, fileName);

                FileStream stream = new FileStream(fullFilePath, FileMode.Create);
                image.CopyTo(stream);


                SqlCommand cmd = new SqlCommand(editItemFromDb, conn);
                cmd.Parameters.AddWithValue("name", item.Name);
                cmd.Parameters.AddWithValue("price", item.Price);
                cmd.Parameters.AddWithValue("desc", item.Description);
                cmd.Parameters.AddWithValue("ImgMain", fileName);
                cmd.Parameters.AddWithValue("itemId", item.Id);

                int nRows = cmd.ExecuteNonQuery();

                return RedirectToAction("Index", "Admin");


            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return View("Error");
        }
    }
}
