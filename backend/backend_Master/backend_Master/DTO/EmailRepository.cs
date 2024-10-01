using backend_Master.Models;

namespace backend_Master.DTO
{
    public class EmailRepository
    {
        private readonly List<EmailMessage> _messages = new List<EmailMessage>();
        private readonly MyDbContext _context;

        public void AddMessage(EmailMessage message)
            {
                _context.EmailMessages.Add(message);
                _context.SaveChanges();
            }

            public IEnumerable<EmailMessage> GetAllMessages()
            {
                return _context.EmailMessages.ToList();
            }
        }

    }

