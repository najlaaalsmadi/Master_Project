using PayPal.Api;
using System;
using System.Data.SqlClient;

public class PaymentRepository
{
        private string connectionString = "YourDatabaseConnectionString";

        public void SavePayment(Payment payment, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Payments (UserId, PaymentMethod, Amount, PaymentDate, PayerId, Status) VALUES (@UserId, @PaymentMethod, @Amount, @PaymentDate, @PayerId, @Status)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId); // ربط بـ UserId
                    command.Parameters.AddWithValue("@PaymentMethod", payment.payer.payment_method);
                    command.Parameters.AddWithValue("@Amount", payment.transactions[0].amount.total);
                    command.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                    command.Parameters.AddWithValue("@PayerId", payment.payer.payer_info.payer_id);
                    command.Parameters.AddWithValue("@Status", payment.state);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
