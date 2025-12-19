using CryptocoinsFilter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CryptocoinsFilter.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Check(Contacts contact)
        {
            if (ModelState.IsValid)
            {
                string connectionString = "Server=LAPTOP-EK1FURHH\\SQLEXPRESS;Database=CryptoExchanges;Trusted_Connection=True;Encrypt=False;";

                using var conn = new SqlConnection(connectionString);
                await conn.OpenAsync();

                // Delete feedbacks older than 1 month
                using (var deleteCmd = new SqlCommand(
                    "DELETE FROM Feedbacks WHERE CreatedAt < DATEADD(MONTH, -1, GETDATE())",
                    conn))
                {
                    await deleteCmd.ExecuteNonQueryAsync();
                }


                using var cmd = new SqlCommand(
                    "INSERT INTO Feedbacks (Name, Email, Message, CreatedAt) VALUES (@Name, @Email, @Message, @CreatedAt)",conn
                );

                cmd.Parameters.AddWithValue("@Name", contact.Name + " " + contact.Surname);
                cmd.Parameters.AddWithValue("@Email", contact.Email);
                cmd.Parameters.AddWithValue("@Message", contact.Message);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                await cmd.ExecuteNonQueryAsync();

                TempData["SuccessMessage"] = "✅ Your message has been sent successfully!";
                return RedirectToAction("Index");
            }

            return View("Index", contact);
        }

        public IActionResult Feedback()
        {
            return View("Index");
        }
    }
}
