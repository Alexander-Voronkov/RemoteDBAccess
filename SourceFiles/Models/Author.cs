using ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Author
    {
        [Key]
        [Index(IsUnique = true)]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        [ForeignKey("Book")]
        public virtual ICollection<Book> Books { get; set; }
        public static byte[] Serialize(Author auth)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, auth);
                return ms.ToArray();
            }
        }
        public static Author Deserialize(byte[] buff)
        {
            using (var ms = new MemoryStream(buff))
            {
                return (Author)new BinaryFormatter().Deserialize(ms); ;
            }
        }

        public static Author FromDTO(AuthorDTO obj)
        {
            return new Author() { Id = obj.Id, Name = obj.Name, Books = obj.Books.Select(x => new Book() { Title = x.Title }).ToList() };
        }

        public static AuthorDTO ToDTO(Author obj)
        {
            return new AuthorDTO() { Id = obj.Id, Name = obj.Name, Books = obj.Books.Select(x=>new BookDTO() { Title = x.Title }).ToList() };
        }
    }
}
