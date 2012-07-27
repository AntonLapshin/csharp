using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace ChatService.Infrastructure.Entities
{
    public class User
    {
        public User()
        {
            Messages = new List<Message>();
        }

        private string _password;
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] HashPassword { get; set; }
        [NotMapped]
        public string Password
        {
            get
            {
                return _password;
            } set
            {
                _password = value;
                SetHashPassword();
            }
        }
        public ICollection<Message> Messages { get; set; }

        public bool IsPasswordValid()
        {
            IStructuralEquatable eq = GetPasswordHash();
            return eq.Equals(HashPassword, StructuralComparisons.StructuralEqualityComparer);
        }

        private byte[] GetPasswordHash()
        {
            var data = Encoding.ASCII.GetBytes(Password);
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            var hashPassword = sha1.ComputeHash(data);
            return hashPassword;
        }

        public void SetHashPassword()
        {
            HashPassword = GetPasswordHash();
        }
    }
}
