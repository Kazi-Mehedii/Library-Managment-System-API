using Microsoft.EntityFrameworkCore;

namespace Library_managment_system_API.Entities
{
    public class ContextDb : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<BookCategory> BookCategories { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Order> Orders { get; set; }



        public ContextDb(DbContextOptions<ContextDb> options) : base(options)
        {

        }

        //seeding/insert data for checking purpose
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                FirstName = "Mehedi",
                LastName = "kazi",
                Email = "mehedi@gmail.com",
                MobileNumber = "01750323645",
                Password = "Password1234",
                UserType = UserType.ADMIN,
                AccountStatus = AccountStatus.ACTIVE,
                CreatedOn = new DateTime(2023, 12, 01, 12, 0, 0)
            });

            modelBuilder.Entity<BookCategory>().HasData(
                new BookCategory() { Id = 1, Catagory = "Computer", SubCatagory = "DataStructure" },
                new BookCategory() { Id = 2, Catagory = "Mechanical", SubCatagory = "Machine" },
                 new BookCategory() { Id = 3, Catagory = "Methamatics", SubCatagory = "Calculas" },
                  new BookCategory() { Id = 4, Catagory = "Methamatics", SubCatagory = "Algebra" },
                  new BookCategory() { Id = 5, Catagory = "Computer", SubCatagory = "C# Programing" }

                );


            modelBuilder.Entity<Book>().HasData(

                  new Book() { Id = 1, BookCategoryId =  1, Ordered = false, Price = 50000, Author= "Thomas H. Cormen", Title= "Author Thomas Cormen’s “Algorithms Unlocked” seeks to take away the mystery of technology and unveils the secrets behind its inner workings" }
                , new Book() { Id = 2, BookCategoryId = 1, Ordered = false, Price = 30000, Author = "Steven S. Skiena", Title = "The Algorithm Design Manual” is an introduction to creating algorithms on your own from scratch" }
                , new Book() { Id = 3, BookCategoryId = 1, Ordered = false, Price = 40000, Author = " Robert Sedgewick and Kevin Wayne ", Title = "A simple title for a not so simple book, “Algorithms” is incredibly succinct in its naming and belies the full depth of what it covers" }


                , new Book() { Id = 4, BookCategoryId = 2, Ordered = false, Price = 30000, Author = "E. Oberg", Title = "Is it possible to find a book with all the information you need on machinery, metalworking, manufacturing, and even architecture" }
                , new Book() { Id = 5, BookCategoryId = 2, Ordered = false, Price = 4000, Author = "An Introduction to Mechanical Engineering by J. Wickert and  K. Lewis", Title = "A simple title for a not so simple book, “Algorithms” is incredibly succinct in its naming and belies the full depth of what it covers" }
                , new Book() { Id = 6, BookCategoryId = 2, Ordered = false , Price = 3000, Author = "Mechanics of Fluids", Title= "This work should lie on the bedside table of every mechanical engineer who is constantly improving their knowledge" }


                , new Book() { Id = 7, BookCategoryId = 3, Ordered = false, Price = 10000, Author = "Calculus: Early Transcendentals by James Stewart", Title = "It’s written in a very easy and intuitive style and has a ton of exercises." }
                , new Book() { Id = 8, BookCategoryId = 3, Ordered = false, Price = 8000, Author = "Calculus by Michael Spivak", Title = "Spivak presents calculus as “the first real encounter with mathematics” and doesn’t just teach you the concepts of calculus." }
                , new Book() { Id = 9, BookCategoryId = 3, Ordered = false, Price = 7000, Author = "Calculus for Dummies by Mark Ryan", Title = "Do you have a hard time learning maths? This is the book to change that!"  }

                , new Book() { Id = 10, BookCategoryId = 4, Ordered = false, Price = 15000, Author = "Algebra: Structure and Method, Book 1\r\nby Richard G. Brown", Title = "The textbook breaks down basic algebraic theories and elementary practical principles at a middle grade and high school level." }
                , new Book() { Id = 11, BookCategoryId = 4, Ordered = false , Price =10000, Author = "Algebra: A Complete Introduction: Teach Yourself\r\nby Hugh Neill", Title = "Neill’s textbook covers all the essential algebra topics from basic operations to quadratic equations and logarithms. This introduction is designed to help students improve their knowledge with clear explanations of algebra theory as well as examples and test questions" }
                , new Book() { Id = 12, BookCategoryId = 4, Ordered = false, Price = 2000, Author = "Abstract Algebra\r\nby David Dummit and Richard Foote", Title = "Abstract Algebra advances key concepts that underpin algebraic structures. Dummit and Foote begin with the most basic theories and build on existing knowledge with numerous examples to help students best appreciate the mathematical theory and its" }

                ,new Book() { Id = 13, BookCategoryId = 5, Ordered = false, Price = 10000, Author = " Mark J. Price", Title = " This comprehensive guide is perfect for beginners, exploring both C# 10 and .NET 6 in detail. Mark J. Price's approach ensures that readers not only learn the syntax but also understand the application of C# in creating modern, high-performing applications. The book includes numerous examples, exercises, and real-world scenarios." }
                ,new Book() { Id= 14, BookCategoryId = 5, Ordered = false, Price = 12000, Author = "Jennifer Greene and Andrew Stellman", Title = "Dive into C# and create apps, user interfaces, games, and more using this fun and highly visual introduction to C#, .NET Core, and Visual Studio. With this completely updated guide, which covers C# 8.0 and Visual Studio 2019, beginning programmers like you will build a fully functional game in the opening chapter." }
                ,new Book() { Id= 15, BookCategoryId = 5, Ordered = false, Price = 1000, Author = " Jon Skeet", Title = "C# in Depth is known for its thorough exploration of C#. Although it can be somewhat advanced, the book starts with fundamental concepts and gradually moves to more complex topics, making it a valuable resource for beginners aiming to deepen their knowledge. It's revered for its in-depth and meticulous examination of modern C#"}

                );
        }

        //For conversion enum index number to enum text
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<UserType>().HaveConversion<string>();
            configurationBuilder.Properties<AccountStatus>().HaveConversion<string>();
        }
    }
}
