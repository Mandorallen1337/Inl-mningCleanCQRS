using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Databases
{
    public class FakeDatabase
    {
        public List<Book> Books = new List<Book>();
        public List<Author> Authors = new List<Author>();
        

        public FakeDatabase()
        {
            Books.AddRange(new List<Book>
                {
                    new Book("The Great Gatsby", "The Great Gatsby is a 1925 novel by American writer F. Scott Fitzgerald. Set in the Jazz Age on Long Island, near New York City, the novel depicts first-person narrator Nick Carraway's interactions with mysterious millionaire Jay Gatsby and Gatsby's obsession to reunite with his former lover, Daisy Buchanan."),
                    new Book("To Kill a Mockingbird", "To Kill a Mockingbird is a novel by the American author Harper Lee. It was published in 1960 and was instantly successful. In the United States, it is widely read in high schools and middle schools."),
                    new Book("1984", "1984 is a dystopian social science fiction novel by English novelist George Orwell. It was published on 8 June 1949 by Secker & Warburg as Orwell's ninth and final book completed in his lifetime."),
                    new Book("Harry Potter and the Philosopher's Stone", "Harry Potter and the Philosopher's Stone is a fantasy novel written by British author J. K. Rowling. The first novel in the Harry Potter series and Rowling's debut novel, it follows Harry Potter, a young wizard who discovers his magical heritage on his eleventh birthday, when he receives a letter of acceptance to Hogwarts School of Witchcraft and Wizardry."),
                    new Book("The Catcher in the Rye", "The Catcher in the Rye is a novel by J. D. Salinger, partially published in serial form in 1945–1946 and as a novel in 1951. It was originally intended for adults but is often read by adolescents for its themes of angst, alienation, and as a critique on superficiality in society.")
                });

            Authors.AddRange(new List<Author>
                {
                    new Author("F. Scott", "Fitzgerald"),
                    new Author("Harper", "Lee"),
                    new Author("George", "Orwell"),
                    new Author("J. K.", "Rowling"),
                    new Author("J. D.", "Salinger")
                });            

        }

        public List<User> Users
        {
            get
            {
                return allUsers;
            }
            set
            {
                allUsers = value;
            }
        }
        private static List<User> allUsers = new(){
            new User{Id = Guid.NewGuid(), UserName = "admin"},
            new User{Id = Guid.NewGuid(), UserName = "user"}
        };
    }
}
