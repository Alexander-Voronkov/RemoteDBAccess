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
    public class Book
    {
        [Key]
        [Index(IsUnique = true)]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Content { get; set; }
        public byte[] Cover { get; set; }
        [ForeignKey("Author")]
        public virtual ICollection<Author> Authors { get; set; }

        public static byte[] Serialize(Book book)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, book);
                return ms.ToArray();
            }
        }
        public static Book Deserialize(byte[] buff)
        {
            using (var ms = new MemoryStream(buff))
            {
                return (Book)new BinaryFormatter().Deserialize(ms); ;
            }
        }

        public static Book FromDTO(BookDTO obj)
        {
            var temp = new Book() { Id = obj.Id, Title = obj.Title, Description = obj.Description, Authors = obj.Authors.Select(x => new Author() { Name = x.Name }).ToList() };
            if(obj.Content != null)
            {
                temp.Content = new byte[obj.Content.Length];
                Array.Copy(obj.Content, temp.Content, temp.Content.Length);
            }
            if (obj.Cover != null)
            {
                temp.Cover = new byte[obj.Cover.Length];
                Array.Copy(obj.Cover, temp.Cover, temp.Cover.Length);
            }
            return temp;
        }

        public static BookDTO ToDTO(Book obj)
        {
            var temp = new BookDTO() { Id = obj.Id, Title = obj.Title, Description = obj.Description, Authors = obj.Authors.Select(x => new AuthorDTO() { Name = x.Name }).ToList() };
            if(obj.Cover != null)
            {
                temp.Cover = new byte[obj.Cover.Length];
                Array.Copy(obj.Cover,temp.Cover, obj.Cover.Length);
            }
            if (obj.Content != null)
            {
                temp.Content = new byte[obj.Content.Length];
                Array.Copy(obj.Content, temp.Content, obj.Content.Length);
            }
            return temp;
        }
    }
}
